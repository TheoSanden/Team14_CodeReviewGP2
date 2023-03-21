using UnityEngine.Events;
using UnityEngine;
[CreateAssetMenu(fileName = "new Vector3Variable", menuName = "ScriptableObjects/Variables/Vector3Variable")]
public class Vector3Variable : ScriptableObject
{
    UnityEvent<Vector3> onVariableChange;
    [SerializeField] private Vector3 _value;
    public Vector3 Value
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
