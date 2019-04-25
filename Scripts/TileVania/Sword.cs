using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
    }

    public void DestroySword()
    {
        Destroy(gameObject);
    }
}
