using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelBlood : MonoBehaviour
{
    float attractionDistance = 1.5f;
    float accelerationAttraction = 0.01f;

    PlayerTileVania player;

    [SerializeField] AudioClip blipSound;
    float volumeSoundBlip = 0.05f;

    Coroutine playSFX;

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
                    if (playSFX == null)
                    {
                        playSFX = StartCoroutine(PlaySFX());
                        GameObject.FindWithTag("GameSession").GetComponent<GameSession>().CounterPixelBlood++;
                        Destroy(gameObject, 0.1f);
                    }
                }
            }
        }
    }

    IEnumerator PlaySFX()
    {
        GetComponent<AudioSource>().PlayOneShot(blipSound, volumeSoundBlip);
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
    }

}
