using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicRock : MonoBehaviour
{
    Vector3 target;
    bool isTargetingPlayerLayer = false;
    float timeToTarget;

    public Vector3 Target { get => target; set => target = value; }
    public bool IsTargetingPlayerLayer { get => isTargetingPlayerLayer; set => isTargetingPlayerLayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        timeToTarget = transform.position.z * Random.Range(5f, 8f) / 150f; // to adjust for depth of volcano
        transform.position = new Vector3(transform.position.x + Random.Range(-5f, 5f),
        transform.position.y + Random.Range(-5f, 5f),
            transform.position.z);
        if (!float.IsNaN(CalculateTrajectoryVelocity(transform.position, Target, timeToTarget).x))
        {
            GetComponent<Rigidbody>().velocity = CalculateTrajectoryVelocity(transform.position, Target, timeToTarget);
        }
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 currentTarget, float t)
    {
        float vx = (currentTarget.x - origin.x) / t;
        float vz = (currentTarget.z - origin.z) / t;
        float vy = ((currentTarget.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }

}
