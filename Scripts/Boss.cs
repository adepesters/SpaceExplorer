using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
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
    [SerializeField] float jitter = 0.5f;

    GameSession gameSession;
    ListOfBonuses listOfBonuses;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        listOfBonuses = FindObjectOfType<ListOfBonuses>();

        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        hitColorChange.r = 255f;
        hitColorChange.g = 0f;
        hitColorChange.b = 0f;
        hitColorChange.a = 255f;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

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
        Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);
        Debug.Log(randomX);
        Debug.Log(randomY);
        randomX = randomX / (Mathf.Abs(randomX) + Mathf.Abs(randomY));
        randomY = randomY / (Mathf.Abs(randomX) + Mathf.Abs(randomY));
        Debug.Log(randomX);
        Debug.Log(randomY);
        randomX *= projectileSpeed;
        randomY *= projectileSpeed;
        Debug.Log(randomX);
        Debug.Log(randomY);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(randomX, randomY);
        AudioSource.PlayClipAtPoint(laserEnemy, Camera.main.gameObject.transform.position, volumeLaserEnemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (collision.gameObject.name != "Player")
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        StartCoroutine(ChangeColorAfterHit());
        if (health <= 0)
        {
            ExplodeEnemy();
            DistributeBonuses();
        }
    }

    private IEnumerator ChangeColorAfterHit()
    {
        //Debug.Log("entered");
        //Debug.Log(hitColorChange);
        //Debug.Log(gameObject.GetComponent<Renderer>().material.color);
        gameObject.GetComponent<Renderer>().material.color = hitColorChange;
        Debug.Log(gameObject.GetComponent<Renderer>().material.color);
        yield return new WaitForSeconds(changeColorTime);
        Debug.Log(gameObject.GetComponent<Renderer>().material.color);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void ExplodeEnemy()
    {
        GameObject vFXParticles = Instantiate(destroyVFXParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        //        gameSession.score += pointsForDestruction;
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
                Instantiate(listOfBonuses.listOfBonuses[2], positionStar, Quaternion.identity);
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
                Instantiate(listOfBonuses.listOfBonuses[1], positionStar, Quaternion.identity);
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
                Instantiate(listOfBonuses.listOfBonuses[0], positionStar, Quaternion.identity);
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
                Instantiate(listOfBonuses.listOfBonuses[3], positionStar, Quaternion.identity);
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
                Instantiate(listOfBonuses.listOfBonuses[4], positionStar, Quaternion.identity);
            }
        }

    }

    private void VFXEnemyDeath(GameObject vFXParticles)
    {
        AudioSource.PlayClipAtPoint(deathEnemy, Camera.main.gameObject.transform.position, volumeDeathEnemy);
        Destroy(vFXParticles, destroyParticlesAfterXTime);
    }
}
