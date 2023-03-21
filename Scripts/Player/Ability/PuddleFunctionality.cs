using System;
using System.Collections;
using System.Collections.Generic;
using Buffs;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PuddleFunctionality : Ability
{
    [SerializeField] private GameObjectPooler _pool;
    [SerializeField] private FloatVariable _lifeTime;
    private float _count = 0;



    void Update()
    {
        SelfDespawn();
    }

    protected void PuddleInterationDespawn(Transform other)
    {
        BuffManager buffManager;
        Buffs.Buff buff;
        switch (other.tag)
        {
            case "Enemy":
                if (other.gameObject.TryGetComponent<BuffManager>(out buffManager))
                {
                    if (buffManager.TryGetBuff(out buff))
                    {
                        if (ElementCheck(buff.ElementType))
                        {
                            Destroy(this.gameObject);
                        }
                    }
                }
                break;
            case "Ability":
                Ability ability;
                if (other.gameObject.TryGetComponent<Ability>(out ability))
                {
                    if (ElementCheck(ability.ElementType))
                    {
                        Destroy(this.gameObject);
                    }

                }
                break; 
        }
    }

    private bool ElementCheck(InteractionArgs.ElementType thatType)
    {
        bool interaction = true;
        InteractionArgs.ElementType thisType = this.elementType;
        
        if ((thisType == InteractionArgs.ElementType.fire || thisType == InteractionArgs.ElementType.radiation) && (thatType == InteractionArgs.ElementType.fire || thatType == InteractionArgs.ElementType.radiation))
        {
            interaction = false;
        }
        else if ((thisType == InteractionArgs.ElementType.wind || thisType == InteractionArgs.ElementType.sludge) && (thatType == InteractionArgs.ElementType.wind || thatType == InteractionArgs.ElementType.sludge))
        {
            interaction = false;
        }
        
        return interaction;
    }
    
    private void SelfDespawn()
    {
        _count += Time.deltaTime;
        if (_count >= _lifeTime.Value)
        {
            Destroy(this.gameObject);
        }
    }
}
