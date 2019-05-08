using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    [SerializeField] [TextArea] string[] lines;
    [SerializeField] string actionText;
    [SerializeField] bool[] isQuestion;

    bool canActivate;
    bool exitedTheScene = false;

    DialogManager dialogManager;
    ActionBoxManager actionBoxManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
        actionBoxManager = FindObjectOfType<ActionBoxManager>();

        //lines[0] = "Je suis une princesse... une fée ... on ne sait pas trop...";
        //lines[1] = "Je vais te dire un secret...";
        //lines[2] = "Il y a une planète merveilleuse au nord d'ici...";
        //lines[3] = "Tu rencontreras beaucoup d'obstacles sur ta route... mais tu es brave et j'ai confiance en toi.";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(FindObjectOfType<DialogManager>().transform.GetChild(0).gameObject);
        if (canActivate && !dialogManager.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            if (GetComponent<MultipleChoiceDialog>() != null)
            {
                dialogManager.ShowDialog(lines, isQuestion, GetComponent<MultipleChoiceDialog>().GetChoices(), transform.gameObject);
            }
            else
            {
                dialogManager.ShowDialog(lines, isQuestion, null, transform.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) // player
        {
            canActivate = true;
            FindObjectOfType<RedRadar>().SetCurrentTargetRedRadar(GameObject.Find("Planet 1"));
            EnableActionBox();
        }
    }

    private void EnableActionBox()
    {
        actionBoxManager.gameObject.GetComponent<Canvas>().enabled = true;
        actionBoxManager.SetPos(transform.position);
        actionBoxManager.SetText(actionText);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) // player
        {
            canActivate = false;
            DisableActionBox();
        }
    }

    private static void DisableActionBox()
    {
        FindObjectOfType<DialogManager>().StopCanShow();
        FindObjectOfType<ActionBoxManager>().gameObject.GetComponent<Canvas>().enabled = false;
    }

}
