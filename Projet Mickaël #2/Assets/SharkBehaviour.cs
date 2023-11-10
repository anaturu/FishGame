using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SharkBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject waypoint1;
    [SerializeField] private GameObject waypoint2;
    [SerializeField] private GameObject spikePos2;
    [SerializeField] private Rigidbody sharkRb;
    [SerializeField] private float travelTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] public bool isHit;
    
    [SerializeField] private int healthPoint;
    [SerializeField] private int maxHealth = 100;
    void Start()
    {
        healthPoint = maxHealth;

        sharkRb = GetComponent<Rigidbody>();
        transform.localScale = Vector3.zero;
     
        transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBounce);
        StartCoroutine(WaypointMovement());
    }

    private void Update()
    { 
        if (healthPoint <= 0) //Death
        {
            StopCoroutine(WaypointMovement());
            transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f).SetEase(Ease.OutBounce);
            transform.position = spikePos2.transform.position;

            //Add Score Here
            
        }
    }


    IEnumerator WaypointMovement()
    {
        transform.LookAt(waypoint2.transform.position);
        sharkRb.AddForce((waypoint2.transform.position - transform.position) * moveSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(travelTime);
        
        transform.LookAt(waypoint1.transform.localPosition);
        sharkRb.AddForce((waypoint1.transform.localPosition - transform.position) * moveSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(travelTime);
        
        StartCoroutine(WaypointMovement());
        
    }

    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
        Debug.Log("TOOK SOME DAMAGE");
        
    }
}
