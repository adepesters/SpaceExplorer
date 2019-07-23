using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoicesConsequencesPlanet2 : MonoBehaviour
{
    int selectedChoice;
    int hasAnswered;

    GameSession gameSession;
    DataManager dataManager;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
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
        //dataManager.SaveSpaceData();
        gameSession.SceneType = "planet";
        LoadingScreen.Instance.Show("Planet 2 Jungle");
        //SceneManager.LoadScene("Planet 1");
        hasAnswered = 0;
        gameSession.CurrentPlanetID = 2;
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
