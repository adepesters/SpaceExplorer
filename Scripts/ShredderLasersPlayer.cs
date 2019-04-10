using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderLasersPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Laser Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
