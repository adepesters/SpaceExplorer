using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRock : MonoBehaviour
{
    float counterDelay;
    Vector3 originalPos;
    Vector3 targetPos;
    bool goUp;
    float delay;

    [SerializeField] float speed = 0.47f;
    [SerializeField] float acceleration = 0.55f;
    [SerializeField] float spread = 0.8f;

    PlayerTileVania player;

    float direction;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;

        direction = Random.Range(-1f, 1f);
        targetPos = new Vector3(originalPos.x, originalPos.y + spread * Mathf.Sign(direction), originalPos.z);
        delay = Random.Range(0f, 1f);
        player = FindObjectOfType<PlayerTileVania>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2f)
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<PolygonCollider2D>().enabled = false; // otherwise there's huge computational issues
        }

        counterDelay += Time.deltaTime;
        if (counterDelay > delay)
        {

            if (Mathf.Sign(direction) > 0)
            {
                if (transform.position.y <= originalPos.y)
                {
                    speed += acceleration * Time.deltaTime;
                }
                else
                {
                    speed -= acceleration * Time.deltaTime;
                }

            }
            else
            {
                if (transform.position.y >= originalPos.y)
                {
                    speed += acceleration * Time.deltaTime;
                }
                else
                {
                    speed -= acceleration * Time.deltaTime;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        }
    }



}
