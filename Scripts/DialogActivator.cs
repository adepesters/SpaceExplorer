using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogActivator : MonoBehaviour
{
    [SerializeField] [TextArea] string[] lines;
    [SerializeField] string actionText;
    [SerializeField] bool[] isQuestion;
    [SerializeField] bool dontNeedToPressX; // don't need to press X to activate dialog --> automatically launches dialog when entering trigger collider
    [SerializeField] GameObject avatarSprite;
    [SerializeField] bool activatesNextDialog;
    [SerializeField] GameObject nextDialog;

    bool canActivate;
    bool exitedTheScene = false;

    DialogManager dialogManager;
    ActionBoxManager actionBoxManager;

    float counter; // to make sure dialog don't jump from one to the next before even playing

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
        if (canActivate && !dialogManager.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            if (GetComponent<MultipleChoiceDialog>() != null)
            {
                dialogManager.ShowDialog(lines, isQuestion, GetComponent<MultipleChoiceDialog>().GetChoices(), transform.gameObject);
                dialogManager.AvatarSprite = avatarSprite.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                dialogManager.ShowDialog(lines, isQuestion, null, transform.gameObject);
                dialogManager.AvatarSprite = avatarSprite.GetComponent<SpriteRenderer>().sprite;
            }
        }

        if (canActivate)
        {
            counter += Time.fixedDeltaTime;
        }

        if (dialogManager.CurrentDialogIsDone)
        {
            if (activatesNextDialog && counter > 0.5f)
            {
                GetComponent<Collider2D>().enabled = false;
                nextDialog.GetComponent<Collider2D>().enabled = true;
                dialogManager.CurrentDialogIsDone = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) // player
        {
            canActivate = true;
            if (dontNeedToPressX)
            {
                dialogManager.DontNeedToPressX = true;
            }
            else
            {
                EnableActionBox();
            }
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
            if (!dontNeedToPressX)
            {
                DisableActionBox();
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
                FindObjectOfType<DialogManager>().StopCanShow();
            }
        }
    }

    private static void DisableActionBox()
    {
        FindObjectOfType<DialogManager>().StopCanShow();
        FindObjectOfType<ActionBoxManager>().gameObject.GetComponent<Canvas>().enabled = false;
    }

}
