using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowFuelEnemyActivator : MonoBehaviour
{
    Vector2 leftSpawnerCenter;
    Vector2 leftSpawnerExtents;
    Vector2 rightSpawnerCenter;
    Vector2 rightSpawnerExtents;
    Vector2 topSpawnerCenter;
    Vector2 topSpawnerExtents;
    Vector2 bottomSpawnerCenter;
    Vector2 bottomSpawnerExtents;

    Player player;
    GameSession gameSession;
    FuelRadar fuelRadar;

    GameObject enemyPrefab;

    Coroutine spawnZoneCoroutineHandler;

    bool shouldSpawn;

    int indexEnemiesZone = 0;
    int maxNumEnemiesZone = 400;
    const string ZONE_ENEMIES_PARENT = "Zone Enemies Parent";
    GameObject zoneEnemiesParent;
    bool zoneCleaned = false;
    float spawningFrequencyZone = 1f;

    bool canDestroyLasers = false;

    bool currentlyFighting = false;

    public bool CurrentlyFighting { get => currentlyFighting; set => currentlyFighting = value; }

    // Start is called before the first frame update
    void Start()
    {
        zoneEnemiesParent = GameObject.Find(ZONE_ENEMIES_PARENT);
        if (zoneEnemiesParent == null)
        {
            zoneEnemiesParent = new GameObject(ZONE_ENEMIES_PARENT);
        }

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        fuelRadar = GameObject.FindWithTag("FuelRadar").GetComponent<FuelRadar>();
    }

    // Update is called once per frame
    void Update()
    {
        leftSpawnerCenter = GameObject.FindWithTag("LeftSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        leftSpawnerExtents = GameObject.FindWithTag("LeftSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;
        rightSpawnerCenter = GameObject.FindWithTag("RightSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        rightSpawnerExtents = GameObject.FindWithTag("RightSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;
        topSpawnerCenter = GameObject.FindWithTag("TopSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        topSpawnerExtents = GameObject.FindWithTag("TopSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;
        bottomSpawnerCenter = GameObject.FindWithTag("BottomSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        bottomSpawnerExtents = GameObject.FindWithTag("BottomSpawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;

        var distance = Vector2.Distance(player.transform.position, fuelRadar.TargetZone.transform.position);

        spawningFrequencyZone = (gameSession.MaxFuelSpacePlayer) / (distance);
        //Debug.Log("distance: " + distance);
        //Debug.Log(gameSession.MaxFuelSpacePlayer);
        //Debug.Log("frequency:" + spawningFrequencyZone);

        if (gameSession.CurrentFuelSpacePlayer <= 0f)
        {
            CurrentlyFighting = true;

            shouldSpawn = true;
            canDestroyLasers = true;

            if (spawnZoneCoroutineHandler == null)
            {
                spawnZoneCoroutineHandler = StartCoroutine(SpawnZone());
            }
        }
        else
        {
            if (canDestroyLasers)
            {
                GameObject[] lasers = GameObject.FindGameObjectsWithTag("LaserEnemy");
                foreach (GameObject laser in lasers)
                {
                    Destroy(laser.gameObject);
                }

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("LowFuelEnemy");
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().ExplodeEnemy();
                }

                canDestroyLasers = false;
                spawnZoneCoroutineHandler = null;
            }

            CurrentlyFighting = false;

            shouldSpawn = false;
        }

    }

    IEnumerator SpawnZone()
    {
        while (shouldSpawn)
        {
            if (indexEnemiesZone < maxNumEnemiesZone)
            {
                int randomSpawner = UnityEngine.Random.Range(0, 4);
                var enemies = GetComponent<RandomSpawnersList>().enemies;
                Enemy randomEnemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];

                if (randomSpawner == 0)
                {
                    Vector3 position = new Vector3(leftSpawnerCenter[0] + UnityEngine.Random.Range(-leftSpawnerExtents[0] / 2f, leftSpawnerExtents[0] / 2f),
                    leftSpawnerCenter[1] + UnityEngine.Random.Range(-leftSpawnerExtents[1] / 2f, leftSpawnerExtents[1] / 2f),
                    player.transform.position.z);

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                else if (randomSpawner == 1)
                {
                    Vector3 position = new Vector3(rightSpawnerCenter[0] + UnityEngine.Random.Range(-rightSpawnerExtents[0] / 2f, rightSpawnerExtents[0] / 2f),
                    rightSpawnerCenter[1] + UnityEngine.Random.Range(-rightSpawnerExtents[1] / 2f, rightSpawnerExtents[1] / 2f),
                    player.transform.position.z);

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                else if (randomSpawner == 2)
                {
                    Vector3 position = new Vector3(bottomSpawnerCenter[0] + UnityEngine.Random.Range(-bottomSpawnerExtents[0] / 2f, bottomSpawnerExtents[0] / 2f),
                    bottomSpawnerCenter[1] + UnityEngine.Random.Range(-bottomSpawnerExtents[1] / 2f, bottomSpawnerExtents[1] / 2f),
                    player.transform.position.z);

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                else if (randomSpawner == 3)
                {
                    Vector3 position = new Vector3(topSpawnerCenter[0] + UnityEngine.Random.Range(-topSpawnerExtents[0] / 2f, topSpawnerExtents[0] / 2f),
                    topSpawnerCenter[1] + UnityEngine.Random.Range(-topSpawnerExtents[1] / 2f, topSpawnerExtents[1] / 2f),
                    player.transform.position.z);

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                indexEnemiesZone++;
            }

            yield return new WaitForSeconds(spawningFrequencyZone);
        }
    }

}
