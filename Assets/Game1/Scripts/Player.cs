using UnityEngine;

/// <summary>
/// Class that takes care of the player's movement.
/// </summary>
public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] bool isPlayer1 = false;

    [Header("Movement")]
    float speed = 4;
    float jump = 9.5f;
    [SerializeField] LayerMask groundMask = 0;
    
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] AudioSource coinSound = null;
    [SerializeField] AudioSource hurtSound = null;
    #endregion

    /// <summary>
    /// Boolean that indicates through a Raycast if the player is touching the ground.
    /// </summary>
    /// <returns>True if the player is on the ground, false if not.</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, sr.bounds.extents.y + 0.01f, 0), Vector2.down, 0.1f, groundMask);

        return hit;
    }

    private void OnEnable()
    {
        if (isPlayer1)
        {
            transform.position = new Vector2(-6.3f, -5.4f);
        }
        else
        {
            transform.position = new Vector2(6.3f, -5.4f);
        }
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
        anim.SetBool("IsWalking", h != 0 && IsGrounded());
        anim.SetBool("IsJumping", !IsGrounded());
    }

    /// <summary>
    /// Function called to make the player jump.
    /// </summary>
    public void Jump()
    {
        if (isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                audioSource.Play();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
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

            hurtSound.Play();
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
