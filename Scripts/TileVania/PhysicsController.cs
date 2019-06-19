using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhysicsController : MonoBehaviour
{
    Collider2D[] colliderObjects1;

    // Start is called before the first frame update
    void Start()
    {
        HandlePhysicsLayers();
    }

    public void HandlePhysicsLayers()
    {
        //colliderObjects1 = FindObjectsOfType<Collider2D>();

        //foreach (Collider2D collider1 in colliderObjects1)
        //{
        //    foreach (Collider2D collider2 in colliderObjects1)
        //    {
        //        if (collider1.gameObject.tag.Contains("Layer") && collider2.gameObject.tag.Contains("Layer"))
        //        {
        //            if (collider1.gameObject.tag != collider2.gameObject.tag)
        //            {
        //                Physics2D.IgnoreCollision(collider1, collider2);
        //            }
        //            else
        //            {
        //                Physics2D.IgnoreCollision(collider1, collider2, false);
        //            }
        //        }
        //    }
        //}

        GameObject[] gameObjectsLayer1 = GameObject.FindGameObjectsWithTag("Layer1");
        GameObject[] gameObjectsLayer2 = GameObject.FindGameObjectsWithTag("Layer2");
        GameObject[] gameObjectsLayer3 = GameObject.FindGameObjectsWithTag("Layer3");

        var array3 = gameObjectsLayer1.Concat(gameObjectsLayer2).ToArray();
        int a;

        foreach (GameObject gameObjectLayer1 in array3)
        {
            foreach (GameObject gameObjectLayer2 in array3)
            {
                if (gameObjectLayer1 != gameObjectLayer2)
                {
                    if (gameObjectLayer1.GetComponent<Collider2D>() != null && gameObjectLayer2.GetComponent<Collider2D>() != null)
                    {
                        if (gameObjectLayer1.gameObject.tag != gameObjectLayer2.gameObject.tag)
                        {
                            a = 0;
                            //Debug.Log("ok1");
                            Physics2D.IgnoreCollision(gameObjectLayer1.GetComponent<Collider2D>(), gameObjectLayer2.GetComponent<Collider2D>(), true);
                        }
                        else
                        {
                            a = 1;
                            //Debug.Log("ok2");
                            Physics2D.IgnoreCollision(gameObjectLayer1.GetComponent<Collider2D>(), gameObjectLayer2.GetComponent<Collider2D>(), false);
                        }
                    }
                    //Physics2D.IgnoreCollision(gameObjectLayer1.GetComponent<Collider2D>(), gameObjectLayer2.GetComponent<Collider2D>());
                }
            }
        }

        //foreach (GameObject gameObjectLayer1 in gameObjectsLayer1)
        //{
        //    foreach (GameObject gameObjectLayer3 in gameObjectsLayer3)
        //    {
        //        if (gameObjectLayer1.GetComponent<Collider2D>() != null && gameObjectLayer2.GetComponent<Collider2D>() != null)
        //        {
        //            Physics2D.IgnoreCollision(gameObjectLayer1.GetComponent<Collider2D>(), gameObjectLayer2.GetComponent<Collider2D>());
        //        }
        //    }
        //}
    }

}


