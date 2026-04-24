using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool playerSprintIsPressed = false;
    [SerializeField] private bool jumpPressed = false;
    
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += HandleMoveInput;
        inputActions.Player.Move.canceled += HandleMoveInputCanceled;
        inputActions.Player.Sprint.performed += HandleSprintInput;
        inputActions.Player.Sprint.canceled += HandleSprintInputCanceled;
        inputActions.Player.Jump.performed += HandleJumpInput;
        inputActions.Player.Jump.canceled += HandleJumpInputCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= HandleMoveInput;
        inputActions.Player.Move.canceled -= HandleMoveInputCanceled;
        inputActions.Player.Sprint.performed -= HandleSprintInput;
        inputActions.Player.Sprint.canceled -= HandleSprintInputCanceled;
        inputActions.Player.Jump.performed -= HandleJumpInput;
        inputActions.Player.Jump.canceled -= HandleJumpInputCanceled;
        inputActions.Player.Disable();
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void HandleMoveInputCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void HandleSprintInput(InputAction.CallbackContext context)
    {
        playerSprintIsPressed = true;
    }

    private void HandleSprintInputCanceled(InputAction.CallbackContext context)
    {
        playerSprintIsPressed = false;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    private void HandleJumpInputCanceled(InputAction.CallbackContext context)
    {
        jumpPressed = false;
    }

    public Vector2 GetMoveInput() => moveInput;
    public bool IsSprintPressed() => playerSprintIsPressed;
    public bool IsJumpPressed() => jumpPressed;
}
