using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceAsteroid : MonoBehaviour
{
    [SerializeField] GameObject virtualCollider;
    [SerializeField] GameObject pixelPrefab;
    [SerializeField] GameObject[] asteroidFragmentPrefabs;

    float health = 3000f;

    Player player;
    GameSession gameSession;
    ColorClassifier colorClassifier;

    int maxNumOfPixelsPerHit = 3;
    int maxNumOfFragments = 10;

    float jitterPosPixelBlood = 1f;

    Color originalColor;
    List<Color> colorSet;

    const string PIXEL_BLOOD = "PixelBlood Parent";
    GameObject pixelBloodParent;

    float asteroidSize;

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

        asteroidSize = GetComponent<Renderer>().bounds.extents.magnitude;

        health = health * asteroidSize / 3.2f; // 3.2f is the magnitude of the default-sized asteroid
        maxNumOfFragments = (int)Mathf.Round(maxNumOfFragments * asteroidSize / 3.2f);
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

        int numOfFragments = Random.Range(maxNumOfFragments / 2, maxNumOfFragments);

        for (int i = 0; i < numOfFragments; i++)
        {
            GameObject fragmentPrefab = asteroidFragmentPrefabs[Random.Range(0, asteroidFragmentPrefabs.Length)];
            Vector3 fragmentPos = transform.position;
            GameObject newFragment = Instantiate(fragmentPrefab, fragmentPos, Quaternion.identity);
            newFragment.GetComponent<RessourceAsteroidFragment>().ParentAsteroidSize = asteroidSize;
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
                //                gameSession.CounterPixelBlood[colorClassifier.WhatColorIsThat(randomColor)]++;
            }
        }
    }
}
