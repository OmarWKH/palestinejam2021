using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private Vector2 walkForce = Vector2.one;
    [SerializeField] private ForceMode2D walkForceMode = ForceMode2D.Impulse;
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
        rb2d.AddForce(walkDirection, walkForceMode);
    }
}
