using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SubtitleTrigger : MonoBehaviour
{
    bool activated = false;
    [SerializeField] SubtitleScript script;
    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (other.transform.CompareTag("Player"))
        {
            if (SubtitleManager.Instance)
            {
                script.Initialize();
                activated = true;
                SubtitleManager.Instance.Display(script);
            }
        }
    }
}
