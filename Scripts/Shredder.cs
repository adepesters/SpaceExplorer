using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Laser>() != null || collision.gameObject.GetComponent<Bonus>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
