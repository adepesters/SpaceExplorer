using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjectsLayer1 = GameObject.FindGameObjectsWithTag("Layer1");
        GameObject[] gameObjectsLayer2 = GameObject.FindGameObjectsWithTag("Layer2");

        foreach (GameObject gameObjectLayer1 in gameObjectsLayer1)
        {
            foreach (GameObject gameObjectLayer2 in gameObjectsLayer2)
            {
                if (gameObjectLayer1.GetComponent<Collider2D>() != null && gameObjectLayer2.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(gameObjectLayer1.GetComponent<Collider2D>(), gameObjectLayer2.GetComponent<Collider2D>());
                }
            }
        }
    }

}
