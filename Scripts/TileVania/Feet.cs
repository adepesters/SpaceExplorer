using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    GameObject currentSurface;
    bool areOnSomething;

    public GameObject CurrentSurface { get => currentSurface; set => currentSurface = value; }
    public bool AreOnSomething { get => areOnSomething; set => areOnSomething = value; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Collider2D>().isTrigger == false) // to make sure the collider is actually a collider and not just a trigger
        {
            areOnSomething = true;
            CurrentSurface = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        areOnSomething = false;
        CurrentSurface = null;
    }

}
