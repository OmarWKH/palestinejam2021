using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/**
 * Moves towards a random point within a circle. Changes target when it arrives.
 *
 * TODO swarm behaviour
 */
[RequireComponent(typeof(Rigidbody2D))]
public class SwarmMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 1f;
    [SerializeField] private float distanceToChangeTarget = 0.1f;
    [SerializeField] private float targetSpaceRadius = 3f;
    [SerializeField] protected internal bool randomizeTarget = true;
    [SerializeField] protected internal Vector3 target = Vector3.zero;

    private Rigidbody2D rb2d;

    void OnValidate()
    {
        //SetTarget();
    }
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;

        SetTarget();
    }

    void SetTarget()
    {
        if (randomizeTarget)
        {
            target = Random.insideUnitSphere * targetSpaceRadius;
            target.z = 0f;
        }
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
        // TODO BUG zionist stops at 1 away instead of 0.1
        Vector3 direction = target - transform.position;
        Vector3 change = direction * velocity * Time.fixedDeltaTime;
        transform.position += change;

        float angle = Mathf.Atan2(direction.y, direction.x);
        rb2d.MoveRotation(angle * Mathf.Rad2Deg);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target, 0.5f);
    }
}
