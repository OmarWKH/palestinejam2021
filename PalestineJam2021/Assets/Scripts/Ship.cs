using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float velocity = 1f;
    [SerializeField] private Vector2 sailDirection = Vector2.right;
    [SerializeField] private float waitOnLading = 3f;

    private float currentVelocity = 0f;
    private bool disembarked = false;

    void Update()
    {
        if (transform.position.x < - 15f) // TODO viewport not 100
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        Vector2 positionChange = sailDirection * currentVelocity * Time.fixedDeltaTime;
        transform.position += new Vector3(positionChange.x, positionChange.y); ;
    }

    private void Die()
    {
        transform.DetachChildren(); // TODO parent them elsewhere
        Destroy(gameObject);
    }

    protected internal void Sail()
    {
        currentVelocity = velocity;
    }

    protected internal void SailAway()
    {
        // TODO destroy after it leaves
        //currentVelocity = -velocity;
        currentVelocity = velocity;
    }

    protected internal void Disembark()
    {
        currentVelocity = 0f;
        // TODO the thing coming from the side I forgot the name oh well
        Spawner spawner = GetComponent<Spawner>();
        if (spawner != null)
        {
            spawner.Spawn(false);
            FindObjectOfType<ScenarioManager>().shouldAssemble = true;
        }

        StartCoroutine(TriggerSailAway());
    }

    IEnumerator TriggerSailAway()
    {
        yield return new WaitForSeconds(waitOnLading);
        SailAway();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!disembarked)
        {
            disembarked = true;
            Disembark();
        }
    }
}
