using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMenu : MonoBehaviour
{
    Camera WorldmapCam;

    float originalZoom = 6100f;
    float speedZoom = 8000f;
    float minWorldmapCamSize = 1000f;
    float maxWorldmapCamSize = 12000f;

    float speedMove = 1000f;
    float minXWorldmapCamBorder = -30000f; //-10000f;
    float maxXWorldmapCamBorder = 30000f; //10000f;
    float minYWorldmapCamBorder = -30000f; //-10000f;
    float maxYWorldmapCamBorder = 30000f; //10000f;

    float xCam;
    float yCam;
    float zCam;

    float xPointer;
    float yPointer;
    float zPointer;

    float previousXpointer;
    float previousYpointer;

    Coroutine checkVisibleClouds;

    //Pointer pointer;
    GameObject pointer;

    bool movePointer = false;

    float currentHeightCam;
    float currentWidthCam;

    float marginPointer;

    // Start is called before the first frame update
    void Start()
    {
        WorldmapCam = GameObject.Find("Worldmap Camera").GetComponent<Camera>();

        WorldmapCam.orthographicSize = originalZoom;
        //originalZoom = WorldmapCam.orthographicSize;

        xCam = WorldmapCam.transform.position.x;
        yCam = WorldmapCam.transform.position.y;
        zCam = WorldmapCam.transform.position.z;

        //pointer = FindObjectOfType<Pointer>();
        pointer = GameObject.Find("Pointer");

        xPointer = pointer.transform.position.x;
        yPointer = pointer.transform.position.y;
        zPointer = pointer.transform.position.z;

        marginPointer = WorldmapCam.orthographicSize / 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PauseController>().IsGamePaused() && FindObjectOfType<PauseMenuController>().GetMenuPage() == "world map")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            ZoomOut();
            ZoomIn();
            Move();
            if (checkVisibleClouds == null)
            {
                checkVisibleClouds = StartCoroutine(CheckVisibleClouds());
            }

            if (FindObjectOfType<PS4ControllerCheck>().IsXPressed())
            {
                //if (movePointer)
                //{
                //    if (pointer.GetIsOnPlanet())
                //    {
                //        pointer.transform.position = pointer.GetPlanet().transform.position;
                //    }
                //}
                movePointer = !movePointer;
            }

            currentHeightCam = WorldmapCam.orthographicSize;
            currentWidthCam = 16f * WorldmapCam.orthographicSize / 9f;

            marginPointer = WorldmapCam.orthographicSize / 3f;

        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
            checkVisibleClouds = null;
        }

        //Debug.Log(pointer.GetIsOnPlanet());

        //Debug.Log("min border: " + minXWorldmapCamBorder);
        //Debug.Log("cam size: " + WorldmapCam.orthographicSize);
        //Debug.Log("corrected cam size: " + (16f * WorldmapCam.orthographicSize / 9f));
        //Debug.Log("full corrected cam size: " + (minXWorldmapCamBorder - (16f * WorldmapCam.orthographicSize / 9f) + 1500));
        //Debug.Log("pointer x: " + xPointer);
        //Debug.Log("width: " + currentWidthCam);
        //Debug.Log("height: " + currentHeightCam);
    }

    IEnumerator CheckVisibleClouds()
    {
        CloudingWorldmap[] clouds = FindObjectsOfType<CloudingWorldmap>();

        foreach (CloudingWorldmap cloud in clouds)
        {
            if (Vector2.Distance(FindObjectOfType<Player>().transform.position, cloud.transform.position) < 2000)
            {
                Destroy(cloud.gameObject);
            }
        }
        yield return null;
    }

    private void Move()
    {
        if (movePointer)
        {
            if (!CheckIfPointerIsInside())
            {
                xPointer = xCam;
                yPointer = yCam;
            }

            previousXpointer = xPointer;
            previousYpointer = yPointer;

            xPointer += Input.GetAxis("Horizontal") * speedMove;
            yPointer += Input.GetAxis("Vertical") * speedMove;

            //Debug.Log("min border: " + minXWorldmapCamBorder);
            //Debug.Log("cam size: " + WorldmapCam.orthographicSize);
            //Debug.Log("corrected cam size: " + (16f * WorldmapCam.orthographicSize / 9f));
            //Debug.Log("full corrected cam size: " + (minXWorldmapCamBorder - (16f * WorldmapCam.orthographicSize / 9f) + 1500));
            //Debug.Log("pointer x: " + xPointer);
            //Debug.Log("width: " + currentWidthCam);
            //Debug.Log("height: " + currentHeightCam);

            xPointer = Mathf.Clamp(xPointer, minXWorldmapCamBorder + marginPointer, maxXWorldmapCamBorder - marginPointer);
            yPointer = Mathf.Clamp(yPointer, minYWorldmapCamBorder + marginPointer, maxYWorldmapCamBorder - marginPointer);

            pointer.transform.position = new Vector3(xPointer, yPointer, zPointer);

            if ((yPointer > yCam + currentHeightCam - marginPointer || yPointer < yCam - currentHeightCam + marginPointer ||
            xPointer > xCam + currentWidthCam - marginPointer || xPointer < xCam - currentWidthCam + marginPointer))
            {
                xCam += Input.GetAxis("Horizontal") * speedMove;
                yCam += Input.GetAxis("Vertical") * speedMove;

                xCam = Mathf.Clamp(xCam, minXWorldmapCamBorder + currentWidthCam, maxXWorldmapCamBorder - currentWidthCam);
                yCam = Mathf.Clamp(yCam, minYWorldmapCamBorder + currentHeightCam, maxYWorldmapCamBorder - currentHeightCam);

                WorldmapCam.transform.position = new Vector3(xCam, yCam, zCam);
            }
        }
        else
        {
            xCam += Input.GetAxis("Horizontal") * speedMove;
            yCam += Input.GetAxis("Vertical") * speedMove;

            xCam = Mathf.Clamp(xCam, minXWorldmapCamBorder + currentWidthCam, maxXWorldmapCamBorder - currentWidthCam);
            yCam = Mathf.Clamp(yCam, minYWorldmapCamBorder + currentHeightCam, maxYWorldmapCamBorder - currentHeightCam);

            WorldmapCam.transform.position = new Vector3(xCam, yCam, zCam);
        }
    }

    private void ZoomIn()
    {
        if (FindObjectOfType<PS4ControllerCheck>().ContinuousR2Press())
        {
            //var xScale = pointer.transform.localScale.x;
            //var yScale = pointer.transform.localScale.y;

            WorldmapCam.orthographicSize -= speedZoom * Time.fixedDeltaTime;
            WorldmapCam.orthographicSize = Mathf.Clamp(WorldmapCam.orthographicSize, minWorldmapCamSize, maxWorldmapCamSize);

            xCam = Mathf.Clamp(xCam, minXWorldmapCamBorder + currentWidthCam, maxXWorldmapCamBorder - currentWidthCam);
            yCam = Mathf.Clamp(yCam, minYWorldmapCamBorder + currentHeightCam, maxYWorldmapCamBorder - currentHeightCam);

            WorldmapCam.transform.position = new Vector3(xCam, yCam, zCam);

            //xScale -= speedZoom * Time.fixedDeltaTime;
            //yScale -= speedZoom * Time.fixedDeltaTime;

            //pointer.transform.localScale = new Vector3(xScale, yScale, 1);
        }
    }

    private void ZoomOut()
    {
        if (FindObjectOfType<PS4ControllerCheck>().ContinuousL2Press())
        {
            WorldmapCam.orthographicSize += speedZoom * Time.fixedDeltaTime;
            WorldmapCam.orthographicSize = Mathf.Clamp(WorldmapCam.orthographicSize, minWorldmapCamSize, maxWorldmapCamSize);

            xCam = Mathf.Clamp(xCam, minXWorldmapCamBorder + currentWidthCam, maxXWorldmapCamBorder - currentWidthCam);
            yCam = Mathf.Clamp(yCam, minYWorldmapCamBorder + currentHeightCam, maxYWorldmapCamBorder - currentHeightCam);

            WorldmapCam.transform.position = new Vector3(xCam, yCam, zCam);
        }
    }

    private bool CheckIfPointerIsInside()
    {
        return (previousYpointer < yCam + currentHeightCam &&
                previousYpointer > yCam - currentHeightCam &&
                previousXpointer < xCam + currentWidthCam &&
                previousXpointer > xCam - currentWidthCam);
    }
}
