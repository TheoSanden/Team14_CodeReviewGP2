using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class SludgePuddleExplosion : MonoBehaviour
{
    [SerializeField] Events.InteractionEvent onInteractionEvent;
    [SerializeField] FloatVariable detonationRadius;
    [SerializeField] IntVariable detonationDamage;
    [SerializeField] FloatVariable detonationChainDelay;
    [SerializeField] LayerMask interactionLayer;
    public UnityEvent onDetonate;
    /*#region TestFunctionality
    //Test Functionality
    SpriteRenderer sr;
    bool hasDetonated = false;
    float detonationTime = 0.2f;
    float gizmoRadius = 0;
     private void OnDrawGizmos()
     {
         Gizmos.matrix = this.transform.localToWorldMatrix;
         Gizmos.color = Color.red;
         if (hasDetonated)
         {
             Gizmos.DrawSphere(Vector3.zero, gizmoRadius);
         }
     }

     IEnumerator AnimateGizmoSphere()
     {
         hasDetonated = true;
         float timer = gizmoRadius = 0;
         while (timer <= detonationTime)
         {
             gizmoRadius = (timer / detonationTime) * detonationRadius.Value;
             timer += Time.deltaTime;
             yield return new WaitForEndOfFrame();
         }
         gizmoRadius = detonationRadius.Value;
     }
     #endregion*/
    [ContextMenu("Test Detonate")]
    public void Detonate()
    {
        HandleReactions();
        onDetonate?.Invoke();
    }
    /* private void TriggerDetonationVisual()
     {
         if (!sr) { sr = GetComponent<SpriteRenderer>(); }
         sr.enabled = true;
         CoroutineHelper.SetAfterSeconds<bool>((result => sr.enabled = result), false, 0.2f);
     }*/
    private void HandleReactions()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, detonationRadius.Value, interactionLayer, QueryTriggerInteraction.Collide);
        //Collider[] chain = hits[1..hits.Length];
        foreach (Collider collider in hits)
        {
            if (collider.CompareTag("Ability"))
            {
                PuddleFunctionality puddle = collider.gameObject.GetComponent<PuddleFunctionality>();
                if (puddle != null && puddle.ElementType == InteractionArgs.ElementType.sludge)
                {
                    InteractionArgs args = new InteractionArgs(InteractionArgs.ElementType.fire, InteractionArgs.ElementType.sludge, collider.transform.position);
                    StartCoroutine(DelayedInteractionEvent(args, 0.1f));
                    collider.enabled = false;
                    Destroy(collider.transform.gameObject);
                }
            }
            if (collider.CompareTag("Enemy"))
            {
                Health health;
                bool success = collider.TryGetComponent<Health>(out health);
                if (success) { health.Apply(-detonationDamage.Value); }

                EnemyMovement enemyMovement;
                success = collider.TryGetComponent<EnemyMovement>(out enemyMovement);
                if (success)
                {
                    Vector3 direction = collider.gameObject.transform.position - this.transform.position;
                    enemyMovement.ApplyKnockback(direction, 1);
                }
            }
        }
    }
    IEnumerator DelayedInteractionEvent(InteractionArgs args, float delay)
    {
        yield return new WaitForSeconds(delay);
        onInteractionEvent.Raise(args);
    }
    /*  void StartDetonationAnimation()
      {
          StartCoroutine(AnimateGizmoSphere());
      }*/

}
