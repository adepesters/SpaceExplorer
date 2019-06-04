using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceAsteroidFragment : MonoBehaviour
{
    [SerializeField] GameObject virtualCollider;
    [SerializeField] GameObject pixelPrefab;

    float health = 1000f;

    Player player;
    GameSession gameSession;
    ColorClassifier colorClassifier;

    int maxNumOfPixelsPerHit = 3;
    int maxNumOfPixelsWhenDestructed = 30;

    float jitterPosPixelBlood = 0.3f;

    float explosionSpeed = 20f;
    float maxDistance;
    float parentAsteroidSize;
    Vector2 initialPos;
    float spreadingAngle;

    Color originalColor;
    List<Color> colorSet;

    const string PIXEL_BLOOD = "PixelBlood Parent";
    GameObject pixelBloodParent;

    public float ParentAsteroidSize { get => parentAsteroidSize; set => parentAsteroidSize = value; }

    // Start is called before the first frame update
    void Start()
    {
        pixelBloodParent = GameObject.Find(PIXEL_BLOOD);
        if (!pixelBloodParent)
        {
            pixelBloodParent = new GameObject(PIXEL_BLOOD);
        }

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        colorClassifier = GameObject.FindWithTag("ColorClassifier").GetComponent<ColorClassifier>();

        originalColor = GetComponent<SpriteRenderer>().color;
        AnalyzeColorsInSprite();

        // params for explosion/spread of bonuses
        spreadingAngle = UnityEngine.Random.Range(0, 360);
        initialPos = new Vector2(transform.position.x, transform.position.y);
        maxDistance = ParentAsteroidSize / UnityEngine.Random.Range(2f, 8f);

        health = Random.Range(600, 1400);
    }

    // Update is called once per frame
    void Update()
    {
        Spread();
    }

    private void Spread()
    {
        float deltaX;
        float deltaY;

        if (Vector2.Distance(initialPos, transform.position) < maxDistance)
        {
            deltaX = Mathf.Cos(spreadingAngle) * explosionSpeed * Time.deltaTime;
            deltaY = Mathf.Sin(spreadingAngle) * explosionSpeed * Time.deltaTime;
            transform.position += new Vector3(deltaX, deltaY, 0);
        }
    }

    private void AnalyzeColorsInSprite()
    {
        Sprite enemySprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = enemySprite.texture;
        colorSet = new List<Color>();

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y) * originalColor;
                if (pixelColor.a == 1f)
                {
                    colorSet.Add(pixelColor);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name.Contains("Laser") || collision.gameObject.name.Contains("Bomb")) && collision.gameObject.name.Contains("Player"))
        {
            GameObject newVirtualCollider = Instantiate(virtualCollider, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(newVirtualCollider, 0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("VirtualCollider"))
        {
            ContactPoint2D[] contact = new ContactPoint2D[1];
            collision.GetContacts(contact);
            ProcessHit(collision, contact[0].point);
            Destroy(collision.gameObject);
        }

    }

    private void ProcessHit(Collision2D collision, Vector2 contactPoint)
    {
        health -= player.LaserDamage;
        if (health <= 0)
        {
            ExplodeAsteroid();
        }
        BleedParticles(contactPoint);
    }

    void ExplodeAsteroid()
    {
        Destroy(gameObject);

        System.Random randomizer = new System.Random();

        for (int i = 0; i < Random.Range(5, maxNumOfPixelsWhenDestructed); i++)
        {
            Color randomColor = colorSet[randomizer.Next(colorSet.Count)];

            Vector3 bonusPos = new Vector3(
            Random.Range(transform.position.x - jitterPosPixelBlood, transform.position.x + jitterPosPixelBlood),
            Random.Range(transform.position.y - jitterPosPixelBlood, transform.position.y + jitterPosPixelBlood),
                transform.position.z);
            GameObject bloodPixel = Instantiate(pixelPrefab, bonusPos, Quaternion.identity, pixelBloodParent.transform);
            bloodPixel.GetComponent<SpriteRenderer>().color = randomColor;
            bloodPixel.GetComponent<Bonus>().SetEnemySize(GetComponent<Renderer>().bounds.extents.magnitude);
            if (gameSession != null)
            {
                gameSession.CounterPixelBlood[colorClassifier.WhatColorIsThat(randomColor)]++;
            }
        }
    }


    private void BleedParticles(Vector2 contactPoint)
    {
        int numOfPixels = Random.Range(1, maxNumOfPixelsPerHit);
        System.Random randomizer = new System.Random();

        for (int i = 0; i < numOfPixels; i++)
        {
            Color randomColor = colorSet[randomizer.Next(colorSet.Count)];

            Vector3 bonusPos = new Vector3(contactPoint.x, contactPoint.y, transform.position.z - 0.01f);
            GameObject bloodPixel = Instantiate(pixelPrefab, bonusPos, Quaternion.identity, pixelBloodParent.transform);
            bloodPixel.GetComponent<SpriteRenderer>().color = randomColor;
            bloodPixel.GetComponent<Bonus>().SetEnemySize(GetComponent<Renderer>().bounds.extents.magnitude - 2f);
            if (gameSession != null)
            {
                gameSession.CounterPixelBlood[colorClassifier.WhatColorIsThat(randomColor)]++;
            }
        }
    }
}
