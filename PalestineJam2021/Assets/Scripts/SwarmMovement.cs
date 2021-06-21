using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/**
 * Moves towards a random point within a circle. Changes target when it arrives.
 *
 * TODO swarm behaviour
 */
public class SwarmMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 1f;
    [SerializeField] private float distanceToChangeTarget = 0.1f;
    [SerializeField] private float targetSpaceRadius = 3f;

    private Vector3 target = Vector3.zero;

    void OnValidate()
    {
        SetTarget();
    }
    
    void Start()
    {
        SetTarget();
    }

    void SetTarget()
    {
        target = Random.insideUnitSphere * targetSpaceRadius;
        target.z = 0f;
    }
    
    void Update()
    {
        float distance = (transform.position - target).magnitude;
        if (distance < distanceToChangeTarget)
        {
            // TODO wait for a random bit
            SetTarget();
        }
    }

    void FixedUpdate()
    {
        // TODO randomize velocity
        transform.position += (target - transform.position) * velocity * Time.fixedDeltaTime;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target, 0.5f);
    }
}
