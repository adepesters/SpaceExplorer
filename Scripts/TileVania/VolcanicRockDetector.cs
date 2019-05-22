using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicRockDetector : MonoBehaviour
{
    [SerializeField] GameObject virtualRock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<VolcanicRock>().IsTargetingPlayerLayer)
        {
            if (gameObject.tag == other.gameObject.tag)
            {
                GameObject newVirtualRock = Instantiate(virtualRock, other.gameObject.transform.position, Quaternion.identity);
                newVirtualRock.tag = other.gameObject.tag;
                Destroy(other.gameObject, 0.01f);
                Destroy(newVirtualRock, 0.01f);
            }
        }
    }
}
