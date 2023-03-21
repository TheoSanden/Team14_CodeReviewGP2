using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldHealth : Health
{
    private int shieldBuffer = 0;
    [Range(0, 1)]
    [SerializeField] float startMaxShield;
    [SerializeField] public UnityEvent<int> onShieldChange;
    [SerializeField] IntVariable maxShield;
    [SerializeField] private bool enableDepletion = true;
    [SerializeField] private float depletionRateInSeconds = 2;

    private float shieldDepletionBuffer;
    public int ShieldBuffer
    {
        get => shieldBuffer;
    }
    public IntVariable MaxShield
    {
        get => maxShield;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        shieldBuffer = Mathf.RoundToInt(maxShield.Value * startMaxShield);
    }
    private void Update()
    {
        if (enableDepletion && shieldBuffer > 0)
        {
            shieldDepletionBuffer += (maxShield.Value / depletionRateInSeconds) * Time.deltaTime;
            if (shieldDepletionBuffer > 1)
            {
                shieldDepletionBuffer = 0;
                ModifyShield(-1);
            }
        }
    }

    public override void Apply(int amount)
    {
        if (amount == 0) { return; }


        //If the damage is greater than the shield aka shield + -amount then we subtract that much from the player 
        if (shieldBuffer + amount < 0)
        {
            base.Apply(shieldBuffer + amount);
            shieldBuffer = 0;
            onShieldChange.Invoke(0);
            return;
        }
        //If we're trying to heal, we dont care about the shield
        else if (amount > 0)
        {
            base.Apply(amount);
            return;
        }
        //If we're taking damage then we subtract it from the health instead. Remember: amount will be negative when were taking damage
        else
        {
            ModifyShield(amount);
        }

        /* int overkill = amount;

         overkill = (shield + amount >= 0) ? 0 : shield + amount;
         shield = (shield + amount < 0) ? 0 : (shield + amount > maxShield.Value) ? maxShield.Value : shield + amount;

         return overkill;
         */

    }
    public void DamageShield(int amount)
    {
        amount = Mathf.Abs(amount);
        shieldBuffer = (shieldBuffer - amount < 0) ? 0 : shieldBuffer - amount;
        onShieldChange?.Invoke(shieldBuffer);
        onHealthChange_Delta?.Invoke(amount, Color.cyan);
    }
    public void SetShield()
    {
        shieldBuffer = maxShield.Value;
        onShieldChange?.Invoke(shieldBuffer);
    }
    public void ModifyShield(int amount)
    {
        shieldBuffer = (shieldBuffer + amount < 0) ? 0 : (shieldBuffer + amount > maxShield.Value) ? maxShield.Value : shieldBuffer + amount;
        onShieldChange?.Invoke(shieldBuffer);
    }
}
