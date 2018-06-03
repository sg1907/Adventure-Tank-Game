using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

public void PlayStoryMode()
    {
        SceneManager.LoadScene(1);
    }

public void PlaySurviveMode()
    {
        SceneManager.LoadScene(2);
    }

public void QuitGame()
    {
        Application.Quit();
    }
}
