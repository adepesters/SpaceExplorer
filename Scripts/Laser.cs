using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float angle;
    GameObject parentEnemy;

    public bool goesBackToEnemy = false;

    bool isImmobile;

    void Start()
    {
        angle = transform.eulerAngles.z - 90;
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

    public void SetParent(GameObject currentParent)
    {
        parentEnemy = currentParent;
    }

    public void SetImmobile(bool currentImmobility)
    {
        isImmobile = currentImmobility;
    }
}
