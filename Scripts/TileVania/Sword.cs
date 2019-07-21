using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    int damage = 15;
    float speedAnimation = 1f;

    Animator animator;
    bool canHit;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        animator = GetComponent<Animator>();
        canHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        damage = gameSession.SwordDamage;
        if (!canHit)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        animator.speed = SpeedAnimation;
        var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
        transform.position = swordPos;
    }

    public void DestroySword()
    {
        Destroy(gameObject);
    }

    public int GetDamage()
    {
        return damage;
    }

    public float SpeedAnimation { get => speedAnimation; set => speedAnimation = value; }

    public void SetCanHit(bool currentCanHit)
    {
        canHit = currentCanHit;
    }
}
