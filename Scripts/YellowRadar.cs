using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class YellowRadar : MonoBehaviour
{
    GameObject target;

    float facingSpeed = 5f;

    Player player;

    float detectionDistance;

    float distanceThreshold;

    [SerializeField] Sprite[] yellowRadarSprites;

    float discoveryThresholdDist = 15f;

    int planetID;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTarget();

        if (Vector2.Distance(player.transform.position, target.transform.position) > detectionDistance) // destroys radar when target too far from player
        {
            Destroy(gameObject);
        }

        //if (target.GetComponent<ManualLayer>().Layer != player.CurrentLayer) // destroys radar when target not on same layer than player
        //{
        //    Destroy(gameObject);
        //}

        distanceThreshold = Mathf.Floor(Vector2.Distance(player.transform.position, target.transform.position) / 100);

        Display();

        // destroy target when close to it
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
                GetComponent<Image>().sprite = yellowRadarSprites[3];
                AdjustColor();
                break;
            case 1:
                GetComponent<Image>().sprite = yellowRadarSprites[2];
                AdjustColor();
                break;
            case 2:
                GetComponent<Image>().sprite = yellowRadarSprites[1];
                AdjustColor();
                break;
            case 3:
                GetComponent<Image>().sprite = yellowRadarSprites[0];
                AdjustColor();
                break;
        }

    }

    private void AdjustColor()
    {
        if (gameSession.IsCleaned[planetID] && target.GetComponent<ManualLayer>().Layer == player.CurrentLayer)
        {
            GetComponent<Image>().color = new Color(0.4708081f, 0.8679245f, 0.6094226f, 1f);
        }
        else if (gameSession.IsCleaned[planetID] && target.GetComponent<ManualLayer>().Layer != player.CurrentLayer)
        {
            GetComponent<Image>().color = new Color(0.4708081f, 0.8679245f, 0.6094226f, 0.3f);
        }
        else if (!gameSession.IsCleaned[planetID] && target.GetComponent<ManualLayer>().Layer != player.CurrentLayer)
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.3f);
        }
        else if (!gameSession.IsCleaned[planetID] && target.GetComponent<ManualLayer>().Layer == player.CurrentLayer)
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
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
        string numbersOnly = Regex.Replace(target.gameObject.name, "[^0-9]", "");
        planetID = int.Parse(numbersOnly);
    }
}
