using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adds additional speed to the starfield by making it move opposite to the player 

public class StarfieldSpeed : MonoBehaviour
{
    Player player;
    Vector3 currentPos;
    Vector3 oldPos;

    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //currentPos = player.transform.position;
        currentPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        oldPos = currentPos;
        //currentPos = player.transform.position;
        currentPos = Camera.main.transform.position;
        if (speed == 0)
        {
            return;
        }
        else
        {
            transform.position -= (currentPos - oldPos) * speed;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

}
