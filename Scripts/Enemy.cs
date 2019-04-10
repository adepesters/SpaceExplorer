﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int pointsForDestruction = 100;

    [Header("Shooting")]
    [SerializeField] float minTimeBetweenShots = 1f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] Laser laserPrefab;
    [SerializeField] float projectileSpeed = -25f;
    [SerializeField] float shotCounter;

    [Header("VFX")]
    [SerializeField] GameObject destroyVFXParticles;
    [SerializeField] float destroyParticlesAfterXTime = 1f;
    [SerializeField] Color hitColorChange;
    [SerializeField] float changeColorTime = 0.1f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathEnemy;
    [SerializeField] AudioClip laserEnemy;
    [SerializeField] [Range(0, 1)] float volumeLaserEnemy = 0.5f;
    [SerializeField] [Range(0, 1)] float volumeDeathEnemy = 1f;

    [Header("Bonus At Kill")]
    [SerializeField] int maxNumberOfBronzeStar;
    [SerializeField] float probabilityOfBronzeStar;
    [SerializeField] int maxNumberOfSilverStar;
    [SerializeField] float probabilityOfSilverStar;
    [SerializeField] int maxNumberOfGoldStar;
    [SerializeField] float probabilityOfGoldStar;
    [SerializeField] int maxNumberOfPowerUp1;
    [SerializeField] float probabilityOfPowerUp1;
    [SerializeField] int maxNumberOfPowerUp2;
    [SerializeField] float probabilityOfPowerUp2;
    [SerializeField] int maxNumberOfPowerUp3;
    [SerializeField] float probabilityOfPowerUp3;
    [SerializeField] float jitter = 0.5f;

    // to face player
    Player player;
    float facingSpeed = 10f;

    // parent game object
    const string ENEMY_LASERS = "Enemy Lasers Parent";
    public GameObject enemyLasersParent;

    const string BONUSES = "Bonuses Parent";
    GameObject bonusesParent;

    bool isDead = false;

    bool isImmobile;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        hitColorChange.r = 255f;
        hitColorChange.g = 0f;
        hitColorChange.b = 0f;
        hitColorChange.a = 255f;

        player = FindObjectOfType<Player>();

        // create parents
        enemyLasersParent = GameObject.Find(ENEMY_LASERS);
        if (!enemyLasersParent)
        {
            enemyLasersParent = new GameObject(ENEMY_LASERS);
        }

        bonusesParent = GameObject.Find(BONUSES);
        if (!bonusesParent)
        {
            bonusesParent = new GameObject(BONUSES);
        }

        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isImmobile)
        {
            CountDownAndShoot();
        }
        //FacePlayer();
    }

    //void FacePlayer()
    //{
    //    Transform target = player.gameObject.transform;
    //    Vector2 direction = target.position - transform.position;
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
    //    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
    //}

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        Laser laser = Instantiate(laserPrefab, transform.position, transform.rotation, enemyLasersParent.transform);
        laser.SetParent(gameObject);
        AudioSource.PlayClipAtPoint(laserEnemy, transform.position, volumeLaserEnemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageDealer>())
        {
            DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
            if (collision.gameObject.name != "Player")
            {
                ProcessHit(damageDealer);
            }
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        StartCoroutine(ChangeColorAfterHit());
        if (health <= 0 && !isDead) // the isDead is necessary to prevent going into this if condition
                                    // many times at once, which will happen is the fire power of the player is very high (which will cause health
                                    // to decrease much more than 0 before being able to destroy the gameObject)
        {
            isDead = true;
            ExplodeEnemy();
            DistributeBonuses();
        }
    }

    private IEnumerator ChangeColorAfterHit()
    {
        gameObject.GetComponent<Renderer>().material.color = hitColorChange;
        yield return new WaitForSeconds(changeColorTime);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void ExplodeEnemy()
    {
        GameObject vFXParticles = Instantiate(destroyVFXParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        FindObjectOfType<GameSession>().score += pointsForDestruction;
        VFXEnemyDeath(vFXParticles);
    }

    private void DistributeBonuses()
    {
        for (int i = 0; i < maxNumberOfGoldStar; i++)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < probabilityOfGoldStar)
            {
                Vector3 positionStar = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                transform.position.z);
                Instantiate(FindObjectOfType<ListOfBonuses>().listOfBonuses[2], positionStar, Quaternion.identity, bonusesParent.transform);
            }
        }

        for (int i = 0; i < maxNumberOfSilverStar; i++)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < probabilityOfSilverStar)
            {
                Vector3 positionStar = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                transform.position.z);
                Instantiate(FindObjectOfType<ListOfBonuses>().listOfBonuses[1], positionStar, Quaternion.identity, bonusesParent.transform);
            }
        }

        for (int i = 0; i < maxNumberOfBronzeStar; i++)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < probabilityOfBronzeStar)
            {
                Vector3 positionStar = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                transform.position.z);
                GameObject tmpBonus = Instantiate(FindObjectOfType<ListOfBonuses>().listOfBonuses[0], positionStar, Quaternion.identity, bonusesParent.transform);
                Bonus newBonus = tmpBonus.GetComponent<Bonus>();
                newBonus.SetEnemySize(GetComponent<Renderer>().bounds.extents.magnitude);
            }
        }

        for (int i = 0; i < maxNumberOfPowerUp1; i++)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < probabilityOfPowerUp1)
            {
                Vector3 positionStar = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                transform.position.z);
                Instantiate(FindObjectOfType<ListOfBonuses>().listOfBonuses[3], positionStar, Quaternion.identity, bonusesParent.transform);
            }
        }

        for (int i = 0; i < maxNumberOfPowerUp2; i++)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < probabilityOfPowerUp2)
            {
                Vector3 positionStar = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                transform.position.z);
                Instantiate(FindObjectOfType<ListOfBonuses>().listOfBonuses[4], positionStar, Quaternion.identity, bonusesParent.transform);
            }
        }

        for (int i = 0; i < maxNumberOfPowerUp3; i++)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < probabilityOfPowerUp3)
            {
                Vector3 positionStar = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                transform.position.z);
                Instantiate(FindObjectOfType<ListOfBonuses>().listOfBonuses[5], positionStar, Quaternion.identity, bonusesParent.transform);
            }
        }

    }

    private void VFXEnemyDeath(GameObject vFXParticles)
    {
        AudioSource.PlayClipAtPoint(deathEnemy, Camera.main.gameObject.transform.position, volumeDeathEnemy);
        Destroy(vFXParticles, destroyParticlesAfterXTime);
    }

    public void SetImmobile(bool currentImmobility)
    {
        isImmobile = currentImmobility;
    }

}