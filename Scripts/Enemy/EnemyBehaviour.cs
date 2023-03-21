using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(AnimatorHandler))]
public class EnemyBehaviour : MonoBehaviour
{
    // true if in an uncancable animation
    protected bool isInteracting;

    protected GameObject[] players;

    public GameObject target;
    protected AnimatorHandler anim;

    public bool isAggro;
    private AggroBubble bubble;

    [SerializeField] protected Events.GameObjectEvent onDeathEvent;

    private void Awake()
    {

        anim = GetComponent<AnimatorHandler>();
        players = GameObject.FindGameObjectsWithTag("Player");

        Setup();

        bubble = GetComponentInChildren<AggroBubble>();
        if (bubble == null)
        {
            isAggro = true;
            StartAggro();
        }
        else
            bubble.Entered += SetAggro;

    }
    //
    private void OnEnable()
    {
        Spawn();
    }
    private void SetAggro()
    {
        isAggro = true;
        StartAggro();
    }

    private void Update()
    {
        if (isInteracting != anim.GetIsInteracting())
        {
            isInteracting = anim.GetIsInteracting();
            OnInteractChange();
        }
        if (isAggro)
            OnUpdate();
    }

    protected virtual void OnInteractChange() { }
    protected virtual void OnUpdate() { }
    protected virtual void Spawn() { }
    protected virtual void Setup() { }
    protected virtual void StartAggro() { }
    public virtual void OnDeath()
    {
        onDeathEvent?.Raise(this.gameObject);
    }
}
