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
    }
}
