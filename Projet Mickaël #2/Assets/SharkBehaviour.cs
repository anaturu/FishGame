using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SharkBehaviour : MonoBehaviour, IDamageable
{
    private GameManager gameManager;
    
    [SerializeField] private Rigidbody sharkRb;
    [SerializeField] private Animator sharkAnimator;
    [SerializeField] private GameObject waypoint;

    [SerializeField] private float cooldownAttack;
    [SerializeField] private float moveSpeed;
    private float speedVelocity;
    [SerializeField] public bool isHit;
    
    [SerializeField] private int healthPoint;
    [SerializeField] private int maxHealth = 100;
    void Start()
    {
        gameManager = GameManager.instance;

        healthPoint = maxHealth;

        sharkRb = GetComponent<Rigidbody>();
        sharkAnimator = GetComponent<Animator>();
        transform.localScale = Vector3.zero;
     
        transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBounce);
        StartCoroutine(AttackFish());
    }

    private void Update()
    { 
        speedVelocity = sharkRb.velocity.magnitude; //Check trident speedVelocity

        //Add Score Here
        
        if (speedVelocity <= 0.2f) //Si shark ne bouge pas
        {
            sharkAnimator.SetBool("isSwimming", false);
        }
        else
        {
            sharkAnimator.SetBool("isSwimming", true);
        }
    }

    


    IEnumerator AttackFish()
    {
        if (healthPoint >= 0)
        {
            yield return new WaitForSeconds(1f);

            Vector3 nextFishPos = gameManager.fishes[Random.Range(0, gameManager.fishes.Count - 1)].transform.position - transform.position;
            transform.LookAt(nextFishPos);
            sharkRb.AddForce(nextFishPos * moveSpeed, ForceMode.VelocityChange);
            yield return new WaitForSeconds(cooldownAttack);
        
            StartCoroutine(AttackFish());
        }
        else if(healthPoint <= 0)//Death
        {
            StopCoroutine(AttackFish());
            transform.LookAt(waypoint.transform.position);
            transform.DOMove(waypoint.transform.position, 1f).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(1f);

            healthPoint = maxHealth;
            StartCoroutine(AttackFish());


            //Add Score Here
        }
        
        
    }

    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
        Debug.Log("TOOK SOME DAMAGE");
        
    }
}
