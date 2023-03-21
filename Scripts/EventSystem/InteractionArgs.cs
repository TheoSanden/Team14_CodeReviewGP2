using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InteractionArgs
{
    public enum InteractionType
    {
        global,
        entity,
    }
    public enum ElementType
    {
        fire,
        sludge,
        wind,
        radiation
    }
    //Entity Interaction
    public InteractionArgs(ElementType type1, ElementType type2, Transform referenceTransform)
    {
        this.type1 = type1;
        this.type2 = type2;
        this.transform = referenceTransform;
        interactionType = InteractionType.entity;
        position = Vector3.zero;
    }
    public InteractionArgs(ElementType type1, ElementType type2, Vector3 position)
    {
        this.type1 = type1;
        this.type2 = type2;
        this.position = position;
        interactionType = InteractionType.global;
        transform = null;
    }
    public InteractionType _InteractionType
    {
        get => interactionType;
    }
    public ElementType ElementType_1
    {
        get => type1;
    }
    public ElementType ElementType_2
    {
        get => type2;
    }
    public Transform TargetTransform
    {
        get => transform;
    }
    public Vector3 Position
    {
        get => position;
    }
    InteractionType interactionType;
    ElementType type1, type2;
    Transform transform;
    Vector3 position;
}
