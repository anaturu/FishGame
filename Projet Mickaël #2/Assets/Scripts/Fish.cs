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
    [SerializeField] private Rigidbody fishRb;
    [SerializeField] private Transform spikePos;
    
    [Header("Booleans")]
    [SerializeField] public bool isHit;
    
    private void Start()
    {
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
        fishRb = GetComponent<Rigidbody>();
        spikePos = GameObject.Find("Dent1").GetComponent<Transform>();
        
        GameManager.instance.fishes.Add(gameObject); //Add to the list every fishes
        
        //Pop Animation
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
            
            GameManager.instance.fishes.Remove(gameObject); //Remove Fish de la liste
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
}
