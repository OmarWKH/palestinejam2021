using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderMap : MonoBehaviour
{
    [SerializeField] private Vector3 expansionSpeed = Vector3.one;

    protected internal bool expanding = false;

    void FixedUpdate()
    {
        if (expanding)
        {
            Vector3 scale = transform.localScale;
            scale += expansionSpeed;

            scale.x = Mathf.Min(10f, scale.x);
            scale.y = Mathf.Min(10f, scale.y);
            scale.z = 1f;

            transform.localScale = scale;
        }
    }
}
