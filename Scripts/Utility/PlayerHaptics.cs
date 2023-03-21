using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Haptics), typeof(Health))]
public class PlayerHaptics : MonoBehaviour
{
    const int MAXDAMAGE = 4;
    Health health;
    Haptics haptics;
    private void Start()
    {
        health = this.GetComponent<Health>();
        haptics = this.GetComponent<Haptics>();

        health.onHealthChange_Delta.AddListener(CalcualateHapticStrenght);
    }
    public void CalcualateHapticStrenght(int damageDelta, Color col)
    {
        if (damageDelta < 0)
        {
            float hapticMod = Mathf.Clamp01((float)Mathf.Abs(damageDelta) / MAXDAMAGE);
            Debug.Log("HAPTICS MOD: " + hapticMod + ", DAMAGE DELTA: " + damageDelta);
            haptics.Rumble(hapticMod, hapticMod, 0.5f);
        }
        else if (damageDelta > 0)
        {
            haptics.Rumble(0.1f, 0.1f, 0.1f);
        }

    }
}
