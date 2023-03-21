using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] KeyCode up, down, left, right;
    [SerializeField] float force = 10;
    [SerializeField] float maxDistanceFromPlayer;
    [SerializeField] Transform otherPlayer;
    void Update()
    {
        Vector3 direction = new Vector3((Input.GetKey(right)) ? 1 : (Input.GetKey(left)) ? -1 : 0,
                                         0,
                                        (Input.GetKey(up)) ? 1 : (Input.GetKey(down)) ? -1 : 0);
        transform.Translate(ModifyTranslation(direction) * Time.deltaTime * force);
    }

    bool ValidateTranslation(Vector3 direction)
    {
        float distance = Mathf.Abs(((this.transform.position + direction) - otherPlayer.position).magnitude);
        return distance <= maxDistanceFromPlayer;
    }
    float GetNormalizedDotProduct(Vector3 a, Vector3 b)
    {
        a = a.normalized;
        b = b.normalized;

        float dot = Vector3.Dot(a, b);
        float normalizedDot = (1 + dot) / 2;
        return normalizedDot;
    }
    Vector3 ModifyTranslation(Vector3 direction)
    {
        Vector3 directionToPlayer = (otherPlayer.position - this.transform.position);
        float distance = Mathf.Abs(directionToPlayer.magnitude);
        Vector3 modified = direction;
        if (distance >= maxDistanceFromPlayer)
        {
            modified = direction * GetNormalizedDotProduct(direction, directionToPlayer.normalized);
        }
        return modified;
    }
}
