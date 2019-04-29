using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YellowRadar : MonoBehaviour
{
    GameObject target;

    float facingSpeed = 5f;

    Player player;

    float detectionDistance;

    float distanceThreshold;

    [SerializeField] Sprite[] yellowRadarSprites;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform targetTransform = target.transform;
        Vector2 direction = targetTransform.position - FindObjectOfType<Player>().gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 135;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);

        if (Vector2.Distance(player.transform.position, target.transform.position) > detectionDistance)
        {
            Destroy(gameObject);
        }

        distanceThreshold = Mathf.Floor(Vector2.Distance(player.transform.position, target.transform.position) / 100);

        switch (distanceThreshold)
        {
            case 0:
                GetComponent<Image>().sprite = yellowRadarSprites[3];
                break;
            case 1:
                GetComponent<Image>().sprite = yellowRadarSprites[2];
                break;
            case 2:
                GetComponent<Image>().sprite = yellowRadarSprites[1];
                break;
            case 3:
                GetComponent<Image>().sprite = yellowRadarSprites[0];
                break;
        }

    }

    public void SetTarget(GameObject currentTarget, float currentDetectionDistance)
    {
        target = currentTarget;
        detectionDistance = currentDetectionDistance;
    }
}
