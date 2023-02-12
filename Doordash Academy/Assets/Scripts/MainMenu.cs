using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads a relevant scene and begins play.
    public void PlayGame()
    {
        AnalyticsManager.LogLevelLoad(name);
        SceneManager.LoadScene("HowToPlay");
    }

    // Loads a scene and begins play.
    public void PlayLevel(string name)
    {
        AnalyticsManager.LogLevelLoad(name);
        SceneManager.LoadScene(name);
    }

    // Terminates the application
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
