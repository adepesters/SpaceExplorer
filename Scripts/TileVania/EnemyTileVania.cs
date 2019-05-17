using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyTileVania : MonoBehaviour
{
    float health = 400f;
    Color originalColor;
    float[] maxNumberOfParticles = new float[] { 4f, 4f, 4f, 4f, 4f, 4f, 4f };
    float[] probabilityOfParticles = new float[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
    Vector3 originalPos;

    const string PIXEL_BLOOD = "PixelBlood Parent";
    GameObject pixelBloodParent;

    List<Color> colorSet;

    Animator animator;
    bool beingHit;

    bool playerOnAir = false;

    PlayerTileVania player;
    Vector3 originalPlayerPos;
    ListOfBonuses ListOfBonuses;
    ColorClassifier colorClassifier;
    GameSession gameSession;

    float chanceOfCriticalHit = 0.2f;

    Vector3 targetPos;

    bool shouldBeKilled;

    // Start is called before the first frame update
    void Start()
    {
        pixelBloodParent = GameObject.Find(PIXEL_BLOOD);
        if (!pixelBloodParent)
        {
            pixelBloodParent = new GameObject(PIXEL_BLOOD);
        }

        originalColor = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
        originalPos = transform.position;
        AnalyzeColorsInSprite();

        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerTileVania>();
        ListOfBonuses = FindObjectOfType<ListOfBonuses>();
        gameSession = FindObjectOfType<GameSession>();
        colorClassifier = FindObjectOfType<ColorClassifier>();

        //assigning a slightly different z position to each object, to avoid clipping when camera is rotated
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + UnityEngine.Random.Range(0.051f, 0.059f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingHit)
        {
            transform.position = new Vector3(originalPos.x, transform.position.y, transform.position.z);
        }
        if (beingHit)
        {
            float step = 10f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (shouldBeKilled)
            {
                StartCoroutine(KillEnemyAfterDelay()); // to be sure the game object is destroyed after x seconds (eg in case it can't reach its 
                                                       // targetPos because it gets blocked by something on the way)
            }
            if (Vector3.Distance(transform.position, targetPos) < 1f)//Mathf.Epsilon)
            {
                beingHit = false;
                if (shouldBeKilled)
                {
                    KillEnemy();
                }
            }
            originalPos = targetPos;
        }
        //Debug.Log(beingHit);
    }

    IEnumerator KillEnemyAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);
        KillEnemy();
    }

    private void AnalyzeColorsInSprite()
    {
        Sprite enemySprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.tag == gameObject.tag) // check if on same layer than player
        {
            if (collision.gameObject.name.Contains("Sword"))
            {
                ContactPoint2D[] contact = new ContactPoint2D[1];
                collision.GetContacts(contact);

                ProcessHit(collision, contact[0].point);
            }
        }
    }

    private void ProcessHit(Collision2D collision, Vector2 contactPoint)
    {
        health -= collision.gameObject.GetComponent<Sword>().GetDamage();
        collision.gameObject.GetComponent<Sword>().SetCanHit(false);
        if (health <= 0)
        {
            StartCoroutine(SFXCriticalHit());
        }
        else
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < chanceOfCriticalHit)
            {
                StartCoroutine(SFXCriticalHit());
            }
            else
            {
                StartCoroutine(SFXNormalHit());
            }
        }
        BleedParticles(contactPoint);
        originalPlayerPos = player.transform.position;
        StartCoroutine(ElevatePlayer());
    }

    IEnumerator ElevatePlayer()
    {
        player.SetPlayerOnAir(true, originalPlayerPos);
        yield return new WaitForSeconds(0.3f);
        player.SetPlayerOnAir(false, originalPlayerPos);
    }

    IEnumerator SFXNormalHit()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        animator.speed = 0f;
        FindObjectOfType<Sword>().SpeedAnimation = 0f;
        player.SetFrozenPlayer(true, player.transform.position);
        yield return new WaitForSeconds(0.05f);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        animator.speed = 1f;
        if (FindObjectOfType<Sword>() != null)
        {
            FindObjectOfType<Sword>().SpeedAnimation = 1f;
        }
        player.SetFrozenPlayer(false, player.transform.position);
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    IEnumerator SFXCriticalHit()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        animator.speed = 0f;
        FindObjectOfType<Sword>().SpeedAnimation = 0f;
        //player.GetComponent<Animator>().SetBool("criticalHit", true);
        FindObjectOfType<Sword>().GetComponent<CameraShaker>().enabled = true;
        player.SetFrozenPlayer(true, player.transform.position);
        yield return new WaitForSeconds(0.15f);
        //player.GetComponent<Animator>().SetBool("criticalHit", false);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        animator.speed = 1f;
        beingHit = true;
        if (player.SwordHitDirection == "left")
        {
            targetPos = new Vector3(transform.position.x - 3f, transform.position.y + 0.75f, transform.position.z);
        }
        else if (player.SwordHitDirection == "right")
        {
            targetPos = new Vector3(transform.position.x + 3f, transform.position.y + 0.75f, transform.position.z);
        }
        if (FindObjectOfType<Sword>() != null)
        {
            FindObjectOfType<Sword>().SpeedAnimation = 1f;
        }
        player.SetFrozenPlayer(false, player.transform.position);
        if (health <= 0)
        {
            shouldBeKilled = true;
        }
    }


    private void BleedParticles(Vector2 contactPoint)
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

                    Vector3 bonusPos = new Vector3(contactPoint.x, contactPoint.y, 0.01f);
                    GameObject bloodPixel = Instantiate(listOfBonuses[bonusIndex], bonusPos, Quaternion.identity, pixelBloodParent.transform);
                    bloodPixel.GetComponent<SpriteRenderer>().color = randomColor;
                    if (gameSession != null)
                    {
                        gameSession.CounterPixelBlood[colorClassifier.WhatColorIsThat(randomColor)]++;
                    }
                    bloodPixel.tag = gameObject.tag;
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

    private void KillEnemy()
    {
        Destroy(gameObject);
        player.SetFrozenPlayer(false, player.transform.position);
        if (FindObjectOfType<Sword>() != null)
        {
            FindObjectOfType<Sword>().SpeedAnimation = 1f;
        }
        player.SetPlayerOnAir(false, originalPlayerPos);
    }
}
