using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    bool isPaused = false;

    PauseMenuController pauseMenuController;
    GameSession gameSession;
    PlayerTileVania playerTileVania;
    Player player;

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    // cam settings
    Camera cam;
    float originalAperture;

    // player sprite in canvas
    [SerializeField] Image playerSprite;
    //[SerializeField] Image playerSpaceSprite;
    Vector3 originalSpriteScale; // original scale of player sprite

    void Start()
    {
        pauseMenuController = FindObjectOfType<PauseMenuController>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();

        originalAperture = cam.GetComponent<DepthOfField>().aperture;

        if (gameSession.SceneType == "planet")
        {
            playerTileVania = FindObjectOfType<PlayerTileVania>();
            if (playerSprite != null)
            {
                originalSpriteScale = new Vector3(playerSprite.GetComponent<RectTransform>().localScale.x,
                playerSprite.GetComponent<RectTransform>().localScale.y, playerSprite.GetComponent<RectTransform>().localScale.z);
            }
        }
        //if (gameSession.SceneType == "space")
        //{
        //    player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //    if (playerSpaceSprite != null)
        //    {

        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton13))
        {
            IsPaused = !IsPaused;
            pauseMenuController.SetIndexMenuPage(0);
        }

        if (gameSession.SceneType == "planet")
        {
            if (IsPaused)
            {
                Time.timeScale = 0;
                FindObjectOfType<ActionBoxManager>().gameObject.GetComponent<Canvas>().enabled = false; // remove potential visible action box
                GameObject.Find("Pause Menu Planet").GetComponent<Canvas>().enabled = true; // I added this background to prevent some clipping 
                                                                                            // when we go from one page of the menu to the next
                                                                                            // change cam settings
                cam.GetComponent<DepthOfField>().aperture = 1;
                cam.GetComponent<Blur>().enabled = true;

                //// add player sprite in UI
                //if (playerTileVania != null)
                //{
                //    playerSprite.GetComponent<Image>().sprite = playerTileVania.GetComponentInChildren<SpriteRenderer>().sprite;
                //    playerSprite.GetComponent<RectTransform>().localScale = new Vector3(originalSpriteScale.x * Mathf.Sign(playerTileVania.transform.localScale.x),
                //    originalSpriteScale.y, originalSpriteScale.z);
                //}
            }
            else
            {
                Time.timeScale = 1;
                if (GameObject.Find("Pause Menu Planet") != null)
                {
                    GameObject.Find("Pause Menu Planet").GetComponent<Canvas>().enabled = false;
                }

                if (cam.gameObject != null)
                {
                    cam.GetComponent<DepthOfField>().aperture = originalAperture;
                    if (cam.GetComponent<Blur>() != null)
                    {
                        cam.GetComponent<Blur>().enabled = false;
                    }
                }
            }
        }

        if (gameSession.SceneType == "space")
        {
            if (IsPaused)
            {
                Time.timeScale = 0;
                FindObjectOfType<ActionBoxManager>().gameObject.GetComponent<Canvas>().enabled = false; // remove potential visible action box
                GameObject.Find("Pause Menu Space").GetComponent<Canvas>().enabled = true; // I added this background to prevent some clipping 
                                                                                           // when we go from one page of the menu to the next
                                                                                           // change cam settings
                cam.GetComponent<DepthOfField>().aperture = 1;
                cam.GetComponent<Blur>().enabled = true;

                // add player sprite in UI
                if (playerTileVania != null)
                {
                    playerSprite.GetComponent<Image>().sprite = playerTileVania.GetComponentInChildren<SpriteRenderer>().sprite;
                    playerSprite.GetComponent<RectTransform>().localScale = new Vector3(originalSpriteScale.x * Mathf.Sign(playerTileVania.transform.localScale.x),
                    originalSpriteScale.y, originalSpriteScale.z);
                }
            }
            else
            {
                Time.timeScale = 1;
                if (GameObject.Find("Pause Menu Space") != null)
                {
                    GameObject.Find("Pause Menu Space").GetComponent<Canvas>().enabled = false;
                }

                if (cam.gameObject != null)
                {
                    cam.GetComponent<DepthOfField>().aperture = originalAperture;
                    if (cam.GetComponent<Blur>() != null)
                    {
                        cam.GetComponent<Blur>().enabled = false;
                    }
                }
            }
        }
    }

    public bool IsGamePaused()
    {
        return IsPaused;
    }
}
