using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction ballMovement;
    Rigidbody2D rb;
    AudioSource audioSource;
    ParticleSystem trailParticles; 

    GameStatus gameStatus;
    public Sprite[] sprites;
    public AudioClip[] audioClips;

    public float speed = 10f;

    private Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trailParticles = GetComponentInChildren<ParticleSystem>();
        gameStatus = GameObject.Find("Canvas/GameStatus").GetComponent<GameStatus>();
        audioSource = GetComponent<AudioSource>();

        playerInput = GetComponent<PlayerInput>();
        ballMovement = playerInput.currentActionMap["BallMovement"];
        ballMovement.Enable();
    }

    private void Start()
    {
        trailParticles.Stop();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = GetChangeSprite();
    }

    private void Update()
    {
        inputVector = ballMovement.ReadValue<Vector2>();

        if (inputVector != Vector2.zero)
        {
            SetParticleDirection(inputVector);
        }
        else
        {
            if (trailParticles.isPlaying)
            {
                trailParticles.Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = ballMovement.ReadValue<Vector2>();
        float actualSpeed;

        if (gameStatus.totalTime <= 100f)
        {
            actualSpeed = speed;
        }
        else
        {
            float multiplyer = gameStatus.totalTime / 100f;
            actualSpeed = speed * multiplyer;
        }
        rb.velocity = new Vector2(inputVector.x * actualSpeed, inputVector.y * actualSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite.name == this.gameObject.GetComponent<SpriteRenderer>().sprite.name && collision.gameObject.tag == "Marble")
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = GetChangeSprite();
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(audioClips[0]);
            gameStatus.scoreUpdate(10);
            gameStatus.timeUpdate(3);
        }
        else if (spriteRenderer.sprite.name != this.gameObject.GetComponent<SpriteRenderer>().sprite.name && collision.gameObject.tag == "Marble")
        {
            gameStatus.liveUpdate(-1);
            audioSource.PlayOneShot(audioClips[1]);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Life")
        {
            Destroy(collision.gameObject);
            gameStatus.liveUpdate(1);
            gameStatus.scoreUpdate(5);
            audioSource.PlayOneShot(audioClips[2]);
        }

    }

    private Sprite GetChangeSprite()
    {
        return sprites[Random.Range(0, sprites.Length)];
    }

    void SetParticleDirection(Vector2 direction)
    {
        Vector2 reverseDirection = -direction;

        var shape = trailParticles.shape;
        shape.rotation = new Vector3(0, 0, Mathf.Atan2(reverseDirection.y, reverseDirection.x) * Mathf.Rad2Deg);

        var main = trailParticles.main;
        main.startColor = GetRandomColor();

        if (!trailParticles.isPlaying)
        {
            trailParticles.Play();
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

}