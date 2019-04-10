using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfield : MonoBehaviour
{
    Player player;
    Vector3 playerPos;

    int width = 140;
    int height = 100;

    int buffer = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 direction = new Vector2(FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>().velocity.x * 10000000,
        //FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>().velocity.y * 10000000)
        //    + new Vector2(transform.position.x, transform.position.y);
        //float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + 90;
        //Debug.Log(angle);
        //Vector3 regularVector3 = new Vector3(angle, 90, -90);
        //transform.eulerAngles = regularVector3;

        playerPos = player.transform.position;

        if (Mathf.Abs(playerPos[0] - transform.position.x) > (width + ((width - (2 * buffer)) / 2)))
        {
            Destroy(gameObject);
        }

        if (playerPos[1] - transform.position.y > (height - buffer))
        {
            Destroy(gameObject);
        }

        if (transform.position.y - playerPos[1] > (2 * height) - buffer)
        {
            Destroy(gameObject);
        }
    }
}
