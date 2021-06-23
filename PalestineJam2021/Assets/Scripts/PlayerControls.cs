using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private WalkMethod walkMethod = WalkMethod.AddVelocity;
    [SerializeField] private Vector2 walkMagnitude = Vector2.one;
    private Vector2 walkDirection = Vector2.zero;

    [Header("Dash")]
    //[SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashForce = 300f;
    [SerializeField] private float maxShrink = 1f;
    private bool shouldDash = false;
    private bool dashing = false;
    private Vector3 dashStartPosition;
    private Vector3 dashTarget;

    [Header("Trap")]
    [SerializeField] private LayerMask trapLayers;
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float trappedSeconds = 1f;
    [SerializeField] private float slowSecondsAfterTrap = 1f;

    [Header("Eviction")]
    [SerializeField] private float evictionVelocity = 100f;
    [SerializeField] private float evictionDuration = 0.3f;

    private float trapVelocityFactor = 1f;
    private bool trapped = false;
    private bool shouldEvict = false;
    private float evictTimer = 0f;
    private GameObject trap;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        walkDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (!shouldDash)
        {
            shouldDash = Input.GetKeyDown(KeyCode.Mouse0);
        }
        dashTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (shouldEvict)
        {
            evictTimer = evictionDuration;
            shouldEvict = false;
            shouldDash = false;
        }
        
        if (shouldDash)
        {
            dashing = true;
            shouldDash = false;
            dashStartPosition = transform.position;
        }

        if (evictTimer > 0f)
        {
            rb2d.velocity = new Vector2(0, evictionVelocity);
            evictTimer -= Time.fixedDeltaTime;
        }
        else if (dashing)
        {
            Vector2 direction = dashTarget - transform.position;
            rb2d.AddForce(direction * dashForce, ForceMode2D.Impulse);
            if (direction.magnitude <= 0.01f)
            {
                dashing = false;
            }
        }
        {
            Move(walkDirection, walkMagnitude);
        }
    }

    private static float InitialVelocity(float direction, float distance, float time, float finalVelocity)
    {
        // TODO removed *2 and seemed to get the right distance and time
        // s / t - v_f
        return (distance * direction) / time - finalVelocity;
    }

    void Move(Vector2 direction, Vector2 magnitude)
    {
        Vector2 value = direction * magnitude * trapVelocityFactor;
        switch (walkMethod)
        {
            case WalkMethod.AddForceNormal:
                rb2d.AddForce(value, ForceMode2D.Force);
                break;
            case WalkMethod.AddForceImpulse:
                rb2d.AddForce(value, ForceMode2D.Impulse);
                break;
            case WalkMethod.AddVelocity:
                rb2d.velocity += value;
                break;
            case WalkMethod.SetVelocity:
                rb2d.velocity = value;
                break;
            default:
                break;
        }
    }

    IEnumerator Trap()
    {
        transform.position = trap.transform.position;

        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(trappedSeconds);
        rb2d.constraints = RigidbodyConstraints2D.None;

        shouldEvict = true;

        trapVelocityFactor = slowFactor;
        yield return new WaitForSeconds(trappedSeconds);
        trapVelocityFactor = 1f;
    }

    IEnumerator Untrap()
    {
        yield return new WaitForSeconds(slowSecondsAfterTrap);
        if (!trapped)
        {
            trapVelocityFactor = 1f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (dashing)
        {
            dashing = false;

            SoliderMap map = collision.gameObject.GetComponent<SoliderMap>();
            if (map != null)
            {
                float distance = (transform.position - dashStartPosition).magnitude;
                map.Scale(-distance/10f * maxShrink);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GetComponent<CircleCollider2D>().IsTouchingLayers(trapLayers))
        {
            trapped = true;
            trap = collider.gameObject;
            StartCoroutine(Trap());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (GetComponent<CircleCollider2D>().IsTouchingLayers(trapLayers))
        {
            trapped = false;
            //StartCoroutine(Untrap());
        }
    }
}

enum WalkMethod
{
    AddForceNormal,
    AddForceImpulse,
    AddVelocity,
    SetVelocity,
}