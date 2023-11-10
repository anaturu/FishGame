using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TridentBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject mouseGizmo;
    [SerializeField] private Rigidbody tridentRb;
    [SerializeField] private BoxCollider tridentBc;
    [SerializeField] private Vector3 tridentPos;
    private GameManager gameManager;
    private UIManager uiManager;
    
    [SerializeField] private float explosionPower;
    [SerializeField] private float speedVelocity;
    [SerializeField] private float callBackTime;
    
    [SerializeField] private bool throwOn;
    [SerializeField] private bool throwOff;

    [SerializeField] private List<GameObject> fishCaught = new List<GameObject>();
    

    private void Start()
    {
        tridentRb = GetComponent<Rigidbody>();
        tridentBc = GetComponent<BoxCollider>();
        tridentPos = transform.position;
        
        gameManager = GameManager.instance;
        uiManager = UIManager.instance;
    }

    void Update()
    {
        speedVelocity = tridentRb.velocity.magnitude; //Check trident speedVelocity
        
        if (speedVelocity <= 0.3f) //Si trident ne bouge pas
        {
            tridentBc.enabled = false;
            Debug.Log("TRIDENT NE BOUGE PLUS");
        }
        else
        {
            tridentBc.enabled = true;
        }
        
        //Look at mouse
        if (throwOn)
        {
            transform.LookAt(mouseGizmo.transform.localPosition); 
        }
        
        TridentLogic();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish")) //Si l'objet touché est un clownfish
        {
            other.transform.GetComponent<Fish>().isHit = true;
            other.transform.GetComponent<BoxCollider>().enabled = false; //Deactivate fish collider when hit
            
            fishCaught.Add(other.gameObject); //Add Fish Caught to list
            
            uiManager.AddScoreFish(10); //Add OverallScore
            uiManager.AddFishCounter(1); //Add FishCounter
        }
        
        if (other.gameObject.CompareTag("Dori")) //Si l'objet touché est un Dori
        {
            other.transform.GetComponent<Fish>().isHit = true;
            other.transform.GetComponent<BoxCollider>().enabled = false; //Deactivate fish collider when hit
            
            fishCaught.Add(other.gameObject); //Add Fish Caught to list
            
            uiManager.AddScoreFish(50); //Add OverallScore
            uiManager.AddFishCounter(1); //Add FishCounter
        }

        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Shark"))
        {
            other.transform.GetComponent<SharkBehaviour>().TakeDamage(20);

        }
    }

    void TridentLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && throwOff)
        {
            StartCoroutine(TridentRecall());
            Debug.Log("TRIDENT IS BACK");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && throwOn)
        {
            TridentThrow();
            Debug.Log("TRIDENT IS GONE");
        }
    }

    IEnumerator TridentRecall()
    {
        transform.DOMove(tridentPos, callBackTime);
        yield return new WaitForSeconds(callBackTime);
        
        throwOn = true;
        throwOff = false;
    }

    private void TridentThrow()
    {
        throwOff = true;
        throwOn = false;
            
        tridentRb.AddForce((mouseGizmo.transform.position - transform.position) * explosionPower, ForceMode.Force);
        gameManager.LightScreenShake();
    }
    
}
