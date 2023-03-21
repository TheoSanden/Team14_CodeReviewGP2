using System.Collections;
using UnityEngine.VFX;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    [SerializeField] VisualEffectAsset vfxAsset;
    VisualEffect vfx;
    const float LERPTIME = 0.5f;
    const float MINSHIELDVALUE = 0.3f;
    [SerializeField] GameObject shieldObject;
    Material material;
    ShieldHealth shieldHealth;
    float lastShieldAmount = 0;
    bool noShield;
    void Start()
    {
        var mr = shieldObject.GetComponent<MeshRenderer>();
        material = Instantiate(mr.material);
        mr.material = material;
        if (!gameObject.TryGetComponent<ShieldHealth>(out shieldHealth))
        {
            Debug.LogWarning("This Gameobject does not contain ShieldHealth.", gameObject);
        }
        if (!this.TryGetComponent<VisualEffect>(out vfx))
        {
            this.gameObject.AddComponent<VisualEffect>();
        }
        shieldHealth.onShieldChange.AddListener(UpdateShieldAmount);
        lastShieldAmount = shieldHealth.ShieldBuffer;
        material.SetFloat("_ShiledHP", (float)shieldHealth.ShieldBuffer / shieldHealth.MaxShield.Value);
    }
    void UpdateShieldAmount(int amount)
    {
        if (amount > 0)
        {
            noShield = false;
        }
        if (!noShield)
        {
            StartCoroutine(LerpShield(amount));
        }
        if (amount == 0)
        {
            noShield = true;
        }
    }
    IEnumerator LerpShield(int target)
    {
        float timer = 0;
        float startValue = (lastShieldAmount == 0) ? 0 : MINSHIELDVALUE + ((1 - MINSHIELDVALUE) * (lastShieldAmount / shieldHealth.MaxShield.Value));
        float targetValue = MINSHIELDVALUE + ((1 - MINSHIELDVALUE) * ((float)target / shieldHealth.MaxShield.Value));
        float delta = targetValue - startValue;
        while (timer < LERPTIME)
        {
            float value = startValue + (delta * (timer / LERPTIME));
            // Debug.Log("Shield Lerp Value: " + value);
            material.SetFloat("_ShiledHP", value);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        lastShieldAmount = target;
        if (target == 0)
        {
            material.SetFloat("_ShiledHP", 0);
            Break();
        }
    }
    private void Break()
    {
        vfx.visualEffectAsset = vfxAsset;
        vfx.Play();
    }

    [ContextMenu("AddShield")]
    public void AddShield()
    {
        shieldHealth.ModifyShield(10);
    }
}
