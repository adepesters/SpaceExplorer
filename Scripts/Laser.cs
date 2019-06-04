﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float angle;
    GameObject parentEnemy;

    public bool goesBackToEnemy = false;

    bool isImmobile;

    Player player;

    bool trackTarget;

    public bool TrackTarget1 { get => trackTarget; set => trackTarget = value; }
    public float ThresholdTargetUpdate { get => thresholdTargetUpdate; set => thresholdTargetUpdate = value; }

    float thresholdTargetUpdate;

    void Start()
    {
        angle = transform.eulerAngles.z - 90;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (gameObject.name.Contains("Laser Enemy"))
        {
            float speed = 25f;
            if (!isImmobile)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 0f;
            }
        }

        if (gameObject.name.Contains("Laser LowFuelEnemy"))
        {
            float speed = 130f;
            if (!isImmobile)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 0f;
            }
        }


        if (gameObject.name.Contains("Bomb Enemy"))
        {
            float speed = 5f;
            if (!isImmobile)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 0f;
            }
        }

        if (goesBackToEnemy)
        {
            angle = transform.eulerAngles.z - 90;
            gameObject.layer = 10;
            AttackBackEnemy();
        }

        if (TrackTarget1)
        {
            angle = transform.eulerAngles.z - 90;
            TrackTarget();
        }
    }

    public void AttackBackEnemy()
    {
        float facingSpeed = 10;
        if (parentEnemy != null)
        {
            Transform target = parentEnemy.gameObject.transform;
            Vector2 direction = target.position - transform.position;
            float angleRot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Quaternion rotation = Quaternion.AngleAxis(angleRot, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
        }
    }

    public void TrackTarget()
    {
        float facingSpeed = 10;
        if (player != null)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > ThresholdTargetUpdate)
            {
                Transform target = player.transform;
                Vector2 direction = target.position - transform.position;
                float angleRot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                Quaternion rotation = Quaternion.AngleAxis(angleRot, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
            }
            else
            {
                TrackTarget1 = false;
            }
        }
    }

    public void SetParent(GameObject currentParent)
    {
        parentEnemy = currentParent;
    }

    public void SetImmobile(bool currentImmobility)
    {
        isImmobile = currentImmobility;
    }
}
