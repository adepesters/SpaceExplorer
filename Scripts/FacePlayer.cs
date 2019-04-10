using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    float facingSpeed;

    bool isImmobile;

    void Start()
    {
        facingSpeed = UnityEngine.Random.Range(1, 10); // the fastest always aim perfectly for the player
        // the slowest are sloppy and miss the player
    }

    // Update is called once per frame
    void Update()
    {
        if (!isImmobile)
        {
            Transform target = FindObjectOfType<Player>().gameObject.transform;
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
        }
    }

    public void SetImmobile(bool currentImmobility)
    {
        isImmobile = currentImmobility;
    }
}
