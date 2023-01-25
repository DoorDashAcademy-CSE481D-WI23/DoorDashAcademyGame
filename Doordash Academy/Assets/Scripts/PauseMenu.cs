using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenu;

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
        gameIsPaused = false;
    }

    public void Pause ()
    {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
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
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
