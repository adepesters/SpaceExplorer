using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTileVania : MonoBehaviour
{
    float runSpeed = 6f;
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] float climbSpeed = 6f;

    // cached variables
    Animator animator;
    Vector2 originalScale;
    Rigidbody2D rigidBody;

    bool isDead = false;
    bool playerIsImmobile = true;

    bool isJumping = false;

    [SerializeField] Sword rightSwordPrefab;
    [SerializeField] Sword leftSwordPrefab;

    bool playerIsFrozen;
    Vector2 frozenPosition;
    bool playerOnAir;
    Vector2 onAirPos;

    bool canJump = false;

    bool isOnATree = false;

    public bool IsOnATree { get => isOnATree; set => isOnATree = value; }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
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
            if (Mathf.Sign(transform.localScale.x) > 0)
            {
                SwingSwordRight();
            }
            else
            {
                SwingSwordLeft();
            }
            //Climb();
        }

        if (!isDead && playerIsFrozen)
        {
            animator.speed = 0f;
            transform.position = frozenPosition;
        }
    }

    private void CheckIfIsImmobile()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Epsilon && Mathf.Abs(rigidBody.velocity.y) < Mathf.Epsilon)
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
        if (playerIsImmobile || (GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")) && (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon)))
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
            float xChange = Input.GetAxis("Horizontal") * runSpeed; // we don't put deltaTime here. See notes.
            rigidBody.velocity = new Vector2(xChange, rigidBody.velocity.y);
            if (xChange != 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(xChange) * originalScale.x, originalScale.y);
            }
        }
    }

    private void Jump()
    {
        if (FindObjectOfType<PS4ControllerCheck>().IsXPressed() && ((FindObjectOfType<Ground>().AreFeetCloseToTheGround()) || isOnATree))
        {
            canJump = true;
        }
        if (canJump == true && (FindObjectOfType<Ground>().AreFeetOnTheGround() || isOnATree))
        {
            rigidBody.gravityScale = 3; // in case we jump from a ladder (where gravity is 0)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
            isJumping = true;
            canJump = false;
            isOnATree = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
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
        if (FindObjectOfType<PS4ControllerCheck>().IsSquarePressed())
        {
            var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
            Sword sword = Instantiate(rightSwordPrefab, swordPos, Quaternion.identity);
        }
    }

    private void SwingSwordLeft()
    {
        if (FindObjectOfType<PS4ControllerCheck>().IsSquarePressed())
        {
            var swordPos = GameObject.Find("SwordHandler").gameObject.transform.position;
            Sword sword = Instantiate(leftSwordPrefab, swordPos, Quaternion.identity);
        }
    }

    public void SetFrozenPlayer(bool currentPlayerIsFrozen, Vector2 currentFrozenPosition)
    {
        playerIsFrozen = currentPlayerIsFrozen;
        frozenPosition = currentFrozenPosition;
    }

    public void SetPlayerOnAir(bool currentPlayerOnAir, Vector2 currentPlayerPos)
    {
        playerOnAir = currentPlayerOnAir;
        onAirPos = currentPlayerPos;
    }
}
