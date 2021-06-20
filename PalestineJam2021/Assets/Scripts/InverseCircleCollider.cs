using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class InverseCircleCollider : MonoBehaviour
{
    [SerializeField] private int vertices;
    [SerializeField] private float radius;

    void OnValidate()
    {
        CreateCollider();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateCollider();
    }

    void CreateCollider()
    {
        // https://forum.unity.com/threads/rigidbodies-inside-circle-collider.212730/

        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();
        Vector2[] points = new Vector2[vertices + 1];

        for (int i = 0; i <= vertices; i++)
        {
            float angle = 360f / vertices * i * Mathf.Deg2Rad;
            points[i] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        }

        collider.points = points;
    }
}
