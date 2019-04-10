using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoicesConsequencesPlanet1 : MonoBehaviour
{
    int selectedChoice;
    int hasAnswered;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasAnswered == 1 && selectedChoice == 0)
        {
            Yes();
        }
        else if (hasAnswered == 1 && selectedChoice == 1)
        {
            No();
        }
    }

    private void Yes()
    {
        // Debug.Log("yes");
        SceneManager.LoadScene("Planet 1");
        hasAnswered = 0;
    }

    private void No()
    {
        // Debug.Log("no");
        hasAnswered = 0;
    }

    public void SetSelectedChoice(Vector2 answeredAndSelectedChoice)
    {
        selectedChoice = (int)answeredAndSelectedChoice[1];
        hasAnswered = (int)answeredAndSelectedChoice[0];
    }
}
