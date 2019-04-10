using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    int waypointIndex = 0;
    List<Transform> waypoints;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        var targetPosition = waypoints[waypointIndex].transform.position;
        var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
        if (transform.position == targetPosition)
        {
            if (waypointIndex <= waypoints.Count - 2)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
        }
    }
}
