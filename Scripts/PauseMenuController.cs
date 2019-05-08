using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public List<string> menuPages;

    string[] defaultMenuPages = new string[] { "pause", "crafting", "world map" };

    int indexMenuPage;

    PS4ControllerCheck PS4ControllerCheck;

    // Start is called before the first frame update
    void Start()
    {
        PS4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();

        foreach (string menuPage in defaultMenuPages)
        {
            menuPages.Add(menuPage);
        }

        indexMenuPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PS4ControllerCheck.IsL1Pressed())
        {
            ChangeToLeftMenuPage();
        }

        if (PS4ControllerCheck.IsR1Pressed())
        {
            ChangeToRightMenuPage();
        }
    }

    private void ChangeToRightMenuPage()
    {
        indexMenuPage++;
        indexMenuPage = Mathf.Clamp(indexMenuPage, 0, menuPages.Count - 1);
    }

    private void ChangeToLeftMenuPage()
    {
        indexMenuPage--;
        indexMenuPage = Mathf.Clamp(indexMenuPage, 0, menuPages.Count - 1);
    }

    public string GetMenuPage()
    {
        return menuPages[indexMenuPage];
    }

    public void SetIndexMenuPage(int newIndexMenuPage)
    {
        indexMenuPage = newIndexMenuPage;
    }
}
