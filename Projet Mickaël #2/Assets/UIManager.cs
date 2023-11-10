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
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("Values")]
    [SerializeField] public int currentOverallScore;
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
        
        fishCounterText.DOTMPFontSize(56, 0.2f).OnComplete(() =>
        {
            fishCounterText.DOTMPFontSize(36, 0.1f);
        });
    }
    public void AddScoreFish(int clownFishScore)
    {
        currentOverallScore += clownFishScore;
        scoreText.text = currentOverallScore + ""; //Update OverallScore with clownFishScore
        
        scoreText.DOTMPFontSize(76, 0.1f).OnComplete(() =>
        {
            scoreText.DOTMPFontSize(36, 0.1f);
        });
    }
    
}
