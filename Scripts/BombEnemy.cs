using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    float speed = 5f;

    void Update()
    {
        if (gameObject.name.Contains("Enemy"))
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y) * speed;// * Time.deltaTime;
        }
    }
}
