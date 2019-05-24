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

    float speedCam = 1000f;
    float minXWorldmapCamBorder = -30000f; //-10000f;
    float maxXWorldmapCamBorder = 30000f; //10000f;
    float minYWorldmapCamBorder = -30000f; //-10000f;
    float maxYWorldmapCamBorder = 30000f; //10000f;

    float xCam;
    float yCam;
    float zCam;

    float speedPointer = 300f;

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

    Coroutine snapToPlanet;
    bool snap = false;
    Planet planetToSnap;

    bool moveWorldmap = true;

    PauseController pauseController;
    PauseMenuController pauseMenuController;
    PS4ControllerCheck PS4ControllerCheck;

    // Start is called before the first frame update
    void Start()
    {
        pauseController = FindObjectOfType<PauseController>();
        pauseMenuController = FindObjectOfType<PauseMenuController>();
        PS4ControllerCheck = GameObject.FindWithTag("PS4ControllerCheck").GetComponent<PS4ControllerCheck>();

        WorldmapCam = GameObject.Find("Worldmap Camera without Perspective").GetComponent<Camera>();

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
        //Debug.Log("moveWorldmap: " + moveWorldmap);
        //Debug.Log("movePointer: " + movePointer);
        //Debug.Log("snap: " + snap);

        if (pauseController.IsGamePaused() && pauseMenuController.GetMenuPage() == "world map")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            ZoomOut();
            ZoomIn();
            Move();
            if (checkVisibleClouds == null)
            {
                checkVisibleClouds = StartCoroutine(CheckVisibleClouds());
            }

            if (PS4ControllerCheck.IsXPressed())
            {
                //if (movePointer)
                //{
                //    if (pointer.GetIsOnPlanet())
                //    {
                //        pointer.transform.position = pointer.GetPlanet().transform.position;
                //    }
                //}

                moveWorldmap = !moveWorldmap;
                movePointer = !moveWorldmap;
            }

            currentHeightCam = WorldmapCam.orthographicSize;
            currentWidthCam = 16f * WorldmapCam.orthographicSize / 9f;

            marginPointer = WorldmapCam.orthographicSize / 3f;

            if (snapToPlanet == null && snap == false)
            {
                Planet[] planets = FindObjectsOfType<Planet>();

                foreach (Planet planet in planets)
                {
                    var planetPos = new Vector2(planet.transform.position.x, planet.transform.position.y);
                    var pointerPos = new Vector2(pointer.transform.position.x, pointer.transform.position.y);

                    if (Vector2.Distance(planetPos, pointerPos) < 400 && planet.transform.GetChild(1).GetComponent<WorldMapIcon>().HasBeenDiscovered == true)
                    {
                        snap = true;
                        planetToSnap = planet;
                        //pointer.transform.position = planet.transform.position;
                    }
                }
            }
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
            checkVisibleClouds = null;
        }

        if (snap == true && snapToPlanet == null)// && !moveWorldmap)// && !cantSnapAgain)
        {
            snapToPlanet = StartCoroutine(SnapToPlanet(planetToSnap));
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

    IEnumerator SnapToPlanet(Planet planet)
    {
        //Debug.Log("into coroutine");
        pointer.transform.position = planet.transform.position;
        movePointer = false;
        yield return new WaitForSecondsRealtime(0.01f);
        //Debug.Log("out of coroutine");
        movePointer = true;
        snap = false;
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
        if (movePointer && !moveWorldmap)
        {
            snapToPlanet = null;
            if (!CheckIfPointerIsInside())
            {
                xPointer = xCam;
                yPointer = yCam;
            }

            previousXpointer = xPointer;
            previousYpointer = yPointer;

            xPointer += Input.GetAxis("Horizontal") * speedPointer;
            yPointer += Input.GetAxis("Vertical") * speedPointer;

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
                xCam += Input.GetAxis("Horizontal") * speedPointer;
                yCam += Input.GetAxis("Vertical") * speedPointer;

                xCam = Mathf.Clamp(xCam, minXWorldmapCamBorder + currentWidthCam, maxXWorldmapCamBorder - currentWidthCam);
                yCam = Mathf.Clamp(yCam, minYWorldmapCamBorder + currentHeightCam, maxYWorldmapCamBorder - currentHeightCam);

                WorldmapCam.transform.position = new Vector3(xCam, yCam, zCam);
            }
        }
        else if (moveWorldmap)
        {
            xCam += Input.GetAxis("Horizontal") * speedCam;
            yCam += Input.GetAxis("Vertical") * speedCam;

            xCam = Mathf.Clamp(xCam, minXWorldmapCamBorder + currentWidthCam, maxXWorldmapCamBorder - currentWidthCam);
            yCam = Mathf.Clamp(yCam, minYWorldmapCamBorder + currentHeightCam, maxYWorldmapCamBorder - currentHeightCam);

            WorldmapCam.transform.position = new Vector3(xCam, yCam, zCam);
        }
    }

    private void ZoomIn()
    {
        if (PS4ControllerCheck.ContinuousR2Press())
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
        if (PS4ControllerCheck.ContinuousL2Press())
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
