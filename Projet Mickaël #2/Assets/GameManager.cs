using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    public static GameManager instance;
    
    [Header("Components")]
    public Camera cam;
    public GameObject clownFish;
    public GameObject dori;
    public Transform camPosStart;
    public Vector3 spawnOffset;
    public GameObject blobFish;
    public Transform[] spawnPosBlobFish;

    
    public List<GameObject> fishes; //List of every fishes in the game

    [Header("Values")]
    public int numberOfNemo;
    public int numberOfDori;
    public int timeBeforeSecondWave;
    public int shakeVibrato;
    public float spawnRange;

    

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of this singleton !!!");
            return;
        }
        instance = this;
        #endregion
        
        StartCoroutine(FirstWave());
    }

    private void Start()
    {
        StartCoroutine(SpawnBlobFish());
    }

    private void Update()
    {
        if (fishes.Count == 0) //When all fishes are caught/dead
        {
            StartCoroutine(SecondWave());
        }
    }

    IEnumerator FirstWave()
    {
        //1st wave
        for (int i = 0; i < numberOfNemo; i++)
        {
            Instantiate(clownFish, Random.insideUnitSphere * spawnRange + spawnOffset, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        
        
    }

    IEnumerator SecondWave()
    {
        
        for (int i = 0; i < numberOfDori; i++)
        {
            Instantiate(dori, Random.insideUnitSphere * spawnRange + spawnOffset, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);

        }
    }
    
    IEnumerator SpawnBlobFish()
    {

        Instantiate(blobFish, spawnPosBlobFish[Random.Range(0, spawnPosBlobFish.Length)]);
        yield return new WaitForSeconds(30);

        StartCoroutine(SpawnBlobFish());

    }

    public void LightScreenShake()
    {
        cam.DOShakePosition(0.2f, Vector3.one * 0.7f, shakeVibrato, 90f, true).OnComplete(()=>
        {
            cam.transform.DOMove(camPosStart.transform.position, 0.1f);
        });
    }
    
    
}
