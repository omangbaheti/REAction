using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable] public class MoveInputEvent: UnityEvent<Vector2> { }
[Serializable] public class JumpInputEvent: UnityEvent<bool> { }
[Serializable] public class MouseInputEvent: UnityEvent<Vector2>{ }
[Serializable] public class ShootInputEvent: UnityEvent<bool>{ }
[Serializable] public class HookShotEvent: UnityEvent<bool>{ }
public class InputController : MonoBehaviour
{
    
    private PlayerControls controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    public MouseInputEvent mouseInputEvent;
    public ShootInputEvent shootInputEvent;
    public HookShotEvent hookInputEvent;

    private bool hookStatus = true;
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
        controls.GroundMovement.Shoot.performed += OnShoot;
        controls.GroundMovement.HookShot.performed += OnHookEvent;
        
        //Step3: Cancel the Horizontal movement so that the vector is not stuck to the previous value (eg (0,1))
        controls.GroundMovement.HorizontalMovement.canceled -= OnMove;
        controls.GroundMovement.Jump.canceled -= OnJump;
        controls.GroundMovement.HookShot.canceled -= OnHookEvent;
        //controls.GroundMovement.Shoot.canceled -= OnShoot;
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
    
    private void OnShoot(InputAction.CallbackContext context)
    {
        shootInputEvent.Invoke(true);
        hookInputEvent.Invoke(false);
    }

    private void OnHookEvent(InputAction.CallbackContext context)
    {
        //hookStatus = !hookStatus;
        hookInputEvent.Invoke(hookStatus);
    }
    
    private void OnDisable()
    {
        controls.GroundMovement.Disable();
    }
}
