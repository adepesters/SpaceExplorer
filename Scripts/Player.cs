using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;
    float acceleration;
    float deceleration;
    float maxSpeed;
    float originalMoveSpeed;

    // Avoid params
    float timeInvincibleDuringAvoid = 0.7f;
    float XOffsetDuringAvoid = 1.5f;

    [Header("Projectile")]
    [SerializeField] Laser laserPrefab;
    [SerializeField] BombPlayer bombPrefab;
    public float laserSpeed = 35f;
    public float originalLaserSpeed;
    public float laserFiringPeriod = 0.1f;
    public float originalLaserFiringPeriod;
    int laserDamage = 100;

    public float bombSpeed = 5f;
    public float originalBombSpeed;
    public float bombFiringPeriod = 0.5f;
    public float originalBombFiringPeriod;
    int bombDamage = 300;

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

    int count_damage;

    float rightStickX;
    float rightStickY;

    bool isFiringCoroutingActive = false;

    int newRotatedDirection = 0;

    bool isInvincible = false;

    bool isImmobile = false;

    bool hasBeganRotating = false;

    // cached references
    PS4ControllerCheck ps4ControllerCheck;
    Rigidbody2D rigidBody;

    // parent game object
    const string PLAYER_LASERS = "Player Lasers Parent";
    GameObject playerLasersParent;

    AttackStyle attackStyle;
    HitCanvas hitCanvas;

    float maxFuel = 1000f;
    float currentFuel;
    GameObject currentBase;
    Vector2 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        hitColorChange.r = 0f;
        hitColorChange.g = 0f;
        hitColorChange.b = 0f;
        hitColorChange.a = 0f;

        count_damage = 0;

        originalLaserSpeed = laserSpeed;
        originalLaserFiringPeriod = laserFiringPeriod;

        originalBombSpeed = bombSpeed;
        originalBombFiringPeriod = bombFiringPeriod;

        ps4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();
        rigidBody = GetComponent<Rigidbody2D>();
        attackStyle = FindObjectOfType<AttackStyle>();
        hitCanvas = FindObjectOfType<HitCanvas>();

        playerLasersParent = GameObject.Find(PLAYER_LASERS);
        if (!playerLasersParent)
        {
            playerLasersParent = new GameObject(PLAYER_LASERS);
        }

        originalMoveSpeed = moveSpeed;

        currentFuel = maxFuel;
        currentBase = GameObject.Find("Home Planet").gameObject;
        transform.position = currentBase.transform.position;
        oldPos = currentBase.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        //AvoidAndFire();

        if (damagePlayerVisualInstance != null)
        {
            damagePlayerVisualInstance.gameObject.transform.position = transform.position;
        }

        currentFuel -= Vector2.Distance(transform.position, oldPos);
        oldPos = transform.position;
        Debug.Log(currentFuel);
    }

    private void AvoidAndFire()
    {
        //if (ps4ControllerCheck.IsL1Pressed())
        //{
        //    StartCoroutine(AvoidAndGoLeft());
        //}

        //if (ps4ControllerCheck.IsR1Pressed())
        //{
        //    StartCoroutine(AvoidAndGoRight());
        //}

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
        Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity, playerLasersParent.transform);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(laserSpeed, 0);
    }

    private void FireLeftProjectile()
    {
        Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity, playerLasersParent.transform);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(-laserSpeed, 0);
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
        if (!isImmobile)
        {
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
                    hasBeganRotating = false;
                }
            }
            if (isFiringCoroutingActive == true)
            {
                float xVelocity = Input.GetAxis("Mouse X") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));
                float yVelocity = Input.GetAxis("Mouse Y") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));
                RotatePlayerToGivenDirection(xVelocity, yVelocity);
            }
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            float xVelocity = Input.GetAxis("Mouse X") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));
            float yVelocity = Input.GetAxis("Mouse Y") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));

            if (!hasBeganRotating)
            {
                RotatePlayerToGivenDirection(xVelocity, yVelocity);
                hasBeganRotating = true;
            }
            if (attackStyle.GetAttackStyle() == "lasers")
            {
                InstantiateLaser(xVelocity, yVelocity);
                yield return new WaitForSeconds(laserFiringPeriod);
            }
            else if (attackStyle.GetAttackStyle() == "bombs")
            {
                InstantiateBomb(xVelocity, yVelocity);
                yield return new WaitForSeconds(bombFiringPeriod);
            }
        }
    }

    private void RotatePlayerToGivenDirection(float xVelocity, float yVelocity)
    {
        Vector2 direction = new Vector2(xVelocity * 10000000, yVelocity * 10000000) + new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100f * Time.deltaTime);
    }

    private void InstantiateLaser(float xVelocity, float yVelocity)
    {
        var projectilePos = new Vector3(transform.position.x, transform.position.y, 0.1f);
        Laser projectile = Instantiate(laserPrefab, projectilePos, transform.rotation, playerLasersParent.transform);
        // projectileFiringPeriod = 0.1f;
        Vector2 directionOfFiring = new Vector2(xVelocity, yVelocity) * laserSpeed;
        projectile.GetComponent<Rigidbody2D>().velocity = directionOfFiring;
        AudioSource.PlayClipAtPoint(laserPlayer, transform.position, volumeLaserPlayer);
    }

    private void InstantiateBomb(float xVelocity, float yVelocity)
    {
        var projectilePos = new Vector3(transform.position.x, transform.position.y, 0.1f);
        BombPlayer projectile = Instantiate(bombPrefab, projectilePos, transform.rotation, playerLasersParent.transform);
        // projectileFiringPeriod = 0.5f;
        Vector2 directionOfFiring = new Vector2(xVelocity, yVelocity) * bombSpeed;
        projectile.GetComponent<Rigidbody2D>().velocity = directionOfFiring;
        AudioSource.PlayClipAtPoint(laserPlayer, transform.position, volumeLaserPlayer);
    }

    private void Move()
    {
        if (isImmobile)
        {
            var deltaX = 0;
            var deltaY = 0;

            rigidBody.velocity = new Vector2(deltaX, deltaY);
        }
        else
        {
            var deltaX = Input.GetAxis("Horizontal") * moveSpeed;
            var deltaY = Input.GetAxis("Vertical") * moveSpeed;

            rigidBody.velocity = new Vector2(deltaX, deltaY);

            //Debug.Log(rigidBody.velocity);

            bool accelerate = ps4ControllerCheck.ContinuousL1Press();

            if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.01))
            {
                if (accelerate)
                {
                    Accelerate();
                }
                else
                {
                    Decelerate();
                }
            }
            else
            {
                moveSpeed = originalMoveSpeed;
            }

            if (isFiringCoroutingActive != true && (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon || Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon))
            {
                RotatePlayerToGivenDirection(deltaX, deltaY);
            }
        }
    }

    private void Decelerate()
    {
        deceleration = 50f;
        moveSpeed -= acceleration * Time.deltaTime;
        moveSpeed = Mathf.Clamp(moveSpeed, originalMoveSpeed, 80f);
    }

    private void Accelerate()
    {
        acceleration = 25f;
        moveSpeed += acceleration * Time.deltaTime;
        moveSpeed = Mathf.Clamp(moveSpeed, moveSpeed, 80f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageDealer>())
        {
            DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
            int layer_collider = collision.gameObject.layer;
            ProcessHit(damageDealer, layer_collider);
        }
    }

    private void ProcessHit(DamageDealer damageDealer, int layer_collider)
    {
        if (!isInvincible)
        {
            health -= damageDealer.GetDamage();
            StartCoroutine(hitCanvas.HandleHitCanvas());
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
        if (count_damage < damagePlayerVisual.Count - 1)
        {
            Destroy(damagePlayerVisualInstance);
        }
        gameObject.GetComponent<Renderer>().material.color = hitColorChange;
        yield return new WaitForSeconds(changeColorTime);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        if (count_damage <= damagePlayerVisual.Count - 1)
        {
            damagePlayerVisualInstance = Instantiate(damagePlayerVisual[count_damage], transform.position, Quaternion.identity);
        }
        count_damage++;
        //Destroy(vFXParticles, destroyParticlesAfterXTime);
    }

    public int GetHealthPlayer()
    {
        return health;
    }

    public int GetDamageLaserPlayer()
    {
        return laserDamage;
    }

    public void SetLaserDamage(int newLaserDamage)
    {
        laserDamage = newLaserDamage;
    }

    public float GetLaserFiringPeriod()
    {
        return laserFiringPeriod;
    }

    public void SetInvincible(bool currentInvincibility)
    {
        isInvincible = currentInvincibility;
    }

    public void SetImmobile(bool currentImmobility)
    {
        isImmobile = currentImmobility;
    }

    public bool IsImmobile()
    {
        return isImmobile;
    }

    public void SetMoveSpeed(float currentMoveSpeed)
    {
        moveSpeed = currentMoveSpeed;
    }
}
