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
    private Vector2 dashStartPosition;
    private Vector2 dashTarget;

    [Header("Trap")]
    [SerializeField] private LayerMask trapLayers;
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float trappedSeconds = 1f;
    [SerializeField] private float slowSecondsAfterTrap = 1f;

    [Header("Eviction")]
    [SerializeField] private float evictionVelocity = 100f;
    [SerializeField] private float evictionDuration = 0.3f;

    [SerializeField] private PhysicsMaterial2D bouncy;

    [SerializeField] private Sprite[] hitSprites;
    [SerializeField] private Beat beat;

    private float trapVelocityFactor = 1f;
    private bool trapped = false;
    private bool shouldEvict = false;
    private float evictTimer = 0f;
    private GameObject trap;

    private Rigidbody2D rb2d;

    protected internal bool endGame = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //originalScale = transform.localScale;
        shrinkEndGame = maxShrink;
        hitCount = hitToEnd;
    }

    //private float scaleDown = 1f;
    //private Vector2 originalScale;
    [SerializeField] private int hitToEnd = 3;
    [SerializeField] private float shrinkEndGame = 1f;
    private int hitCount;
    private bool canDash = true;

    [SerializeField] private float coolDown = 3f;
    private float lastHit = -1f;
    private bool locked;
    void Update()
    {
        locked = Time.time - lastHit <= coolDown;

        walkDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (locked)
        {
            walkDirection = Vector2.zero;
        }

        if (endGame)
        {
            rb2d.sharedMaterial = bouncy;
            walkMethod = WalkMethod.AddForceImpulse;
            rb2d.drag = 20;
            walkMagnitude = new Vector2(2, 2);
            dashForce = 10;
            //if (Input.GetKey(KeyCode.Mouse0))
            //{
            //    scaleDown -= 0.01f;
            //}
            //if (!shouldDash)
            //{
            //    shouldDash = Input.GetKeyDown(KeyCode.Mouse0);
            //    if (shouldDash)
            //    {
            //        dashTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    }
            //}
        }

        //if (canDash && Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    dashStartPosition = transform.position;
        //    dashing = true;
        //    canDash = false;
        //}

        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    dashing = false;
        //    canDash = true;
        //}

        if (!locked && endGame && !shouldDash)
        {
            shouldDash = Input.GetKeyDown(KeyCode.Mouse0);
            if (shouldDash)
            {
                dashTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //dashTarget.z = 0f;
            }
        }
    }

    //private float dashDuration = 1f;
    //private float dashTimer = 0f;
    void FixedUpdate()
    {
        //if (shouldEvict)
        //{
        //    evictTimer = evictionDuration;
        //    shouldEvict = false;
        //    shouldDash = false;
        //}

        if (shouldDash)
        {
            dashing = true;
            shouldDash = false;
            //dashTimer = 0f;
            //dashStartPosition = transform.position;
        }

        if (dashing)
        {
            //dashTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3[] line = new Vector3[2];
            line[0] = dashStartPosition;
            line[1] = dashTarget;
            FindObjectOfType<LineRenderer>().SetPositions(line);
            Vector2 direction = dashTarget - (Vector2)transform.position;
            //rb2d.MovePosition(dashTarget * Time.fixedDeltaTime);
            //Vector2 velocity = rb2d.velocity;
            // v' = ds / dt
            // vf - vi = ds / dt
            //  vf = ds / dt + vi
            //velocity.x += (dashTarget.x - transform.position.x) / 1f;
            //velocity.y += (dashTarget.y - transform.position.y) / 1f;
            //rb2d.velocity = velocity;
            rb2d.AddForce(direction * dashForce, ForceMode2D.Impulse);
            //dashTimer += Time.fixedDeltaTime;
            //rb2d.MovePosition(Vector2.Lerp(transform.position, dashTarget, dashTimer/dashDuration));
            //if (direction.magnitude <= 0.01f)
            //{
            //    dashing = false;
            //}
            //if (direction.magnitude <= 0.1f)
            //{
            //    dashing = false;
            //scaleDown = 1f;
            //}
        }
        
        Move(walkDirection, walkMagnitude);

        //transform.localScale = scaleDown * originalScale;
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

        //Vector3 directionVector = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        //float angle = Mathf.Atan2(directionVector.y, directionVector.x);
        
        //rb2d.MoveRotation(90);
    }
    
    private int spriteIndex = 0;

    [SerializeField] private float maxPushTime = 3f;
    private float pushTimer = 0f;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (endGame && dashing)
        {
            dashing = false;

            Hit(collision);
        }

        if (!endGame)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer < maxPushTime)
            {
                Push(collision);
            }
            else
            {
                rb2d.AddForce(rb2d.position);
                //rb2d.velocity = collision.relativeVelocity;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (endGame && dashing)
        {
            dashing = false;

            Hit(collision);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        pushTimer = 0f;
        SoliderMap map = collision.gameObject.GetComponent<SoliderMap>();
        if (map != null)
        {
            map.expanding = true;
        }
    }

    void Push(Collision2D collision)
    {
        SoliderMap map = collision.gameObject.GetComponent<SoliderMap>();
        if (map != null)
        {
            map.expanding = false;
            //float distance = ((Vector2)transform.position - dashStartPosition).magnitude;
            //map.Scale(-distance / 10f * maxShrink);
        }
    }

    void Hit(Collision2D collision)
    {
        // stop moving for a time, and fade in out or change color
        SoliderMap map = collision.gameObject.GetComponent<SoliderMap>();
        if (map != null)
        {
            if (!endGame)
            {
                float distance = ((Vector2)transform.position - dashStartPosition).magnitude;
                map.Scale(-distance / 10f * maxShrink);
            }
            else
            {
                lastHit = Time.time;
                hitCount--;
                //Color color = GetComponent<SpriteRenderer>().color;
                //color.a = (hitCount + 1f) / hitToEnd;
                //GetComponent<SpriteRenderer>().color = color;
                GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
                spriteIndex = Mathf.Min(spriteIndex + 1, hitSprites.Length - 1);
                map.Scale(-shrinkEndGame);

                if (hitCount <= 0)
                {
                    FindObjectOfType<ScenarioManager>().FadeOut();
                }
                else
                {
                    beat.StartBeat(coolDown);
                }
            }
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