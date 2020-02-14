using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Werkt");
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {

    }

    public void Controls()
    {

    }

    public void Quit()
    {
        Debug.Log("Werkt");
        Application.Quit();
    }
}
