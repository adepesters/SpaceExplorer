using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YellowRadarManager : MonoBehaviour
{
    [SerializeField] GameObject yellowRadarPrefab;

    YellowRadarActivator[] targets;

    HashSet<GameObject> targetsFound = new HashSet<GameObject> { };

    Player player;

    float detectionDistance = 400f;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        targets = FindObjectsOfType<YellowRadarActivator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        transform.position = FindObjectOfType<RedRadar>().transform.position;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.SceneType == "space")
        {
            foreach (YellowRadarActivator target in targets)
            {
                if (Vector2.Distance(target.transform.position, player.transform.position) < detectionDistance)
                {
                    if (!targetsFound.Contains(target.gameObject))
                    {
                        if (!target.HasBeenDiscovered)
                        {
                            targetsFound.Add(target.gameObject);
                            StartCoroutine(DisplayYellowRadar(target));
                        }
                    }
                }
                else
                {
                    targetsFound.Remove(target.gameObject);
                }
            }
        }
    }

    IEnumerator DisplayYellowRadar(YellowRadarActivator target)
    {
        GameObject newYellowRadar = Instantiate(yellowRadarPrefab, transform.position, Quaternion.identity, transform.parent);
        newYellowRadar.GetComponent<YellowRadar>().SetTarget(target.gameObject, detectionDistance);
        newYellowRadar.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(0.8f); // added small delay because it takes some time for the radar to rotate to point towards the correct direction
        if (newYellowRadar != null)
        {
            newYellowRadar.GetComponent<Image>().enabled = true;
        }
    }

}
