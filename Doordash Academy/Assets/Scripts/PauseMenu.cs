using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject levelUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume ()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        levelUI.SetActive(true);
        gameIsPaused = false;
    }

    public void Pause ()
    {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
        levelUI.SetActive(false);
        gameIsPaused = true;
    }

    // Returns to the main menu
    public void QuitLevel()
    {
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
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
