using System.Collections;
using UnityEngine;

public class DamageNumberDisplay : EntityUi
{
    const int INITIAL_POOL_SIZE = 20;
    const int NUMBER_DISPLAY_TIME = 1;
    GameObjectPooler objectPooler;
    Health health;
    [SerializeField] GameObject numberDisplayObject;
    public override void Awake()
    {
        base.Awake();
        if (!numberDisplayObject.GetComponent<DamageNumber>())
        {
            numberDisplayObject.AddComponent<DamageNumber>();
        }
        objectPooler = gameObject.AddComponent<GameObjectPooler>();
        objectPooler.poolObject = numberDisplayObject;
        objectPooler.Populate(INITIAL_POOL_SIZE);
    }
    public override void Subscribe(GameObject go, Camera camera, Canvas canvas)
    {
        base.Subscribe(go, camera, canvas);
        if (!_gameObject.TryGetComponent<Health>(out health))
        {
            Debug.LogError("Subscribed GameObject does not have a Health Component", _gameObject);
        }
        else
        {
            health.onHealthChange_Delta.AddListener(DisplayNumber);
        }
    }
    public void DisplayNumber(int damage, Color color)
    {
        if (!InRange()) { return; }
        damage = Mathf.Abs(damage);
        GameObject go = objectPooler.PopFor(DamageNumber.LIFETIME);
        DamageNumber displayObject = go.GetComponent<DamageNumber>();
        displayObject.color = color;
        displayObject.Display(damage);
    }
    [ContextMenu("Test")]
    public void Test_()
    {
        StartCoroutine(Test());
    }
    protected override void Update()
    {
        base.Update();
    }
    IEnumerator Test()
    {
        while (true)
        {
            DisplayNumber(Random.Range(0, 20), Random.ColorHSV());
            yield return new WaitForSeconds(1);
        }
    }
}
