using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IHeadBobber
{
    [field: SerializeField] public float SprintSpeed {get; private set;}
    [field: SerializeField] public float SneakSpeed { get; private set; }
    [field: SerializeField] public float WalkSpeed { get; private set; }

    public float BackwardsSpeedMultiplier = 0.5f;
    public float SidewaysSpeedMultiplier = 0.8f;

    private CharacterController controller;

    Vector2 inputMovement;
    Vector3 gravityMovement;
    Vector3 finalMovement;
    bool sprint;
    bool sneak;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float playerSpeed = sneak ? SneakSpeed : sprint ? SprintSpeed : WalkSpeed;

        Vector2 playerMovement = new Vector2(
            inputMovement.y < 0 ? inputMovement.y * playerSpeed * BackwardsSpeedMultiplier : playerSpeed * inputMovement.y,
            playerMovement.x = inputMovement.x * playerSpeed * SidewaysSpeedMultiplier
        );

        finalMovement = (transform.forward * playerMovement.y) + (transform.right * playerMovement.x);

        if (controller.isGrounded)
        {
            gravityMovement = Vector3.zero;
        }
        else
        {
            gravityMovement += Physics.gravity * Time.deltaTime;
        }

        controller.Move((finalMovement + gravityMovement) * Time.deltaTime);
    }
    public float GetMovementSpeed()
    {
        return finalMovement.magnitude;
    }
    public float GetMaxMovementSpeed()
    {
        return SprintSpeed;
    }

    #region GetInput
    public void OnWalk(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.action.IsPressed();
    }
    public void OnSneak(InputAction.CallbackContext context)
    {
        sneak = context.action.IsPressed();
    }

    #endregion
}