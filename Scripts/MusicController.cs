using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource[] musicTracks = new AudioSource[5];

    LowFuelEnemyActivator lowFuelEnemyActivator;
    GameObject[] spawningEnemyAreas;

    int mode = 0; // default exploration music tracks

    int fighting = 0;

    // Start is called before the first frame update
    void Start()
    {
        lowFuelEnemyActivator = GameObject.FindWithTag("LowFuelEnemyActivator").GetComponent<LowFuelEnemyActivator>();
        spawningEnemyAreas = GameObject.FindGameObjectsWithTag("SpawningEnemyArea");

        musicTracks[1] = GameObject.FindWithTag("Track1").GetComponent<AudioSource>();
        musicTracks[2] = GameObject.FindWithTag("Track2").GetComponent<AudioSource>();
        musicTracks[3] = GameObject.FindWithTag("Track3").GetComponent<AudioSource>();
        musicTracks[4] = GameObject.FindWithTag("Track4").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCurrentlyFighting();

        if (fighting > 0)
        {
            mode = 1;
        }
        else
        {
            mode = 0;
        }


        switch (mode)
        {
            case 0:
                DefaultMode();
                break;

            case 1:
                FightingMode();
                break;
        }
    }

    private void FightingMode()
    {
        musicTracks[2].volume -= 0.3f * Time.deltaTime;
        musicTracks[3].volume -= 0.3f * Time.deltaTime;
        musicTracks[4].volume += 0.2f * Time.deltaTime;

        musicTracks[2].volume =
        Mathf.Clamp(musicTracks[2].volume, 0, 1);
        musicTracks[3].volume =
        Mathf.Clamp(musicTracks[3].volume, 0, 1);
        musicTracks[4].volume =
        Mathf.Clamp(musicTracks[4].volume, 0, 1);
    }

    private void DefaultMode()
    {
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

    private void CheckIfCurrentlyFighting()
    {
        fighting = 0;
        if (lowFuelEnemyActivator.CurrentlyFighting)
        {
            fighting++;
        }
        else
        {
            foreach (GameObject spawningEnemyArea in spawningEnemyAreas)
            {
                if (spawningEnemyArea.GetComponent<SpawningEnemyArea>().CurrentlyFighting)
                {
                    fighting++;
                }
            }
        }
    }
}
