using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[DefaultExecutionOrder(-100)]
public class SaveScript : MonoBehaviour
{
    const string savedFilePath = "/save.dat";

    public static SaveScript instance;

    public static int bestScore = 0;
    public static int gamePlayed = 0;
    public static List<int> top10Scores = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static bool saving = false;
    public static bool loading = false;

    // saved data
    public int bestScoreSaved = 0;
    public int gamePlayedSaved = 0;
    public List<int> top10ScoresSaved = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    void dataInit()
    {
        bestScoreSaved = 0;
        gamePlayedSaved = 0;
        top10ScoresSaved = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        SaveData();
        LoadData();
    }

    public void SaveData()
    {
        string fileLocation = Application.persistentDataPath + savedFilePath;

        bestScoreSaved = bestScore;
        gamePlayedSaved = gamePlayed;
        top10ScoresSaved = top10Scores;
        string jsonData = JsonUtility.ToJson(this);

        string encryptedData = EncryptionUtility.Encrypt(jsonData);

        try
        {
            StreamWriter writer = new StreamWriter(fileLocation);
            writer.Write(encryptedData);
            writer.Close();
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save data: " + e.Message);
        }
    }

    public void LoadData()
    {
        string fileLocation = Application.persistentDataPath + savedFilePath;

        if (File.Exists(fileLocation))
        {
            try
            {
                StreamReader reader = new StreamReader(fileLocation);
                string encryptedData = reader.ReadToEnd();
                reader.Close();

                string decryptedData = EncryptionUtility.Decrypt(encryptedData);
                JsonUtility.FromJsonOverwrite(decryptedData, this);
                bestScore = bestScoreSaved;
                gamePlayed = gamePlayedSaved;
                top10Scores = top10ScoresSaved;
            }
            catch (IOException e)
            {
                Debug.LogError("Failed to load data: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Save file does not exist, initializing new data.");
            dataInit();
        }
    }

    public void UpdateTop10Scores(int newScore)
    {
        if (top10Scores.Count < 10 || newScore > top10Scores.Min())
        {
            if (top10Scores.Count >= 10)
            {
                top10Scores.Remove(top10Scores.Min());
            }
            top10Scores.Add(newScore);
            top10Scores = top10Scores.OrderByDescending(score => score).ToList();
        }
        SaveData();
    }
}
