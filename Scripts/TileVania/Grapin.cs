using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapin : MonoBehaviour
{
    bool grapinLaunched;
    bool rotateGrapin;
    bool returnGrapin;
    bool gotStuck;
    bool canGetStuck;

    Vector2 target;

    PlayerTileVania player;
    GameObject grapinHandler;

    float speedGrapin;

    Vector2 stuckPos;

    float detectionThreshold = 0.5f;

    float grapinLength = 4f;

    Coroutine enableGrapinJump;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTileVania>();
        grapinHandler = GameObject.Find("Grapin Handler");
        rotateGrapin = true;
        transform.position = grapinHandler.transform.position;
        speedGrapin = 20f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateGrapin)
        {
            RotatePlayerToGivenDirection();
            transform.position = grapinHandler.transform.position;
        }

        if (FindObjectOfType<PS4ControllerCheck>().IsSquarePressed())
        {
            grapinLaunched = true;
            returnGrapin = false;
            target = new Vector2(transform.position.x, transform.position.y) - grapinLength * new Vector2(transform.up.x, transform.up.y);
        }

        if (grapinLaunched)
        {
            StartCoroutine(LaunchGrapin());
        }

        if (returnGrapin)
        {
            target = new Vector2(grapinHandler.transform.position.x, grapinHandler.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speedGrapin);
            if (Mathf.Abs(transform.position.x - target.x) < Mathf.Epsilon && Mathf.Abs(transform.position.y - target.y) < Mathf.Epsilon)
            {
                rotateGrapin = true;
                enableGrapinJump = null;
            }
        }

        if (gotStuck)
        {
            canGetStuck = false;
            transform.position = stuckPos;
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, speedGrapin);
            player.GetComponent<Rigidbody2D>().simulated = false;
            if (Vector2.Distance(transform.position, player.transform.position) < detectionThreshold)
            {
                returnGrapin = true;
                player.GetComponent<Rigidbody2D>().simulated = true;
                if (enableGrapinJump == null && gotStuck)
                {
                    enableGrapinJump = StartCoroutine("EnableGrapinJump");
                }
                gotStuck = false;
            }
        }

        //Debug.Log(gotStuck);
        //Debug.Log(enableGrapinJump);
    }

    IEnumerator EnableGrapinJump()
    {
        player.GrapinJump = true;
        yield return new WaitForSeconds(0.5f);
        player.GrapinJump = false;
    }

    IEnumerator LaunchGrapin()
    {
        rotateGrapin = false;
        canGetStuck = true;
        transform.position = Vector2.MoveTowards(transform.position, target, speedGrapin);
        if (Mathf.Abs(transform.position.x - target.x) < Mathf.Epsilon && Mathf.Abs(transform.position.y - target.y) < Mathf.Epsilon)
        {
            grapinLaunched = false;
            canGetStuck = false;
        }
        yield return new WaitForSeconds(0.5f);
        if (!gotStuck)
        {
            returnGrapin = true;
        }
    }

    private void RotatePlayerToGivenDirection()
    {
        float xVelocity = Input.GetAxis("Horizontal") / (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));
        float yVelocity = Input.GetAxis("Vertical") / (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));

        Vector2 direction = new Vector2(xVelocity * 10000000, yVelocity * 10000000) + new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 270;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canGetStuck)
        {
            gotStuck = true;
            grapinLaunched = false;
            stuckPos = transform.position;
        }
    }
}
