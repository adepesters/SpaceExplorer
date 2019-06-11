using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SpawningEnemyArea : MonoBehaviour
{
    // coordinates spawners around player
    Vector2 leftSpawnerCenter;
    Vector2 leftSpawnerExtents;
    Vector2 rightSpawnerCenter;
    Vector2 rightSpawnerExtents;
    Vector2 topSpawnerCenter;
    Vector2 topSpawnerExtents;
    Vector2 bottomSpawnerCenter;
    Vector2 bottomSpawnerExtents;

    bool enteredZone = false;
    Coroutine spawnZoneCoroutineHandler;
    int indexEnemiesZone = 0;
    int maxNumEnemiesZone = 1;
    const string ZONE_ENEMIES_PARENT = "Zone Enemies Parent";
    GameObject zoneEnemiesParent;
    bool zoneCleaned = false;
    float spawningFrequencyZone = 0.3f;

    [SerializeField] GameObject areaClearedText;

    Coroutine areaCleanedRoutine;
    Coroutine appear;
    Coroutine disappear;

    float a = 0f;

    bool currentlyFighting = false;

    int spawningEnemyAreaID;

    GameSession gameSession;

    public bool EnteredZone { get => enteredZone; set => enteredZone = value; }
    public Coroutine SpawnZoneCoroutineHandler { get => spawnZoneCoroutineHandler; set => spawnZoneCoroutineHandler = value; }
    public bool CurrentlyFighting { get => currentlyFighting; set => currentlyFighting = value; }
    public bool ZoneCleaned { get => zoneCleaned; set => zoneCleaned = value; }
    public int SpawningEnemyAreaID { get => spawningEnemyAreaID; set => spawningEnemyAreaID = value; }

    void Awake()
    {
        string numbersOnly = Regex.Replace(this.gameObject.name, "[^0-9]", "");
        SpawningEnemyAreaID = int.Parse(numbersOnly);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        zoneEnemiesParent = GameObject.Find(ZONE_ENEMIES_PARENT);
        if (zoneEnemiesParent == null)
        {
            zoneEnemiesParent = new GameObject(ZONE_ENEMIES_PARENT);
        }

        Color color = new Color(1f, 0.9243603f, 0.2028302f, 0);
        areaClearedText.GetComponent<Text>().color = color;
        gameSession.IsCleaned[SpawningEnemyAreaID] = false;
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

        if (enteredZone)
        {
            CurrentlyFighting = true;

            if (SpawnZoneCoroutineHandler == null)
            {
                SpawnZoneCoroutineHandler = StartCoroutine(SpawnZone());
            }

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

        int numEnemiesZone = zoneEnemiesParent.transform.childCount;
        if (numEnemiesZone == 0 && indexEnemiesZone == maxNumEnemiesZone && ZoneCleaned == false)
        {
            ZoneCleaned = true;
            GetComponent<PolygonCollider2D>().enabled = false;

            if (areaCleanedRoutine == null)
            {
                areaCleanedRoutine = StartCoroutine(LaunchAreaClearedText());
                areaCleanedRoutine = null;
            }
            GetComponent<EnemyRadarActivator>().HasBeenCleared = true;
        }

        if (ZoneCleaned)
        {
            CurrentlyFighting = false;
            gameSession.IsCleaned[SpawningEnemyAreaID] = true;

            StarfieldGeneratorFast[] starfieldGenerators = FindObjectsOfType<StarfieldGeneratorFast>();
            foreach (StarfieldGeneratorFast starfieldGenerator in starfieldGenerators)
            {
                starfieldGenerator.SetCanSpawn(true);
            }

        }
    }

    private List<int> GetSpawnersCollidingWithArea()
    {
        GameObject spawnersAroundPlayer = GameObject.FindWithTag("SpawnersAroundPlayer");
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

    IEnumerator SpawnZone()
    {
        while (EnteredZone)
        {
            if (indexEnemiesZone < maxNumEnemiesZone)
            {
                int randomSpawnerIndex = UnityEngine.Random.Range(0, GetSpawnersCollidingWithArea().Count);
                int randomSpawner = GetSpawnersCollidingWithArea()[randomSpawnerIndex];
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
