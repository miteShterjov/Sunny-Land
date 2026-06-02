using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Input References")]
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private bool jumpPressed;
    
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
            inputActions.Player.Jump.performed += HandleJumpInput;
            inputActions.Player.Jump.canceled += HandleJumpInputCanceled;
            inputActions.Player.Attack.performed += HandleAttackInput;            
        }

        private void OnDisable()
        {
            inputActions.Player.Move.performed -= HandleMoveInput;
            inputActions.Player.Move.canceled -= HandleMoveInputCanceled;
            inputActions.Player.Jump.performed -= HandleJumpInput;
            inputActions.Player.Jump.canceled -= HandleJumpInputCanceled;
            inputActions.Player.Attack.performed -= HandleAttackInput;
            inputActions.Player.Disable();
        }
        
        public float GetClimbInput() => 
            Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f :
            Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? -1f : 0f;
        
        public Vector2 GetMoveInput() => moveInput;
        public bool IsSprintPressed() => inputActions.Player.Sprint.IsPressed();
        public bool IsJumpPressed()
        {
            if (!jumpPressed) return false;
            jumpPressed = false;
            return true;
        }

        private void HandleMoveInput(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void HandleMoveInputCanceled(InputAction.CallbackContext context)
        {
            moveInput = Vector2.zero;
        }

        private void HandleJumpInput(InputAction.CallbackContext context)
        {
            jumpPressed = true;
        }

        private void HandleJumpInputCanceled(InputAction.CallbackContext context)
        {
            jumpPressed = false;
        }

        private void HandleAttackInput(InputAction.CallbackContext context)
        {
            GetComponent<PlayerAttackController>().SpecialAttack();
        }
    }
}
