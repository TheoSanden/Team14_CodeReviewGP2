using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenLoader : MonoBehaviour
{
    public GameObject vfx;
    public void StartEndScreen()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        vfx.SetActive(true);
        yield return new WaitForSeconds(2);
        LoadEndScene();
    }
    private void LoadEndScene()
    {
        SceneManager.LoadSceneAsync("EndScene");
    }
}

