using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelBlood : MonoBehaviour
{
    float attractionDistance = 1f;
    float accelerationAttraction = 0.01f;

    PlayerTileVania player;

    // Start is called before the first frame update
    void Start()
    {
        float forceX = UnityEngine.Random.Range(-100f, 100f);
        float forceY = UnityEngine.Random.Range(100f, 200f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));

        player = FindObjectOfType<PlayerTileVania>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == player.tag)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < attractionDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (accelerationAttraction) * Time.deltaTime);
                accelerationAttraction += accelerationAttraction;
                if (transform.position == player.transform.position)
                {
                    Destroy(gameObject);

                    //if (this.name.Contains("pink"))
                    //{
                    //    FindObjectOfType<GameSession>().counterStarBronze++;
                    //    Destroy(gameObject);
                    //    AudioSource.PlayClipAtPoint(coinSound, player.transform.position, volumeSound);
                    //}

                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "Layer1" && collision.gameObject.tag == "Layer2")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (gameObject.tag == "Layer2" && collision.gameObject.tag == "Layer1")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

}
