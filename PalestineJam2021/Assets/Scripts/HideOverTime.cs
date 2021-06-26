using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOverTime : MonoBehaviour
{
    [SerializeField] private float timeToHide = 1f;

    protected internal bool hiding = false;

    private SpriteRenderer renderer;
    private float hideTimer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        hideTimer = timeToHide;
    }
    
    void Update()
    {
        if (hideTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // TODO with dotween or animation
        if (hiding && hideTimer >= 0f)
        {
            Color color = renderer.color;
            Debug.Log($"{hideTimer / timeToHide} - {(1 - hideTimer / timeToHide)} - {color.a}");
            color.a = hideTimer / timeToHide;
            renderer.color = color;
            hideTimer -= Time.fixedDeltaTime;
        }
    }
}
