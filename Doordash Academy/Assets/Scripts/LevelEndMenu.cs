using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndMenu : MonoBehaviour
{
    public GameObject levelEndMenu;
    public GameObject levelUI;

    public void LevelEnd()
    {
        Time.timeScale = 0.0f;
        levelEndMenu.SetActive(true);
        levelUI.SetActive(false);
    }

    // Returns to the main menu
    public void QuitLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

}
