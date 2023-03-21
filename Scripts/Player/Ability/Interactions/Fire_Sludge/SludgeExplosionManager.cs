using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SludgeExplosionManager : MonoBehaviour
{
    [SerializeField] GameObject sludgeExplosionPrefab;
    GameObjectPooler goPool;

    [SerializeField] private AudioSource Bomb;
    [SerializeField] private AudioClip Pang;
    private void Start()
    {
        goPool = gameObject.AddComponent<GameObjectPooler>();
        goPool.poolObject = sludgeExplosionPrefab;
    }
    public void InstantiateExplosion(Vector3 position)
    {
        GameObject go = goPool.PopFor(2f);
        SludgePuddleExplosion instance = go.GetComponent<SludgePuddleExplosion>();
        go.transform.position = position;
        instance.Detonate();

        Bomb.PlayOneShot(Pang);
    }
}
