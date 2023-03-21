using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new Color", menuName = "ScriptableObjects/Color")]
public class PersistentColor : ScriptableObject
{
    public Color color;
}
