using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    [SerializeField] int chestID;
    [SerializeField] bool isOpen = false;
    int planetID;
    [SerializeField] Sprite chestOpenSprite;

    float[] maxNumberOfParticles = new float[] { 100f, 100f, 100f, 100f, 100f, 100f, 100f };
    float[] probabilityOfParticles = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f };

    GameSession gameSession;
    PlayerTileVania player;

    ListOfBonuses ListOfBonuses;
    List<Color> colorSet;

    public bool IsOpen { get => isOpen; set => isOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        string numbersOnly = Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", "");
        planetID = int.Parse(numbersOnly);

        player = FindObjectOfType<PlayerTileVania>();

        ListOfBonuses = FindObjectOfType<ListOfBonuses>();

        GetComponent<ActionTrigger>().MyDelegate1 = OpenChest;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        isOpen = gameSession.OpenChests[planetID, chestID];
        if (isOpen)
        {
            GetComponentInParent<SpriteRenderer>().sprite = chestOpenSprite;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<ActionTrigger>().CanAppear = false;
            GetComponent<ActionTrigger>().DisableActionBox();
            player.XisActionTrigger1 = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        colorSet = new List<Color>();
        colorSet.Add(new Color(0, 0.4148602f, 1, 1));
        colorSet.Add(new Color(0, 0.8751855f, 1, 1));
        colorSet.Add(new Color(1, 0, 0.8056722f, 1));
        colorSet.Add(new Color(1, 0.7929319f, 0.5801887f, 1));
        colorSet.Add(new Color(1, 0, 0.4358644f, 1));
        colorSet.Add(new Color(1, 0.514151f, 0.6382167f, 1));
        colorSet.Add(new Color(1, 0.9897198f, 0.4481132f, 1));
    }

    void OpenChest()
    {
        isOpen = true;
        GetComponentInParent<SpriteRenderer>().sprite = chestOpenSprite;
        BleedParticles();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<ActionTrigger>().CanAppear = false;
        GetComponent<ActionTrigger>().DisableActionBox();
        gameSession.OpenChests[1, chestID] = true;
        player.XisActionTrigger1 = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void BleedParticles()
    {
        List<GameObject> listOfBonuses = ListOfBonuses.listOfBonuses;
        int numBonuses = listOfBonuses.Count;
        RandomizeOrder(listOfBonuses);
        System.Random randomizer = new System.Random();

        for (int bonusIndex = 0; bonusIndex < numBonuses; bonusIndex++)
        {
            for (int i = 0; i < maxNumberOfParticles[bonusIndex]; i++)
            {
                float rand = UnityEngine.Random.Range(0f, 1f);
                if (rand < probabilityOfParticles[bonusIndex])
                {
                    Color randomColor = colorSet[randomizer.Next(colorSet.Count)];

                    Vector3 bonusPos = new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + Random.Range(-0.3f, 0.3f), transform.position.z - 0.01f);
                    GameObject bloodPixel = Instantiate(listOfBonuses[bonusIndex], bonusPos, Quaternion.identity, transform);
                    bloodPixel.tag = gameObject.tag;
                    if (planetID == 1)
                    {
                        if (transform.position.z < 2)
                        {
                            bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 25f);
                        }
                        if (transform.position.z < 7 && transform.position.z > 3)
                        {
                            bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 25f);
                        }
                        if (transform.position.z > 9)
                        {
                            bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5f);
                        }
                    }
                    if (planetID == 2)
                    {
                        if (transform.position.z < 2)
                        {
                            bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5f);
                        }
                        if (transform.position.z < 7 && transform.position.z > 3)
                        {
                            bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5f);
                        }
                        if (transform.position.z > 9)
                        {
                            bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5f);
                        }
                    }

                    //bloodPixel.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 15f);
                    //bloodPixel.GetComponent<Collider2D>().enabled = false;
                    bloodPixel.GetComponent<SpriteRenderer>().color = randomColor;
                }
            }
        }
    }

    private static void RandomizeOrder(List<GameObject> listOfBonuses)
    {
        for (int i = 0; i < listOfBonuses.Count; i++)
        {
            GameObject temp = listOfBonuses[i];
            int randomIndex = UnityEngine.Random.Range(i, listOfBonuses.Count);
            listOfBonuses[i] = listOfBonuses[randomIndex];
            listOfBonuses[randomIndex] = temp;
        }
    }
}
