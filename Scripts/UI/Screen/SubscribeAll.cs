using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is just a test script 
public class SubscribeAll : MonoBehaviour
{
    [SerializeField] GameObject entityUI;
    [SerializeField] Canvas canvas;
    [SerializeField] Camera _camera;
    [SerializeField] Transform parent;
    private void Start()
    {
        Subscribe();
    }
    public void Subscribe()
    {
        StartCoroutine(DelayedSubscribe());
    }
    IEnumerator DelayedSubscribe()
    {
        yield return new WaitUntil(() => EnemyUiManager.Instance != null);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in enemies)
        {
            EnemyUiManager.Instance.SubscribeGameObject(go, true, true, true);
        }
    }
}

