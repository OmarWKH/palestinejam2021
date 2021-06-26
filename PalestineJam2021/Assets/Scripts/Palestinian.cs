using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palestinian : MonoBehaviour
{
    protected internal bool evicted = false;
    
    void Update()
    {
        if (evicted)
        {
            if (transform.position.magnitude > 25f)
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
