using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scripts")]
    public static UIManager instance;
    
    private GameManager gameManager;
    [SerializeField] private FishSO fishSO;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI fishCounterText;
    [SerializeField] private TextMeshProUGUI overallScoreText;
    [SerializeField] private TextMeshProUGUI scoreSharkText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Values")] 
    [SerializeField] private float timeRemaining;
    [SerializeField] public int currentOverallScore;
    [SerializeField] public int currentSharkScore;
    [SerializeField] private int currentFishCaught;
    
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
                timerText.text = "" + (seconds + 1);
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

        //END GAME HERE

        Debug.Log("TIMER HAS ENDED");
        
        yield return new WaitForSeconds(1f);

        timeRemaining = 0;
    }

    public void AddFishCounter(int fishCaught)
    {
        currentFishCaught += fishCaught;
        fishCounterText.text = "x" + currentFishCaught; //Update FishCounter
        
        fishCounterText.DOTMPFontSize(200, 0.2f).OnComplete(() =>
        {
            fishCounterText.DOTMPFontSize(100, 0.1f);
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
