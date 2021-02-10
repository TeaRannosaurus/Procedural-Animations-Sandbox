using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoController : MonoBehaviour
{

    [SerializeField] private Transform HeadBone;
    [SerializeField] private Transform LookAtTarget;


    void Start()
    {
        
    }

    void Update()
    {
        Vector3 towardsObjectFromHead = LookAtTarget.position - HeadBone.position;
        HeadBone.rotation = Quaternion.LookRotation(towardsObjectFromHead, transform.up);
    }
}
