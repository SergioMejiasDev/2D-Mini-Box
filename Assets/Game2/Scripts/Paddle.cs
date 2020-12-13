﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls the movement of the paddles.
/// </summary>
public class Paddle : MonoBehaviour
{
    [SerializeField] bool isPlayer1 = false;
    float speed = 5;
    Rigidbody2D rb;
    AudioSource audioSource;
    float movement;
    Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isPlayer1)
        {
            movement = Input.GetAxisRaw("Player1Vertical");

            if (Input.GetButtonDown("Cancel"))
            {
                GameManager2.manager.PauseGame();
            }
        }
        else
        {
            movement = Input.GetAxisRaw("Player2Vertical");
        }

        rb.velocity = new Vector2(rb.velocity.x, movement * speed);
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
        transform.position = startPosition;
    }
}
