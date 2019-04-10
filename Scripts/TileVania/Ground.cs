using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    bool feetAreTouchingGround = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Feet"))
        {
            feetAreTouchingGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Feet"))
        {
            feetAreTouchingGround = false;
        }
    }

    public bool AreFeetOnTheGround()
    {
        return feetAreTouchingGround;
    }
}


