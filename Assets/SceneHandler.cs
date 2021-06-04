using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
