using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
   public GameObject pauseMenu;
   public GameObject optionsMenu;
   public bool isPaused;
   public bool isOptions;

   private void Update()
   {
      if (!isOptions) //Si le menu Options est fermé
      {
         if (Input.GetKeyDown(KeyCode.Escape))
         {
            if (isPaused) //Si le menu Pause est ouvert
            {
               ButtonResumeGame(); //Ferme le menu Pause
            }
            else if (!isPaused) //Si le menu Pause est fermé
            {
               PauseGame(); //Ouvre le menu Pause
            }
         } 
      }

      if (isOptions) //Si le menu Options est ouvert
      {
         if (Input.GetKeyDown(KeyCode.Escape))
         {
            if (!isPaused) //Si le menu Pause est fermé
            {
               ButtonBack(); //Retourne au menu Pause
            }
            else //Si le menu Pause est ouvert
            {
               ButtonResumeGame();
            }
         }
      }
   }
   
   public void PauseGame()
   {
      pauseMenu.SetActive(true); //Active Menu Pause
      Time.timeScale = 0f;
      Debug.Log("PAUSE");
        
      isPaused = true;
   }
   
   public void ButtonResumeGame()
   {
      pauseMenu.SetActive(false); //Désactive Menu Pause
      Time.timeScale = 1f;
      Cursor.visible = false;

      isPaused = false;
   }
   
   public void ButtonOptionsMenu()
   {
      pauseMenu.SetActive(false); //Désactive Menu Pause
      optionsMenu.SetActive(true); //Active Menu Options

      isOptions = true;
      isPaused = false;

   }
   
   public void ButtonBack()
   {
      optionsMenu.SetActive(false); //Désactive Menu Options
      pauseMenu.SetActive(true); //Active Menu Pause

      isOptions = false;
      isPaused = true;
   }
   
   public void ButtonMainMenu()
   {
      Time.timeScale = 1f;
      Cursor.visible = true;

      SceneManager.LoadScene("MainMenu");
   }
   public void ButtonPlayAgain()
   {
      SceneManager.LoadScene("PlayScene");
   }

   #region MainMenu

   public void PlayButton()
   {
      SceneManager.LoadScene("PlayScene");
      Debug.Log("PLAY THE GAME");
   }
   
   public void OptionsButton()
   {
      
      Debug.Log("MENUS IS OPEN");
   }
   
   public void QuitButton()
   {
      Application.Quit();
      Debug.Log("PLAYER HAS LEFT THE GAME");
   }

   #endregion
  
}
