using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    private UIManager uiManager;
    private GameManager gameManager;

    [SerializeField] private Rigidbody fishRb;
    [SerializeField] private Transform spikePos;
    [SerializeField] public bool isHit;
    
    [SerializeField] private int healthPoint;
    [SerializeField] private int maxHealth = 100;
    
    [SerializeField] private int addClownFishScore = 10;
    
    [SerializeField] private float moveSpeed;


    private void Start()
    {
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;

        //Add to the list every fishes
        GameManager.instance.fishes.Add(gameObject);
        
        healthPoint = maxHealth;
        spikePos = GameObject.Find("Dent1").GetComponent<Transform>();
        fishRb = GetComponent<Rigidbody>();
        
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.OutBounce);

        StartCoroutine(FishRandomMovement());

    }

    private void Update()
    {
        if (isHit)
        {
            StopCoroutine(FishRandomMovement());
            transform.position = spikePos.position;
            
            GameManager.instance.fishes.Remove(gameObject);

            
        }
    }

    public IEnumerator FishRandomMovement()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10;
        
        transform.LookAt(randomPoint);
        fishRb.AddForce((randomPoint - transform.position) * moveSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(1f, 10f));

        if (!isHit)
        {
            StartCoroutine(FishRandomMovement());
        }
    }
}
