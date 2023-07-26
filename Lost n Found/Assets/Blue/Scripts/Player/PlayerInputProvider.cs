using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputProvider : MonoBehaviour
{
    public bool sprinting { get; private set; }
    public bool sneaking { get; private set; }
    public Vector2 inputMovement { get; private set; }
    public void OnWalk(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        sprinting = context.action.IsPressed() && !sneaking;
    }
    public void OnSneak(InputAction.CallbackContext context)
    {
        sneaking = context.action.IsPressed();
    }
}
