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

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    // cam settings
    Camera cam;
    float originalAperture;

    // player sprite in canvas
    [SerializeField] Image playerSprite;
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
            originalSpriteScale = new Vector3(playerSprite.GetComponent<RectTransform>().localScale.x,
            playerSprite.GetComponent<RectTransform>().localScale.y, playerSprite.GetComponent<RectTransform>().localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton13))
        {
            IsPaused = !IsPaused;
            pauseMenuController.SetIndexMenuPage(0);
        }

        if (IsPaused)
        {
            Time.timeScale = 0;
            GameObject.Find("Background Pause Menu").GetComponent<Canvas>().enabled = true; // I added this background to prevent some clipping 
                                                                                            // when we go from one page of the menu to the next
            if (gameSession.SceneType == "planet")
            {
                // change cam settings
                cam.GetComponent<DepthOfField>().aperture = 1;
                cam.GetComponent<Blur>().enabled = true;

                // add player sprite in UI
                playerSprite.GetComponent<Image>().sprite = playerTileVania.GetComponentInChildren<SpriteRenderer>().sprite;
                playerSprite.GetComponent<RectTransform>().localScale = new Vector3(originalSpriteScale.x * Mathf.Sign(playerTileVania.transform.localScale.x),
                originalSpriteScale.y, originalSpriteScale.z);
            }
        }
        else
        {
            Time.timeScale = 1;
            GameObject.Find("Background Pause Menu").GetComponent<Canvas>().enabled = false;

            if (gameSession.SceneType == "planet")
            {
                cam.GetComponent<DepthOfField>().aperture = originalAperture;
                cam.GetComponent<Blur>().enabled = false;
            }
        }
    }

    public bool IsGamePaused()
    {
        return IsPaused;
    }
}
