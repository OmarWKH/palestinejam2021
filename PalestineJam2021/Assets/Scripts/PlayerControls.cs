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

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        walkDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        switch (walkMethod)
        {
            case WalkMethod.AddForceNormal:
                rb2d.AddForce(walkDirection * walkMagnitude, ForceMode2D.Force);
                break;
            case WalkMethod.AddForceImpulse:
                rb2d.AddForce(walkDirection * walkMagnitude, ForceMode2D.Impulse);
                break;
            case WalkMethod.AddVelocity:
                rb2d.velocity += walkDirection * walkMagnitude;
                break;
            case WalkMethod.SetVelocity:
                rb2d.velocity = walkDirection * walkMagnitude;
                break;
            default:
                break;
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