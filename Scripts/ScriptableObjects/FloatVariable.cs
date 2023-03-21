using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "new FloatVariable", menuName = "ScriptableObjects/Variables/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    UnityEvent<float> onVariableChange;
    [SerializeField] private float _value;
    public float Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            onVariableChange?.Invoke(_value);
        }
    }
}
