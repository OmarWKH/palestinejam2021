using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOverTime : MonoBehaviour
{
    [SerializeField] private float timeToHide = 1f;

    protected internal bool fading = false;

    private SpriteRenderer renderer;
    private float hideTimer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        hideTimer = timeToHide;
    }

    private void FixedUpdate()
    {
        // TODO with dotween or animation
        if (fading && hideTimer >= 0f)
        {
            Color color = renderer.color;
            Debug.Log($"{hideTimer / timeToHide} - {(1 - hideTimer / timeToHide)} - {color.a}");
            color.a = 1 - hideTimer / timeToHide;
            renderer.color = color;
            hideTimer -= Time.fixedDeltaTime;
        }
    }
}