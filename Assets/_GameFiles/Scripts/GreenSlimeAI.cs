using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

// Enum AI Machine States: Patrol, Chase, Attack
public enum SlimeAIMachineState
{
    Patrol,
    Chase,
    Attack
}

public class GreenSlimeAI : MonoBehaviour
{
    SlimeAIMachineState _currentState = SlimeAIMachineState.Patrol;
    Rigidbody2D rb;

    public Transform playerPosition; // Player position
    public float playerDistance = 0.5f; // Distance to the player
    float currentPlayerDistance = 100; // Current distance to the player

    int slimeSpeed = 1; // Speed of the slime

    // bool to check if the slime is facing right
    bool isFacingRight = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Switch statement to handle the AI state machine
        switch (_currentState)
        {
            case SlimeAIMachineState.Patrol:
                Patrol();
                break;
            case SlimeAIMachineState.Chase:
                Chase();
                break;
            case SlimeAIMachineState.Attack:
                Attack();
                break;
        }
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Chase()
    {
        Debug.Log("Chasing Player");
        SlimeMove();
        CheckPlayerDistance();
    }

    private void Patrol()
    {
        SlimeMove();
        CheckPlayerDistance();
    }

    void SlimeMove()
    {
        // Move the slime left and right until hitting a collider
        if(isFacingRight) rb.linearVelocity = new Vector2(slimeSpeed, 0); // Move the slime right
        else rb.linearVelocity = new Vector2(-slimeSpeed, 0); // Move the slime left
    }

    // oncollisionenter2d is called when the slime collides with another collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the slime collides with a wall, change direction
        if (collision.gameObject.CompareTag("Solid"))
        {
            isFacingRight = !isFacingRight;
        }
    }

    // check player distance
    void CheckPlayerDistance()
    {
        currentPlayerDistance = transform.position.x - playerPosition.position.x;
        if (math.abs(currentPlayerDistance) < playerDistance)
        {
            if (currentPlayerDistance < 0) isFacingRight = true;
            else isFacingRight = false;

            _currentState = SlimeAIMachineState.Chase;
        }
        else
        {
            _currentState = SlimeAIMachineState.Patrol;
        }
    }       
}
