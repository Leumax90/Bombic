using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    [SerializeField] private float gravityValue = -9.81f;
    protected CharacterController controller;
    protected PlayerActionsExample playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool playerTuchPortal = false;

    public bool isAtack = false;
    private bool isMoving = false;
    public Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerActionsExample();
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(0, 0, movement.y);

        if (!playerTuchPortal)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            // Player Animations
            isMoving = (movement.y != 0) && (playerSpeed != 0);
            animator.SetBool("walk", isMoving);

            isAtack = (isAtack != false);
            animator.SetBool("atack", isAtack);
            isAtack = false;
        }
        else
        {
            transform.Translate(new Vector3(0,0, playerSpeed) * Time.deltaTime);
        }

        Vector3 currentPosition = transform.position;
        currentPosition.x = 0;
        transform.position = currentPosition;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}