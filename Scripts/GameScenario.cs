using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenario : MonoBehaviour
{
    [SerializeField] GameObject enemyFirstEncounter;
    [SerializeField] GameObject dialogZephyr1;
    [SerializeField] GameObject dialogZephyr2;
    [SerializeField] GameObject dialogCat1;
    [SerializeField] GameObject dialogZephyr3;

    [SerializeField] GameObject virtualCam;

    // Start is called before the first frame update
    void Start()
    {
        dialogZephyr1.GetComponent<Collider2D>().enabled = true;
        enemyFirstEncounter.GetComponent<Collider2D>().enabled = false;
        dialogCat1.GetComponent<Collider2D>().enabled = false;
        dialogZephyr2.GetComponent<Collider2D>().enabled = false;
        dialogZephyr3.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (virtualCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView == 104)
        {
            dialogZephyr2.GetComponent<Collider2D>().enabled = true;
        }
    }
}
