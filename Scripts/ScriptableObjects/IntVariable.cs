using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "new IntVariable", menuName = "ScriptableObjects/Variables/IntVariable")]
public class IntVariable : ScriptableObject
{
    UnityEvent<int> onVariableChange;
    [SerializeField] private int _value;
    public int Value
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
