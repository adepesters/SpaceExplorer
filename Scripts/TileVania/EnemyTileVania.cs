﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyTileVania : MonoBehaviour
{
    float health = 5000f;
    Color originalColor;
    float[] maxNumberOfParticles = new float[] { 4f, 4f, 4f, 4f, 4f, 4f, 4f };
    float[] probabilityOfParticles = new float[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
    Vector3 originalPos;

    const string PIXEL_BLOOD = "PixelBlood Parent";
    GameObject pixelBloodParent;

    HashSet<Color> colorSet;

    Animator animator;
    bool beingHit;

    bool playerOnAir = false;

    PlayerTileVania player;
    Vector2 originalPlayerPos;

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

        Sprite enemySprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = enemySprite.texture;
        colorSet = new HashSet<Color>();

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y) * transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
                if (pixelColor.a == 1f)
                {
                    colorSet.Add(pixelColor);
                }
            }
        }

        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerTileVania>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingHit)
        {
            transform.position = new Vector2(originalPos.x, transform.position.y);
        }
        Debug.Log(beingHit);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Sword"))
        {
            ContactPoint2D[] contact = new ContactPoint2D[1];
            collision.GetContacts(contact);

            ProcessHit(collision, contact[0].point);
            if (health <= 0)
            {
                KillEnemy();
            }
        }
    }


    private void ProcessHit(Collision2D collision, Vector2 contactPoint)
    {
        health -= collision.gameObject.GetComponent<Sword>().GetDamage();
        collision.gameObject.GetComponent<Sword>().SetCanHit(false);
        StartCoroutine(ChangeColorWhenHit());
        BleedParticles(contactPoint);
        originalPlayerPos = player.transform.position;
        StartCoroutine(ElevatePlayer(originalPlayerPos));
    }

    IEnumerator ElevatePlayer(Vector2 currentOriginalPlayerPos)
    {
        player.SetPlayerOnAir(true, currentOriginalPlayerPos);
        yield return new WaitForSeconds(1f);
        player.SetPlayerOnAir(false, currentOriginalPlayerPos);
    }

    IEnumerator ChangeColorWhenHit()
    {
        //Debug.Log(FindObjectOfType<Sword>().SpeedAnimation);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        animator.speed = 0f;
        FindObjectOfType<Sword>().SpeedAnimation = 0f;
        FindObjectOfType<PlayerTileVania>().SetFrozenPlayer(true, FindObjectOfType<PlayerTileVania>().transform.position);
        //beingHit = true;
        yield return new WaitForSeconds(0.15f);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        animator.speed = 1f;
        var targetPos = new Vector2(transform.position.x - 0.75f, transform.position.y + 0.75f);
        float step = 100f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
        if (Vector2.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            beingHit = false;
        }
        originalPos = targetPos;
        FindObjectOfType<Sword>().SpeedAnimation = 1f;
        FindObjectOfType<PlayerTileVania>().SetFrozenPlayer(false, FindObjectOfType<PlayerTileVania>().transform.position);
        //beingHit = false;
    }

    private void BleedParticles(Vector2 contactPoint)
    {
        List<GameObject> listOfBonuses = FindObjectOfType<ListOfBonuses>().listOfBonuses;
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
                    //Vector3 bonusPos = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                    //UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                    //transform.position.z);

                    Color[] asArray = colorSet.ToArray();
                    Color randomColor = asArray[randomizer.Next(asArray.Length)];

                    Vector3 bonusPos = new Vector3(contactPoint.x, contactPoint.y, transform.position.z);
                    GameObject bloodPixel = Instantiate(listOfBonuses[bonusIndex], bonusPos, Quaternion.identity, pixelBloodParent.transform);
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

    private void KillEnemy()
    {
        Destroy(gameObject);
    }


}
