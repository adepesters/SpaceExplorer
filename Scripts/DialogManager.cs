using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    [SerializeField] GameObject dialogPanel;
    [SerializeField] Text dialogText;

    string[] dialogLines;
    bool[] isAQuestion;
    List<string> choices;
    int currentLineIndex;

    Coroutine makeLineAppear;
    Coroutine makeQuestionAppear;

    float textDisplaySpeed = 0.04f;

    [SerializeField] AudioClip blipSound;
    [SerializeField] float blipSoundVolume = 1f;

    bool canShow;
    bool questionWasAsked;

    int selectedChoice = 0;

    GameObject parent;

    PS4ControllerCheck PS4ControllerCheck;
    Player player;
    ActionBoxManager actionBoxManager;

    // Start is called before the first frame update
    void Start()
    {
        PS4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();
        player = FindObjectOfType<Player>();
        actionBoxManager = FindObjectOfType<ActionBoxManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(canShow);
        //Debug.Log(makeQuestionAppear);
        //Debug.Log(questionWasAsked);
        //Debug.Log(selectedChoice);
        if (canShow)
        {
            if (PS4ControllerCheck.IsXPressed() && makeLineAppear == null)
            {
                actionBoxManager.gameObject.GetComponent<Canvas>().enabled = false;
                currentLineIndex++;
                if (currentLineIndex >= dialogLines.Length)
                {
                    MakeGameObjectsMobile();
                    dialogPanel.SetActive(false);
                    canShow = false;
                }
                else
                {
                    if (makeLineAppear == null)
                    {
                        if (isAQuestion.Length != 0)
                        {
                            if (!isAQuestion[currentLineIndex])
                            {
                                dialogPanel.SetActive(true);
                                MakeGameObjectsImmobile();
                                dialogText.text = "";
                                makeLineAppear = StartCoroutine(MakeLineAppear(dialogLines[currentLineIndex]));
                            }
                            else
                            {
                                MakeGameObjectsImmobile();
                                //Debug.Log("is question");
                                dialogPanel.SetActive(true);
                                dialogText.text = "";
                                makeQuestionAppear = StartCoroutine(MakeQuestionAppear(dialogLines[currentLineIndex]));
                            }
                        }
                        else
                        {
                            MakeGameObjectsImmobile();
                            dialogPanel.SetActive(true);
                            dialogText.text = "";
                            makeLineAppear = StartCoroutine(MakeLineAppear(dialogLines[currentLineIndex]));
                        }
                    }
                }
            }

            if (makeQuestionAppear == null && questionWasAsked == true)
            {
                if (PS4ControllerCheck.DiscreteMoveDown())
                {
                    selectedChoice++;
                    selectedChoice = Mathf.Clamp(selectedChoice, 0, choices.Count - 1);

                    dialogText.text = "";
                    dialogText.text += dialogLines[currentLineIndex];
                    for (int i = 0; i < choices.Count; i++)
                    {
                        if (i == selectedChoice)
                        {
                            dialogText.text += "\n" + "<color=yellow>" + choices[i] + "</color>";
                        }
                        else
                        {
                            dialogText.text += "\n" + "<color=grey>" + choices[i] + "</color>";
                        }
                    }
                }

                if (PS4ControllerCheck.DiscreteMoveUp())
                {
                    selectedChoice--;
                    selectedChoice = Mathf.Clamp(selectedChoice, 0, choices.Count - 1);

                    dialogText.text = "";
                    dialogText.text += dialogLines[currentLineIndex];
                    for (int i = 0; i < choices.Count; i++)
                    {
                        if (i == selectedChoice)
                        {
                            dialogText.text += "\n" + "<color=yellow>" + choices[i] + "</color>";
                        }
                        else
                        {
                            dialogText.text += "\n" + "<color=grey>" + choices[i] + "</color>";
                        }
                    }
                }

                if (PS4ControllerCheck.IsXPressed())
                {
                    parent.BroadcastMessage("SetSelectedChoice", new Vector2(1, selectedChoice));
                    questionWasAsked = false;
                }
            }
        }

        if (PS4ControllerCheck.ContinuousXPress())
        {
            textDisplaySpeed = 0.000001f;
            blipSoundVolume = 0.5f;
        }
        else
        {
            textDisplaySpeed = 0.04f;
            blipSoundVolume = 1f;
        }
    }

    IEnumerator MakeLineAppear(string line)
    {
        foreach (char caracter in line)
        {
            dialogText.text += caracter;
            GetComponent<AudioSource>().PlayOneShot(blipSound, blipSoundVolume);
            yield return new WaitForSeconds(textDisplaySpeed);
        }
        makeLineAppear = null;
    }

    IEnumerator MakeQuestionAppear(string line)
    {
        foreach (char caracter in line)
        {
            dialogText.text += caracter;
            GetComponent<AudioSource>().PlayOneShot(blipSound, blipSoundVolume);
            yield return new WaitForSeconds(textDisplaySpeed);
        }
        dialogText.text += "\n" + "<color=yellow>" + choices[0] + "</color>";
        for (int i = 1; i < choices.Count; i++)
        {
            dialogText.text += "\n" + "<color=grey>" + choices[i] + "</color>";
        }
        makeQuestionAppear = null;
        questionWasAsked = true;
    }

    public void ShowDialog(string[] currentLines, bool[] isQuestion, List<string> currentChoices, GameObject currentParent)
    {
        dialogLines = currentLines;
        isAQuestion = isQuestion;
        choices = currentChoices;
        parent = currentParent;
        currentLineIndex = -1;
        canShow = true;
    }

    public void StopCanShow()
    {
        canShow = false;
    }

    private void MakeGameObjectsImmobile()
    {
        player.SetImmobile(true);

        Laser[] lasers = FindObjectsOfType<Laser>();
        foreach (Laser laser in lasers)
        {
            laser.SetImmobile(true);
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetImmobile(true);
        }

        FacePlayer[] facePlayers = FindObjectsOfType<FacePlayer>();
        foreach (FacePlayer facePlayer in facePlayers)
        {
            facePlayer.SetImmobile(true);
        }

        MoveAroundPlayer[] moveAroundPlayers = FindObjectsOfType<MoveAroundPlayer>();
        foreach (MoveAroundPlayer moveAroundPlayer in moveAroundPlayers)
        {
            moveAroundPlayer.SetImmobile(true);
        }
    }

    private void MakeGameObjectsMobile()
    {
        player.SetImmobile(false);

        Laser[] lasers = FindObjectsOfType<Laser>();
        foreach (Laser laser in lasers)
        {
            laser.SetImmobile(false);
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetImmobile(false);
        }

        FacePlayer[] facePlayers = FindObjectsOfType<FacePlayer>();
        foreach (FacePlayer facePlayer in facePlayers)
        {
            facePlayer.SetImmobile(false);
        }

        MoveAroundPlayer[] moveAroundPlayers = FindObjectsOfType<MoveAroundPlayer>();
        foreach (MoveAroundPlayer moveAroundPlayer in moveAroundPlayers)
        {
            moveAroundPlayer.SetImmobile(false);
        }
    }
}
