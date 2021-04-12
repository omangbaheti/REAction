using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable] public class MoveInputEvent: UnityEvent<Vector2> { }
[Serializable] public class JumpInputEvent: UnityEvent<bool> { }
[Serializable] public class MouseInputEvent: UnityEvent<Vector2>{ }
public class InputController : MonoBehaviour
{
    
    private PlayerControls controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    public MouseInputEvent mouseInputEvent;
    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        //Step1: Enable the GroundMovement action map
        controls.GroundMovement.Enable();
        
        //Step 2: Subscribe the actions of horizontal movement and Jump to OnMove and OnJump functions
        //when they are performed
        controls.GroundMovement.Look.performed += MouseMove;
        controls.GroundMovement.HorizontalMovement.performed += OnMove;
        controls.GroundMovement.Jump.performed += OnJump;
        
        //Step3: Cancel the Horizontal movement so that the vector is not stuck to the previous value (eg (0,1))
        controls.GroundMovement.HorizontalMovement.canceled -= OnMove;
        controls.GroundMovement.Jump.canceled -= OnJump;
    }

    private void MouseMove(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();
        mouseInputEvent.Invoke(mouseInput);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInputEvent.Invoke(true);
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
