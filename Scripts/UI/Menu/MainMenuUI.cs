using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour
{
    
    public void OnStartClick()
    {
        SceneManager.LoadScene("_MainScene");
    }
    
    public void OnSelectClick()
    {
        Debug.Log("Select Characters");
    }
    
    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnControlClick()
    {
        SceneManager.LoadScene("CONTROLS");
    }
}
