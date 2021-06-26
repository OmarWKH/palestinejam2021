using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    private bool shouldBeat = false;
    private SpriteRenderer renderer;
    private float duration;
    private float timer = -1f;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 0f;
        renderer.color = color;
    }

    void FixedUpdate()
    {
        if (timer >= 0f)
        {
            Color color = renderer.color;
            color.a = AlphaEquation(duration - timer, duration);
            renderer.color = color;
            timer -= Time.fixedDeltaTime;
        }
    }

    float AlphaEquation(float time, float duration)
    {
        float f = 1f / Mathf.Pow(duration / 2f, 2);
        return -f * Mathf.Pow(time - duration / 2f, 2f) + 1f;
    }

    protected internal void StartBeat(float duration)
    {
        this.duration = duration;
        timer = duration;
    }
}
