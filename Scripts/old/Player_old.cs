using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_old : MonoBehaviour
{
    // config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;

    // Avoid params
    float timeInvincibleDuringAvoid = 0.2f;
    float XOffsetDuringAvoid = 1.5f;

    [Header("Projectile")]
    [SerializeField] Laser laserPrefab;
    [SerializeField] public float projectileSpeed;
    public float originalProjectileSpeed;
    [SerializeField] public float projectileFiringPeriod = 0.2f;
    public float originalProjectileFiringPeriod;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathPlayer;
    [SerializeField] AudioClip laserPlayer;
    [SerializeField] [Range(0, 1)] float volumeLaserPlayer = 0.1f;
    [SerializeField] [Range(0, 1)] float volumeDeathPlayer = 1f;
    [SerializeField] AudioClip damagePlayer;
    [SerializeField] [Range(0, 1)] float volumeDamagePlayer = 1f;

    [Header("Damage Effects")]
    [SerializeField] Color hitColorChange;
    [SerializeField] float changeColorTime = 0.05f;
    [SerializeField] List<GameObject> damagePlayerVisual;
    [SerializeField] GameObject destroyVFXParticles;
    [SerializeField] float destroyParticlesAfterXTime = 1f;

    Coroutine firingCoroutine;

    GameObject damagePlayerVisualInstance;

    float xMin;
    float yMin;
    float xMax;
    float yMax;

    int count_damage;

    float rightStickX;
    float rightStickY;

    bool isFiringCoroutingActive = false;

    int newRotatedDirection = 0;

    bool isInvincible = false;

    // cached references

    PS4ControllerCheck ps4ControllerCheck;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        hitColorChange.r = 0f;
        hitColorChange.g = 0f;
        hitColorChange.b = 0f;
        hitColorChange.a = 0f;

        count_damage = 0;

        originalProjectileSpeed = projectileSpeed;
        originalProjectileFiringPeriod = projectileFiringPeriod;

        ps4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        AvoidAndFire();

        if (damagePlayerVisualInstance != null)
        {
            damagePlayerVisualInstance.gameObject.transform.position = transform.position;
        }
    }

    private void AvoidAndFire()
    {
        if (ps4ControllerCheck.IsL1Pressed())
        {
            StartCoroutine(AvoidAndGoLeft());
        }

        if (ps4ControllerCheck.IsR1Pressed())
        {
            StartCoroutine(AvoidAndGoRight());
        }

        if (ps4ControllerCheck.LongL1Press())
        {
            FireRightProjectile();
        }

        if (ps4ControllerCheck.LongR1Press())
        {
            FireLeftProjectile();
        }
    }

    private void FireRightProjectile()
    {
        Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
    }

    private void FireLeftProjectile()
    {
        Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
    }

    IEnumerator AvoidAndGoLeft()
    {
        isInvincible = true;
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPos = new Vector2(currentPos.x - XOffsetDuringAvoid, currentPos.y);
        transform.position = newPos;
        yield return new WaitForSeconds(timeInvincibleDuringAvoid);
        isInvincible = false;
    }

    IEnumerator AvoidAndGoRight()
    {
        isInvincible = true;
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPos = new Vector2(currentPos.x + XOffsetDuringAvoid, currentPos.y);
        transform.position = newPos;
        yield return new WaitForSeconds(timeInvincibleDuringAvoid);
        isInvincible = false;
    }

    private void Fire()
    {
        //if (Input.GetButtonDown("Fire1")) // this is a discrete event (even when we continuously press A, there is only a single event)
        //{
        //    firingCoroutine = StartCoroutine(FireContinuously());
        //}
        //if (Input.GetButtonUp("Fire1"))
        //{
        //    StopCoroutine(firingCoroutine);
        //}

        rightStickX = Input.GetAxis("Mouse X");
        rightStickY = Input.GetAxis("Mouse Y");

        if ((Mathf.Abs(rightStickX) > 0.01 || Mathf.Abs(rightStickY) > 0.01) && isFiringCoroutingActive == false)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
            isFiringCoroutingActive = true;
        }
        if ((Mathf.Abs(rightStickX) < 0.01 && Mathf.Abs(rightStickY) < 0.01) && isFiringCoroutingActive == true)
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                isFiringCoroutingActive = false;
            }
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            float xVelocity = Input.GetAxis("Mouse X") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));
            float yVelocity = Input.GetAxis("Mouse Y") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));

            Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            //Vector2 directionOfFiring = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * projectileSpeed;
            Vector2 directionOfFiring = new Vector2(xVelocity, yVelocity) * projectileSpeed;
            //directionOfFiring += rigidBody.velocity;
            //// Vector2 directionOfFiring = Vector;
            //if (Mathf.Sign(xVelocity) > 1)
            //{
            //    directionOfFiring.x = Math.Max(directionOfFiring.x, xVelocity * projectileSpeed);
            //}
            //if (Mathf.Sign(xVelocity) < 1)
            //{
            //    directionOfFiring.x = Math.Min(directionOfFiring.x, xVelocity * projectileSpeed);
            //}
            //if (Mathf.Sign(yVelocity) > 1)
            //{
            //    directionOfFiring.y = Math.Max(directionOfFiring.y, yVelocity * projectileSpeed);
            //}
            //if (Mathf.Sign(yVelocity) < 1)
            //{
            //    directionOfFiring.y = Math.Min(directionOfFiring.y, yVelocity * projectileSpeed);
            //}
            //directionOfFiring.x = Mathf.Sign(xVelocity) * Mathf.Max(Mathf.Abs(tmp.x), Mathf.Abs(xVelocity * projectileSpeed));
            //directionOfFiring.y = Mathf.Sign(yVelocity) * Mathf.Max(Mathf.Abs(tmp.y), Mathf.Abs(yVelocity * projectileSpeed));
            laser.GetComponent<Rigidbody2D>().velocity = directionOfFiring;
            AudioSource.PlayClipAtPoint(laserPlayer, Camera.main.gameObject.transform.position, volumeLaserPlayer);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {

        var deltaX = Input.GetAxis("Horizontal") * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * moveSpeed;

        rigidBody.velocity = new Vector2(deltaX, deltaY);
        Debug.Log("speed player: " + rigidBody.velocity.x + " " + rigidBody.velocity.y);

        var newXPos = transform.position.x + deltaX;
        var newYPos = transform.position.y + deltaY;

        //transform.position = new Vector2(newXPos, newYPos);

        //transform.position = new Vector2(newXPos, transform.position.y);

        //Vector3 targetPosition = transform.position;
        //Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        //targetRotation.x = 0;
        //targetRotation.y = 0;
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);

        //float xVelocity = Input.GetAxis("Horizontal") / (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));
        //float yVelocity = Input.GetAxis("Vertical") / (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));

        //Vector2 refVector = new Vector2(0, 1);
        //Vector2 directionVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        ////Debug.Log("vector:" + directionVector);
        ////Debug.Log("angle:" + Vector2.Angle(refVector, directionVector));

        //int rotatedDirection = Mathf.RoundToInt(Vector2.Angle(refVector, directionVector));
        //if (rotatedDirection != newRotatedDirection)
        //{
        //    newRotatedDirection = rotatedDirection;
        //    Debug.Log("changed");
        //    transform.Rotate(0, 0, rotatedDirection);
        //}
        //// transform.Rotate(0, 0, Vector2.Angle(refVector, directionVector));
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        int layer_collider = collision.gameObject.layer;
        ProcessHit(damageDealer, layer_collider);
    }

    private void ProcessHit(DamageDealer damageDealer, int layer_collider)
    {
        if (!isInvincible)
        {
            health -= damageDealer.Damage;
        }

        if (layer_collider != 9)
        {
            damageDealer.Hit();
        }
        if (health <= 0)
        {
            DeathVFX();
            FindObjectOfType<Level>().LoadGameOver();
            FindObjectOfType<GameSession>().counterStarBronze = 0;
            FindObjectOfType<GameSession>().counterStarSilver = 0;
            FindObjectOfType<GameSession>().counterStarGold = 0;
        }
        else
        {
            DamageVFX();
        }
    }

    private void DeathVFX()
    {
        GameObject vFXParticles = Instantiate(destroyVFXParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(damagePlayerVisualInstance);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathPlayer, Camera.main.gameObject.transform.position, volumeDeathPlayer);
        Destroy(vFXParticles, destroyParticlesAfterXTime);
    }

    private void DamageVFX()
    {
        StartCoroutine(ShowDamageAfterHit());
        //ShowDamageAfterHit();
        AudioSource.PlayClipAtPoint(damagePlayer, Camera.main.gameObject.transform.position, volumeDamagePlayer);
    }

    private IEnumerator ShowDamageAfterHit()
    {
        //GameObject vFXParticles = Instantiate(destroyVFXParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(damagePlayerVisualInstance);
        gameObject.GetComponent<Renderer>().material.color = hitColorChange;
        yield return new WaitForSeconds(changeColorTime);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        damagePlayerVisualInstance = Instantiate(damagePlayerVisual[count_damage], transform.position, Quaternion.identity);
        count_damage++;
        //Destroy(vFXParticles, destroyParticlesAfterXTime);
    }

    public int ReturnHealthPlayer()
    {
        return health;
    }
}
