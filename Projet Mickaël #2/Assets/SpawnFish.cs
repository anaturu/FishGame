using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnFish : MonoBehaviour
{
    private UIManager uiManager;

    
    
    public GameObject shark;

    void Start()
    {
        uiManager = UIManager.instance;
        
        
    }

    void SpawnShark()
    {
        if (uiManager.currentOverallScore >= 100)
        {
            Instantiate(shark, Random.insideUnitSphere * 10, Quaternion.identity);

        }
    }
}
