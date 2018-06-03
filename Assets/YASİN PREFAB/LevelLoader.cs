using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public void PlayStoryMode()
    {
        SceneManager.LoadScene(1);
    }

    public void PlaySurviveMode()
    {
        SceneManager.LoadScene(2);
    }

}

