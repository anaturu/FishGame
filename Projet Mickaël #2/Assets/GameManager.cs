using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<GameObject> fishes; //List of every fishes in the game
    
    public GameObject clownFish;
    public GameObject dori;
    public int numberOfNemo;
    public int numberOfDori;
    public int timeBeforeSecondWave;
    
    public Transform camPosStart;
    public Camera cam;
    
    
    public int shakeVibrato;

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
    }

    private void Start()
    {
        StartCoroutine(FirstWave());
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
            Instantiate(clownFish, Random.insideUnitSphere * 10, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator SecondWave()
    {
        
        for (int i = 0; i < numberOfDori; i++)
        {
            Instantiate(dori, Random.insideUnitSphere * 10, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);

        }
    }

    public void LightScreenShake()
    {
        cam.DOShakePosition(0.2f, Vector3.one * 0.5f, shakeVibrato, 90f, true).OnComplete(()=>
        {
            cam.transform.DOMove(camPosStart.transform.position, 0.1f);
        });
    }
}
