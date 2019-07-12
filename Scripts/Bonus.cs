using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    float explosionSpeed = 30f;
    float bonusSpeed = 0.2f;
    float maxDistance;
    float spreadingAngle;
    float attractionDistance = 6f;
    float accelerationAttraction = 0.01f;

    [SerializeField] AudioClip coinSound;
    [SerializeField] [Range(0, 1)] float volumeSound = 0.5f;
    float timeBeforeDestroy = 1f;

    Player player;

    bool isFiringCoroutingActive = false;
    [SerializeField] Laser laserPrefab;
    [SerializeField] BombPlayer bombPrefab;
    Coroutine firingCoroutine;
    float originalLaserFiringSpeed;
    float originalLaserFiringPeriod;
    float originalBombFiringSpeed;
    float originalBombFiringPeriod;

    // power ups effects duration
    float durationPowerUp1 = 10f;
    float durationPowerUp2 = 10f;
    float durationPowerUp3 = 10f;

    public Coroutine powerUp1Routine;
    public Coroutine powerUp2Routine;
    public Coroutine powerUp3Routine;

    // cached variables

    Bonus[] allBonuses;

    // parent game objects
    const string PLAYER_LASERS = "Player Lasers Parent";
    GameObject playerLasersParent;

    float enemySize;
    Vector2 initialPos;

    GameSession gameSession;
    Shield shield;
    AttackStyle attackStyle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        originalLaserFiringSpeed = player.GetComponent<Player>().OriginalLaserSpeed;
        originalLaserFiringPeriod = player.GetComponent<Player>().OriginalLaserFiringPeriod;
        originalBombFiringSpeed = player.GetComponent<Player>().OriginalBombSpeed;
        originalBombFiringPeriod = player.GetComponent<Player>().OriginalBombFiringPeriod;

        playerLasersParent = GameObject.Find(PLAYER_LASERS);
        if (!playerLasersParent)
        {
            playerLasersParent = new GameObject(PLAYER_LASERS);
        }

        // params for explosion/spread of bonuses
        spreadingAngle = UnityEngine.Random.Range(0, 360);
        initialPos = new Vector2(transform.position.x, transform.position.y);
        maxDistance = enemySize / UnityEngine.Random.Range(2f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        Spread();

        if (player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < attractionDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (bonusSpeed + accelerationAttraction) * Time.deltaTime);
                accelerationAttraction += accelerationAttraction;
                if (transform.position == player.transform.position)
                {
                    GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    if (this.name.Contains("PowerUp 1") && powerUp1Routine == null)
                    {
                        DestroyExistingPowerUp1();
                        powerUp1Routine = StartCoroutine(PowerUp1Routine());
                    }
                    if (this.name.Contains("PowerUp 2") && powerUp2Routine == null)
                    {
                        DestroyExistingPowerUp2();
                        powerUp2Routine = StartCoroutine(PowerUp2Routine());
                    }
                    if (this.name.Contains("PowerUp 3") && powerUp3Routine == null)
                    {
                        DestroyExistingPowerUp3();
                        powerUp3Routine = StartCoroutine(PowerUp3Routine());
                    }
                    if (this.name.Contains("pink"))
                    {
                        GameObject.FindWithTag("GameSession").GetComponent<GameSession>().counterStarBronze++;
                        Destroy(gameObject);
                        AudioSource.PlayClipAtPoint(coinSound, player.transform.position, volumeSound);
                    }
                    if (this.name.Contains("cyan"))
                    {
                        GameObject.FindWithTag("GameSession").GetComponent<GameSession>().counterStarSilver++;
                        Destroy(gameObject);
                        AudioSource.PlayClipAtPoint(coinSound, player.transform.position, volumeSound);
                    }
                    if (this.name.Contains("green"))
                    {
                        GameObject.FindWithTag("GameSession").GetComponent<GameSession>().counterStarGold++;
                        Destroy(gameObject);
                        AudioSource.PlayClipAtPoint(coinSound, player.transform.position, volumeSound);
                    }
                }
            }
        }

        if (powerUp3Routine != null)
        {
            MakeLasersPhysical(); // so that they can physically collide with the shield
        }
    }

    private void Spread()
    {
        float deltaX;
        float deltaY;

        if (Vector2.Distance(initialPos, transform.position) < maxDistance)
        {
            deltaX = Mathf.Cos(spreadingAngle) * explosionSpeed * Time.deltaTime;
            deltaY = Mathf.Sin(spreadingAngle) * explosionSpeed * Time.deltaTime;
            transform.position += new Vector3(deltaX, deltaY, 0);
        }
        else
        {
            deltaX = Mathf.Cos(spreadingAngle) * bonusSpeed * Time.deltaTime;
            deltaY = Mathf.Sin(spreadingAngle) * bonusSpeed * Time.deltaTime;
            transform.position += new Vector3(deltaX, deltaY, 0);
        }
    }

    private void DestroyExistingPowerUp1()
    {
        allBonuses = FindObjectsOfType<Bonus>();

        foreach (Bonus bonus in allBonuses)
        {
            if (bonus.gameObject.name.Contains("PowerUp 1"))
            {
                if (bonus.powerUp1Routine != null)
                {
                    Destroy(bonus.gameObject);
                }
            }
        }
    }

    private void DestroyExistingPowerUp2()
    {
        allBonuses = FindObjectsOfType<Bonus>();

        foreach (Bonus bonus in allBonuses)
        {
            if (bonus.gameObject.name.Contains("PowerUp 2"))
            {
                if (bonus.powerUp2Routine != null)
                {
                    Destroy(bonus.gameObject);
                }
            }
        }
    }

    private void DestroyExistingPowerUp3()
    {
        allBonuses = FindObjectsOfType<Bonus>();

        foreach (Bonus bonus in allBonuses)
        {
            if (bonus.gameObject.name.Contains("PowerUp 3"))
            {
                if (bonus.powerUp3Routine != null)
                {
                    Destroy(bonus.gameObject);
                }
            }
        }
    }

    private static void MakeLasersPhysical()
    {
        GameObject enemyLasersParent = GameObject.Find("Enemy Lasers Parent");
        foreach (Transform laserTransform in enemyLasersParent.transform)
        {
            laserTransform.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }

    private IEnumerator PowerUp1Routine()
    {
        player.GetComponent<Player>().LaserFiringPeriod = originalLaserFiringPeriod / 2f;
        player.GetComponent<Player>().BombFiringPeriod = originalBombFiringPeriod / 2f;
        yield return new WaitForSeconds(durationPowerUp1);
        player.GetComponent<Player>().LaserFiringPeriod = originalLaserFiringPeriod;
        player.GetComponent<Player>().BombFiringPeriod = originalBombFiringPeriod;
        Destroy(gameObject);
    }

    private IEnumerator PowerUp2Routine()
    {
        TripleFire();
        yield return new WaitForSeconds(durationPowerUp2);
        Destroy(gameObject);
    }

    private IEnumerator PowerUp3Routine()
    {
        shield = FindObjectOfType<Shield>();
        player.IsInvincible = true;
        shield.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        shield.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        shield.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
        //enemyLasersParent.GetComponentInChildren<CapsuleCollider2D>().isTrigger = false;

        //        GameObject.Find("Laser Enemy").GetComponent<CapsuleCollider2D>().isTrigger = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(durationPowerUp3);
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.IsInvincible = false;
        shield.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        shield.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        //GameObject.Find("Laser Enemy").GetComponent<CapsuleCollider2D>().isTrigger = true;
        Destroy(gameObject);
    }

    private IEnumerator PowerUp4Routine()
    {
        //Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //transform.position = new Vector3(transform.position.x, transform.position.y, 10);
        //player = FindObjectOfType<Player>();
        ////Debug.Log(player.posi)
        //BombPlayer[] bomb;
        //for (int i = 0; i < 10; i++)
        //{
        //    //   bomb[i] = Instantiate(BombPlayer, currentPosition, Quaternion.identity);
        //}
        ////Instantiate("Bomb Player", transform.position, Quaternion.identity);
        //float originalFiringPerdiod = player.GetComponent<Player>().projectileFiringPeriod;
        //player.GetComponent<Player>().projectileFiringPeriod = 0.03f;
        yield return new WaitForSeconds(5);
        //player.GetComponent<Player>().projectileFiringPeriod = originalFiringPerdiod;
        //Destroy(gameObject);
    }

    private void TripleFire()
    {
        firingCoroutine = StartCoroutine(FireContinuously());
    }

    private IEnumerator FireContinuously()
    {
        attackStyle = FindObjectOfType<AttackStyle>();
        while (true)
        {
            float laserFiringPeriod = player.GetComponent<Player>().LaserFiringPeriod;
            float bombFiringPeriod = player.GetComponent<Player>().BombFiringPeriod;
            float firingPeriod = 0;
            float rightStickX = Input.GetAxis("Mouse X");
            float rightStickY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(rightStickX) > 0.01 || Mathf.Abs(rightStickY) > 0.01)
            {
                float xVelocity = Input.GetAxis("Mouse X") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));
                float yVelocity = Input.GetAxis("Mouse Y") / (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")));

                if (attackStyle.GetAttackStyle() == "lasers")
                {
                    Vector2 originalLaserRotation = new Vector2(xVelocity, yVelocity);
                    Vector2 rotatedVectorLaser1 = Quaternion.Euler(0, 0, -20) * originalLaserRotation * originalLaserFiringSpeed;
                    Vector2 rotatedVectorLaser2 = Quaternion.Euler(0, 0, 20) * originalLaserRotation * originalLaserFiringSpeed;

                    var projectilePos = new Vector3(player.transform.position.x, player.transform.position.y, 0.1f);
                    Laser laser1 = Instantiate(laserPrefab, projectilePos, player.transform.rotation, playerLasersParent.transform);
                    Laser laser2 = Instantiate(laserPrefab, projectilePos, player.transform.rotation, playerLasersParent.transform);

                    laser1.GetComponent<Rigidbody2D>().velocity = rotatedVectorLaser1;
                    laser2.GetComponent<Rigidbody2D>().velocity = rotatedVectorLaser2;
                    firingPeriod = laserFiringPeriod;
                }
                else if (attackStyle.GetAttackStyle() == "bombs")
                {
                    Vector2 originalLaserRotation = new Vector2(xVelocity, yVelocity);
                    Vector2 rotatedVectorLaser1 = Quaternion.Euler(0, 0, -20) * originalLaserRotation * originalBombFiringSpeed;
                    Vector2 rotatedVectorLaser2 = Quaternion.Euler(0, 0, 20) * originalLaserRotation * originalBombFiringSpeed;

                    var projectilePos = new Vector3(player.transform.position.x, player.transform.position.y, 0.1f);
                    BombPlayer bomb1 = Instantiate(bombPrefab, projectilePos, player.transform.rotation, playerLasersParent.transform);
                    BombPlayer bomb2 = Instantiate(bombPrefab, projectilePos, player.transform.rotation, playerLasersParent.transform);

                    bomb1.GetComponent<Rigidbody2D>().velocity = rotatedVectorLaser1;
                    bomb2.GetComponent<Rigidbody2D>().velocity = rotatedVectorLaser2;
                    firingPeriod = bombFiringPeriod;
                }
            }

            yield return new WaitForSeconds(firingPeriod);
        }
    }

    public void SetEnemySize(float currentEnemySize)
    {
        enemySize = currentEnemySize;
    }

}
