using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRadar : MonoBehaviour
{
    GameObject target;

    float facingSpeed = 5f;

    Player player;

    float detectionDistance;

    float distanceThreshold;

    [SerializeField] Sprite[] enemyRadarSprites;

    float discoveryThresholdDist = 15f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTarget();

        if (Vector2.Distance(player.transform.position, target.transform.position) > detectionDistance)
        {
            Destroy(gameObject);
        }

        distanceThreshold = Mathf.Floor(Vector2.Distance(player.transform.position, target.transform.position) / 100);

        Display();

        if (target.GetComponent<EnemyRadarActivator>().HasBeenDiscovered || target.GetComponent<EnemyRadarActivator>().HasBeenCleared)
        {
            Destroy(gameObject);
        }

        if (Vector2.Distance(player.transform.position, target.transform.position) < discoveryThresholdDist)
        {
            Destroy(gameObject);
        }

    }

    private void Display()
    {
        switch (distanceThreshold)
        {
            case 0:
                GetComponent<Image>().sprite = enemyRadarSprites[3];
                break;
            case 1:
                GetComponent<Image>().sprite = enemyRadarSprites[2];
                break;
            case 2:
                GetComponent<Image>().sprite = enemyRadarSprites[1];
                break;
            case 3:
                GetComponent<Image>().sprite = enemyRadarSprites[0];
                break;
        }
    }

    private void RotateTowardsTarget()
    {
        Transform targetTransform = target.transform;
        Vector2 direction = targetTransform.position - player.gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 135;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
    }

    public void SetTarget(GameObject currentTarget, float currentDetectionDistance)
    {
        target = currentTarget;
        detectionDistance = currentDetectionDistance;
    }
}
