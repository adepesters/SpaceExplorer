using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerCraftingMenu : MonoBehaviour
{
    ///<summary>Placeholder delegate function for our buttonList</summary>
    public delegate void ButtonAction();
    ///<summary>Array of buttons, created from a struct, below.</summary>
    public MyButton[] buttonList;
    ///<summary>Index reference to our currently selected button.</summary>
    public int selectedButton = 0;

    PauseController pauseController;
    PauseMenuController pauseMenuController;
    PS4ControllerCheck PS4ControllerCheck;
    CraftingMenu craftingMenu;

    void Start()
    {
        pauseController = FindObjectOfType<PauseController>();
        pauseMenuController = FindObjectOfType<PauseMenuController>();
        PS4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();
        craftingMenu = FindObjectOfType<CraftingMenu>();

        // Instantiate buttonList to hold the amount of buttons we are using.
        buttonList = new MyButton[1];
        // Set up the first button, finding the game object based off its name. We also 
        // must set the expected onClick method, and should trigger the selected colour.
        buttonList[0].image = GameObject.Find("Button Upgrade 1").GetComponent<Image>();
        buttonList[0].image.color = new Color(1, 1, 1, 0f);
        buttonList[0].action = PlayButtonAction;
        // Do the same for the second button. We are also ensuring the image colour is
        // set to our normalColor, to ensure uniformity.
        //buttonList[1].image = GameObject.Find("OptionsButton").GetComponent<Image>();
        //buttonList[1].image.color = new Color(0, 0f, 0f, 0.4f);
        //buttonList[1].action = OptionsButtonAction;
    }

    void Update()
    {
        if (pauseController.IsGamePaused() && pauseMenuController.GetMenuPage() == "crafting")
        {
            if (PS4ControllerCheck.DiscreteMoveUp())
            {
                MoveToPreviousButton();
            }
            else if (PS4ControllerCheck.DiscreteMoveDown())
            {
                MoveToNextButton();
            }
            if (PS4ControllerCheck.IsXPressed())
            {
                buttonList[selectedButton].action();
            }
        }
    }

    void MoveToNextButton()
    {
        // Reset the currently selected button to the default colour.
        buttonList[selectedButton].image.color = new Color(0, 0f, 0f, 0.4f);
        // Increment our selected button index by 1.
        selectedButton++;
        selectedButton = Mathf.Clamp(selectedButton, 0, buttonList.Length - 1);
        // Set the currently selected button to the "selected" colour.
        buttonList[selectedButton].image.color = new Color(1, 1, 1, 0f);
    }

    void MoveToPreviousButton()
    {
        // Should be self explanatory; similar in function to MoveToNextButton,
        // but instead, we are moving back a button.
        buttonList[selectedButton].image.color = new Color(0, 0f, 0f, 0.4f);
        selectedButton--;
        selectedButton = Mathf.Clamp(selectedButton, 0, buttonList.Length - 1);
        buttonList[selectedButton].image.color = new Color(1, 1, 1, 0f);
    }

    ///<summary>This is the method that will call when selecting "Play".</summary>
    void PlayButtonAction()
    {
        craftingMenu.Upgrade1();
    }

    ///<summary>This is the method that will call when selecting "Options".</summary>
    void OptionsButtonAction()
    {
        Debug.Log("Options");
    }

    ///<summary>A struct to represent individual buttons. This makes it easier to wrap
    /// the required variables into a single container. Don't forget 
    /// [System.Serializable], if you wish to see your final array in the inspector.
    [System.Serializable]
    public struct MyButton
    {
        /// <summary>The image contained in the button.</summary>
        public Image image;
        /// <summary>The delegate method to invoke on action.</summary>
        public ButtonAction action;
    }
}
