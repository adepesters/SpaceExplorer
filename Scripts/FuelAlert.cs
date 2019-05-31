using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelAlert : MonoBehaviour
{
    Player player;
    GameSession gameSession;
    Image image;
    GameObject[] planets;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        int numberOfCompletedPlanets = 0;
        int counterUnreachablePlanets = 0;
        planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject planet in planets)
        {
            if (gameSession.HasBeenCompleted[planet.GetComponent<Planet>().PlanetID])
            {
                if (Vector2.Distance(planet.transform.position, player.transform.position) > gameSession.CurrentFuelSpacePlayer)
                {
                    counterUnreachablePlanets++;
                }
                numberOfCompletedPlanets++;
            }
        }

        if (counterUnreachablePlanets == numberOfCompletedPlanets)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
    }
}
