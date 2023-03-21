using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUiManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private Transform UiParent;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject damageNumberUI_Prefab, healthbarUI_Prefab;
    Dictionary<int, EntityUi[]> entityUiDictionary = new Dictionary<int, EntityUi[]>();
    Dictionary<EntityUi, GameObject> activeDamageNumberUI = new Dictionary<EntityUi, GameObject>();
    Dictionary<EntityUi, GameObject> activeHealthNumberUI = new Dictionary<EntityUi, GameObject>();

    private GameObjectPooler damageNumberPooler;
    private GameObjectPooler healthbarPooler;
    bool initialized = false;
    static private EnemyUiManager instance;
    static public EnemyUiManager Instance
    {
        get => instance;
    }
    private void Start()
    {
        Validate();
        if (!initialized) return;

        instance = this;
        UiParent = Instantiate(new GameObject(), transform).transform;

        damageNumberPooler = gameObject.AddComponent<GameObjectPooler>();
        damageNumberPooler.poolObject = damageNumberUI_Prefab;
        damageNumberPooler.Populate(20);

        healthbarPooler = gameObject.AddComponent<GameObjectPooler>();
        healthbarPooler.poolObject = healthbarUI_Prefab;
        healthbarPooler.Populate(20);
    }
    public void SubscribeGameObject(GameObject go, bool hasHealthBar, bool hasBuffManager, bool hasDamageNumbers)
    {
        if (!initialized)
        {
            Debug.LogError("Not Initialized", gameObject);
        }
        List<EntityUi> UIObjects = new List<EntityUi>();
        if (hasHealthBar)
        {
            GameObject healthBarObject = healthbarPooler.Pop();
            healthBarObject.transform.SetParent(UiParent);
            EntityUi e_UI = healthBarObject.GetComponent<EntityUi>();
            e_UI.Subscribe(go, mainCamera, canvas);
            UIObjects.Add(e_UI);
            activeHealthNumberUI.Add(e_UI, healthBarObject);
        }
        if (hasBuffManager)
        {

        }
        if (hasDamageNumbers)
        {
            GameObject damageNumberObject = damageNumberPooler.Pop();
            damageNumberObject.transform.SetParent(UiParent);
            EntityUi e_UI = damageNumberObject.GetComponent<EntityUi>();
            e_UI.Subscribe(go, mainCamera, canvas);
            UIObjects.Add(e_UI);
            activeDamageNumberUI.Add(e_UI, damageNumberObject);
        }
        entityUiDictionary.Add(go.GetInstanceID(), UIObjects.ToArray());
    }
    //TODO enqueue the objects again
    public void UnsubscribeGameObject(GameObject go)
    {
        if (!entityUiDictionary.ContainsKey(go.GetInstanceID())) { return; }
        if (!initialized)
        {
            Debug.LogError("Not Initialized", gameObject);
        }
        foreach (EntityUi ui in entityUiDictionary[go.GetInstanceID()])
        {
            ui.Unsubscribe();
            EnqueueUI(ui);
        }
        entityUiDictionary.Remove(go.GetInstanceID());
    }
    private void EnqueueUI(EntityUi ui)
    {
        if (activeDamageNumberUI.ContainsKey(ui))
        {
            damageNumberPooler.Enqueue(activeDamageNumberUI[ui]);
            activeDamageNumberUI.Remove(ui);
            return;
        }
        else if (activeHealthNumberUI.ContainsKey(ui))
        {
            healthbarPooler.Enqueue(activeHealthNumberUI[ui]);
            activeHealthNumberUI.Remove(ui);
            return;
        }
        Debug.LogWarning("The EntityUi you're trying to enqueue is not registered.", this.gameObject);
    }
    private void Validate()
    {
        initialized = false;
        if (canvas == null)
        {
            Debug.LogError("No Canvas Referenced", gameObject);
            return;
        }
        if (mainCamera == null)
        {
            Debug.LogError("No Camera Referenced", gameObject);
            return;
        }
        if (!damageNumberUI_Prefab.GetComponent<DamageNumberDisplay>())
        {
            Debug.LogError("Prefab Does not contain component DamageNumberDisplay", damageNumberUI_Prefab);
            return;
        }
        if (!healthbarUI_Prefab.GetComponent<EntityHealthBar>())
        {
            Debug.LogError("Prefab Does not contain component EntityHealthBar", healthbarUI_Prefab);
            return;
        }
        initialized = true;
    }
}
