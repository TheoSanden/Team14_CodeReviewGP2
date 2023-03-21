using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EntityHealthBar : EntityUi
{
    [SerializeField] Image healthFill, shieldFill;
    ShieldHealth shieldHealth;
    Health health;

    bool hasShieldHealth;

    public override void Subscribe(GameObject go, Camera camera, Canvas canvas)
    {
        hasShieldHealth = false;
        if (!go.TryGetComponent<ShieldHealth>(out shieldHealth))
        {
            if (!go.TryGetComponent<Health>(out health))
            {
                Debug.LogError("GameObject you're trying to subscribe has no health script attached.", go);
                return;
            }
        }
        else
        {
            hasShieldHealth = true;
        }

        if (hasShieldHealth)
        {
            shieldHealth.onHealthChange.AddListener(SetHealthFill);
            shieldHealth.onShieldChange.AddListener(SetShieldFill);
            SetShieldFill(shieldHealth.ShieldBuffer);
            SetHealthFill(shieldHealth.CurrentHealth);
        }
        else
        {
            health.onHealthChange.AddListener(SetHealthFill);
            SetHealthFill(health.CurrentHealth);
        }
        base.Subscribe(go, camera, canvas);
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        if (hasShieldHealth)
        {
            shieldHealth.onHealthChange.RemoveListener(SetHealthFill);
            shieldHealth.onShieldChange.RemoveListener(SetShieldFill);
        }
        else
        {
            health.onHealthChange.RemoveListener(SetHealthFill);
        }
    }

    private void SetHealthFill(int amount)
    {
        healthFill.fillAmount = (float)amount / ((hasShieldHealth) ? shieldHealth.MaxHealth.Value : health.MaxHealth.Value);
        Debug.Log(healthFill.fillAmount);
    }
    private void SetShieldFill(int amount)
    {
        shieldFill.fillAmount = (float)amount / shieldHealth.MaxShield.Value;
    }
}
