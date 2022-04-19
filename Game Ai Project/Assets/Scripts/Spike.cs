using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    Rigidbody2D rb;
    public float movementSpeed;
    public float leftAngle;
    public float rightAngle;

    bool Clockwise;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Clockwise = true;
    }

    // Update is called once per frame
    void Update()
    {

        Move();
    }

    public void Direction()
    {
        if (transform.rotation.z > rightAngle)
        {
            Clockwise = false;
        }
        if (transform.rotation.z < leftAngle)
        {
            Clockwise = true;
        }

    }

    public void Move()
    {
        Direction();

        if (Clockwise)
        {
            rb.angularVelocity = movementSpeed;
        }

        if (!Clockwise)
        {
            rb.angularVelocity = -1 * movementSpeed;
        }
    }
}
