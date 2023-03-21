using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperSenses
{
    public static Transform FindClosestTarget(Transform origin)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject bestTarget = null;
        float bestDist = 9999999;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, origin.transform.position);
            if (distance < bestDist)
            {
                bestDist = distance;
                bestTarget = player;
            }
        }

        return bestTarget.transform;
    }
    public static Transform FindClosestTarget(Vector3 origin)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject bestTarget = null;
        float bestDist = 9999999;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, origin);
            if (distance < bestDist)
            {
                bestDist = distance;
                bestTarget = player;
            }
        }

        return bestTarget.transform;
    }

    public static Transform FindFarthestTarget(Transform origin)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject bestTarget = null;
        float bestDist = 0;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, origin.transform.position);
            if (distance > bestDist)
            {
                bestDist = distance;
                bestTarget = player;
            }
        }

        return bestTarget.transform;
    }
    public static Transform FindFarthestTarget(Vector3 origin)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject bestTarget = null;
        float bestDist = 0;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, origin);
            if (distance > bestDist)
            {
                bestDist = distance;
                bestTarget = player;
            }
        }

        return bestTarget.transform;
    }

    /// <summary>
    /// can use. but is a bit slow. prefer just using Physics.OverlapSphere instead
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private static List<Transform> SphereFind(Vector3 origin, float radius)
    {
        List<Transform> foundPlayers = new List<Transform>();
        Collider[] hits = Physics.OverlapSphere(origin, radius);
        foreach (Collider hit in hits)
        {
            if(hit.tag == "Player")
            {
                foundPlayers.Add(hit.transform);
            }
        }

        return foundPlayers;
    }
}
