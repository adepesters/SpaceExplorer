using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTutoAfterPlaying : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Collider2D>().enabled == false)
        {
            gameObject.GetComponent<DialogActivator>().enabled = false;
        }
    }

}
