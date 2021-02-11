using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoController : MonoBehaviour
{
    //Head variables
    [SerializeField] private Transform HeadBone;
    [SerializeField] private Transform LookAtTarget;
    [SerializeField] private float LookAtSpeed;
    [SerializeField] private float MaxHeadTurnAngle;

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

    //Late update used for animations
    private void LateUpdate()
    {
        UpdateHead();
        UpdateEyes();
    }

    //Head rotation
    private void UpdateHead()
    {
        // Storing rotation before resetting
        Quaternion currentLocalRotation = HeadBone.localRotation;
        // Resetting headbone roation to identity quaterion so that world to local transformation will use the zero rotation
        HeadBone.localRotation = Quaternion.identity;
        
        Vector3 targetWorldLookDirection = LookAtTarget.position - HeadBone.position;
        Vector3 targetLocalLookDirection = HeadBone.InverseTransformDirection(targetWorldLookDirection);


        // Aplying the angle limit
        targetLocalLookDirection = Vector3.RotateTowards(
            Vector3.forward,
            targetLocalLookDirection,
            Mathf.Deg2Rad * MaxHeadTurnAngle, //Mathf.Deg2Rad converts degree to radians
            0.0f
            );

        Quaternion targetLocalRotation = Quaternion.LookRotation(
            targetLocalLookDirection,
            Vector3.up
            );

        // Smooth look at
        HeadBone.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1.0f - Mathf.Exp(-LookAtSpeed * Time.deltaTime)
            );

    }

    //Eyes look at
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

        ///Clamp eye rotation
        float leftEyeCurrentYRotation = LeftEye.localEulerAngles.y;
        float rightEyeCurrentYRotation = RightEye.localEulerAngles.y;

        if(leftEyeCurrentYRotation > 180.0f)
        {
            leftEyeCurrentYRotation -= 360;
        }
        if(rightEyeCurrentYRotation > 180.0f)
        {
            rightEyeCurrentYRotation -= 360;
        }

        // Clamping Y rotation
        float leftEyeClampedYRotation = Mathf.Clamp(
            leftEyeCurrentYRotation,
            LeftEyeMinYRotation,
            LeftEyeMaxYRotation
            );
         float rightEyeClampedYRotation = Mathf.Clamp(
            rightEyeCurrentYRotation,
            RightEyeMinYRotation,
            RightEyeMaxYRotation
            );

        // Apply clamp
        LeftEye.localEulerAngles = new Vector3(
            LeftEye.localEulerAngles.x,
            leftEyeClampedYRotation,
            LeftEye.localEulerAngles.z
            );
        RightEye.localEulerAngles = new Vector3(
            RightEye.localEulerAngles.x,
            rightEyeClampedYRotation,
            RightEye.localEulerAngles.z
            );

    }
}
