using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRadarManager : MonoBehaviour
{

    [SerializeField] GameObject enemyRadarPrefab;

    EnemyRadarActivator[] targets;

    HashSet<GameObject> targetsFound = new HashSet<GameObject> { };

    Player player;

    float detectionDistance = 400f;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        targets = FindObjectsOfType<EnemyRadarActivator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        transform.position = FindObjectOfType<RedRadar>().transform.position;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.SceneType == "space")
        {
            foreach (EnemyRadarActivator target in targets)
            {
                if (Vector2.Distance(target.transform.position, player.transform.position) < detectionDistance)
                {
                    if (target.HasBeenDiscovered)
                    {
                        targetsFound.Remove(target.gameObject);
                    }
                    if (!targetsFound.Contains(target.gameObject))
                    {
                        if (!target.HasBeenDiscovered && !target.HasBeenCleared)
                        {
                            targetsFound.Add(target.gameObject);
                            StartCoroutine(DisplayEnemyRadar(target));
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

    IEnumerator DisplayEnemyRadar(EnemyRadarActivator target)
    {
        GameObject newEnemyRadar = Instantiate(enemyRadarPrefab, transform.position, Quaternion.identity, transform.parent);
        newEnemyRadar.GetComponent<EnemyRadar>().SetTarget(target.gameObject, detectionDistance);
        newEnemyRadar.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(0.8f); // added small delay because it takes some time for the radar to rotate to point towards the correct direction
        if (newEnemyRadar != null)
        {
            newEnemyRadar.GetComponent<Image>().enabled = true;
        }
    }
}
