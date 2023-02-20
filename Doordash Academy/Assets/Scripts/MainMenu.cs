using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static bool isInitialized = false;

    // On launch
    public void Start()
    {
        if (!isInitialized) {
            PlayerPrefs.SetInt("tutorialCompleted", 0);
            PlayerPrefs.SetInt("money", 0);
            isInitialized = true;
        }
    }

    // Loads a relevant scene and begins play.
    public void PlayGame()
    {
        string currentLevel = PlayerPrefs.GetInt("tutorialCompleted", 0) == 0 ? "HowToPlay" : "City";
        AnalyticsManager.LogLevelLoad(currentLevel);
        SceneManager.LoadScene(currentLevel);
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
