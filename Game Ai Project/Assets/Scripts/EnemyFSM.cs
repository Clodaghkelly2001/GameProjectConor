using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{

    public GameObject target;
    public float speed = 1.0f;
    public float patternRange = 5;

    public GameObject[] pattern;
    private int patternIndex = 0;

    //flip direction
    private SpriteRenderer sp;
    private Rigidbody2D enemy;
    private float dirX = 0f;

    public enum EnemyStates
    {
        PATROL,
        CHASE

    };

    public EnemyStates currentState = EnemyStates.PATROL;
    public Sprite patrol;
    public Sprite chase;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        dirX = Input.GetAxisRaw("Horizontal");
        enemy.velocity = new Vector2(dirX * speed, enemy.velocity.y);

        switch (currentState)
        {
            case EnemyStates.PATROL:
                Patrol();
                break;
            case EnemyStates.CHASE:
                chaseLOS();
                break;
        }
    }

    void Patrol()
    {
        GameObject waypoint = pattern[patternIndex];

        Vector3 rangeToClose = waypoint.transform.position - transform.position;

        Debug.DrawRay(transform.position, rangeToClose, Color.cyan);

        float distance = rangeToClose.magnitude;

        float speedDelta = speed * Time.deltaTime;

        if (distance <= speedDelta)
        {
            patternIndex++;
            sp.flipX = true;

            if (patternIndex >= pattern.Length)
            {
                patternIndex = 0;
                sp.flipX = false;

            }

            waypoint = pattern[patternIndex];

            rangeToClose = waypoint.transform.position - transform.position;


        }

        Vector3 normalizedRangeToClose = rangeToClose.normalized;

        Debug.DrawRay(transform.position, normalizedRangeToClose, Color.magenta);

        Vector3 delta = speedDelta * normalizedRangeToClose;

        transform.Translate(delta);

    }

    void chaseLOS()
    {
        float speedDelta = speed * Time.deltaTime;
        Vector3 newPosition = tusMoveTowards(speedDelta);
        transform.position = newPosition;

        Vector3 tusMoveTowards(float sd)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = target.transform.position;

            Vector3 rangeToClose = targetPosition - currentPosition;

            Debug.DrawRay(currentPosition, rangeToClose, Color.red);

            Vector3 newPosition;

            // Get the magnitude of the vector
            float magnitude = rangeToClose.magnitude;

            if (magnitude < patternRange)
            {
                Vector3 normalisedRangeToClose = rangeToClose.normalized;
                Debug.DrawRay(currentPosition, normalisedRangeToClose, Color.green);

                Debug.Log("Magnitude: " + magnitude + " Direction: " + normalisedRangeToClose);

                // Move speedDelta along the normalisedRangeToClose direction
                Vector3 velocity = normalisedRangeToClose * sd;

                newPosition = currentPosition + velocity;
            }
            else
            {
                newPosition = currentPosition;
            }

            return newPosition;
        }
    }

    void CheckDistance()
    {
        Vector3 range = target.transform.position - transform.position;
        Debug.DrawRay(transform.position, range, Color.cyan);
        if (range.magnitude > patternRange)
        {
            currentState = EnemyStates.PATROL;

        }
        else
        {
            currentState = EnemyStates.CHASE;
        }
    }
}