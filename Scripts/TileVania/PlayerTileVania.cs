using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class PlayerTileVania : MonoBehaviour
{
    float health = 10000f;

    float runSpeed = 6f;
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] float climbSpeed = 6f;

    // cached variables
    Animator animator;
    Vector3 originalScale;
    Rigidbody2D rigidBody;

    bool isDead = false;
    bool playerIsImmobile = true;

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

    float currentPos;
    float previousPos;

    string swordHitDirection;

    bool inFrontOfBridge;
    Transform entryFrontLayer;
    Transform entryBackLayer;
    bool moveToBackLayer;
    bool moveToFrontLayer;

    float layer1zdepth;
    float layer2zdepth;

    [SerializeField] AudioClip[] footstepsDryLeavesSound;
    float volumeSoundFootSteps = 1f;
    float counterFootSteps;

    AudioSource audiosource;

    [SerializeField] AudioClip[] swordSlashSound;
    float volumeSoundswordSlash = 1f;

    float counterHit;

    float originalGravity;
    FeetLowGravity feetLowGravity;
    GameSession gameSession;

    int planetID;

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

        if (GameObject.Find("Layer 1") != null)
        {
            layer1zdepth = GameObject.Find("Layer 1").gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.position.z;
        }
        if (GameObject.Find("Layer 2") != null)
        {
            layer2zdepth = GameObject.Find("Layer 2").gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.position.z;
        }
        IgnorePhysicsLayer2();

        originalGravity = rigidBody.gravityScale;
        feetLowGravity = FindObjectOfType<FeetLowGravity>();

        string numbersOnly = Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", "");
        planetID = int.Parse(numbersOnly);
    }

    void Update()
    {
        //Debug.Log(health);
        counterHit += Time.deltaTime;

        if (!isDead && playerOnAir && !playerIsFrozen)
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

        if (!isDead && !playerIsFrozen && !playerOnAir)
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

        if (!isDead && playerIsFrozen)
        {
            animator.speed = 0f;
            transform.position = frozenPosition;
        }
    }

    private void MoveToFrontLayer()
    {
        gameObject.tag = "Layer1";
        IgnorePhysicsLayer2();
        DontIgnorePhysicsLayer1();
        RestoreOpaquenessLayer1();

        rigidBody.simulated = false;
        Vector3 targetPos = new Vector3(entryFrontLayer.position.x, entryFrontLayer.position.y, layer1zdepth + 0.003f);
        float speedBridge = 20f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedBridge);
        if (Vector3.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            moveToFrontLayer = false;
            rigidBody.simulated = true;
        }
    }

    private void MoveToBackLayer()
    {
        gameObject.tag = "Layer2";
        IgnorePhysicsLayer1();
        DontIgnorePhysicsLayer2();
        ReduceTransparencyLayer1();

        rigidBody.simulated = false;
        Vector3 targetPos = new Vector3(entryFrontLayer.position.x, entryBackLayer.position.y, layer2zdepth + 0.003f);
        float speedBridge = 20f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedBridge);
        if (Vector3.Distance(transform.position, targetPos) < Mathf.Epsilon)
        {
            moveToBackLayer = false;
            rigidBody.simulated = true;
        }
    }

    private void CheckIfIsImmobile()
    {
        if (PS4ControllerCheck.noButtonPressed() && feet.AreOnSomething)
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
            animator.SetBool("isRunning", false);
            rigidBody.drag = 1000;
        }
        else
        {
            rigidBody.drag = 0;
            animator.SetBool("isRunning", true);
            float xChange = Input.GetAxis("Horizontal") * runSpeed; // we don't put deltaTime here. See notes.
            rigidBody.velocity = new Vector2(xChange, rigidBody.velocity.y);
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

    private void WalkOnDryLeavesSFX(float xChange)
    {
        xChange = MapValue(0f, 6f, 0.1f, 1.5f, Mathf.Abs(xChange));
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
        if (Input.GetAxis("Vertical") > 0.99)
        {
            if (inFrontOfBridge)
            {
                moveToBackLayer = true;
            }
        }

        if (Input.GetAxis("Vertical") < -0.99)
        {
            if (inFrontOfBridge)
            {
                moveToFrontLayer = true;
            }
        }
    }

    private void RestoreOpaquenessLayer1()
    {
        GameObject[] gameObjectsLayer1 = GameObject.FindGameObjectsWithTag("Layer1");
        foreach (GameObject eachGameObject in gameObjectsLayer1)
        {
            if (eachGameObject.GetComponent<SpriteRenderer>() != null)
            {
                Color color = eachGameObject.GetComponent<SpriteRenderer>().color;
                eachGameObject.GetComponent<SpriteRenderer>().color =
                new Color(color.r, color.g, color.b, 1f);
            }
        }
    }

    private void ReduceTransparencyLayer1()
    {
        GameObject[] gameObjectsLayer1 = GameObject.FindGameObjectsWithTag("Layer1");
        foreach (GameObject eachGameObject in gameObjectsLayer1)
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

    private void IgnorePhysicsLayer2()
    {
        GameObject[] gameObjectsLayer2 = GameObject.FindGameObjectsWithTag("Layer2");
        foreach (GameObject eachGameObject in gameObjectsLayer2)
        {
            if (eachGameObject.GetComponent<Collider2D>() != null)
            {
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), feet.GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), extendedLegs.GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), grapin.GetComponent<Collider2D>(), true);
            }
        }
    }

    private void DontIgnorePhysicsLayer2()
    {
        GameObject[] gameObjectsLayer2 = GameObject.FindGameObjectsWithTag("Layer2");
        foreach (GameObject eachGameObject in gameObjectsLayer2)
        {
            if (eachGameObject.GetComponent<Collider2D>() != null)
            {
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), feet.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), extendedLegs.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), grapin.GetComponent<Collider2D>(), false);
            }
        }
    }

    private void IgnorePhysicsLayer1()
    {
        GameObject[] gameObjectsLayer1 = GameObject.FindGameObjectsWithTag("Layer1");
        foreach (GameObject eachGameObject in gameObjectsLayer1)
        {
            if (eachGameObject.GetComponent<Collider2D>() != null)
            {
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), feet.GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), extendedLegs.GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), grapin.GetComponent<Collider2D>(), true);
            }
        }
    }

    private void DontIgnorePhysicsLayer1()
    {
        GameObject[] gameObjectsLayer1 = GameObject.FindGameObjectsWithTag("Layer1");
        foreach (GameObject eachGameObject in gameObjectsLayer1)
        {
            if (eachGameObject.GetComponent<Collider2D>() != null)
            {
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), feet.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), extendedLegs.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(eachGameObject.GetComponent<Collider2D>(), grapin.GetComponent<Collider2D>(), false);
            }
        }
    }

    private void Jump()
    {
        if (originalGravity < 1)
        {
            if (PS4ControllerCheck.IsXPressed() && ((extendedLegs.AreOnSomething) || grapinJump))
            {
                canJump = true;
                rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)

            }
            if (canJump == true && (feetLowGravity.AreOnSomething || grapinJump))
            {
                //rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                isJumping = true;
                canJump = false;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }
        else
        {
            if (PS4ControllerCheck.IsXPressed() && ((extendedLegs.AreOnSomething) || grapinJump))
            {
                canJump = true;
                rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)

            }
            if (canJump == true && (feet.AreOnSomething || grapinJump))
            {
                //rigidBody.gravityScale = originalGravity; // in case we jump from a ladder (where gravity is 0)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                isJumping = true;
                canJump = false;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
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
            inFrontOfBridge = true;
            entryFrontLayer = collision.gameObject.transform.Find("Entry Front Layer").gameObject.transform;
            entryBackLayer = collision.gameObject.transform.Find("Entry Back Layer").gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            inFrontOfBridge = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("VirtualRock"))
        {
            if (collision.gameObject.tag == gameObject.tag)
            {
                float damage = 100f;
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
            SceneManager.LoadScene("Space");
            gameSession.SceneType = "space";
        }

        if (collision.gameObject.name.Contains("Complete Planet"))
        {
            gameSession.HasBeenCompleted[gameSession.CurrentPlanetID] = true;
        }
    }

    void ProcessHit(float damage)
    {
        if (counterHit > 0.2f)
        {
            if (health <= 0)
            {
                //Die(); not yet implemented
            }
            else
            {
                counterHit = 0f;
                health -= damage;
                StartCoroutine(ChangeColorAfterHit());
            }
        }
    }

    IEnumerator ChangeColorAfterHit()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
