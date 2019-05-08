using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedLegs : MonoBehaviour
{
    GameObject currentSurface;
    bool areOnSomething;

    public GameObject CurrentSurface { get => currentSurface; set => currentSurface = value; }
    public bool AreOnSomething { get => areOnSomething; set => areOnSomething = value; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        areOnSomething = true;
        CurrentSurface = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        areOnSomething = false;
    }
}
