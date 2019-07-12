using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float padding = 0.5f;
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
    private float laserSpeed = 35f;
    private float originalLaserSpeed;
    private float laserFiringPeriod = 0.1f;
    private float originalLaserFiringPeriod;
    int laserDamage = 200;

    private float bombSpeed = 5f;
    private float originalBombSpeed;
    private float bombFiringPeriod = 0.5f;
    private float originalBombFiringPeriod;
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

    GameObject currentBase;
    Vector2 oldPos;

    GameSession gameSession;

    SpawningEnemyArea zoneEntered;

    AudioSource audiosource;

    // to move across layers
    bool moveToBackLayer;
    bool moveToFrontLayer;
    Vector3 currentPosition;
    float speedBridge = 80f;
    int currentLayer = 1;
    [SerializeField] AudioClip layerChange;
    float volumeSoundLayerChange = 0.2f;
    float layer1Depth = 0f;
    float layer2Depth = 25f;
    float layer3Depth = 50f;
    Rigidbody2D[] rigidbodyObjects;
    Collider2D[] colliderObjects;

    // display position on Space UI
    GameObject xcoordinate, ycoordinate, zcoordinate;

    public float LaserSpeed { get => laserSpeed; set => laserSpeed = value; }
    public float OriginalLaserSpeed { get => originalLaserSpeed; set => originalLaserSpeed = value; }
    public float LaserFiringPeriod { get => laserFiringPeriod; set => laserFiringPeriod = value; }
    public float OriginalLaserFiringPeriod { get => originalLaserFiringPeriod; set => originalLaserFiringPeriod = value; }
    public int LaserDamage { get => laserDamage; set => laserDamage = value; }
    public float BombSpeed { get => bombSpeed; set => bombSpeed = value; }
    public float OriginalBombSpeed { get => originalBombSpeed; set => originalBombSpeed = value; }
    public float OriginalBombFiringPeriod { get => originalBombFiringPeriod; set => originalBombFiringPeriod = value; }
    public float BombFiringPeriod { get => bombFiringPeriod; set => bombFiringPeriod = value; }
    public int BombDamage { get => bombDamage; set => bombDamage = value; }
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public bool IsImmobile1 { get => isImmobile; set => isImmobile = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public int CurrentLayer { get => currentLayer; set => currentLayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        hitColorChange.r = 1f;
        hitColorChange.g = 0f;
        hitColorChange.b = 0f;
        hitColorChange.a = 0f;

        count_damage = 0;

        OriginalLaserSpeed = LaserSpeed;
        OriginalLaserFiringPeriod = LaserFiringPeriod;

        OriginalBombSpeed = BombSpeed;
        OriginalBombFiringPeriod = BombFiringPeriod;

        ps4ControllerCheck = GameObject.FindWithTag("PS4ControllerCheck").GetComponent<PS4ControllerCheck>();
        rigidBody = GetComponent<Rigidbody2D>();
        attackStyle = GameObject.FindWithTag("AttackStyle").GetComponent<AttackStyle>();
        hitCanvas = GameObject.FindWithTag("HitCanvas").GetComponent<HitCanvas>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        audiosource = GetComponent<AudioSource>();

        playerLasersParent = GameObject.Find(PLAYER_LASERS);
        if (!playerLasersParent)
        {
            playerLasersParent = new GameObject(PLAYER_LASERS);
        }
        HandlePhysicsLayers();

        originalMoveSpeed = MoveSpeed;

        transform.position = gameSession.PositionSpacePlayer;

        currentBase = GameObject.Find("Home Planet 0").gameObject;
        //transform.position = currentBase.transform.position;
        //oldPos = currentBase.transform.position;
        oldPos = transform.position;

        // get UI elements to display coordinates
        xcoordinate = GameObject.FindWithTag("XCoordinateDisplay");
        ycoordinate = GameObject.FindWithTag("YCoordinateDisplay");
        zcoordinate = GameObject.FindWithTag("ZCoordinateDisplay");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        //AvoidAndFire();

        MoveAcrossLayers();
        if (moveToBackLayer)
        {
            MoveToBackLayer();
        }

        if (moveToFrontLayer)
        {
            MoveToFrontLayer();
        }

        if (damagePlayerVisualInstance != null)
        {
            damagePlayerVisualInstance.gameObject.transform.position = transform.position;
        }

        // update game session
        gameSession.CurrentFuelSpacePlayer -= Vector2.Distance(transform.position, oldPos);
        oldPos = transform.position;
        gameSession.PositionSpacePlayer = transform.position;

        // update Space UI coordinates
        float modifiedX = (int)transform.position.x;
        xcoordinate.GetComponent<Text>().text = modifiedX.ToString();
        float modifiedY = (int)transform.position.y;
        ycoordinate.GetComponent<Text>().text = modifiedY.ToString();
        float modifiedZ = 100 + (int)transform.position.z * 25;
        zcoordinate.GetComponent<Text>().text = modifiedZ.ToString();
    }

    private void MoveToFrontLayer()
    {
        //spriterenderer.enabled = false;
        Vector3 targetPos = new Vector3(0, 0, 0);
        if (CurrentLayer == 3)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, layer2Depth + 0.003f);
        }
        if (CurrentLayer == 2)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, layer1Depth + 0.003f);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedBridge * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            CurrentLayer -= 1;
            HandlePhysicsLayers();

            foreach (Rigidbody2D rigidbodyObject in rigidbodyObjects)
            {
                if (!rigidbodyObject.gameObject.name.Contains("Shield"))
                {
                    rigidbodyObject.simulated = true;
                }
            }

            foreach (Collider2D colliderObject in colliderObjects)
            {
                if (!colliderObject.gameObject.name.Contains("Shield"))
                {
                    colliderObject.enabled = true;
                }
            }

            gameSession.CurrentFuelSpacePlayer -= 500f;
            moveToFrontLayer = false;
            //spriterenderer.enabled = true;

        }
    }

    private void MoveToBackLayer()
    {
        //spriterenderer.enabled = false;
        Vector3 targetPos = new Vector3(0, 0, 0);
        if (CurrentLayer == 1)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, layer2Depth + 0.003f);
        }
        if (CurrentLayer == 2)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, layer3Depth + 0.003f);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedBridge * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            CurrentLayer += 1;
            HandlePhysicsLayers();

            foreach (Rigidbody2D rigidbodyObject in rigidbodyObjects)
            {
                if (!rigidbodyObject.gameObject.name.Contains("Shield"))
                {
                    rigidbodyObject.simulated = true;
                }
            }

            foreach (Collider2D colliderObject in colliderObjects)
            {
                if (!colliderObject.gameObject.name.Contains("Shield"))
                {
                    colliderObject.enabled = true;
                }
            }

            gameSession.CurrentFuelSpacePlayer -= 500f;
            moveToBackLayer = false;
            //spriterenderer.enabled = true;

        }
    }

    private void MoveAcrossLayers()
    {
        if (Input.GetAxis("Vertical") > 0.5 && ps4ControllerCheck.IsXPressed())
        {
            if (transform.position.z < 40f)
            {
                DisablePhysics();

                moveToBackLayer = true;
                //audiosource.PlayOneShot(layerChange, volumeSoundLayerChange);
            }
        }

        if (Input.GetAxis("Vertical") < -0.5 && ps4ControllerCheck.IsXPressed())
        {
            if (transform.position.z > 5f)
            {
                DisablePhysics();

                moveToFrontLayer = true;
                //audiosource.PlayOneShot(layerChange, volumeSoundLayerChange);
            }
        }
    }

    private void HandlePhysicsLayers()
    {
        colliderObjects = FindObjectsOfType<Collider2D>();
        foreach (Collider2D colliderObject in colliderObjects)
        {
            if (colliderObject.GetComponent<ManualLayer>().Layer != CurrentLayer)
            {
                Physics2D.IgnoreCollision(colliderObject, GetComponent<Collider2D>(), true);
            }
            else
            {
                Physics2D.IgnoreCollision(colliderObject, GetComponent<Collider2D>(), false);
            }
        }
    }

    private void DisablePhysics()
    {
        rigidbodyObjects = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rigidbodyObject in rigidbodyObjects)
        {
            if (!rigidbodyObject.gameObject.name.Contains("Shield"))
            {
                rigidbodyObject.simulated = false;
            }
        }

        colliderObjects = FindObjectsOfType<Collider2D>();
        foreach (Collider2D colliderObject in colliderObjects)
        {
            if (!colliderObject.gameObject.name.Contains("Shield"))
            {
                colliderObject.enabled = false;
            }
        }
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
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(LaserSpeed, 0);
    }

    private void FireLeftProjectile()
    {
        Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity, playerLasersParent.transform);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(-LaserSpeed, 0);
    }

    IEnumerator AvoidAndGoLeft()
    {
        IsInvincible = true;
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPos = new Vector2(currentPos.x - XOffsetDuringAvoid, currentPos.y);
        transform.position = newPos;
        yield return new WaitForSeconds(timeInvincibleDuringAvoid);
        IsInvincible = false;
    }

    IEnumerator AvoidAndGoRight()
    {
        IsInvincible = true;
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPos = new Vector2(currentPos.x + XOffsetDuringAvoid, currentPos.y);
        transform.position = newPos;
        yield return new WaitForSeconds(timeInvincibleDuringAvoid);
        IsInvincible = false;
    }

    private void Fire()
    {
        if (!IsImmobile1)
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
                yield return new WaitForSeconds(LaserFiringPeriod);
            }
            else if (attackStyle.GetAttackStyle() == "bombs")
            {
                InstantiateBomb(xVelocity, yVelocity);
                yield return new WaitForSeconds(BombFiringPeriod);
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
        var projectilePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        Laser projectile = Instantiate(laserPrefab, projectilePos, transform.rotation, playerLasersParent.transform);
        // projectileFiringPeriod = 0.1f;
        Vector2 directionOfFiring = new Vector2(xVelocity, yVelocity) * LaserSpeed;
        projectile.GetComponent<Rigidbody2D>().velocity = directionOfFiring;
        AudioSource.PlayClipAtPoint(laserPlayer, transform.position, volumeLaserPlayer);
        projectile.GetComponent<DamageDealer>().Damage = LaserDamage;
    }

    private void InstantiateBomb(float xVelocity, float yVelocity)
    {
        var projectilePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        BombPlayer projectile = Instantiate(bombPrefab, projectilePos, transform.rotation, playerLasersParent.transform);
        // projectileFiringPeriod = 0.5f;
        Vector2 directionOfFiring = new Vector2(xVelocity, yVelocity) * BombSpeed;
        projectile.GetComponent<Rigidbody2D>().velocity = directionOfFiring;
        AudioSource.PlayClipAtPoint(laserPlayer, transform.position, volumeLaserPlayer);
        projectile.GetComponent<DamageDealer>().Damage = BombDamage;
    }

    private void Move()
    {
        if (IsImmobile1)
        {
            var deltaX = 0;
            var deltaY = 0;

            rigidBody.velocity = new Vector2(deltaX, deltaY);
        }
        else
        {
            var deltaX = Input.GetAxis("Horizontal") * MoveSpeed;
            var deltaY = Input.GetAxis("Vertical") * MoveSpeed;

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
                MoveSpeed = originalMoveSpeed;
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
        MoveSpeed -= acceleration * Time.deltaTime;
        MoveSpeed = Mathf.Clamp(MoveSpeed, originalMoveSpeed, 80f);
    }

    private void Accelerate()
    {
        acceleration = 25f;
        MoveSpeed += acceleration * Time.deltaTime;
        MoveSpeed = Mathf.Clamp(MoveSpeed, MoveSpeed, 80f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageDealer>()
        && collision.gameObject.GetComponent<ManualLayer>().Layer == currentLayer)
        {
            DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
            int layer_collider = collision.gameObject.layer;
            ProcessHit(damageDealer, layer_collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Spawner Zone"))
        {
            zoneEntered = collision.gameObject.GetComponentInChildren<SpawningEnemyArea>();
            zoneEntered.EnteredZone = true;
            zoneEntered.gameObject.GetComponent<EnemyRadarActivator>().HasBeenDiscovered = true;
        }

        if (collision.gameObject.name.Contains("Planet"))
        {
            if (gameSession.HasBeenCompleted[collision.gameObject.GetComponent<Planet>().PlanetID])
            {
                gameSession.CurrentFuelSpacePlayer = gameSession.MaxFuelSpacePlayer; // refuels when flying over completed planet
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Spawner Zone"))
        {
            zoneEntered.EnteredZone = false;
            zoneEntered.GetComponent<EnemyRadarActivator>().HasBeenDiscovered = false;
            zoneEntered.SpawnZoneCoroutineHandler = null;

            StarfieldGeneratorFast[] starfieldGenerators = FindObjectsOfType<StarfieldGeneratorFast>();
            foreach (StarfieldGeneratorFast starfieldGenerator in starfieldGenerators)
            {
                starfieldGenerator.SetCanSpawn(true);
            }
        }
    }

    private void ProcessHit(DamageDealer damageDealer, int layer_collider)
    {
        if (!IsInvincible)
        {
            gameSession.CurrentHealthSpacePlayer -= damageDealer.Damage;
            StartCoroutine(hitCanvas.HandleHitCanvas());
        }

        if (layer_collider != 9)
        {
            damageDealer.Hit();
        }
        if (gameSession.CurrentHealthSpacePlayer <= 0)
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
        //if (count_damage < damagePlayerVisual.Count - 1)
        //{
        //    Destroy(damagePlayerVisualInstance);
        //}
        gameObject.GetComponent<Renderer>().material.color = hitColorChange;
        yield return new WaitForSeconds(changeColorTime);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        //if (count_damage <= damagePlayerVisual.Count - 1)
        //{
        //    damagePlayerVisualInstance = Instantiate(damagePlayerVisual[count_damage], transform.position, Quaternion.identity);
        //}
        //count_damage++;
    }

    public int GetHealthPlayer()
    {
        return gameSession.CurrentHealthSpacePlayer;
    }

}
