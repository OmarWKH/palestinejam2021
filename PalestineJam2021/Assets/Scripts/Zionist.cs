using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zionist : MonoBehaviour
{
    protected internal bool hidden = false;

    void Update()
    {
        if (hidden)
        {
            if (transform.position.magnitude < 0.1f)
            {
                DestroyObject();
            }
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
