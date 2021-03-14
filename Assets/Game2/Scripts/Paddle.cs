using UnityEngine;

/// <summary>
/// Class that controls the movement of the paddles.
/// </summary>
public class Paddle : MonoBehaviour
{
    [Header("Movement")]
    float speed = 3.5f;
    
    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] AudioSource audioSource = null;

    void Update()
    {
        float v = Input.GetAxisRaw("Player1Vertical");

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager2.manager.PauseGame();
        }

        rb.velocity = new Vector2(rb.velocity.x, v * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Function called to reset the paddle position.
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector2(-5.75f, 0);
    }
}
