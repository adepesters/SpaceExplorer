using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    bool isOnPlanet = false;
    GameObject planet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isOnPlanet = true;
        planet = collision.gameObject;
        transform.position = collision.gameObject.transform.position;
        Debug.Log("on planet");
    }

    public bool GetIsOnPlanet()
    {
        return isOnPlanet;
    }

    public GameObject GetPlanet()
    {
        return planet;
    }

}
