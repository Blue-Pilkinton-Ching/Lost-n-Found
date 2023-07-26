using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputProvider))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    CharacterController controller;
    PlayerMovementSettings playerMovementSettings;
    CinemachinePOV cameraPOV;
    CinemachineRecomposer cameraRecomposer;
    CinemachineCameraOffset cameraOffset;
    PlayerInputProvider playerInputProvider;
    HeadBobSettings headBobSettings;
    Vector3 gravityMovement;
    Vector3 finalMovement;
    float oldSpeed = 0;
    float smoothSpeed = 0;
    float bobValue = 0;
    float maxSpeed;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInputProvider = GetComponent<PlayerInputProvider>();

        playerMovementSettings = ScenelessDependencies.Singleton.PlayerMovementSettings;

        cameraPOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        cameraRecomposer = virtualCamera.GetComponent<CinemachineRecomposer>();
        cameraOffset = virtualCamera.GetComponent<CinemachineCameraOffset>();

        headBobSettings = ScenelessDependencies.Singleton.HeadBobSettings;

        maxSpeed = ScenelessDependencies.Singleton.PlayerMovementSettings.SprintSpeed;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(virtualCamera.gameObject);
            controller.enabled = false;
            enabled = false;
        }
    }

    private void Update()
    {
        Move();
        BobHead();
    }
    private void Move()
    {
        float playerSpeed = playerInputProvider.sneaking ? playerMovementSettings.SneakSpeed : playerInputProvider.sprinting ? playerMovementSettings.SprintSpeed : playerMovementSettings.WalkSpeed;

        Vector2 playerMovement = new Vector2(
            playerInputProvider.inputMovement.y < 0 ? playerInputProvider.inputMovement.y * playerSpeed * playerMovementSettings.BackwardsSpeedMultiplier : playerSpeed * playerInputProvider.inputMovement.y,
            playerMovement.x = playerInputProvider.inputMovement.x * playerSpeed * playerMovementSettings.SidewaysSpeedMultiplier
        );

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + cameraPOV.m_HorizontalAxis.Value, transform.rotation.z);

        finalMovement = (transform.forward * playerMovement.x) + (transform.right * playerMovement.y);

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

    private void BobHead()
    {
        float speed = finalMovement.magnitude;

        if (speed != oldSpeed)
        {
            DOTween.To(() => smoothSpeed, x => smoothSpeed = x, speed, headBobSettings.BobBlendTime);
        }

        oldSpeed = speed;

        if (smoothSpeed == 0) return;

        bobValue += headBobSettings.BobSpeed.Evaluate(smoothSpeed / maxSpeed) * Time.deltaTime;

        if (bobValue > 360)
        {
            bobValue -= 360;
        }

        float bobValueSin = Mathf.Sin(bobValue);
        float bobValueSinSlow = Mathf.Sin(bobValue * 0.5f);

        cameraOffset.m_Offset = new Vector3(BobSlow(headBobSettings.XBobAmount), Bob(headBobSettings.YBobAmount) + headBobSettings.BobHeight.Evaluate(smoothSpeed / maxSpeed), 0);
        cameraRecomposer.m_Dutch = BobSlow(headBobSettings.DutchBobAmount);
        cameraRecomposer.m_Pan = BobSlow(headBobSettings.PanBobAmount);
        cameraRecomposer.m_Tilt = headBobSettings.TiltBobOnSecondFootstep ? BobSlow(headBobSettings.TiltBobAmount) : Bob(headBobSettings.TiltBobAmount);

        float BobSlow(AnimationCurve amp)
        {
            return bobValueSinSlow * amp.Evaluate(smoothSpeed / maxSpeed);
        }
        float Bob(AnimationCurve amp)
        {
            return bobValueSin * amp.Evaluate(smoothSpeed / maxSpeed);
        }
    }
}