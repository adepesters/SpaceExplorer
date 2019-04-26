using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    float damage = 100;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
        transform.position = swordPos;
    }

    public void DestroySword()
    {
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}
