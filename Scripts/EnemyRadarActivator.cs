using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadarActivator : MonoBehaviour
{
    private bool hasBeenDiscovered;
    private bool hasBeenCleared;

    public bool HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }
    public bool HasBeenCleared { get => hasBeenCleared; set => hasBeenCleared = value; }

    Player player;

    float discoveryThresholdDist = 15f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        HasBeenDiscovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < discoveryThresholdDist)
        {
            hasBeenDiscovered = true;
        }
    }
}
