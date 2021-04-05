using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable] public class MoveInputEvent: UnityEvent<Vector2> { }
[Serializable] public class JumpInputEvent: UnityEvent<float> { }
public class InputController : MonoBehaviour
{
    
    private PlayerControls controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.GroundMovement.Enable();
        controls.GroundMovement.HorizontalMovement.performed += OnMove;
        controls.GroundMovement.Jump.performed += OnJump;
        controls.GroundMovement.HorizontalMovement.canceled += OnMove;
        //controls.GroundMovement.HorizontalMovement.canceled += OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInputEvent.Invoke(1.5f);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput);
    }
    
    private void OnDisable()
    {
        controls.GroundMovement.Disable();
    }
}
