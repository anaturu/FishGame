using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlowfishBehaviour : MonoBehaviour
{
    [SerializeField] private float deathSize;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = UIManager.instance;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Trident"))
        {
            transform.DOScale(Vector3.one * deathSize, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                transform.DOScale(Vector3.zero * deathSize, 3f).SetEase(Ease.InBounce);
                uiManager.RemoveScore(20);
            });
        }
    }
}
