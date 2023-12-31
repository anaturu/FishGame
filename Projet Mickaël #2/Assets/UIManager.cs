using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [Header("Scripts")]
    public static UIManager instance;
    
    private TridentBehaviour tridentManager;
    private GameManager gameManager;
    [SerializeField] private FishSO fishSO;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI fishCounterText;
    [SerializeField] private TextMeshProUGUI overallScoreText;
    [SerializeField] private TextMeshProUGUI scoreSharkText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI sharkScoreEndGame;
    [SerializeField] private TextMeshProUGUI playerScoreEndGame;
    [SerializeField] private GameObject playerBucket;
    [SerializeField] private GameObject sharkBucket;
    [SerializeField] private GameObject shark;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject mouseGizmoRedBall;


    [Header("Values")] 
    [SerializeField] private float timeRemaining;
    [SerializeField] public int currentOverallScore;
    [SerializeField] public int currentSharkScore;
    [SerializeField] private int currentFishCaught;
    [SerializeField] private float distanceEndCam;
    
    [SerializeField] private float timeDecalCamera;
    [SerializeField] private float timeEmptyBucket;
    
    [SerializeField] private bool timerIsRunning;
    
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
        tridentManager = TridentBehaviour.instance;
        gameManager = GameManager.instance;
        
        timerIsRunning = true; //Start timer
    }


    private void Update()
    {
        if (timerIsRunning)
        {
            
            if (timeRemaining >= 0f)
            {
                timeRemaining -= Time.deltaTime; //Start timer
                float seconds = Mathf.FloorToInt(timeRemaining % 60); //Conversion en secondes
                timerText.text = (seconds + 1) + "s";
            }
            else
            {
                StartCoroutine(TimerEnded());
                timeRemaining = 0;
                timerIsRunning = false; //flag
            }
        }
        
    }

    private IEnumerator TimerEnded()
    {
        //EndText
        endText.DOTMPFontSize(150, 2f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            endText.DOTMPFontSize(0, 2f).SetEase(Ease.InElastic);
        });
        Debug.Log("TIMER HAS ENDED");
        yield return new WaitForSeconds(1f);
        timeRemaining = 0;
        
        //END GAME HERE
        
        //Deactivate UI
        inGameUI.SetActive(false);
        
        //Deactivate Control Player
        tridentManager.enabled = false; 
        
        //Rigidbody Fix Drag
        foreach (var fishInBucket in gameManager.fishesInBucket)
        {
            fishInBucket.GetComponent<Rigidbody>().drag = 0;
            Debug.Log("How many fishes ?");
        }
        
        //Deactivate Shark
        shark.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            shark.GetComponent<SharkBehaviour>().enabled = false;
        });
        
        //Despawn all living fishes
        foreach (var fish in gameManager.fishes)
        {
            fish.transform.DOScale(Vector3.zero, Random.Range(0.2f, 1)).SetEase(Ease.InElastic).OnComplete(() =>
            {
                fish.SetActive(false);
            });
        }

        mouseGizmoRedBall.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(2f);

        //Cam
        gameManager.cam.transform.DOMove(new Vector3(distanceEndCam, 1, -20), timeDecalCamera).SetEase(Ease.Linear);
        
        yield return new WaitForSeconds(timeDecalCamera);
        
        yield return new WaitForSeconds(2f);

        
        //Bucket
        playerBucket.transform.DOScale(Vector3.zero, timeEmptyBucket);
        yield return new WaitForSeconds(timeEmptyBucket);

        playerScoreEndGame.text = "x" + currentFishCaught;

        playerScoreEndGame.DOTMPFontSize(200, 0.5f).OnComplete(() =>
        {
            playerScoreEndGame.DOTMPFontSize(100, 0.1f);
        });
        yield return new WaitForSeconds(2f);


        sharkBucket.transform.transform.DOScale(Vector3.zero, timeEmptyBucket);
        yield return new WaitForSeconds(timeEmptyBucket);
        
        sharkScoreEndGame.text = "x" + currentSharkScore;

        sharkScoreEndGame.DOTMPFontSize(200, 0.5f).OnComplete(() =>
        {
            sharkScoreEndGame.DOTMPFontSize(100, 0.1f);
        });
        yield return new WaitForSeconds(2f);

        sharkScoreEndGame.enabled = false;
        playerScoreEndGame.enabled = false;
        Cursor.visible = true;

        if (currentFishCaught >= currentSharkScore) //Si player a plus de poissons que le requin
        {
            //Win Player
            endMenu.SetActive(true);
            loseText.enabled = false;
            winText.enabled = true;
            
            Debug.Log("PLAYER HAS WON");
        }
        else
        {
            //Win Shark
            endMenu.SetActive(true);
            winText.enabled = false;
            loseText.enabled = true;

            Debug.Log("SHARK HAS WON");
        }
        
    }

    public void AddFishCounter(int fishCaught)
    {
        currentFishCaught += fishCaught;
        fishCounterText.text = "x" + currentFishCaught; //Update FishCounter
        
        fishCounterText.DOTMPFontSize(200, 0.2f).OnComplete(() =>
        {
            fishCounterText.DOTMPFontSize(100, 0.1f);
        });
        
        fishCounterText.DOColor(Color.green, 1f).OnComplete(() =>
        {
            fishCounterText.DOColor(Color.white, 1f);
        });
    }
    public void AddScoreFish(int clownFishScore)
    {
        currentOverallScore += clownFishScore;
        overallScoreText.text = currentOverallScore + ""; //Update OverallScore with clownFishScore
        
        overallScoreText.DOTMPFontSize(200, 0.1f).OnComplete(() =>
        {
            overallScoreText.DOTMPFontSize(100, 0.1f);
        });
    }
    public void AddScoreShark(int sharkScore)
    {
        currentSharkScore += sharkScore;
        scoreSharkText.text = "x" + currentSharkScore; //Update OverallScore with clownFishScore
        
        scoreSharkText.DOTMPFontSize(200, 0.3f).OnComplete(() =>
        {
            scoreSharkText.DOTMPFontSize(100, 0.1f);
        });
        
        scoreSharkText.DOColor(Color.red, 1f).OnComplete(() =>
        {
            scoreSharkText.DOColor(Color.white, 1f);
        });
    }

    public void RemoveScore(int penaltyScore)
    {
        currentOverallScore -= penaltyScore;
        overallScoreText.text = currentOverallScore + "";

        overallScoreText.DOTMPFontSize(200, 0.2f).OnComplete(() =>
        {
            overallScoreText.DOTMPFontSize(70, 0.1f);
        });
        
        overallScoreText.DOColor(Color.red, 1f).OnComplete(() =>
        {
            overallScoreText.DOColor(Color.white, 1f);
        });
        
    }
    
    
    
}
