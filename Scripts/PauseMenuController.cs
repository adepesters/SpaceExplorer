using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public List<string> menuPages;

    string[] defaultMenuPages = new string[] { "pause", "crafting", "world map" };

    int indexMenuPage;

    // Start is called before the first frame update
    void Start()
    {
        foreach (string menuPage in defaultMenuPages)
        {
            menuPages.Add(menuPage);
        }

        indexMenuPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PS4ControllerCheck>().IsL1Pressed())
        {
            ChangeToLeftMenuPage();
        }

        if (FindObjectOfType<PS4ControllerCheck>().IsR1Pressed())
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
