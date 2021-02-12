using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    //Stay in range of this Transfrom
    [SerializeField] private Transform  HomeLocation;
    [SerializeField] private float      MaxStepDistance;
    [SerializeField] private float      MoveDuration;
    /*[HideInInspector]*/ public bool       IsMoving;

    private void Update()
    {
        if (IsMoving) 
            return;

        float distanceFromHomeLocation = Vector3.Distance(transform.position, HomeLocation.position);

        if (distanceFromHomeLocation > MaxStepDistance)
            StartCoroutine(MoveToHomeLocation());
    }

    // Routine for moving the feet back in place
    private IEnumerator MoveToHomeLocation()
    {
        IsMoving = true;

        //Start position and rotation
        Quaternion startRotation = transform.rotation;
        Vector3 startPosition = transform.position;

        //End position and rotation
        Quaternion endRotation = HomeLocation.rotation;
        Vector3 endPosition = HomeLocation.position;

        float elapsedTime = 0.0f;

        //Do loop till we reach goal within time
        while (elapsedTime < MoveDuration)
        {
            // Time since last frame and normalized value to get step distance
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / MoveDuration;

            transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, normalizedTime);

            yield return null;
        }

        IsMoving = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (IsMoving)
            Gizmos.color = Color.red; //Is in incorrect pos
        else
            Gizmos.color = Color.green; //Is in correct pos

        Gizmos.DrawWireSphere(transform.position, 0.25f);
        Gizmos.DrawLine(transform.position, HomeLocation.position);
        Gizmos.DrawWireCube(HomeLocation.position, Vector3.one * 0.1f);
    }

}
