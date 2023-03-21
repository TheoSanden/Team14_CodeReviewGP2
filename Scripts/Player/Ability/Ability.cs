using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using Buffs;

public class Ability : MonoBehaviour
{
    [SerializeField] protected Events.InteractionEvent onInteractionEvent;
    protected InteractionArgs.ElementType elementType;
    
    public InteractionArgs.ElementType ElementType
    {
        get => elementType;
    }

    void OnInteraction(InteractionArgs.ElementType type)
    {
        InteractionArgs args = new InteractionArgs(elementType, type, Vector3.zero);
        onInteractionEvent.Raise(args);
    }
    protected void HandleInteractions(Transform other)
    {
        BuffManager buffManager;
        switch (other.tag)
        {
            case "Player":
                if (other.TryGetComponent<BuffManager>(out buffManager))
                {
                    HandlePlayerInteractions(buffManager);
                }
                break;
            case "Enemy":
                if (other.TryGetComponent<BuffManager>(out buffManager))
                {
                    HandleEnemyInteractions(buffManager);
                }
                break;
            case "Ability":
                Ability ability;
                if (other.TryGetComponent<Ability>(out ability))
                {
                    HandleAbilityInteractions(ability);
                }
                break;
        }
    }
    private void HandlePlayerInteractions(BuffManager buffmanager)
    {
        Buffs.Buff buff;
        if (buffmanager.TryGetBuff(out buff))
        {
            InteractionArgs.ElementType type = buff.ElementType;
            switch (type)
            {
                case InteractionArgs.ElementType.fire:
                    break;
                case InteractionArgs.ElementType.wind:
                    break;
                case InteractionArgs.ElementType.radiation:
                    switch (elementType)
                    {
                        case InteractionArgs.ElementType.radiation: buffmanager.Refresh(); break;
                    }
                    break;
                case InteractionArgs.ElementType.sludge:
                    switch (elementType)
                    {
                        case InteractionArgs.ElementType.sludge: buffmanager.Refresh(); break;
                    }
                    break;
            }
            return;
        }
        switch (elementType)
        {
            case InteractionArgs.ElementType.fire:
                break;
            case InteractionArgs.ElementType.wind:
                break;
            case InteractionArgs.ElementType.radiation:
                buffmanager.Apply(Buffs.BuffType.RADIATION_HOT);
                break;
            case InteractionArgs.ElementType.sludge:
                buffmanager.Apply(Buffs.BuffType.SLUDGE_SPEEDBUFF);
                break;
        }

    }
    private void HandleEnemyInteractions(BuffManager buffmanager)
    {
        Buffs.Buff buff;
        if (buffmanager.TryGetBuff(out buff))
        {
            InteractionArgs.ElementType type = buff.ElementType;
            switch (type)
            {
                case InteractionArgs.ElementType.fire:
                    switch (elementType)
                    {
                        case InteractionArgs.ElementType.sludge:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.wind:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.radiation:
                            buffmanager.Clear();
                            buffmanager.Apply(Buffs.BuffType.RADIATION_DOT);
                            break;
                        case InteractionArgs.ElementType.fire:
                            buffmanager.Refresh();
                            break;
                    }
                    break;
                case InteractionArgs.ElementType.wind:
                    switch (elementType)
                    {
                        case InteractionArgs.ElementType.fire:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.radiation:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.sludge:
                            buffmanager.Clear();
                            buffmanager.Apply(Buffs.BuffType.SLUDGE_SPEEDDEBUFF);
                            break;
                        case InteractionArgs.ElementType.wind:
                            buffmanager.Refresh();
                            break;
                    }
                    break;
                case InteractionArgs.ElementType.radiation:
                    switch (elementType)
                    {
                        case InteractionArgs.ElementType.wind:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.sludge:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.fire:
                            buffmanager.Clear();
                            buffmanager.Apply(Buffs.BuffType.FIRE_DOT);
                            break;
                        case InteractionArgs.ElementType.radiation:
                            buffmanager.Refresh();
                            break;
                    }
                    break;
                case InteractionArgs.ElementType.sludge:
                    switch (elementType)
                    {
                        case InteractionArgs.ElementType.fire:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.radiation:
                            buffmanager.Clear();
                            onInteractionEvent?.Raise(new InteractionArgs(elementType, type, buffmanager.transform.position));
                            break;
                        case InteractionArgs.ElementType.wind:
                            buffmanager.Clear();
                            buffmanager.Apply(Buffs.BuffType.STORM_DOT);
                            break;
                        case InteractionArgs.ElementType.sludge:
                            buffmanager.Refresh();
                            break;
                    }
                    break;
            }
            return;
        }
        switch (elementType)
        {
            case InteractionArgs.ElementType.fire:
                buffmanager.Apply(Buffs.BuffType.FIRE_DOT);
                break;
            case InteractionArgs.ElementType.wind:
                buffmanager.Apply(Buffs.BuffType.STORM_DOT);
                break;
            case InteractionArgs.ElementType.radiation:
                buffmanager.Apply(Buffs.BuffType.RADIATION_DOT);
                break;
            case InteractionArgs.ElementType.sludge:
                buffmanager.Apply(Buffs.BuffType.SLUDGE_SPEEDDEBUFF);
                break;
        }
    }
    private void HandleAbilityInteractions(Ability ability)
    {
        switch (elementType)
        {
            case InteractionArgs.ElementType.fire:
                switch (ability.ElementType)
                {
                    case InteractionArgs.ElementType.wind:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, ability.transform.position));
                        Destroy(ability.gameObject);
                        break;
                }
                break;
            case InteractionArgs.ElementType.wind:
                switch (ability.ElementType)
                {
                    case InteractionArgs.ElementType.fire:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, ability.transform.position));
                        break;
                    case InteractionArgs.ElementType.radiation:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, ability.transform.position));
                        break;
                }
                break;
            case InteractionArgs.ElementType.radiation:
                switch (ability.elementType)
                {
                    case InteractionArgs.ElementType.wind:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, this.transform.position));
                        break;
                    case InteractionArgs.ElementType.sludge:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, ability.transform.position));
                        break;
                }
                break;
            case InteractionArgs.ElementType.sludge:
                switch (ability.ElementType)
                {
                    case InteractionArgs.ElementType.fire:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, this.transform.position));
                        break;
                    case InteractionArgs.ElementType.radiation:
                        onInteractionEvent?.Raise(new InteractionArgs(elementType, ability.elementType, ability.transform.position));
                        break;
                }
                break;
        }
    }
}
