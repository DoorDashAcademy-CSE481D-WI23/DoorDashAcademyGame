using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool mapIsOpen = false;

    public GameObject pauseMenu;
    public GameObject levelUI;
    public GameObject fullMap;

    // Update is called once per frame
    void Update()
    {
        // assumes these states are mutually exclusive
        if (gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Resume();
        }
        else if (mapIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
                CloseMap();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();

            if (Input.GetKeyDown(KeyCode.M))
                OpenMap();
        }
    }

    public void CloseMap ()
    {
        Time.timeScale = 1.0f;
        fullMap.SetActive(false);
        // levelUI.SetActive(true);
        mapIsOpen = false;
    }

    public void OpenMap ()
    {
        Time.timeScale = 0.0f;
        fullMap.SetActive(true);
        // levelUI.SetActive(false);
        mapIsOpen = true;
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
