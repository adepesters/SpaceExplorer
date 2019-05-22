using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicRockDetector : MonoBehaviour
{
    [SerializeField] GameObject virtualRock;

    private void OnTriggerEnter(Collider other)
    {
        GameObject newVirtualRock = Instantiate(virtualRock, other.gameObject.transform.position, Quaternion.identity);
        Destroy(other.gameObject, 0.01f);
        Destroy(newVirtualRock, 0.01f);
    }
}
