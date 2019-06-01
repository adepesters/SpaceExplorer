using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelRadar : MonoBehaviour
{
    GameObject targetZone;
    float facingSpeed = 5f;
    Player player;
    GameSession gameSession;
    Image image;
    GameObject[] planets;

    float distance = 10000f;

    public GameObject TargetZone { get => targetZone; set => targetZone = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        TargetZone = GameObject.Find("Pointer");
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject planet in planets)
        {
            if (gameSession.HasBeenCompleted[planet.GetComponent<Planet>().PlanetID])
            {
                if (Vector2.Distance(planet.transform.position, player.transform.position) < distance)
                {
                    TargetZone = planet;
                }
                distance = Vector2.Distance(planet.transform.position, player.transform.position);
            }
        }

        Transform target = TargetZone.gameObject.transform;
        Vector2 direction = target.position - player.gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 135;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
    }
}
