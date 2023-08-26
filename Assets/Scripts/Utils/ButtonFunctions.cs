using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Reloads the currently loaded scene
    /// </summary>
    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    /// <summary>
    /// Loads Specified Scene
    /// </summary>
    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene(levelNum);
    }


    /// <summary>
    /// Loads Specified Scene
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
