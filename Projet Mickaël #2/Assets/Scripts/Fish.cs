using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [Header("Scripts")]
    private UIManager uiManager;
    private GameManager gameManager;
    [SerializeField] private FishSO fishSO;
    
    [Header("Components")]
    [SerializeField] private Animator fishAnimator;
    [SerializeField] private Rigidbody fishRb;
    [SerializeField] private BoxCollider fishBc;
    [SerializeField] private Transform spikePos;
    [SerializeField] private GameObject mouthPos;
    
    [Header("Booleans")]
    [SerializeField] public bool isHit;
    [SerializeField] public bool isEaten;
    
    [Header("Values")]
    [SerializeField] private float waitBeforeEat;
    private float speedVelocity;

    
    
    private void Start()
    {
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
        fishRb = GetComponent<Rigidbody>();
        fishBc = GetComponent<BoxCollider>();
        fishAnimator = GetComponent<Animator>();
        spikePos = GameObject.Find("Dent1").transform;
        mouthPos = GameObject.Find("Shark").transform.GetChild(2).gameObject;

        isEaten = false;
        
        GameManager.instance.fishes.Add(gameObject); //Add to the list every fishes
        
        //Pop Animation
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.OutBounce);
        
        StartCoroutine(FishRandomMovement());
    }

    private void Update()
    {
        speedVelocity = fishRb.velocity.magnitude; //Check trident speedVelocity
        
        if (speedVelocity <= 0.2f) //Si shark ne bouge pas
        {
            fishAnimator.SetBool("isSwimming", false);
        }
        else
        {
            fishAnimator.SetBool("isSwimming", true);
        }
        
        if (isHit)
        {
            StopCoroutine(FishRandomMovement());
            transform.position = spikePos.position;
            
            GameManager.instance.fishes.Remove(gameObject); //Remove Fish de la liste
        }

        if (isEaten)
        {
            transform.position = mouthPos.transform.position;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shark"))
        {
            uiManager.AddScoreShark(1);
            StartCoroutine(DisappearFish());
        }
    }

    public IEnumerator FishRandomMovement()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10;

        transform.LookAt(randomPoint);
        fishRb.AddForce((randomPoint - transform.position) * fishSO.moveSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(1f, 10f));

        if (!isHit)
        {
            StartCoroutine(FishRandomMovement());
        }
    }

    IEnumerator DisappearFish()
    {
        isEaten = true;
        fishBc.enabled = false;
            
        GameManager.instance.fishes.Remove(gameObject); //Remove Fish de la liste
        yield return new WaitForSeconds(waitBeforeEat);
        
        transform.DOScale(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
        
    }
}
