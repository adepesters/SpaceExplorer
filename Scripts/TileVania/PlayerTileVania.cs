using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class PlayerTileVania : MonoBehaviour
{
    // health variables
    //float health = 10000f;
    [SerializeField] Image[] hearts; // actual UI hearts
    [SerializeField] Image[] heartsSprites; // heart sprites with quarter/half/3 quarters hearts
    //float initialHealth = 20f;
    //float maxHealth = 64f;
    //int[] counterHeartFragments = new int[16];

    float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] float climbSpeed = 6f;

    // cached variables
    Animator animator;
    Vector3 originalScale;
    Rigidbody2D rigidBody;

    bool isDead = false;
    bool playerIsImmobile = true;
    bool forceImmobility = false;

    bool isJumping = false;

    [SerializeField] Sword rightSwordPrefab;
    [SerializeField] Sword leftSwordPrefab;

    bool playerIsFrozen;
    Vector3 frozenPosition;
    bool playerOnAir;
    Vector3 onAirPos;

    bool canJump = false;

    Feet feet;
    ExtendedLegs extendedLegs;
    PS4ControllerCheck PS4ControllerCheck;
    ToolSelector toolSelector;
    Grapin grapin;
    DataManager dataManager;

    bool grapinJump = false;
    bool isTargeting = false;

    public bool GrapinJump { get => grapinJump; set => grapinJump = value; }
    public bool IsTargeting { get => isTargeting; set => isTargeting = value; }
    public string SwordHitDirection { get => swordHitDirection; set => swordHitDirection = value; }
    public int CurrentLayer { get => currentLayer; set => currentLayer = value; }
    public bool XisActionTrigger1 { get => XisActionTrigger; set => XisActionTrigger = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool InFrontOfBridge { get => inFrontOfBridge; set => inFrontOfBridge = value; }
    public bool ForceImmobility { get => forceImmobility; set => forceImmobility = value; }

    float currentPos;
    float previousPos;

    string swordHitDirection;

    bool inFrontOfBridge;
    Transform entryFrontLayer;
    Transform entryBackLayer;
    bool moveToBackLayer;
    bool moveToFrontLayer;

    [SerializeField] AudioClip[] footstepsDryLeavesSound;
    float volumeSoundFootSteps = 0.3f;
    float counterFootSteps;

    AudioSource audiosource;

    [SerializeField] AudioClip[] swordSlashSound;
    float volumeSoundswordSlash = 0.6f;

    float counterHit;

    float originalGravity;
    FeetLowGravity feetLowGravity;
    GameSession gameSession;

    int planetID;

    SpriteRenderer spriterenderer;

    PlayerTileVaniaDoubleMirror doubleMirror;

    bool isRunning = false;

    bool previousFeetContact, currentFeetContact;

    Rigidbody2D[] rigidbodyObjects;
    Collider2D[] colliderObjects;

    int currentLayer = 1;

    float speedBridge = 30f;

    [SerializeField] AudioClip layerChange;
    float volumeSoundLayerChange = 0.2f;

    bool XisActionTrigger = false;

    bool playerInPortal = false;

    Scene scene;

    bool beingHit = false;

    PauseController pauseController;
    DialogManager dialogManager;

    bool inDialogZone = false;

    void Start()
    {
        grapin = FindObjectOfType<Grapin>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        feet = FindObjectOfType<Feet>();
        extendedLegs = FindObjectOfType<ExtendedLegs>();
        PS4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();
        toolSelector = FindObjectOfType<ToolSelector>();
        audiosource = GetComponent<AudioSource>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        doubleMirror = FindObjectOfType<PlayerTileVaniaDoubleMirror>();
        pauseController = FindObjectOfType<PauseController>();
        dialogManager = FindObjectOfType<DialogManager>();

        gameObject.tag = "Layer" + currentLayer;
        foreach (Transform child in transform)
        {
            child.tag = "Layer" + currentLayer;
        }
        HandlePhysicsLayers();

        originalGravity = rigidBody.gravityScale;
        feetLowGravity = FindObjectOfType<FeetLowGravity>();

        string numbersOnly = Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", "");
        planetID = int.Parse(numbersOnly);

        spriterenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        previousFeetContact = false;
        currentFeetContact = false;

        scene = SceneManager.GetActiveScene();

        //health = initialHealth;


        /////// hearts UI ///////

        //foreach (Image heart in hearts)
        //{
        //    heart.GetComponent<Image>().sprite = heartsSprites[3].GetComponent<Image>().sprite;
        //}

        // initializing heart UI appearance
        //for (int i = 0; i < (int)initialHealth / 4; i++)
        //{
        //    hearts[i].GetComponent<Image>().enabled = true;
        //}

        //for (int i = (int)initialHealth / 4; i < (int)maxHealth / 4; i++)
        //{
        //    hearts[i].GetComponent<Image>().enabled = false;
        //}

        //for (int i = 0; i < 5; i++)
        //{
        //    counterHeartFragments[i] = 4;
        //}

        ///////////////////
    }

    void Update()
    {
        previousFeetContact = currentFeetContact;
        currentFeetContact = feet.AreOnSomething;

        counterHit += Time.deltaTime;

        if (forceImmobility)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            animator.speed = 0f;
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (feet.AreOnSomething && !previousFeetContact && !forceImmobility)
        {
            IsJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (!isDead && playerOnAir && !playerIsFrozen && !forceImmobility)
        {
            animator.speed = 0.2f;
            transform.position = onAirPos;
            if (Mathf.Sign(transform.localScale.x) > 0)
            {
                SwingSwordRight();
            }
            else
            {
                SwingSwordLeft();
            }
        }

        if (!isDead && !playerIsFrozen && !playerOnAir && !pauseController.IsPaused && !forceImmobility)
        {
            animator.speed = 1f;
            CheckIfIsImmobile();
            Jump();
            Move();
            MoveAcrossLayers();

            if (moveToBackLayer)
            {
                MoveToBackLayer();
            }

            if (moveToFrontLayer)
            {
                MoveToFrontLayer();
            }

            if (Mathf.Sign(transform.localScale.x) > 0)
            {
                SwingSwordRight();
            }
            else
            {
                SwingSwordLeft();
            }
            //Climb();
            counterFootSteps += Time.deltaTime;

            //StartCoroutine(WalkDryLeavesSFX());
        }

        if (!isDead && playerIsFrozen && !forceImmobility)
        {
            animator.speed = 0f;
            transform.position = frozenPosition;
        }

    }

    private void MoveToFrontLayer()
    {
        //spriterenderer.enabled = false;
        Vector3 targetPos = new Vector3(entryFrontLayer.position.x, entryFrontLayer.position.y, entryFrontLayer.position.z + 0.003f);
        speedBridge = Mathf.Abs(entryBackLayer.position.z - entryFrontLayer.position.z) * 30f / 5f;
        speedBridge = Mathf.Clamp(speedBridge, 5f, 80f);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedBridge * Time.deltaTime);

        doubleMirror.IsCrossing = true; // to lock double mirror position while player is crossing layers

        if (Vector3.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            currentLayer -= 1;
            gameObject.tag = "Layer" + currentLayer;
            foreach (Transform child in transform)
            {
                child.tag = "Layer" + currentLayer;
            }
            HandlePhysicsLayers();
            RestoreOpaquenessLayer(currentLayer);

            foreach (Rigidbody2D rigidbodyObject in rigidbodyObjects)
            {
                rigidbodyObject.simulated = true;
            }

            foreach (Collider2D colliderObject in colliderObjects)
            {
                colliderObject.enabled = true;
            }

            moveToFrontLayer = false;
            //spriterenderer.enabled = true;

            doubleMirror.IsCrossing = false;
        }
    }

    private void MoveToBackLayer()
    {
        //spriterenderer.enabled = false;
        Vector3 targetPos = new Vector3(entryFrontLayer.position.x, entryBackLayer.position.y, entryBackLayer.position.z + 0.003f);
        speedBridge = Mathf.Abs(entryBackLayer.position.z - entryFrontLayer.position.z) * 30f / 5f;
        speedBridge = Mathf.Clamp(speedBridge, 5f, 80f);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedBridge * Time.deltaTime);

        doubleMirror.IsCrossing = true; // to lock double mirror position while player is crossing layers

        if (Vector3.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            currentLayer += 1;
            gameObject.tag = "Layer" + currentLayer;
            foreach (Transform child in transform)
            {
                child.tag = "Layer" + currentLayer;
            }
            HandlePhysicsLayers();
            ReduceTransparencyLayer(currentLayer - 1);

            foreach (Rigidbody2D rigidbodyObject in rigidbodyObjects)
            {
                rigidbodyObject.simulated = true;
            }

            foreach (Collider2D colliderObject in colliderObjects)
            {
                colliderObject.enabled = true;
            }

            moveToBackLayer = false;
            //spriterenderer.enabled = true;

            doubleMirror.IsCrossing = false;
        }
    }

    private void CheckIfIsImmobile()
    {
        if (PS4ControllerCheck.noButtonPressed() && feet.AreOnSomething && !beingHit)
        {
            rigidBody.velocity = new Vector2(0f, 0f);
            playerIsImmobile = true;
        }
        else
        {
            playerIsImmobile = false;
        }
    }

    private void Move()
    {
        if (playerIsImmobile || (GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")) && (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon)) || isTargeting)
        {
            if (!pauseController.IsPaused)
            {
                animator.SetTrigger("isIdle");
                animator.SetBool("isRunning", false);
                isRunning = false;
                rigidBody.drag = 1000;
                if (doubleMirror != null)
                {
                    doubleMirror.IsJumping = false;
                }
            }
        }
        else
        {
            if (!pauseController.IsPaused)
            {
                isRunning = true;
                rigidBody.drag = 0;
                animator.SetBool("isRunning", true);
                float xChange = Input.GetAxis("Horizontal") * runSpeed; // we don't put deltaTime here. See notes.
                if (!scene.name.Contains("Circular") && !beingHit)
                {
                    rigidBody.velocity = new Vector2(xChange, rigidBody.velocity.y);
                }
                if (xChange != 0)
                {
                    transform.localScale = new Vector3(Mathf.Sign(xChange) * originalScale.x, originalScale.y, originalScale.z);
                }
                if (feet.CurrentSurface != null)
                {
                    if (feet.AreOnSomething && feet.CurrentSurface.name.Contains("Ground"))
                    {
                        WalkOnDryLeavesSFX(xChange);
                    }
                }
            }
        }
    }

    private void WalkOnDryLeavesSFX(float xChange)
    {
        xChange = MapValue(0f, runSpeed, 0.1f, 1.5f, Mathf.Abs(xChange));
        float frequencySteps = 1f / Mathf.Exp(xChange);
        if (counterFootSteps > frequencySteps)
        {
            audiosource.PlayOneShot(footstepsDryLeavesSound[UnityEngine.Random.Range(0, footstepsDryLeavesSound.Length - 1)], volumeSoundFootSteps);
            counterFootSteps = 0;
        }
    }

    private float MapValue(float a0, float a1, float b0, float b1, float a)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }

    private void MoveAcrossLayers()
    {
        if (Input.GetAxis("Vertical") > 0.9)
        {
            if (InFrontOfBridge &&
        Mathf.Abs(entryFrontLayer.position.z - transform.position.z) < Mathf.Abs(entryBackLayer.position.z - transform.position.z))
            {
                DisablePhysics();

                moveToBackLayer = true;
                audiosource.PlayOneShot(layerChange, volumeSoundLayerChange);
            }
        }

        if (Input.GetAxis("Vertical") < -0.9)

        {
            if (InFrontOfBridge &&
            Mathf.Abs(entryFrontLayer.position.z - transform.position.z) > Mathf.Abs(entryBackLayer.position.z - transform.position.z))
            {
                DisablePhysics();

                moveToFrontLayer = true;
                audiosource.PlayOneShot(layerChange, volumeSoundLayerChange);
            }
        }
    }

    private void DisablePhysics()
    {
        rigidbodyObjects = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rigidbodyObject in rigidbodyObjects)
        {
            if (!rigidbodyObject.gameObject.name.Contains("Double Mirror"))
            {
                rigidbodyObject.simulated = false;
            }
        }

        colliderObjects = FindObjectsOfType<Collider2D>();
        foreach (Collider2D colliderObject in colliderObjects)
        {
            if (!colliderObject.gameObject.name.Contains("Camera") && !colliderObject.gameObject.name.Contains("Bridge")
            && !colliderObject.gameObject.name.Contains("Double Mirror"))
            {
                colliderObject.enabled = false;
            }
        }
    }

    private void RestoreOpaquenessLayer(int layerID)
    {
        GameObject[] gameObjectsLayer = GameObject.FindGameObjectsWithTag("Layer" + layerID);
        foreach (GameObject eachGameObject in gameObjectsLayer)
        {
            if (eachGameObject.GetComponent<SpriteRenderer>() != null)
            {
                Color color = eachGameObject.GetComponent<SpriteRenderer>().color;
                eachGameObject.GetComponent<SpriteRenderer>().color =
                new Color(color.r, color.g, color.b, 1f);
            }
        }
    }

    private void ReduceTransparencyLayer(int layerID)
    {
        GameObject[] gameObjectsLayer = GameObject.FindGameObjectsWithTag("Layer" + layerID);
        foreach (GameObject eachGameObject in gameObjectsLayer)
        {
            if (eachGameObject.GetComponent<SpriteRenderer>() != null)
            {
                if (eachGameObject.GetComponent<SpriteRenderer>().color.a == 1f)
                {
                    Color color = eachGameObject.GetComponent<SpriteRenderer>().color;
                    eachGameObject.GetComponent<SpriteRenderer>().color =
                    new Color(color.r, color.g, color.b, 0.74f);
                }
            }
        }
    }

    private void HandlePhysicsLayers()
    {
        colliderObjects = FindObjectsOfType<Collider2D>();
        foreach (Collider2D colliderObject in colliderObjects)
        {
            if (colliderObject.tag != this.tag)
            {
                if (!colliderObject.gameObject.name.Contains("Camera") && !colliderObject.gameObject.name.Contains("Bridge")
            && !colliderObject.gameObject.name.Contains("Double Mirror"))
                {
                    Physics2D.IgnoreCollision(colliderObject, GetComponent<Collider2D>(), true);
                    Physics2D.IgnoreCollision(colliderObject, feet.GetComponent<Collider2D>(), true);
                    Physics2D.IgnoreCollision(colliderObject, extendedLegs.GetComponent<Collider2D>(), true);
                    Physics2D.IgnoreCollision(colliderObject, grapin.GetComponent<Collider2D>(), true);
                }
            }
            else
            {
                Physics2D.IgnoreCollision(colliderObject, GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(colliderObject, feet.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(colliderObject, extendedLegs.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(colliderObject, grapin.GetComponent<Collider2D>(), false);
            }
        }
    }

    private void Jump()
    {
        if (originalGravity < 1)
        {
            if (PS4ControllerCheck.IsXPressed() && ((extendedLegs.AreOnSomething) || grapinJump) && !XisActionTrigger1 && !inDialogZone)
            {
                canJump = true;
                rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)

            }
            if (canJump == true && (feetLowGravity.AreOnSomething || grapinJump))
            {
                //rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)
                if (!beingHit)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                }
                IsJumping = true;
                isRunning = false;
                canJump = false;
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", true);
                if (doubleMirror != null)
                {
                    doubleMirror.IsJumping = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsJumping = false;
            }
        }
        else
        {
            if (PS4ControllerCheck.IsXPressed() && ((extendedLegs.AreOnSomething) || grapinJump) && !XisActionTrigger1 && !inDialogZone)
            {
                canJump = true;
                rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)

            }
            if (canJump == true && (feet.AreOnSomething || grapinJump))
            {
                //rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)
                if (!beingHit)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                }
                IsJumping = true;
                isRunning = false;
                canJump = false;
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", true);
                if (doubleMirror != null)
                {
                    doubleMirror.IsJumping = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsJumping = false;
            }
        }
    }

    //private void Jump()
    //{
    //    if (FindObjectOfType<PS4ControllerCheck>().IsXPressed() && (FindObjectOfType<Ground>().AreFeetOnTheGround())) // || FindObjectOfType<Ladders>().AreFeetOnTheGround()))
    //    {
    //        rigidBody.gravityScale = 3; // in case we jump from a ladder (where gravity is 0)
    //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
    //        isJumping = true;
    //    }
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        isJumping = false;
    //    }
    //}

    //private void Climb()
    //{
    //    if (GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")) && !isJumping)
    //    {
    //        rigidBody.gravityScale = 0;
    //        if (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon)
    //        {
    //            animator.SetBool("isClimbing", true);
    //            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Sign(Input.GetAxis("Vertical")) * climbSpeed);
    //        }
    //        else if (Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Epsilon)
    //        {
    //            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
    //        }
    //    }
    //    else
    //    {
    //        animator.SetBool("isClimbing", false);
    //        rigidBody.gravityScale = 1;
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!isDead)
    //    {
    //        if (collision.gameObject.name.Contains("Enemy") || collision.gameObject.name.Contains("Spikes"))
    //        {
    //            isDead = true;
    //            StartCoroutine(DeathVFX());
    //        }
    //        if (collision.gameObject.name.Contains("Exit"))
    //        {
    //            StartCoroutine(ExitLevel());
    //        }
    //    }
    //}

    //IEnumerator ExitLevel()
    //{
    //    rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    //    animator.SetBool("isExiting", true);
    //    yield return new WaitForSeconds(2);
    //    CoinsInLevel coins = FindObjectOfType<CoinsInLevel>();
    //    Destroy(coins.gameObject);
    //    FindObjectOfType<GameSession>().ResetNumberOfCoins();
    //    FindObjectOfType<LevelLoader>().LoadNextScene();
    //}

    //IEnumerator DeathVFX()
    //{
    //    animator.SetBool("isDead", true);
    //    StartCoroutine(DeathKick());
    //    yield return new WaitForSeconds(2);
    //    FindObjectOfType<GameSession>().RemoveALife();
    //}

    //IEnumerator DeathKick()
    //{
    //    rigidBody.velocity = new Vector2(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(5f, 20f));
    //    yield return new WaitForSeconds(1);
    //    rigidBody.velocity = new Vector2(0f, 0f);
    //}

    public bool IsPlayerDead()
    {
        return isDead;
    }

    private void SwingSwordRight()
    {
        if (toolSelector.GetTool() == "sword")
        {
            if (PS4ControllerCheck.IsSquarePressed())
            {
                animator.SetTrigger("isAttacking1");
                var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
                Sword sword = Instantiate(rightSwordPrefab, swordPos, Quaternion.identity);
                SwordHitDirection = "right";
                audiosource.PlayOneShot(swordSlashSound[UnityEngine.Random.Range(0, swordSlashSound.Length - 1)], volumeSoundswordSlash);
            }
        }
    }

    private void SwingSwordLeft()
    {
        if (toolSelector.GetTool() == "sword")
        {
            if (PS4ControllerCheck.IsSquarePressed())
            {
                animator.SetTrigger("isAttacking1");
                var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
                Sword sword = Instantiate(leftSwordPrefab, swordPos, Quaternion.identity);
                SwordHitDirection = "left";
                audiosource.PlayOneShot(swordSlashSound[UnityEngine.Random.Range(0, swordSlashSound.Length - 1)], volumeSoundswordSlash);
            }
        }
    }

    public void SetFrozenPlayer(bool currentPlayerIsFrozen, Vector3 currentFrozenPosition)
    {
        playerIsFrozen = currentPlayerIsFrozen;
        frozenPosition = currentFrozenPosition;
    }

    public void SetPlayerOnAir(bool currentPlayerOnAir, Vector3 currentPlayerPos)
    {
        playerOnAir = currentPlayerOnAir;
        onAirPos = currentPlayerPos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            // check if portals are actually on layer where the player is or on layer where the layer can access via the portal
            int tmpPreviousLayer = currentLayer - 1;
            int tmpNextLayer = currentLayer + 1;
            if (collision.gameObject.transform.Find("Entry Front Layer").gameObject.tag == "Layer" + currentLayer ||
            collision.gameObject.transform.Find("Entry Front Layer").gameObject.tag == "Layer" + tmpPreviousLayer ||
                collision.gameObject.transform.Find("Entry Back Layer").gameObject.tag == "Layer" + currentLayer ||
            collision.gameObject.transform.Find("Entry Back Layer").gameObject.tag == "Layer" + tmpNextLayer)
            {
                InFrontOfBridge = true;
                entryFrontLayer = collision.gameObject.transform.Find("Entry Front Layer").gameObject.transform;
                entryBackLayer = collision.gameObject.transform.Find("Entry Back Layer").gameObject.transform;

                doubleMirror.EntryFrontLayer = entryFrontLayer;
                doubleMirror.EntryBackLayer = entryBackLayer;
            }
        }

        if (collision.gameObject.name.Contains("Bridge") && playerInPortal)
        {
            doubleMirror.GetComponentInChildren<SpriteRenderer>().enabled = true;
            doubleMirror.GetComponentInChildren<ErasePixels>().Portal = collision.gameObject.transform.parent.transform.GetChild(0).gameObject;
        }

        if (collision.gameObject.name.Contains("Bridge") && !playerInPortal)
        {
            doubleMirror.GetComponentInChildren<SpriteRenderer>().enabled = false;
            doubleMirror.GetComponentInChildren<ErasePixels>().Portal = collision.gameObject.transform.parent.transform.GetChild(0).gameObject;
        }

        if (collision.gameObject.name.Contains("StartUpdatingPixels") && playerInPortal)
        {
            doubleMirror.GetComponentInChildren<ErasePixels>().UpdateColors = true;
            doubleMirror.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }

        if (collision.gameObject.name.Contains("StartUpdatingPixels") && !playerInPortal)
        {
            doubleMirror.GetComponentInChildren<ErasePixels>().UpdateColors = true;
            doubleMirror.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        if (collision.gameObject.name.Contains("StopUpdatingPixels"))
        {
            doubleMirror.GetComponentInChildren<SpriteRenderer>().enabled = false;
            doubleMirror.GetComponentInChildren<ErasePixels>().UpdateColors = false;
        }

        if (collision.gameObject.name.Contains("EndOfLand"))
        {
            inDialogZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            InFrontOfBridge = false;
        }

        if (collision.gameObject.name.Contains("Chest opener"))
        {
            XisActionTrigger1 = false;
        }

        if (collision.gameObject.name.Contains("portail Front Layer") || collision.gameObject.name.Contains("portail Back Layer"))
        {
            playerInPortal = false;
        }

        if (collision.gameObject.name.Contains("EndOfLand"))
        {
            inDialogZone = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("VirtualRock"))
        {
            if (collision.gameObject.tag == gameObject.tag)
            {
                int damage = 10;
                Destroy(collision.gameObject);
                ProcessHit(damage);
            }
        }

        if (collision.gameObject.name.Contains("Exit To Space"))
        {
            dataManager.SavePlanetData();
            if (gameSession.HasBeenCompleted[planetID])
            {
                gameSession.CurrentFuelSpacePlayer = gameSession.MaxFuelSpacePlayer; // maxout fuel if planet is completed
            }
            //SceneManager.LoadScene("Space");
            LoadingScreen.Instance.Show("Space");
            gameSession.SceneType = "space";
        }

        if (collision.gameObject.name.Contains("Complete Planet"))
        {
            gameSession.HasBeenCompleted[gameSession.CurrentPlanetID] = true;
        }

        if (collision.gameObject.name.Contains("Chest opener"))
        {
            if (!collision.gameObject.GetComponent<Chest>().IsOpen)
            {
                XisActionTrigger1 = true;
            }
        }

        if (collision.gameObject.name.Contains("portail Front Layer") || collision.gameObject.name.Contains("portail Back Layer"))
        {
            playerInPortal = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            if (collision.gameObject.tag == gameObject.tag)
            {
                animator.SetTrigger("isHurt");
                beingHit = true;
                int damage = 10;
                //UpdateHeartsUI(damage);

                ProcessHit(damage);
                GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Sign(transform.position.x - collision.transform.position.x) * 5f, 5f, 0f);
            }
        }
    }

    //private void UpdateHeartsUI(float damage)
    //{
    //    int tmpDamage = (int)damage;

    //    while (tmpDamage > 0)
    //    {
    //        for (int i = 15; i >= 0; i--)
    //        {
    //            int tmp = tmpDamage - counterHeartFragments[i];
    //            if (tmp > 0 && i != 0)
    //            {
    //                tmpDamage = tmp;
    //                counterHeartFragments[i] = 0;
    //            }
    //            else if (tmp <= 0)
    //            {
    //                counterHeartFragments[i] = -tmp;
    //                tmpDamage = 0;
    //                break;
    //            }
    //            else if (tmp > 0 && i == 0)
    //            {
    //                tmpDamage = 0;
    //                counterHeartFragments[i] = 0;
    //            }
    //        }
    //    }

    //    int j = 0;
    //    foreach (Image heart in hearts)
    //    {
    //        if (counterHeartFragments[j] == 0)
    //        {
    //            heart.GetComponent<Image>().enabled = false;
    //        }
    //        else
    //        {
    //            heart.GetComponent<Image>().sprite = heartsSprites[counterHeartFragments[j] - 1].GetComponent<Image>().sprite;
    //        }
    //        j++;
    //    }
    //}

    void ProcessHit(int damage)
    {
        if (counterHit > 0.2f)
        {
            gameSession.CurrentHealthPlanetPlayer -= damage;
            if (gameSession.CurrentHealthPlanetPlayer <= 0)
            {
                Debug.Log("dead");
                //Die(); not yet implemented
            }
            else
            {
                counterHit = 0f;
                StartCoroutine(ChangeColorAfterHit());
            }
        }
    }

    IEnumerator ChangeColorAfterHit()
    {
        spriterenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriterenderer.color = Color.white;
        beingHit = false;
    }

}
