using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    bool canActivate;

    ActionBoxManager actionBoxManager;
    PS4ControllerCheck pS4Controller;
    PauseController pauseController;

    [SerializeField] string actionText;

    bool canAppear = true;

    public delegate void MyDelegate();
    MyDelegate myDelegate;

    public MyDelegate MyDelegate1 { get => myDelegate; set => myDelegate = value; }
    public bool CanAppear { get => canAppear; set => canAppear = value; }

    // Start is called before the first frame update
    void Start()
    {
        pauseController = FindObjectOfType<PauseController>();
        actionBoxManager = FindObjectOfType<ActionBoxManager>();
        pS4Controller = GameObject.FindWithTag("PS4ControllerCheck").GetComponent<PS4ControllerCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && pS4Controller.IsXPressed() && !pauseController.IsPaused)
        {
            myDelegate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAppear)
        {
            if (collision.gameObject.name.Contains("Player") || collision.gameObject.name.Contains("PlayerTileVania"))
            {
                canActivate = true;
                EnableActionBox();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player") || collision.gameObject.name.Contains("PlayerTileVania"))
        {
            canActivate = false;
            DisableActionBox();
        }
    }

    public void EnableActionBox()
    {
        actionBoxManager.gameObject.GetComponent<Canvas>().enabled = true;
        actionBoxManager.SetPos(transform.position);
        actionBoxManager.SetText(actionText);
    }

    public void DisableActionBox()
    {
        FindObjectOfType<ActionBoxManager>().gameObject.GetComponent<Canvas>().enabled = false;
    }

}
