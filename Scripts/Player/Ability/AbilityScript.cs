using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityScript : MonoBehaviour
{
    public float _cdTimer;
    public bool _onCD;
    public virtual void UseAbility()
    {
        return;
        
    }

    public float CooldownTimer()
    {
        if (_onCD)
        {

            _cdTimer -= Time.deltaTime;
            if (_cdTimer <= 0)
            {
                _onCD = !_onCD;
            }
            return _cdTimer;
        }
        return 0;
    }
    
}
