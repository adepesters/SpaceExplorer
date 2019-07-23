using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenario : MonoBehaviour
{
    [SerializeField] GameObject enemyPlanet1;

    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //enemyPlanet1.GetComponent<Collider2D>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
