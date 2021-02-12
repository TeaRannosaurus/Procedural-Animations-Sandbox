using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    //Stay in range of this Transfrom
    [SerializeField] private Transform  HomeLocation;
    [SerializeField] private float      MaxStepDistance;
    [SerializeField] private float      MoveDuration;
    [HideInInspector] public bool       IsMoving;

    private IEnumerator MoveToHomeLocation()
    {
        IsMoving = true;

        //Start position and rotation

        //End position and rotation

        //Do loop till we reach goal within time

        //Stop moving

        yield return null;
    }

}
