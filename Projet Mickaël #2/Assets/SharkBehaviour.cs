using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SharkBehaviour : MonoBehaviour, IDamageable
{
    [Header("Scripts")]
    private GameManager gameManager;
    private UIManager uiManager;
    
    [Header("Components")]
    [SerializeField] private Rigidbody sharkRb;
    [SerializeField] private Animator sharkAnimator;
    [SerializeField] private GameObject waypoint;
    
    [Header("Values")]
    [SerializeField] private int healthPoint;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float cooldownAttack;
    [SerializeField] private float cooldownComeback;
    [SerializeField] private float scaredMovementSpeed;
    [SerializeField] private float moveSpeed;
    private float speedVelocity;
    
    [Header("Booleans")]
    [SerializeField] public bool isScared;

    void Start()
    {
        gameManager = GameManager.instance;
        uiManager = UIManager.instance;

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
        
        if (healthPoint <= 0)//Death
        {
            transform.LookAt(waypoint.transform.position);
            sharkRb.AddForce((waypoint.transform.position - transform.position) * scaredMovementSpeed, ForceMode.VelocityChange);

            healthPoint = maxHealth;
            
            isScared = true;
            //Add Score Here
            
            StartCoroutine(AttackFish());

        }
    }

    


    IEnumerator AttackFish()
    {
        if (isScared)
        {
            Debug.Log("IS RUNNING AWAY");
            yield return new WaitForSeconds(cooldownComeback);
            isScared = false;
        }
        
        Vector3 nextFishPos = gameManager.fishes[Random.Range(0, gameManager.fishes.Count - 1)].transform.position - transform.position;
        transform.LookAt(nextFishPos);
        sharkRb.AddForce(nextFishPos * moveSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(cooldownAttack);

        if (!isScared)
        {
            StartCoroutine(AttackFish());
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
        Debug.Log("TOOK SOME DAMAGE");
        
    }
}
