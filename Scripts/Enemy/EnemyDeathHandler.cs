using UnityEngine.VFX;
using UnityEngine;

[RequireComponent(typeof(Events.GameObjectEventListener))]
public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] VisualEffectAsset onDeathEffect;
    GameObjectPooler deathEffectPool;
    void Start()
    {
        if (!onDeathEffect)
        {
            Debug.Log("No visual effect referenced.", this.gameObject);
            return;
        }
        deathEffectPool = this.gameObject.AddComponent<GameObjectPooler>();
        GameObject poolObject = new GameObject();
        VisualEffect vfx = poolObject.AddComponent<VisualEffect>();
        vfx.visualEffectAsset = onDeathEffect;
        deathEffectPool.poolObject = poolObject;
        deathEffectPool.Populate(10);
    }
    public void HandleDeath(GameObject go)
    {
        Debug.Log(go.name);
        UnsubscribeUI(go);
        go.SetActive(false);

        if (onDeathEffect)
        {
            GameObject deathEffectObject = deathEffectPool.PopFor(3);
            deathEffectObject.transform.position = go.transform.position;
            deathEffectObject.GetComponent<VisualEffect>().Play();
        }
    }
    private void UnsubscribeUI(GameObject go)
    {
        if (EnemyUiManager.Instance != null)
        {
            EnemyUiManager.Instance.UnsubscribeGameObject(go);
        }
        else
        {
            Debug.LogError("No Instance of EnemyUIManager found");
        }
    }
}
