using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{

    public Animator animator;
    private SpriteRenderer sp;
    private Scene scene;
    private Rigidbody2D mainCharacter;
    private BoxCollider2D collider;
    [SerializeField] private LayerMask jumpGround;
    public int points = 0;


    private float dirX = 0f;
    [SerializeField] private float speed = 7f;
    private float Jumpforce = 17f;

    private enum movement { idle, right, jump };
    public int LivesLeft = 5;

    [SerializeField] private AudioSource coinsSoundEffect;

    public GameObject Block;
    public GameObject Flag;

    //lives as time goes up
    public int healthDecrease = 1;
    float currentTime;
    float increaseAmountTime = 1.0f;


    void Start()
    {
        scene = SceneManager.GetActiveScene();
        mainCharacter = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        Block.SetActive(false);
        Flag.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        mainCharacter.velocity = new Vector2(dirX * speed, mainCharacter.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            mainCharacter.velocity = new Vector2(mainCharacter.velocity.x, Jumpforce);
        }

        Animations();
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpGround);
    }

    private void Animations()
    {
        movement state;
        if (dirX > 0f)
        {
            state = movement.right;
            sp.flipX = false;
        }
        else if (dirX < 0)
        {
            state = movement.right;
            sp.flipX = true;
        }
        else
        {
            state = movement.idle;
        }
        if (mainCharacter.velocity.y > 0.1f)
        {
            state = movement.jump;
        }
        else if (mainCharacter.velocity.y < -0.1f)
        {
            state = movement.idle;
        }

        animator.SetInteger("state", (int)state);
    }

    public void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 50), "Points: " + points + " \nLives: " + LivesLeft);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinsSoundEffect.Play();
            points += 10;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            LivesLeft--;
            if (LivesLeft > 0)
            {
                points -= 5;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
                points = 0;
            }
        }
        if (other.gameObject.CompareTag("Melon"))
        {
           
            if (LivesLeft < 5)
            {
                points += 5;
                LivesLeft += 1;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
                points += 5;
            }
        }
       
        if (other.gameObject.CompareTag("Spike"))
        {
            LivesLeft--;
            if (LivesLeft > 0)
            {
                points -= 5;
            }
            else
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
                points = 0;
            }
        }

        if (other.gameObject.CompareTag("Saw"))
        {
            LivesLeft--;
            if (LivesLeft > 0)
            {
                points -= 5;
            }
            else
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
                points = 0;
            }
        }
         
        if (other.gameObject.CompareTag("Lava"))
        {
            LivesLeft--;

            if (LivesLeft > 0)
            {
                points -= 5;
            }
            else
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
                points = 0;
            }
        }
        
        if (other.gameObject.CompareTag("Floor"))
        {
            Destroy(other.gameObject,1);
        }

        if (other.gameObject.CompareTag("SpikeDeath"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }

        if (other.gameObject.CompareTag("SwingingSpike"))
        {
                points -= 5;
        }

        if (other.gameObject.CompareTag("Block"))
        {
            Block.SetActive(true);
        }

        if (other.gameObject.CompareTag("Rockman"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }

        if (other.gameObject.CompareTag("FlagBlock"))
        {
            Flag.SetActive(true);
        }

        if (other.gameObject.CompareTag("CheckPoint"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}

