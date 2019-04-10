using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    const string ENEMIES_PARENT = "Enemies Parents";
    GameObject enemiesParent; // gameObject where ennemies will spawn in the hierarchy
    public bool zoneEntered = false;

    Coroutine spawnAllWaves;

    void Update()
    {
        if (zoneEntered)
        {
            if (waveConfigs.Count > 0) // otherwise there's a nasty bug that freezes unity
            {
                if (spawnAllWaves == null)
                {
                    enemiesParent = GameObject.Find(ENEMIES_PARENT);
                    if (!enemiesParent)
                    {
                        enemiesParent = new GameObject(ENEMIES_PARENT);
                    }
                    spawnAllWaves = StartCoroutine(SpawnAllWaves());
                }
            }
        }
    }

    private IEnumerator SpawnAllWaves()
    {
        do
        {
            for (int i = startingWave; i < waveConfigs.Count; i++)
            {
                var currentWave = waveConfigs[i];
                if (currentWave.GetIsBoss() == false)
                {
                    yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
                }
                else
                {
                    yield return new WaitForSeconds(10);
                    yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
                    while (FindObjectOfType<Boss>() != null)
                    {
                        yield return null;
                    }
                    yield return new WaitForSeconds(3);
                }
            }
        } while (looping);
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
            foreach (GameObject enemyType in waveConfig.GetEnemyPrefab())
            {
                var newEnemy = Instantiate(enemyType,
                 waveConfig.GetWaypoints()[0].transform.position,
                     Quaternion.identity);
                newEnemy.transform.parent = enemiesParent.transform;
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
            }
        }
    }
}
