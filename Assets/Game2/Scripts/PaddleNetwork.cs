using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the functions of the paddle in online mode.
/// </summary>
public class PaddleNetwork : MonoBehaviourPun, IPunObservable
{
    float speed = 3.5f;
    [SerializeField] float startPosition;

    [Header("Components")]
    [SerializeField] PhotonView pv = null;
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] AudioSource hitSound = null;

    void Update()
    {
        if (pv.IsMine)
        {
            float v = Input.GetAxisRaw("Player1Vertical");

            if (Input.GetButtonDown("Cancel"))
            {
                OnlineManager2.onlineManager.PauseGame();
            }

            rb.velocity = new Vector2(rb.velocity.x, v * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            hitSound.Play();
        }
    }

    /// <summary>
    /// Function called to reset the paddle position.
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector2(startPosition, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position.y);
        }

        else
        {
            transform.position = new Vector2(startPosition, (float)stream.ReceiveNext());
        }
    }
}
