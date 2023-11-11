using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scripts")]
    public static UIManager instance;
    [SerializeField] private FishSO fishSO;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI fishCounterText;
    [SerializeField] private TextMeshProUGUI overallScoreText;
    [SerializeField] private TextMeshProUGUI scoreSharkText;
    
    [Header("Values")]
    [SerializeField] public int currentOverallScore;
    [SerializeField] public int currentSharkScore;
    [SerializeField] private int currentFishCaught;
    
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
    void Start()
    {
        fishCounterText.text = "0";
    }

    public void AddFishCounter(int fishCaught)
    {
        currentFishCaught += fishCaught;
        fishCounterText.text = currentFishCaught + ""; //Update FishCounter
        
        fishCounterText.DOTMPFontSize(150, 0.2f).OnComplete(() =>
        {
            fishCounterText.DOTMPFontSize(100, 0.1f);
        });
    }
    public void AddScoreFish(int clownFishScore)
    {
        currentOverallScore += clownFishScore;
        overallScoreText.text = currentOverallScore + ""; //Update OverallScore with clownFishScore
        
        overallScoreText.DOTMPFontSize(150, 0.1f).OnComplete(() =>
        {
            overallScoreText.DOTMPFontSize(100, 0.1f);
        });
    }
    public void AddScoreShark(int sharkScore)
    {
        currentSharkScore += sharkScore;
        scoreSharkText.text = currentSharkScore + ""; //Update OverallScore with clownFishScore
        
        scoreSharkText.DOTMPFontSize(100, 0.1f).OnComplete(() =>
        {
            scoreSharkText.DOTMPFontSize(70, 0.1f);
        });
    }

    public void RemoveScore(int penaltyScore)
    {
        currentOverallScore -= penaltyScore;
        overallScoreText.text = currentOverallScore + "";

        overallScoreText.DOTMPFontSize(100, 0.2f).OnComplete(() =>
        {
            overallScoreText.DOTMPFontSize(70, 0.1f);
        });
        
        overallScoreText.DOColor(Color.red, 1f).OnComplete(() =>
        {
            overallScoreText.DOColor(Color.white, 1f);
        });
        
    }
    
    
    
}
