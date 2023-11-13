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
    [SerializeField] public Transform[] spikePos;
    [SerializeField] public Transform[] bucketPos;
    [SerializeField] public Transform[] sharkBucketPos;
    private Rigidbody tridentRb;
    private BoxCollider tridentBc;
    private Vector3 tridentPos;
    public GameObject deadNemo;
    public GameObject deadDori;
    
    [Header("Values")]
    [SerializeField] private float explosionPower;
    [SerializeField] public float callBackTime;
    private float speedVelocity;
    
    [Header("Booleans")]
    [SerializeField] private bool throwOn;
    [SerializeField] private bool throwOff;
    
    [Header("Raycast")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform mouseWorldPosGizmo;
    [SerializeField] private float maxRange = 100;
    [SerializeField] private float throwDistance;

    private Vector3 mouseScreenPos;
    public Vector3 mouseWorldPos;
    private Vector3 origin;
    private Vector3 dir;
    private RaycastHit hit;

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
            transform.DOLookAt(mouseGizmo.transform.localPosition, 0.3f); 
        }
        
        TridentLogic();

        if (Input.GetMouseButtonDown(0) && throwOn)
        {
            TridentThrow();
        }
    }

    private void FixedUpdate()
    {
        ThrowRaycast();
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish")) //Si l'objet touché est un clownfish
        {
            other.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f);

            other.transform.GetComponent<Fish>().isHit = true;
            other.transform.GetComponent<BoxCollider>().enabled = false; //Deactivate fish collider when hit
            
            uiManager.AddScoreFish(clownFishSO.addScore); //Add OverallScore
            uiManager.AddFishCounter(1); //Add FishCounter
            yield return new WaitForSeconds(5f);
            
            other.transform.DOScale(Vector3.zero, 0.3f);
            yield return new WaitForSeconds(0.3f);
            
            Instantiate(deadNemo, bucketPos[Random.Range(0, bucketPos.Length)].position, Quaternion.identity);
        }
        
        if (other.gameObject.CompareTag("Dori")) //Si l'objet touché est un Dori
        {
            other.transform.GetComponent<Fish>().isHit = true;
            other.transform.GetComponent<BoxCollider>().enabled = false; //Deactivate fish collider when hit
            other.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f);
            
            uiManager.AddScoreFish(doriSO.addScore); //Add OverallScore
            uiManager.AddFishCounter(1); //Add FishCounter
            yield return new WaitForSeconds(5f);
            
            other.transform.DOScale(Vector3.zero, 0.3f);
            other.transform.GetComponent<Fish>().isHit = false;
            //other.transform.GetComponent<BoxCollider>().enabled = true;
            //other.transform.GetComponent<BoxCollider>().isTrigger = false;
            other.transform.GetComponent<CapsuleCollider>().enabled = true;
            yield return new WaitForSeconds(0.3f);
            
            Instantiate(deadDori, bucketPos[Random.Range(0, bucketPos.Length)].position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
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
        if (Input.GetMouseButtonDown(1) && throwOff)
        {
            StartCoroutine(TridentRecall());
            //Debug.Log("TRIDENT IS BACK");
        }
        
    }

    IEnumerator TridentRecall()
    {
        throwOff = false;
        
        transform.DOMove(tridentPos, callBackTime);
        yield return new WaitForSeconds(callBackTime);
        
        tridentRb.velocity = Vector3.zero; //Kill velocity
        throwOn = true;
    }
    
    
    void TridentThrow()
    {
        if (throwOn)
        {
            throwOff = true;
            throwOn = false;
    
            tridentRb.AddForce((hit.point - transform.position) * explosionPower, ForceMode.Force);
            gameManager.LightScreenShake();

            tridentParticle.Play();
            tridentParticleFollow.Play();
        }
    }

    void ThrowRaycast()
    {
        mouseScreenPos = Input.mousePosition; //Donne les posX + posY de la souris en pixel
        mouseScreenPos.z = maxRange; //Ajout de la profondeur

        mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos); //Conversion Pixel en WorldPos

        mouseWorldPosGizmo.position = mouseWorldPos; //follow le transform de l'objet sur la mousePosWorld

        Vector3 origin = mainCam.transform.position;
        Vector3 dir = mouseWorldPos - origin; //Distance entre transform.position - mousePosWorld
        
        Debug.DrawRay(origin, dir.normalized * maxRange, Color.cyan);

        if (Physics.Raycast(origin, dir.normalized, out hit, maxRange))
        {
            if (hit.collider.CompareTag("Fish") || hit.collider.CompareTag("Dori") || hit.collider.CompareTag("Shark") || hit.collider.CompareTag("Blowfish"))
            {
                mouseWorldPosGizmo.position = origin + dir.normalized * hit.distance;
            }
        }
        else
        {
            hit.point = origin + dir.normalized * maxRange;
        }
    }

    
}
