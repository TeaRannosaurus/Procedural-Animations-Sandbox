using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoController : MonoBehaviour
{
    
    //Transforms
    [SerializeField] private Transform HeadBone;
    [SerializeField] private Transform LookAtTarget;

    //Floats
    [SerializeField] private float LookAtSpeed;


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

        Quaternion targetRoation = Quaternion.LookRotation(
            towardsObjectFromHead,
            transform.up
            );

        HeadBone.rotation = Quaternion.Slerp(
            HeadBone.rotation,
            targetRoation,
            1.0f - Mathf.Exp(-LookAtSpeed * Time.deltaTime)
            );
    }

    private void UpdateEyes()
    {

    }
}
