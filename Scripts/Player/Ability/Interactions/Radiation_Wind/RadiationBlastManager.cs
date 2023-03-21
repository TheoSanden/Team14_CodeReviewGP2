using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadiationBlastManager : MonoBehaviour
{
    [SerializeField] private Events.InteractionEvent onInteractionEvent;
    [SerializeField] private GameObject _radStorm;
    [SerializeField] private FloatVariable _force;
    GameObjectPooler goPool;
    
    [SerializeField] private float _blastGracePeriod = 0.1f;
    [SerializeField] private float _chainReactionSpeed = 0.5f;
    [SerializeField] private float _chainReactionRadius = 0.1f;

    [SerializeField] private bool _canBlast = true;
    private Vector3 _offset = Vector3.up * 0.7f;

    public void SpawnRadStorm(Vector3 position)
    {
        if (_canBlast)
        {
            _canBlast = false;
            CreateBlast(position);
            FirstCheck(CheckDistances(position), position);
            StartCoroutine(BlastGracePeriod());
        
        }

    }
    private void CreateBlast(Vector3 position)
    {
        Vector3[] spawnPoints =
        {
            position + Vector3.forward,
            position + Vector3.left,
            position - Vector3.forward,
            position - Vector3.left,
            position + (Vector3.forward + Vector3.left).normalized,
            position - (Vector3.forward + Vector3.left).normalized,
            position + (Vector3.forward - Vector3.left).normalized,
            position - (Vector3.forward - Vector3.left).normalized
            
        };
        foreach (Vector3 spawn in spawnPoints)
        {
            Quaternion lookDir = Quaternion.LookRotation(spawn - position, Vector3.up);
            Storm(spawn + _offset, lookDir);
        }


    }
    private void Storm(Vector3 spawn, Quaternion rotation)
    {
        GameObject storm = Instantiate(_radStorm, spawn, rotation);
        storm.GetComponent<Rigidbody>().AddForce(storm.transform.forward * _force.Value, ForceMode.Impulse);
    }
    
    
    private void CreateChainReaction(Vector3 position)
    {
        ReactionCheck(CheckDistances(position), position);
    }



    private List<Collider> CheckDistances(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, _chainReactionRadius);
        List<Collider> cols = new List<Collider>();
        
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Ability"))
            {
                PuddleFunctionality puddle = col.gameObject.GetComponent<PuddleFunctionality>();
                if (puddle != null && puddle.ElementType == InteractionArgs.ElementType.radiation)
                {
                    cols.Add(col);
                }
            }
        }

        return cols;

    }


    private void FirstCheck(List<Collider> cols, Vector3 point)
    {
        //Only called on elemental interaction, starts a chain reaction
        //checking if we less than two possible interactions
        switch (cols.Count)
        {
            case 0:
                return;
            case 1:
                StartCoroutine(ChainReaction(cols[0].transform.position));
                Destroy(cols[0].gameObject);
                return;
        }

        //trying to get two initial, opposite points
        Collider col1 = cols[0];
        Collider col2 = cols[0];
        for (int i = 1; i < cols.Count; i++)
        {
            Transform tf = cols[i].transform;
            Transform compTF = col1.transform;
            
            
            Vector3 vec = (tf.position - point).normalized;
            Vector3 compVec = compTF.position - point;
            
            if (Vector3.Dot(vec, compVec) < 0)
            {
                col2 = cols[i];
                break;
            }
        }

        //if points are the same just start one interaction
        if (col2 == col1)
        {
            col1.enabled = false;
            StartCoroutine(ChainReaction(col1.gameObject.transform.position));
            Destroy(col1.gameObject);
            return;
        }

        //comparing all points to the to initialized points to get the two closest ones
        foreach (Collider col in cols)
        {
            Transform colTF = col.transform;
            Transform tf1 = col1.transform;
            Transform tf2 = col2.transform;
            Vector3 colVec = (colTF.position -  point);
            Vector3 vec1 = tf1.position - point;
            Vector3 vec2 = tf2.position - point;
            float colLength = colVec.magnitude;
            float length1 = vec1.magnitude;
            float length2 = vec2.magnitude;
            if (colLength > length1 && Vector3.Dot(colVec, vec1) > 0)
            {
                col1 = col;
            }
            else if (colLength > length2 && Vector3.Dot(colVec, vec2) > 0)
            {
                col2 = col;
            }
            col.enabled = false;
        }

        //starting chain reaction at the two points
        Vector3 pos1 = col1.gameObject.transform.position;
        Vector3 pos2 = col2.gameObject.transform.position;

        StartCoroutine(ChainReaction(pos1));
        StartCoroutine(ChainReaction(pos2));
        foreach (Collider col in cols)
        {
            Destroy(col.gameObject);
        }


    }

    private void ReactionCheck(List<Collider> cols, Vector3 point)
    {
        //checking if we less than two possible interactions
        switch (cols.Count)
        {
            case 0:
                return;
            case 1:
                StartCoroutine(ChainReaction(cols[0].transform.position));
                Destroy(cols[0].gameObject);
                return;
        }
        
        //checking closest possible interaction point
        Collider outCol = cols[0];
        foreach (Collider col in cols)
        {
            Vector3 vec1 = col.transform.position -  point;
            Vector3 vec2 = outCol.transform.position - point;
            float length1 = vec1.magnitude;
            float length2 = vec2.magnitude;
            if (length1 > length2)
            {
                outCol = col;
            }
            col.enabled = false;
        }
        //starting chain reaction at the interaction point
        Vector3 pos = outCol.gameObject.transform.position;
        StartCoroutine(ChainReaction(pos));
        foreach (Collider col in cols)
        {
            Destroy(col.gameObject);
        }



    }
    
    
    
    
    
    IEnumerator ChainReaction(Vector3 position)
    {
        yield return new WaitForSeconds(_chainReactionSpeed);
        CreateBlast(position);
        CreateChainReaction(position);
    }
    IEnumerator BlastGracePeriod()
    {
        yield return new WaitForSeconds(_blastGracePeriod);
        _canBlast = true;
    }

}
