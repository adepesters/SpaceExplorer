using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTargetPlayer : MonoBehaviour
{
    Vector2 playerPos;
    Vector2 targetPos;
    Vector2 currentPos;

    void Start()
    {
        currentPos = transform.position;
        playerPos = FindObjectOfType<Player>().transform.position;
        targetPos = playerPos + 100000 * (playerPos - currentPos);
    }

    void Update()
    {
        if (gameObject.name.Contains("Laser Enemy") || gameObject.name.Contains("Bomb"))
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 30f * Time.deltaTime);
        }
    }
}
