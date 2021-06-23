using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderMap : MonoBehaviour
{
    [SerializeField] private Vector3 expansionSpeed = Vector3.one;

    private bool expanding = false;

    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    void FixedUpdate()
    {
        if (expanding)
        {
            Scale(expansionSpeed);
        }
    }

    protected internal void Scale(float toAdd)
    {
        Scale(Vector3.one * toAdd);
    }

    protected internal void Scale(Vector3 toAdd)
    {
        Vector3 scale = transform.localScale;
        scale += toAdd;

        scale.x = Mathf.Max(Mathf.Min(10f, scale.x), 0f);
        scale.y = Mathf.Max(Mathf.Min(10f, scale.y), 0f);
        scale.z = 1f;

        transform.localScale = scale;
    }

    protected internal void StartExpanding()
    {
        GetComponent<Collider2D>().enabled = true;
        expanding = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity);
    }
}
