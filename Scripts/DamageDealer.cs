using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    int damage;

    void Start()
    {
        damage = FindObjectOfType<Player>().GetDamageLaserPlayer();
    }

    public int GetDamage() { return damage; }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
