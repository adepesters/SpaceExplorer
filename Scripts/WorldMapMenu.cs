using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMenu : MonoBehaviour
{
    Camera WorldmapCam;

    float originalZoom;
    float speedZoom = 8000f;
    float minWorldmapCamSize = 1000f;
    float maxWorldmapCamSize = 12000f;

    float speedMove = 1000f;
    float minXWorldmapCamBorder = -10000f;
    float maxXWorldmapCamBorder = 10000f;
    float minYWorldmapCamBorder = -10000f;
    float maxYWorldmapCamBorder = 10000f;

    float x;
    float y;
    float z;

    // Start is called before the first frame update
    void Start()
    {
        WorldmapCam = GameObject.Find("Worldmap Camera").GetComponent<Camera>();
        originalZoom = WorldmapCam.orthographicSize;

        x = WorldmapCam.transform.position.x;
        y = WorldmapCam.transform.position.y;
        z = WorldmapCam.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PauseController>().IsGamePaused() && FindObjectOfType<PauseMenuController>().GetMenuPage() == "world map")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }

        ZoomOut();
        ZoomIn();
        Move();
    }

    private void Move()
    {
        x += Input.GetAxis("Horizontal") * speedMove;
        y += Input.GetAxis("Vertical") * speedMove;

        x = Mathf.Clamp(x, minXWorldmapCamBorder, maxXWorldmapCamBorder);
        y = Mathf.Clamp(y, minYWorldmapCamBorder, maxYWorldmapCamBorder);

        WorldmapCam.transform.position = new Vector3(x, y, z);
    }

    private void ZoomIn()
    {
        if (FindObjectOfType<PS4ControllerCheck>().ContinuousR2Press())
        {
            WorldmapCam.orthographicSize -= speedZoom * Time.fixedDeltaTime;
            WorldmapCam.orthographicSize = Mathf.Clamp(WorldmapCam.orthographicSize, minWorldmapCamSize, maxWorldmapCamSize);
        }
    }

    private void ZoomOut()
    {
        if (FindObjectOfType<PS4ControllerCheck>().ContinuousL2Press())
        {
            WorldmapCam.orthographicSize += speedZoom * Time.fixedDeltaTime;
            WorldmapCam.orthographicSize = Mathf.Clamp(WorldmapCam.orthographicSize, minWorldmapCamSize, maxWorldmapCamSize);
        }
    }
}
