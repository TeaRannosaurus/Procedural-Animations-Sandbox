using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoController : MonoBehaviour
{
    //Head variables
    [SerializeField] private Transform HeadBone;
    [SerializeField] private Transform LookAtTarget;
    [SerializeField] private float LookAtSpeed;

    //Eyes variables
    [SerializeField] private Transform LeftEye;
    [SerializeField] private Transform RightEye;
    [SerializeField] private float EyeLookAtSpeed;
    [SerializeField] private float LeftEyeMaxYRotation;
    [SerializeField] private float LeftEyeMinYRotation;
    [SerializeField] private float RightEyeMaxYRotation;
    [SerializeField] private float RightEyeMinYRotation;

    private void Start()
    {
        if (HeadBone == null)
            Debug.LogError("Head is not set in gecko class animations cannot play\n", this);
        if (LookAtTarget == null)
            Debug.LogError("No target to look at for gecko. Lookat animations cannot play\n", this);
    }

    private void LateUpdate()
    {
        UpdateHead();
        UpdateEyes();
    }

    //Head rotation
    private void UpdateHead()
    {
        Vector3 towardsObjectFromHead = LookAtTarget.position - HeadBone.position;

        Quaternion targetRotation = Quaternion.LookRotation(
            towardsObjectFromHead,
            transform.up
            );

        HeadBone.rotation = Quaternion.Slerp(
            HeadBone.rotation,
            targetRotation,
            1.0f - Mathf.Exp(-LookAtSpeed * Time.deltaTime)
            );
    }

    private void UpdateEyes()
    {
        // Local eye position
        Quaternion targetEyeRotation = Quaternion.LookRotation(
            LookAtTarget.position - HeadBone.position,
            transform.up
            );

        //Left eye
        LeftEye.rotation = Quaternion.Slerp(
            LeftEye.rotation,
            targetEyeRotation,
            1.0f - Mathf.Exp(-EyeLookAtSpeed * Time.deltaTime)
            );

        //Right eye
        RightEye.rotation = Quaternion.Slerp(
            RightEye.rotation,
            targetEyeRotation,
            1.0f - Mathf.Exp(-EyeLookAtSpeed * Time.deltaTime)
            );
    }
}
