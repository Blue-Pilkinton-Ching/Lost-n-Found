using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(IHeadBobber))]
public class PlayerHeadBob : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    float speed = 0;
    float oldSpeed = 0;
    float smoothSpeed = 0;
    float bobValue = 0;

    float maxSpeed;
    IHeadBobber headBobber;
    CinemachinePOV cameraPOV;
    CinemachineRecomposer cameraRecomposer;
    CinemachineCameraOffset cameraOffset;

    HeadBobSettings headBobSettings;
    private void Awake() {

        cameraPOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        cameraRecomposer = virtualCamera.GetComponent<CinemachineRecomposer>();
        cameraOffset = virtualCamera.GetComponent<CinemachineCameraOffset>();
        headBobber = GetComponent<IHeadBobber>();

        headBobSettings = MainDependencies.Singleton.HeadBobSettings;
    }

    private void Start() {
        maxSpeed = headBobber.GetMaxMovementSpeed();
    }

    private void Update() {
        BobHead();
    }
    private void BobHead()
    {
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

        speed = headBobber.GetMovementSpeed();

        if (speed != oldSpeed)
        {
            DOTween.To(() => smoothSpeed, x => smoothSpeed = x, speed, headBobSettings.BobBlendTime);
        }

        oldSpeed = speed;

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
