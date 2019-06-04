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

    AudioSource[] musicTracks = new AudioSource[5];

    bool canDestroyLasers = false;

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

        musicTracks[1] = GameObject.FindWithTag("Track1").GetComponent<AudioSource>();
        musicTracks[2] = GameObject.FindWithTag("Track2").GetComponent<AudioSource>();
        musicTracks[3] = GameObject.FindWithTag("Track3").GetComponent<AudioSource>();
        musicTracks[4] = GameObject.FindWithTag("Track4").GetComponent<AudioSource>();
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
            shouldSpawn = true;
            canDestroyLasers = true;

            musicTracks[2].volume -= 0.3f * Time.deltaTime;
            musicTracks[3].volume -= 0.3f * Time.deltaTime;
            musicTracks[4].volume += 0.2f * Time.deltaTime;

            musicTracks[2].volume =
            Mathf.Clamp(musicTracks[2].volume, 0, 1);
            musicTracks[3].volume =
            Mathf.Clamp(musicTracks[3].volume, 0, 1);
            musicTracks[4].volume =
            Mathf.Clamp(musicTracks[4].volume, 0, 1);

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

            shouldSpawn = false;

            musicTracks[2].volume += 0.1f * Time.deltaTime;
            musicTracks[3].volume += 0.1f * Time.deltaTime;
            musicTracks[4].volume -= 0.2f * Time.deltaTime;

            musicTracks[2].volume =
            Mathf.Clamp(musicTracks[2].volume, 0, 1);
            musicTracks[3].volume =
            Mathf.Clamp(musicTracks[3].volume, 0, 1);
            musicTracks[4].volume =
            Mathf.Clamp(musicTracks[4].volume, 0, 1);
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
                    Vector2 position = new Vector2(leftSpawnerCenter[0] + UnityEngine.Random.Range(-leftSpawnerExtents[0] / 2f, leftSpawnerExtents[0] / 2f),
                    leftSpawnerCenter[1] + UnityEngine.Random.Range(-leftSpawnerExtents[1] / 2f, leftSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                else if (randomSpawner == 1)
                {
                    Vector2 position = new Vector2(rightSpawnerCenter[0] + UnityEngine.Random.Range(-rightSpawnerExtents[0] / 2f, rightSpawnerExtents[0] / 2f),
                    rightSpawnerCenter[1] + UnityEngine.Random.Range(-rightSpawnerExtents[1] / 2f, rightSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                else if (randomSpawner == 2)
                {
                    Vector2 position = new Vector2(bottomSpawnerCenter[0] + UnityEngine.Random.Range(-bottomSpawnerExtents[0] / 2f, bottomSpawnerExtents[0] / 2f),
                    bottomSpawnerCenter[1] + UnityEngine.Random.Range(-bottomSpawnerExtents[1] / 2f, bottomSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                else if (randomSpawner == 3)
                {
                    Vector2 position = new Vector2(topSpawnerCenter[0] + UnityEngine.Random.Range(-topSpawnerExtents[0] / 2f, topSpawnerExtents[0] / 2f),
                    topSpawnerCenter[1] + UnityEngine.Random.Range(-topSpawnerExtents[1] / 2f, topSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zoneEnemiesParent.transform);
                }
                indexEnemiesZone++;
            }

            yield return new WaitForSeconds(spawningFrequencyZone);
        }
    }

}
