using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new Modifier", menuName = "ScriptableObjects/Variables/Modifier")]
public class Modifier : ScriptableObject
{
    [SerializeField] float _startValue = 1;
    private float _value;

    public float Value
    {
        get => _value;
        set => _value = value;
    }
    public void Reset()
    {
        _value = _startValue;
    }
    void Awake()
    {
        Reset();
    }
}
