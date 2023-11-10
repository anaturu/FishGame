using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private GameObject waypoint1;
    [SerializeField] private GameObject waypoint2;
    [SerializeField] private float travelTime;
    void Start()
    {
        StartCoroutine(WaypointMovement());
    }

    IEnumerator WaypointMovement()
    {
        transform.LookAt(waypoint2.transform.position);
        transform.DOMove(waypoint2.transform.position, travelTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(travelTime);
        
        transform.LookAt(waypoint1.transform.position);
        transform.DOMove(waypoint1.transform.position, travelTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(travelTime);

        StartCoroutine(WaypointMovement());
    }
}
