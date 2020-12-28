using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that takes care of the player's movement.
/// </summary>
public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] bool isPlayer1 = false;

    [Header("Movement")]
    float speed = 4;
    float jump = 9.5f;
    bool isGrounded;
    
    [Header("Components")]
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] AudioSource coinSound = null;
    #endregion

    private void OnEnable()
    {
        if (isPlayer1)
        {
            transform.position = new Vector2(-8.76f, -5.4f);
        }
        else
        {
            transform.position = new Vector2(7.5f, -5.4f);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float h;

        if (isPlayer1)
        {
            h = Input.GetAxisRaw("Player1Horizontal");
        }
        else
        {
            h = Input.GetAxisRaw("Player2Horizontal");
        }

        Movement(h);

        Animation(h);

        Jump();
    }

    /// <summary>
    /// Function called to make the player move.
    /// </summary>
    /// <param name="h">Direction of movement of the player, positive if it is to the right and negative if it is to the left.</param>
    public void Movement(float h)
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * h);

        if (h > 0)
        {
            sr.flipX = false;
        }
        else if (h < 0)
        {
            sr.flipX = true;
        }
    }

    /// <summary>
    /// Function that activates character animations.
    /// </summary>
    /// <param name="h">Direction of movement of the player, positive if it is to the right and negative if it is to the left.</param>
    public void Animation(float h)
    {
        anim.SetBool("IsWalking", (h != 0) && (isGrounded));
        anim.SetBool("IsJumping", !isGrounded);
    }

    /// <summary>
    /// Function called to make the player jump.
    /// </summary>
    public void Jump()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position - new Vector3(0, sr.bounds.extents.y + 0.01f, 0), Vector2.down, 0.1f);
        
        {
            if (hit)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        if (isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.W) && (isGrounded))
            {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                audioSource.Play();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && (isGrounded))
            {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                audioSource.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Game1/Enemy")) || (other.gameObject.CompareTag("Game1/Missile")))
        {
            if (GameManager1.manager.isMultiplayer)
            {
                GameManager1.manager.Respawn(isPlayer1);
            }
            else
            {
                gameObject.SetActive(false);
                GameManager1.manager.GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Game1/Coin"))
        {
            coinSound.Play();
            other.gameObject.SetActive(false);
            GameManager1.manager.UpdateScore(isPlayer1);
        }
    }
}
