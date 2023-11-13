using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [Header("Scripts")] 
    private TridentBehaviour tridentManager;
    private UIManager uiManager;
    private GameManager gameManager;
    [SerializeField] private FishSO fishSO;
    
    [Header("Components")]
    [SerializeField] private Animator fishAnimator;
    [SerializeField] private Rigidbody fishRb;
    [SerializeField] private CapsuleCollider fishCapsuleCollider;
    [SerializeField] private BoxCollider fishBc;
    [SerializeField] private GameObject mouthPos;
    [SerializeField] private GameObject fishBone;
    [SerializeField] private ParticleSystem trailEatBloodParticle;
    [SerializeField] private GameObject deadNemo;
    [SerializeField] private GameObject deadDori;

    
    [Header("Booleans")]
    [SerializeField] public bool isHit;
    [SerializeField] public bool isEaten;
    [SerializeField] public bool isInBucket;

    [Header("Values")] 
    private int randomIndex;
    [SerializeField] private float waitBeforeEat;
    private float speedVelocity;

    
    
    private void Start()
    {
        tridentManager = TridentBehaviour.instance;
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
        fishRb = GetComponent<Rigidbody>();
        fishBc = GetComponent<BoxCollider>();
        fishCapsuleCollider = GetComponent<CapsuleCollider>();
        fishAnimator = GetComponent<Animator>();
        mouthPos = GameObject.Find("Shark").transform.GetChild(2).gameObject;

        randomIndex = Random.Range(0, tridentManager.spikePos.Length);

        isEaten = false;
        isInBucket = false;
        
        GameManager.instance.fishes.Add(gameObject); //Add to the list every fishes
        
        //Pop Animation
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(4f, 4f, 4f), 0.5f).SetEase(Ease.OutBounce);
        
        StartCoroutine(FishRandomMovement());
    }

    private void Update()
    {
        speedVelocity = fishRb.velocity.magnitude; //Check trident speedVelocity
        
        if (speedVelocity <= 0.2f && fishAnimator != null) //Si shark ne bouge pas
        {
            fishAnimator.SetBool("isSwimming", false);
        }
        else
        {
            fishAnimator.SetBool("isSwimming", true);
        }
        
        if (isHit)
        {
            transform.position = tridentManager.spikePos[randomIndex].position;
            Debug.Log(randomIndex);
            
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
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameManager.spawnOffset, gameManager.spawnRange);
    }

    public IEnumerator FishRandomMovement()
    {
        Vector3 randomPoint = Random.insideUnitSphere * gameManager.spawnRange;
        randomPoint += gameManager.spawnOffset;

        transform.DOLookAt(randomPoint, 0.5f);
        fishRb.AddForce((randomPoint - transform.position) * fishSO.moveSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(1f, 10f));

        StartCoroutine(FishRandomMovement());
    }

    IEnumerator DisappearFish()
    {
        isEaten = true;
        fishBc.enabled = false;
            
        GameManager.instance.fishes.Remove(gameObject); //Remove Fish de la liste
        trailEatBloodParticle.Play();
        yield return new WaitForSeconds(waitBeforeEat);
        
        transform.DOScale(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(1f);

        isEaten = false;
        Instantiate(fishBone, transform.position, Quaternion.identity); //Spawn FishBone
        yield return new WaitForSeconds(0.3f);

        Instantiate(deadNemo, tridentManager.sharkBucketPos[Random.Range(0, tridentManager.sharkBucketPos.Length)].position, Quaternion.identity);

    }
}
