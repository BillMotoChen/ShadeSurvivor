using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGlow : MonoBehaviour
{
    public float colorChangeSpeed = 1.0f;

    public float emissionIntensity = 2.0f;

    private Renderer ballRenderer;

    void Start()
    {
        ballRenderer = GetComponent<Renderer>();
        ballRenderer.material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float r = Mathf.PingPong(Time.time * colorChangeSpeed, 1.0f);
        float g = Mathf.PingPong(Time.time * colorChangeSpeed + 0.33f, 1.0f);
        float b = Mathf.PingPong(Time.time * colorChangeSpeed + 0.66f, 1.0f);

        Color neonColor = new Color(r, g, b) * 2.0f;

        ballRenderer.material.color = neonColor;

        Color emissionColor = neonColor * emissionIntensity;
        ballRenderer.material.SetColor("_EmissionColor", emissionColor);
    }
}