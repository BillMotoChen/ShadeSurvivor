using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform panel;

    private void Awake()
    {
        panel = GetComponent<RectTransform>();

        Vector2 safeAreaMinPos = Screen.safeArea.position;
        Vector2 safeAreaMaxPos = Screen.safeArea.position + Screen.safeArea.size;

        safeAreaMinPos.x = safeAreaMinPos.x / Screen.width;
        safeAreaMinPos.y = safeAreaMinPos.y / Screen.height;

        safeAreaMaxPos.x = safeAreaMaxPos.x / Screen.width;
        safeAreaMaxPos.y = safeAreaMaxPos.y / Screen.height;

        panel.anchorMin = safeAreaMinPos;
        panel.anchorMax = safeAreaMaxPos;


    }
}
