using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public GameObject audioCue;

    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
            Debug.Log("MORE THAN ONE INSTANCE OF THIS SINGLETON HAS BEEN SPAWNED");
        }
        else
        {
            instance = this;
        }
    }
    
    public enum SFXType
    {
        explosion,
        hit,
        powerUp,
        goodEvent,
        badEvent,
        countdown
    }
    
    public AudioClip[] explosion, hit, powerUp, goodEvent, badEvent, countDown;

    private void Start()
    {
        
    }

    public void SpawnAudio(SFXType type, bool randomPitch, float pitch = 1, int index = 0, float volume = 1f)
    {
        GameObject newSound = Instantiate(audioCue, transform.position, Quaternion.identity);
        AudioSource newSoundAS = newSound.GetComponent<AudioSource>();

        switch (type)
        {
            case SFXType.explosion:
                newSoundAS.clip = explosion[index];
                break;
            case SFXType.hit:
                newSoundAS.clip = hit[index];
                break;
            case SFXType.powerUp:
                newSoundAS.clip = powerUp[index];
                break;
            case SFXType.goodEvent:
                newSoundAS.clip = goodEvent[index];
                break;
            case SFXType.badEvent:
                newSoundAS.clip = badEvent[index];
                break;
            case SFXType.countdown:
                newSoundAS.clip = countDown[index];
                break;
        }

        newSoundAS.volume = volume;

        if (randomPitch)
        {
            newSoundAS.pitch = Random.Range(1-pitch, 1+pitch);
        }
        else
        {
            newSoundAS.pitch = pitch;
        }
        
        newSoundAS.Play();
    }
}
