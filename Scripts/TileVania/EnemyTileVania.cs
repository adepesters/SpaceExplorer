using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTileVania : MonoBehaviour
{
    float health = 500;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Sword"))
        {
            ProcessHit(collision);
            if (health <= 0)
            {
                KillEnemy();
            }
        }
    }

    private void ProcessHit(Collider2D collision)
    {
        health -= collision.gameObject.GetComponent<Sword>().GetDamage();
        StartCoroutine(ChangeColorWhenHit());
    }

    IEnumerator ChangeColorWhenHit()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
    }


}
