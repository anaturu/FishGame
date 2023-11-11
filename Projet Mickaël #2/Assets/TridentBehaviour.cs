using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TridentBehaviour : MonoBehaviour
{
    [Header("Scripts")] 
    public static TridentBehaviour instance;

    private GameManager gameManager;
    private UIManager uiManager;
    [SerializeField] private FishSO clownFishSO;
    [SerializeField] private FishSO doriSO;
    
    [Header("Components")]
    [SerializeField] private ParticleSystem tridentParticle;
    [SerializeField] private ParticleSystem tridentParticleFollow;
    [SerializeField] private GameObject mouseGizmo;
    private Rigidbody tridentRb;
    private BoxCollider tridentBc;
    private Vector3 tridentPos;
    
    [SerializeField] private List<GameObject> fishCaught = new List<GameObject>();
    
    [Header("Values")]
    [SerializeField] private float explosionPower;
    [SerializeField] private float callBackTime;
    private float speedVelocity;
    
    [Header("Booleans")]
    [SerializeField] private bool throwOn;
    [SerializeField] private bool throwOff;

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
            //Debug.Log("TRIDENT NE BOUGE PLUS");
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
            
            uiManager.AddScoreFish(clownFishSO.addScore); //Add OverallScore
            uiManager.AddFishCounter(1); //Add FishCounter
        }
        
        if (other.gameObject.CompareTag("Dori")) //Si l'objet touché est un Dori
        {
            other.transform.GetComponent<Fish>().isHit = true;
            other.transform.GetComponent<BoxCollider>().enabled = false; //Deactivate fish collider when hit
            
            fishCaught.Add(other.gameObject); //Add Fish Caught to list
            
            uiManager.AddScoreFish(doriSO.addScore); //Add OverallScore
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
            //Debug.Log("TRIDENT IS BACK");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && throwOn)
        {
            TridentThrow();
            //Debug.Log("TRIDENT IS GONE");
        }
    }

    IEnumerator TridentRecall()
    {
        throwOn = true;
        throwOff = false;
        
        transform.DOMove(tridentPos, callBackTime);
        yield return new WaitForSeconds(callBackTime);
        
    }

    public void TridentThrow()
    {
        throwOff = true;
        throwOn = false;
            
        tridentRb.AddForce((mouseGizmo.transform.position - transform.position) * explosionPower, ForceMode.Force);
        gameManager.LightScreenShake();
        
        tridentParticle.Play();
        tridentParticleFollow.Play();
    }
    
}
