using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum projectileStyle
{
    normal,
    big
}

[CreateAssetMenu(menuName = "ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public int Damage;
    public float Speed;
    public float LifeTime;

    public projectileStyle style;
}
