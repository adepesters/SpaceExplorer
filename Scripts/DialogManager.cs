﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogPanel;
    [SerializeField] Text dialogText;
    Sprite avatarSprite;

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
    PlayerTileVania playerTileVania;
    ActionBoxManager actionBoxManager;
    GameSession gameSession;

    bool dontNeedToPressXToLaunch = false; // dialog launches automatically
    bool dontNeedToPressXToPass = false; // dialog goes to next line automatically

    bool currentDialogIsDone = false;

    float counterNextLineAutomatedDialog; // how much time between lines of dialog when it passes automatically

    public bool CanShow { get => canShow; set => canShow = value; }
    public bool DontNeedToPressXToLaunch { get => dontNeedToPressXToLaunch; set => dontNeedToPressXToLaunch = value; }
    public Sprite AvatarSprite { get => avatarSprite; set => avatarSprite = value; }
    public bool CurrentDialogIsDone { get => currentDialogIsDone; set => currentDialogIsDone = value; }
    public bool DontNeedToPressXToPass { get => dontNeedToPressXToPass; set => dontNeedToPressXToPass = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        PS4ControllerCheck = GameObject.FindWithTag("PS4ControllerCheck").GetComponent<PS4ControllerCheck>();

        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        if (FindObjectOfType<PlayerTileVania>() != null)
        {
            playerTileVania = FindObjectOfType<PlayerTileVania>();
        }
        actionBoxManager = FindObjectOfType<ActionBoxManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canShow);
        Debug.Log(currentLineIndex);
        counterNextLineAutomatedDialog += Time.fixedDeltaTime;
        if (CanShow)
        {
            if ((dontNeedToPressXToPass && counterNextLineAutomatedDialog > 5f && currentLineIndex > -1 ||
                dontNeedToPressXToLaunch && currentLineIndex == -1 ||
            PS4ControllerCheck.IsXPressed()) && makeLineAppear == null)
            {
                actionBoxManager.gameObject.GetComponent<Canvas>().enabled = false;
                currentLineIndex++;
                Debug.Log("length:" + dialogLines.Length);
                if (currentLineIndex >= dialogLines.Length)
                {
                    MakeGameObjectsMobile();
                    dialogPanel.SetActive(false);
                    CanShow = false;
                    currentDialogIsDone = true;
                    dontNeedToPressXToLaunch = false;
                    dontNeedToPressXToPass = false;
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
                                GameObject.FindWithTag("SpeakerAvatar").GetComponent<Image>().sprite = avatarSprite;
                                MakeGameObjectsImmobile();
                                dialogText.text = "";
                                makeLineAppear = StartCoroutine(MakeLineAppear(dialogLines[currentLineIndex]));
                            }
                            else
                            {
                                MakeGameObjectsImmobile();
                                //Debug.Log("is question");
                                dialogPanel.SetActive(true);
                                GameObject.FindWithTag("SpeakerAvatar").GetComponent<Image>().sprite = avatarSprite;
                                dialogText.text = "";
                                makeQuestionAppear = StartCoroutine(MakeQuestionAppear(dialogLines[currentLineIndex]));
                            }
                        }
                        else
                        {
                            MakeGameObjectsImmobile();
                            dialogPanel.SetActive(true);
                            GameObject.FindWithTag("SpeakerAvatar").GetComponent<Image>().sprite = avatarSprite;

                            // deals with mini cat avatar
                            if (avatarSprite.name.Contains("mini"))
                            {
                                GameObject.FindWithTag("SpeakerAvatar").GetComponent<RectTransform>().localPosition = new Vector2(-618, -308);
                                GameObject.FindWithTag("SpeakerAvatar").GetComponent<RectTransform>().sizeDelta = new Vector2(395.4f, 527.6f);
                            }
                            else
                            {
                                GameObject.FindWithTag("SpeakerAvatar").GetComponent<RectTransform>().localPosition = new Vector2(-590.9f, -115.8f);
                                GameObject.FindWithTag("SpeakerAvatar").GetComponent<RectTransform>().sizeDelta = new Vector2(395.4f, 746.1f);
                            }

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
        else if (!canShow && PS4ControllerCheck.IsXPressed()) // in rare bugs, found that canShow is false but it hasn't deactivated dialog panel
        {
            MakeGameObjectsMobile();
            dialogPanel.SetActive(false);
            currentDialogIsDone = true;
            dontNeedToPressXToLaunch = false;
            dontNeedToPressXToPass = false;
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
        selectedChoice = 0;
        foreach (char caracter in line)
        {
            dialogText.text += caracter;
            GetComponent<AudioSource>().PlayOneShot(blipSound, blipSoundVolume);
            yield return new WaitForSeconds(textDisplaySpeed);
        }
        makeLineAppear = null;
        counterNextLineAutomatedDialog = 0f;
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
        CanShow = true;
    }

    public void StopCanShow()
    {
        CanShow = false;
    }

    private void MakeGameObjectsImmobile()
    {
        if (gameSession.SceneType == "space")
        {
            player.ForceImmobility = true;

            player.IsImmobile1 = true;

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
        else if (gameSession.SceneType == "planet")
        {
            playerTileVania.ForceImmobility = true;
        }
    }

    private void MakeGameObjectsMobile()
    {
        if (gameSession.SceneType == "space")
        {
            player.ForceImmobility = false;
            player.IsImmobile1 = false;

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
        else if (gameSession.SceneType == "planet")
        {
            playerTileVania.ForceImmobility = false;
        }
    }
}
