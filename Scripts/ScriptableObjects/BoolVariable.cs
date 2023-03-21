using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "new BoolVariable", menuName = "ScriptableObjects/Variables/BoolVariable")]
public class BoolVariable : ScriptableObject
{
    UnityEvent<bool> onVariableChange;
    [SerializeField] private bool _value;
    public bool Value
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
