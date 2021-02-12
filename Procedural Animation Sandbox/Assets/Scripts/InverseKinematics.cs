using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple 3 bone IK 
public class InverseKinematics : MonoBehaviour
{

    [SerializeField] private Transform Target;
    [SerializeField] private Transform Pole;

    [SerializeField] private Transform FirstBone;
    [SerializeField] private Vector3 FirstBoneEulerAngleOffset;
    [SerializeField] private Transform SecondBone;
    [SerializeField] private Vector3 SecondBoneEulerAngleOffset;
    [SerializeField] private Transform ThridBone;
    [SerializeField] private Vector3 ThirdBoneEulerAngleOffset;
    [SerializeField] private bool AllignThridBoneWithTargetRotation = true;

    private bool IsSetUp()
    {
        return !(FirstBone == null ||
                SecondBone == null ||
                ThridBone == null ||
                Target == null ||
                Pole == null);
    }

    private void OnEnable()
    {
        // Stop component if a bone is not set up WE NEED ALL
        if(!IsSetUp())
        {
            Debug.LogError("Bones are not configured for this IK component. This component will be disabled.", this);
            enabled = false;
            return;
        }
    }

    // Animupdate as last logic
    void LateUpdate()
    {
        Vector3 towardPole = Pole.position - FirstBone.position;
        Vector3 towardTarget = Target.position - FirstBone.position;

        float rootBoneLength = Vector3.Distance(FirstBone.position, SecondBone.position);
        float secondBoneLength = Vector3.Distance(SecondBone.position, ThridBone.position);
        float totalChainLength = rootBoneLength + secondBoneLength;

        //Aligning root with target
        FirstBone.rotation = Quaternion.LookRotation(towardTarget, towardPole);
        FirstBone.localRotation *= Quaternion.Euler(FirstBoneEulerAngleOffset);

        Vector3 towardSecondBone = SecondBone.position - FirstBone.position;

        float targetDistance = Vector3.Distance(FirstBone.position, Target.position);

        // Limiting the hypothenuse to prevent invalid tiangles
        targetDistance = Mathf.Min(targetDistance, totalChainLength * 0.9999f);

        float adjacent =
            (
                (rootBoneLength * rootBoneLength) +
                (targetDistance* targetDistance) -
                (secondBoneLength * secondBoneLength)
            ) / (2.0f * targetDistance * rootBoneLength);
        float angle = Mathf.Acos(adjacent) * Mathf.Rad2Deg;

        Vector3 cross = Vector3.Cross(towardPole, towardSecondBone);

        if(!float.IsNaN(angle))
        {
            FirstBone.RotateAround(FirstBone.position, cross, -angle);
        }

        Quaternion secondBoneTargetRotation = Quaternion.LookRotation(Target.position - SecondBone.position, cross);
        secondBoneTargetRotation *= Quaternion.Euler(SecondBoneEulerAngleOffset);
        SecondBone.rotation = secondBoneTargetRotation;

        if(AllignThridBoneWithTargetRotation)
        {
            ThridBone.rotation = Target.rotation;
            ThridBone.localRotation *= Quaternion.Euler(ThirdBoneEulerAngleOffset);
        }

    }

    private void OnDrawGizmos()
    {
        if (!IsSetUp()) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Target.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(SecondBone.position, Pole.position);


        Gizmos.color = Color.green;
        Gizmos.DrawLine(FirstBone.position, SecondBone.position);
        Gizmos.DrawLine(SecondBone.position, ThridBone.position);

    }
}
