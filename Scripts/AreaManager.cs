using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
    GameObject zoneEntered;

    // coordinates spawners around player
    Vector2 leftSpawnerCenter;
    Vector2 leftSpawnerExtents;
    Vector2 rightSpawnerCenter;
    Vector2 rightSpawnerExtents;
    Vector2 topSpawnerCenter;
    Vector2 topSpawnerExtents;
    Vector2 bottomSpawnerCenter;
    Vector2 bottomSpawnerExtents;

    bool enteredZone2 = false;
    Coroutine spawnZone2;
    int indexEnemiesZone2 = 0;
    int maxNumEnemiesZone2 = 50;
    const string ZONE2_ENEMIES_PARENT = "Zone2 Enemies Parent";
    GameObject zone2EnemiesParent;
    bool zone2Cleaned = false;
    float spawningFrequencyZone2 = 0.7f;

    bool enteredZone3 = false;
    Coroutine spawnZone3;
    int indexEnemiesZone3 = 0;
    int maxNumEnemiesZone3 = 30;
    const string ZONE3_ENEMIES_PARENT = "Zone3 Enemies Parent";
    GameObject zone3EnemiesParent;
    bool zone3Cleaned = false;
    float spawningFrequencyZone3 = 0.3f;

    [SerializeField] GameObject areaClearedText;

    Coroutine areaCleanedRoutine;
    Coroutine appear;
    Coroutine disappear;

    float a = 0f;

    void Start()
    {
        zone2EnemiesParent = GameObject.Find(ZONE2_ENEMIES_PARENT);
        if (zone2EnemiesParent == null)
        {
            zone2EnemiesParent = new GameObject(ZONE2_ENEMIES_PARENT);
        }

        zone3EnemiesParent = GameObject.Find(ZONE3_ENEMIES_PARENT);
        if (zone3EnemiesParent == null)
        {
            zone3EnemiesParent = new GameObject(ZONE3_ENEMIES_PARENT);
        }

        Color color = new Color(1f, 0.9243603f, 0.2028302f, 0);
        areaClearedText.GetComponent<Text>().color = color;
    }

    void Update()
    {
        leftSpawnerCenter = GameObject.Find("Left Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        leftSpawnerExtents = GameObject.Find("Left Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;
        rightSpawnerCenter = GameObject.Find("Right Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        rightSpawnerExtents = GameObject.Find("Right Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;
        topSpawnerCenter = GameObject.Find("Top Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        topSpawnerExtents = GameObject.Find("Top Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;
        bottomSpawnerCenter = GameObject.Find("Bottom Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.center;
        bottomSpawnerExtents = GameObject.Find("Bottom Spawner").gameObject.GetComponent<BoxCollider2D>().bounds.extents;

        int numEnemiesZone2 = zone2EnemiesParent.transform.childCount;
        if (numEnemiesZone2 == 0 && indexEnemiesZone2 == maxNumEnemiesZone2 && zone2Cleaned == false)
        {
            zone2Cleaned = true;
            zoneEntered.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            if (areaCleanedRoutine == null)
            {
                areaCleanedRoutine = StartCoroutine(LaunchAreaClearedText());
                areaCleanedRoutine = null; // so that we can launch the routine again in the next cleared area
            }
        }

        int numEnemiesZone3 = zone3EnemiesParent.transform.childCount;
        if (numEnemiesZone3 == 0 && indexEnemiesZone3 == maxNumEnemiesZone3 && zone3Cleaned == false)
        {
            zone3Cleaned = true;
            zoneEntered.gameObject.GetComponent<PolygonCollider2D>().enabled = false;

            if (areaCleanedRoutine == null)
            {
                areaCleanedRoutine = StartCoroutine(LaunchAreaClearedText());
                areaCleanedRoutine = null;
            }
            GameObject.Find("Zone 3").gameObject.GetComponent<EnemyRadarActivator>().HasBeenCleared = true;
        }

        if (zone3Cleaned)
        {
            GameObject.Find("Track 2").GetComponent<AudioSource>().volume += 0.1f * Time.deltaTime;
            GameObject.Find("Track 3").GetComponent<AudioSource>().volume += 0.1f * Time.deltaTime;
            GameObject.Find("Track 4").GetComponent<AudioSource>().volume -= 0.2f * Time.deltaTime;

            GameObject.Find("Track 2").GetComponent<AudioSource>().volume =
            Mathf.Clamp(GameObject.Find("Track 2").GetComponent<AudioSource>().volume, 0, 1);
            GameObject.Find("Track 3").GetComponent<AudioSource>().volume =
            Mathf.Clamp(GameObject.Find("Track 3").GetComponent<AudioSource>().volume, 0, 1);
            GameObject.Find("Track 4").GetComponent<AudioSource>().volume =
            Mathf.Clamp(GameObject.Find("Track 4").GetComponent<AudioSource>().volume, 0, 1);

            StarfieldGeneratorFast[] starfieldGenerators = FindObjectsOfType<StarfieldGeneratorFast>();
            foreach (StarfieldGeneratorFast starfieldGenerator in starfieldGenerators)
            {
                starfieldGenerator.SetCanSpawn(true);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Zone"))
        {
            if (collision.gameObject.name.Contains("Zone 1"))
            {
                zoneEntered = GameObject.Find("Zone 1");
                zoneEntered.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                zoneEntered.gameObject.transform.GetComponentInChildren<EnemySpawner>().zoneEntered = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Zone"))
        {
            if (collision.gameObject.name.Contains("Zone 2"))
            {
                zoneEntered = GameObject.Find("Zone 2");
                enteredZone2 = true;
                if (spawnZone2 == null)
                {
                    spawnZone2 = StartCoroutine(SpawnZone2());
                }
            }

            if (collision.gameObject.name.Contains("Zone 3"))
            {
                zoneEntered = GameObject.Find("Zone 3");
                enteredZone3 = true;
                GameObject.Find("Zone 3").gameObject.GetComponent<EnemyRadarActivator>().HasBeenDiscovered = true;
                if (spawnZone3 == null)
                {
                    spawnZone3 = StartCoroutine(SpawnZone3());
                }

                GameObject.Find("Track 2").GetComponent<AudioSource>().volume -= 0.3f * Time.deltaTime;
                GameObject.Find("Track 3").GetComponent<AudioSource>().volume -= 0.3f * Time.deltaTime;
                GameObject.Find("Track 4").GetComponent<AudioSource>().volume += 0.2f * Time.deltaTime;

                GameObject.Find("Track 2").GetComponent<AudioSource>().volume =
                Mathf.Clamp(GameObject.Find("Track 2").GetComponent<AudioSource>().volume, 0, 1);
                GameObject.Find("Track 3").GetComponent<AudioSource>().volume =
                Mathf.Clamp(GameObject.Find("Track 3").GetComponent<AudioSource>().volume, 0, 1);
                GameObject.Find("Track 4").GetComponent<AudioSource>().volume =
                Mathf.Clamp(GameObject.Find("Track 4").GetComponent<AudioSource>().volume, 0, 1);

                MakeStarfieldDisappear[] starfields = FindObjectsOfType<MakeStarfieldDisappear>();
                foreach (MakeStarfieldDisappear starfield in starfields)
                {
                    starfield.SetTransparencyToZero(0f);
                }

                StarfieldGeneratorFast[] starfieldGenerators = FindObjectsOfType<StarfieldGeneratorFast>();
                foreach (StarfieldGeneratorFast starfieldGenerator in starfieldGenerators)
                {
                    //if (starfieldGenerator.GetSpeed() != 0)
                    //{
                    starfieldGenerator.SetCanSpawn(false);
                    // }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Zone"))
        {
            if (collision.gameObject.name.Contains("Zone 2"))
            {
                enteredZone2 = false;
                spawnZone2 = null;
                // indexEnemiesZone2 = 0; // in case we want to reset the count to zero when the player leaves the area
            }

            if (collision.gameObject.name.Contains("Zone 3"))
            {
                enteredZone3 = false;
                GameObject.Find("Zone 3").gameObject.GetComponent<EnemyRadarActivator>().HasBeenDiscovered = false;
                spawnZone3 = null;
                // indexEnemiesZone2 = 0;

                StarfieldGeneratorFast[] starfieldGenerators = FindObjectsOfType<StarfieldGeneratorFast>();
                foreach (StarfieldGeneratorFast starfieldGenerator in starfieldGenerators)
                {
                    starfieldGenerator.SetCanSpawn(true);
                }
            }
        }
    }

    private List<int> GetSpawnersCollidingWithArea()
    {
        GameObject spawnersAroundPlayer = GameObject.Find("Spawners Around Player");
        List<int> indexOfCollidingSpawners = new List<int>();
        int i = 0;
        foreach (Transform spawner in spawnersAroundPlayer.transform)
        {
            if (spawner.GetComponent<ColliderSpawner>().IsCollidingWithArea())
            {
                indexOfCollidingSpawners.Add(i);
            }
            else
            {
                indexOfCollidingSpawners.Remove(i);
            }
            i++;
        }

        return indexOfCollidingSpawners;
    }

    IEnumerator SpawnZone2()
    {
        while (enteredZone2)
        {
            if (indexEnemiesZone2 < maxNumEnemiesZone2)
            {
                int randomSpawnerIndex = UnityEngine.Random.Range(0, GetSpawnersCollidingWithArea().Count);
                int randomSpawner = GetSpawnersCollidingWithArea()[randomSpawnerIndex];
                var enemies = zoneEntered.GetComponentInChildren<RandomSpawnersList>().enemies;
                Enemy randomEnemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];

                if (randomSpawner == 0)
                {
                    Vector2 position = new Vector2(leftSpawnerCenter[0] + UnityEngine.Random.Range(-leftSpawnerExtents[0] / 2f, leftSpawnerExtents[0] / 2f),
                    leftSpawnerCenter[1] + UnityEngine.Random.Range(-leftSpawnerExtents[1] / 2f, leftSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone2EnemiesParent.transform);
                }
                else if (randomSpawner == 1)
                {
                    Vector2 position = new Vector2(rightSpawnerCenter[0] + UnityEngine.Random.Range(-rightSpawnerExtents[0] / 2f, rightSpawnerExtents[0] / 2f),
                    rightSpawnerCenter[1] + UnityEngine.Random.Range(-rightSpawnerExtents[1] / 2f, rightSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone2EnemiesParent.transform);
                }
                else if (randomSpawner == 2)
                {
                    Vector2 position = new Vector2(bottomSpawnerCenter[0] + UnityEngine.Random.Range(-bottomSpawnerExtents[0] / 2f, bottomSpawnerExtents[0] / 2f),
                    bottomSpawnerCenter[1] + UnityEngine.Random.Range(-bottomSpawnerExtents[1] / 2f, bottomSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone2EnemiesParent.transform);
                }
                else if (randomSpawner == 3)
                {
                    Vector2 position = new Vector2(topSpawnerCenter[0] + UnityEngine.Random.Range(-topSpawnerExtents[0] / 2f, topSpawnerExtents[0] / 2f),
                    topSpawnerCenter[1] + UnityEngine.Random.Range(-topSpawnerExtents[1] / 2f, topSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone2EnemiesParent.transform);
                }
                indexEnemiesZone2++;
            }
            yield return new WaitForSeconds(spawningFrequencyZone2);
        }
    }

    IEnumerator SpawnZone3()
    {
        while (enteredZone3)
        {
            if (indexEnemiesZone3 < maxNumEnemiesZone3)
            {
                int randomSpawnerIndex = UnityEngine.Random.Range(0, GetSpawnersCollidingWithArea().Count);
                int randomSpawner = GetSpawnersCollidingWithArea()[randomSpawnerIndex];
                var enemies = zoneEntered.GetComponentInChildren<RandomSpawnersList>().enemies;
                Enemy randomEnemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];

                if (randomSpawner == 0)
                {
                    Vector2 position = new Vector2(leftSpawnerCenter[0] + UnityEngine.Random.Range(-leftSpawnerExtents[0] / 2f, leftSpawnerExtents[0] / 2f),
                    leftSpawnerCenter[1] + UnityEngine.Random.Range(-leftSpawnerExtents[1] / 2f, leftSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone3EnemiesParent.transform);
                }
                else if (randomSpawner == 1)
                {
                    Vector2 position = new Vector2(rightSpawnerCenter[0] + UnityEngine.Random.Range(-rightSpawnerExtents[0] / 2f, rightSpawnerExtents[0] / 2f),
                    rightSpawnerCenter[1] + UnityEngine.Random.Range(-rightSpawnerExtents[1] / 2f, rightSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone3EnemiesParent.transform);
                }
                else if (randomSpawner == 2)
                {
                    Vector2 position = new Vector2(bottomSpawnerCenter[0] + UnityEngine.Random.Range(-bottomSpawnerExtents[0] / 2f, bottomSpawnerExtents[0] / 2f),
                    bottomSpawnerCenter[1] + UnityEngine.Random.Range(-bottomSpawnerExtents[1] / 2f, bottomSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone3EnemiesParent.transform);
                }
                else if (randomSpawner == 3)
                {
                    Vector2 position = new Vector2(topSpawnerCenter[0] + UnityEngine.Random.Range(-topSpawnerExtents[0] / 2f, topSpawnerExtents[0] / 2f),
                    topSpawnerCenter[1] + UnityEngine.Random.Range(-topSpawnerExtents[1] / 2f, topSpawnerExtents[1] / 2f));

                    Instantiate(randomEnemy, position, Quaternion.identity, zone3EnemiesParent.transform);
                }
                indexEnemiesZone3++;
            }
            yield return new WaitForSeconds(spawningFrequencyZone3);
        }
    }

    IEnumerator LaunchAreaClearedText()
    {
        appear = StartCoroutine(MakeAreaClearedAppear());
        yield return new WaitForSeconds(3);
        disappear = StartCoroutine(MakeAreaClearedDisappear());
    }

    IEnumerator MakeAreaClearedAppear()
    {
        if (disappear != null)
        {
            StopCoroutine(disappear);
        }
        while (true)
        {
            Color color = new Color(1f, 0.9243603f, 0.2028302f, a);
            areaClearedText.GetComponent<Text>().color = color;
            yield return new WaitForSeconds(0.01f);
            a += 0.01f;
            a = Mathf.Clamp(a, 0f, 1f);
        }
    }

    IEnumerator MakeAreaClearedDisappear()
    {
        if (appear != null)
        {
            StopCoroutine(appear);
        }
        while (true)
        {
            Color color = new Color(1f, 0.9243603f, 0.2028302f, a);
            areaClearedText.GetComponent<Text>().color = color;
            yield return new WaitForSeconds(0.01f);
            a -= 0.01f;
            a = Mathf.Clamp(a, 0f, 1f);
        }
    }

}
