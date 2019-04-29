using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRadarManager : MonoBehaviour
{
    [SerializeField] GameObject yellowRadarPrefab;

    YellowRadarActivator[] targets;

    HashSet<GameObject> targetsFound = new HashSet<GameObject> { };

    Player player;

    float detectionDistance = 400f;

    // Start is called before the first frame update
    void Start()
    {
        targets = FindObjectsOfType<YellowRadarActivator>();
        player = FindObjectOfType<Player>();
        transform.position = FindObjectOfType<RedRadar>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (YellowRadarActivator target in targets)
        {
            if (Vector2.Distance(target.transform.position, player.transform.position) < detectionDistance)
            {
                if (!targetsFound.Contains(target.gameObject))
                {
                    Debug.Log("added");
                    targetsFound.Add(target.gameObject);
                    GameObject newYellowRadar = Instantiate(yellowRadarPrefab, transform.position, Quaternion.identity, transform.parent);
                    newYellowRadar.GetComponent<YellowRadar>().SetTarget(target.gameObject, detectionDistance);
                }
            }
            else
            {
                targetsFound.Remove(target.gameObject);
            }
        }

        //foreach (GameObject targetFound in targetsFound)
        //{
        //    GetComponent<Image>().enabled = true;
        //    Transform targetTransform = targetFound.transform;
        //    Vector2 direction = targetTransform.position - FindObjectOfType<Player>().gameObject.transform.position;
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 135;
        //    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);

        //}
    }
}
