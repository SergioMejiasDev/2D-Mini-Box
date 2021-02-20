using UnityEngine;

/// <summary>
/// Class that manages the main functions of the player.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    float maxBound = 7, minBound = -7;
    [SerializeField] AudioSource shootAudio = null;
    [SerializeField] Transform shootPoint = null;
    [SerializeField] float cadency = 1;

    public float nextFire;

    void Update()
    {
        float h = Input.GetAxisRaw("Player1Horizontal");
        
        if (transform.position.x < minBound && h < 0)
        {
            h = 0;
        }
        else if (transform.position.x > maxBound && h > 0)
        {
            h = 0;
        }
        
        transform.Translate(Vector2.right * h * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) && (Time.time > nextFire))
        {
            nextFire = Time.time + cadency;
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Game3/BulletPlayer");
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = Quaternion.identity;
            }
            shootAudio.Play();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager3.manager3.PauseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Game3/BulletEnemy"))
        {
            other.gameObject.SetActive(false);
            GameManager3.manager3.LoseHealth(1);
        }
    }

}
