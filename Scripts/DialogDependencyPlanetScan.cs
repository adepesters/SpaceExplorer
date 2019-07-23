using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogDependencyPlanetScan : MonoBehaviour
{
    [SerializeField] GameObject dialogDependency; // dialog depends on condition to be met in dialogDependency to launch

    Vector3 offset;
    bool canActivate = false;
    int planetID;

    GameSession gameSession;

    public bool CanActivate { get => canActivate; set => canActivate = value; }

    // Start is called before the first frame update
    void Awake()
    {
        offset = new Vector3(-2, 1.3f, 0);
    }

    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        planetID = GetComponentInParent<Planet>().PlanetID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            if (!gameSession.IsCleaned[planetID]) // player can't scan planet yet
            {
                canActivate = false;
                GameObject.FindWithTag("LockImage").GetComponent<Image>().enabled = true;
                GameObject.FindWithTag("LockImage").GetComponent<RectTransform>().transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
            }
            else // player destroyed all enemies and can scan planet
            {
                canActivate = true;
                GameObject.FindWithTag("LockImage").GetComponent<Image>().enabled = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            if (!gameSession.IsCleaned[planetID]) // player can't scan planet yet
            {
                canActivate = false;
                GameObject.FindWithTag("LockImage").GetComponent<Image>().enabled = true;
                GameObject.FindWithTag("LockImage").GetComponent<RectTransform>().transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
            }
            else // player destroyed all enemies and can scan planet
            {
                canActivate = true;
                GameObject.FindWithTag("LockImage").GetComponent<Image>().enabled = false;
            }
        }
    }
}
