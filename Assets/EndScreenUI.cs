using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    SceneHandler sceneHandler;
    
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] string winConditionString = "You win!";
    [SerializeField] string loseConditionString = "Try again!";

    void Awake()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();      
    }

    public void PlayAgain()
    {
        sceneHandler.RestartScene();
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void SetPlayerWins(bool playerWins)
    {
        if (playerWins)
        {
            winText.text = winConditionString;
        }
        else
        {
            winText.text = loseConditionString;
        }
    }
}
