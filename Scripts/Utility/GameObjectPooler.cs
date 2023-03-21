using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler : MonoBehaviour
{
    [SerializeField] public GameObject poolObject;
    Queue<GameObject> pool = new Queue<GameObject>();
    /*public GameObjectPooler(GameObject poolObject)
    {
        Debug.Log(poolObject.name);
        this.poolObject = poolObject;
    }*/
    public void Populate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateGo();
        }
    }
    public GameObject Pop()
    {
        if (pool.Count == 0)
        {
            CreateGo();
        }
        return pool.Dequeue();
    }
    public GameObject PopFor(float seconds)
    {
        if (pool.Count == 0)
        {
            CreateGo();
        }
        GameObject go = pool.Dequeue();
        StartCoroutine(EnqueueAfter(go, seconds));
        return go;
    }
    public void Enqueue(GameObject go)
    {
        pool.Enqueue(go);
    }
    public void CreateGo()
    {
        if (poolObject == null) { Debug.LogWarning("No PoolObject Referenced", this.gameObject); return; }
        GameObject go = Instantiate(poolObject, this.gameObject.transform.position, Quaternion.identity, this.transform);
        pool.Enqueue(go);
    }
    IEnumerator EnqueueAfter(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.transform.position = this.transform.position;
        pool.Enqueue(go);
    }
}
