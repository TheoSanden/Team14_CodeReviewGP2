using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleSender : MonoBehaviour
{
    [SerializeField] SubtitleScript script;
    void Start()
    {
        script.Initialize();
    }
    public void Send()
    {
        if (SubtitleManager.Instance)
        {
            SubtitleManager.Instance.Display(script);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Send();
        }
    }
}
