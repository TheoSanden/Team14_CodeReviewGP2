using UnityEngine.Events;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 100;
    [SerializeField] IntVariable maxHealth;
    [SerializeField] public UnityEvent<int> onHealthChange;
    [SerializeField] public UnityEvent onHealthZero;
    [SerializeField] public UnityEvent<int, Color> onHealthChange_Delta;

    public IntVariable MaxHealth
    {
        get => maxHealth;
    }
    public int CurrentHealth
    {
        get => health;
    }
    bool deltaEventInvoked = false;
    protected virtual void OnEnable()
    {
        health = maxHealth.Value;
    }
    public virtual void Apply(int amount)
    {
        if (!deltaEventInvoked) { onHealthChange_Delta?.Invoke(amount, Color.white); deltaEventInvoked = true; }
        if (amount == 0) return;
        health = (health + amount < 0) ? 0 : (health + amount > maxHealth.Value) ? maxHealth.Value : health + amount;
        if (health > 0) { onHealthChange?.Invoke(health); }
        else { onHealthZero?.Invoke(); }
        deltaEventInvoked = false;
    }
    public void Apply(int amount, Color color)
    {
        onHealthChange_Delta?.Invoke(amount, color);
        deltaEventInvoked = true;
        Apply(amount);
    }
}
