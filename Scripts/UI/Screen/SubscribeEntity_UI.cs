using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscribeEntity_UI : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Canvas _canvas;
    [SerializeField] DamageNumberDisplay _numberDisplay;
    void Awake()
    {
        _numberDisplay.Subscribe(this.gameObject, _camera, _canvas);
    }
}
