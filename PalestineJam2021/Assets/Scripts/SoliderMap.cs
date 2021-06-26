using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderMap : MonoBehaviour
{
    [SerializeField] private Vector3 expansionSpeed = Vector3.one;

    protected internal bool expanding = false;

    void Start()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
    }

    void FixedUpdate()
    {
        if (expanding && transform.localScale.y != 10f)
        {
            Scale(expansionSpeed);

            Vector2 size = transform.localScale;
            size /= 5f;
            size.x *= 1.5f;
            //GetComponent<CapsuleCollider2D>().size = size;
            //GetComponent<SpriteMask>();
            GetComponent<PolygonCollider2D>().CreatePrimitive(50, new Vector2(0.5f, 0.5f));
        }

        if (transform.localScale.y >= 10f)
        {
            FindObjectOfType<PlayerControls>().endGame = true;
            FindObjectOfType<ScenarioManager>().StopFights();
        }
    }

    protected internal void Scale(float toAdd)
    {
        Scale(Vector3.one * toAdd);
    }

    protected internal void Scale(Vector3 toAdd)
    {
        Vector3 scale = transform.localScale;
        scale += toAdd;

        scale.x = Mathf.Max(Mathf.Min(10f, scale.x), 0f);
        scale.y = Mathf.Max(Mathf.Min(10f, scale.y), 0f);
        scale.z = 1f;

        transform.localScale = scale;
    }

    protected internal void StartExpanding()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
        expanding = true;
    }
}
