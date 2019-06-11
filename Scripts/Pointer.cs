using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    GameSession gameSession;

    private void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        transform.position = gameSession.PositionPointer;
    }

    private void Update()
    {
        gameSession.PositionPointer = transform.position;
    }

}
