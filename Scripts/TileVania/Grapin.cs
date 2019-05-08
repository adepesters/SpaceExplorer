﻿using System.Collections;
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

    GameObject grapinTarget;

    bool displayTarget;

    Color targetColor = new Color(0.1620135f, 0.6698113f, 0.03475436f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        player = FindObjectOfType<PlayerTileVania>();
        grapinHandler = GameObject.Find("Grapin Handler");
        grapinTarget = GameObject.Find("Grapin Target");
        grapinTarget.GetComponent<SpriteRenderer>().enabled = false;
        rotateGrapin = true;
        transform.position = grapinHandler.transform.position;
        speedGrapin = 20f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<ToolSelector>().GetTool() == "grappin")
        {
            GetComponent<SpriteRenderer>().enabled = true;

            if (rotateGrapin)
            {
                RotatePlayerToGivenDirection();
                transform.position = grapinHandler.transform.position;
            }

            if (FindObjectOfType<PS4ControllerCheck>().IsSquarePressed())
            {
                displayTarget = true;
                if (FindObjectOfType<Feet>().AreOnSomething)
                {
                    player.IsTargeting = true;
                }
            }

            if (FindObjectOfType<PS4ControllerCheck>().IsSquareReleased())
            {
                target = new Vector2(transform.position.x, transform.position.y) - grapinLength * new Vector2(transform.up.x, transform.up.y);
                displayTarget = false;
                grapinLaunched = true;
                returnGrapin = false;
                player.IsTargeting = false;
            }


            if (displayTarget)
            {
                DisplayTarget();
            }
            else
            {
                grapinTarget.GetComponent<SpriteRenderer>().enabled = false;
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
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void DisplayTarget()
    {
        grapinTarget.GetComponent<SpriteRenderer>().enabled = true;
        target = new Vector2(transform.position.x, transform.position.y) - grapinLength * new Vector2(transform.up.x, transform.up.y);
        grapinTarget.transform.position = target;
        RaycastUntilTarget();
    }

    private void RaycastUntilTarget()
    {
        int layerMask = (1 << 8) | (1 << 22) | (1 << 23);
        layerMask = ~layerMask;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position - (1f * transform.up), -transform.up, grapinLength - 1f, layerMask);
        //Debug.DrawRay(transform.position - (1.5f * transform.up), -transform.up * grapinLength);

        if (hits.Length > 0)
        {
            grapinTarget.GetComponent<SpriteRenderer>().color = targetColor;
            grapinTarget.transform.position = hits[0].point;
        }
        else
        {
            grapinTarget.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
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
