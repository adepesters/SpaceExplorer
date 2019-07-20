using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyTileVania : MonoBehaviour
{
    float health = 400f;
    Color originalColor;
    float[] maxNumberOfParticles = new float[] { 10f, 4f, 4f, 4f, 4f, 4f, 4f };
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

    [SerializeField] AudioClip[] swordSlashSound;
    float volumeSoundswordSlash = 0.5f;

    Vector3 originalScale;

    bool jumpAttack = false; // should enemy attack
    float counterAttack = 0f; // counting time between attacks
    float counterAttackFrequency; // how often does enemy attack
    float timeToTarget = 1.5f; // determines the speed of the attack
    float probOfImmediateAttack = 0.4f; // chance that enemy attacks as soon as player is in sight
    float tmpRandomImmediateAttack; // randomly chosen number between 0 and 1

    bool isOnGround = false; // is enemy touching the ground

    //GameObject nonPixelatedVersion;

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
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + UnityEngine.Random.Range(0.001f, 0.009f));

        HandlePhysicsLayers();

        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        counterAttackFrequency = UnityEngine.Random.Range(0f, 5f); // initialization of counterAttackFrequency
        tmpRandomImmediateAttack = UnityEngine.Random.Range(0f, 1f); // initialization of tmpRandomImmediateAttack

        //        nonPixelatedVersion = transform.GetChild(1).gameObject;
        //      nonPixelatedVersion.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        JumpAttack();

        if (!beingHit && !jumpAttack)
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

        if (player.tag == gameObject.tag)
        {
            transform.localScale = new Vector3(-Mathf.Sign(transform.position.x - player.transform.position.x) * originalScale.x, transform.localScale.y, transform.localScale.z);
        }

        counterAttack += Time.deltaTime;

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

        if (collision.gameObject.name.Contains("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Ground"))
        {
            isOnGround = false;
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
        AudioSource.PlayClipAtPoint(swordSlashSound[UnityEngine.Random.Range(0, swordSlashSound.Length - 1)], player.transform.position, volumeSoundswordSlash);
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
        AudioSource.PlayClipAtPoint(swordSlashSound[UnityEngine.Random.Range(0, swordSlashSound.Length - 1)], player.transform.position, volumeSoundswordSlash * 5);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        animator.speed = 0f;
        FindObjectOfType<Sword>().SpeedAnimation = 0f;
        //player.GetComponent<Animator>().SetBool("criticalHit", true);
        FindObjectOfType<Sword>().GetComponent<CameraShaker>().enabled = true;
        player.SetFrozenPlayer(true, player.transform.position);
        yield return new WaitForSeconds(0.2f);
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

    public void JumpAttack()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 5f || player.tag != gameObject.tag)
        {
            if (tmpRandomImmediateAttack > probOfImmediateAttack) // determines if enemy attacks as soon as player is in sight
            {
                counterAttack = 0f;
            }
        }
        if (isOnGround && counterAttack > counterAttackFrequency && player.tag == gameObject.tag && Vector2.Distance(player.transform.position, transform.position) < 5f)
        {
            jumpAttack = true;

            Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            GetComponent<Rigidbody2D>().velocity = CalculateTrajectoryVelocity(transform.position, target, timeToTarget);
            counterAttack = 0f;
            counterAttackFrequency = UnityEngine.Random.Range(0f, 5f);
            timeToTarget = UnityEngine.Random.Range(1.3f, 2f);
            tmpRandomImmediateAttack = UnityEngine.Random.Range(0f, 1f);
        }
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 currentTarget, float t)
    {
        float vx = (currentTarget.x - origin.x) / t;
        float vz = (currentTarget.z - origin.z) / t;
        float vy = ((currentTarget.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
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

                    Vector3 bonusPos = new Vector3(contactPoint.x, contactPoint.y, transform.position.z - 0.01f);
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
        //gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //nonPixelatedVersion.GetComponent<SpriteRenderer>().enabled = true;
        player.SetFrozenPlayer(false, player.transform.position);
        if (FindObjectOfType<Sword>() != null)
        {
            FindObjectOfType<Sword>().SpeedAnimation = 1f;
        }
        player.SetPlayerOnAir(false, originalPlayerPos);
    }

    private void HandlePhysicsLayers()
    {
        Collider2D[] colliderObjects = FindObjectsOfType<Collider2D>();
        foreach (Collider2D colliderObject in colliderObjects)
        {
            if (colliderObject.tag != this.tag)
            {
                if (!colliderObject.gameObject.name.Contains("Camera") && !colliderObject.gameObject.name.Contains("Bridge")
            && !colliderObject.gameObject.name.Contains("Double Mirror"))
                {
                    Physics2D.IgnoreCollision(colliderObject, GetComponent<Collider2D>(), true);
                }
            }
            else
            {
                Physics2D.IgnoreCollision(colliderObject, GetComponent<Collider2D>(), false);
            }
        }
    }
}
