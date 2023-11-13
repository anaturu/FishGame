using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BlowfishBehaviour : MonoBehaviour
{
    
    private UIManager uiManager;
    private GameManager gameManager;
    private TridentBehaviour tridentManager;
    
    public SpriteRenderer backgroundImage;
    
    [SerializeField] private float deathSize;
    [SerializeField] private float powerCooldown;
    [SerializeField] private TextMeshProUGUI powerUpText;

    private void Start()
    {
        tridentManager = TridentBehaviour.instance;
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;


        backgroundImage = GameObject.Find("Gradient").GetComponent<SpriteRenderer>();
        powerUpText = GameObject.Find("PowerUpText").GetComponent<TextMeshProUGUI>();
        
        GameManager.instance.fishes.Add(gameObject); //Add to the list every fishes

    }

    private IEnumerator OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Trident"))
        {
            backgroundImage.DOColor(Color.yellow, 1f);
            gameManager.LightScreenShake();

            powerUpText.DOTMPFontSize(50f, 3f).SetEase(Ease.OutElastic);
            
            transform.DOScale(Vector3.one * deathSize, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                transform.DOScale(Vector3.zero * deathSize, 0.3f).SetEase(Ease.InBounce);
                
                tridentManager.callBackTime = 0.2f;
            });
            yield return new WaitForSeconds(powerCooldown);

            backgroundImage.DOColor(Color.cyan, 1f);
            powerUpText.DOTMPFontSize(0, 2f).SetEase(Ease.OutElastic);

            tridentManager.callBackTime = 1f;

        }
    }
}
