using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldGenerator : MonoBehaviour
{
    [SerializeField] GameObject starfieldPrefab;

    Player player;
    Vector3 playerPos;

    float oldValX;
    float oldValY;

    Vector3 starfieldPos;

    int width = 140;
    int height = 100;

    int buffer = 20;

    int thresholdDistX;
    int thresholdDistY;

    float previousSignX;
    float previousSignY;

    bool firstX = true;
    bool firstY = true;

    float valYStarfield;
    float valXStarfield;

    bool XwasPos = false;
    bool YwasPos = false;

    // Start is called before the first frame update
    void Start()
    {
        thresholdDistX = 120; //width - buffer;
        thresholdDistY = 80;  //height - buffer;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerPos = player.transform.position;
        oldValX = playerPos[0];
        oldValY = playerPos[1];

        valXStarfield = oldValX;
        valYStarfield = oldValY;

        starfieldPos = new Vector3(width / 2, height, 0) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
        starfieldPos = new Vector3(width / 2, 0, 0) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
        starfieldPos = new Vector3(-width / 2, height, 0) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
        starfieldPos = new Vector3(-width / 2, 0, 0) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalSpawn();
        VerticalSpawn();

        Debug.Log("valXStarfield: " + valXStarfield);
        Debug.Log("valYStarfield: " + valYStarfield);
        Debug.Log("oldValX: " + oldValX);
        Debug.Log("oldValY: " + oldValY);
        //Debug.Log("player x: " + playerPos[0]);
        //Debug.Log("player y: " + playerPos[1]);

    }

    private void HorizontalSpawn()
    {
        playerPos = player.transform.position;

        if (!firstX && previousSignX == Mathf.Sign(playerPos[0] - oldValX))
        {
            thresholdDistX = width;
        }
        else if (!firstX && previousSignX != Mathf.Sign(playerPos[0] - oldValX))
        {
            thresholdDistX = width - 2 * buffer;
        }

        if (Mathf.Abs(playerPos[0] - oldValX) > thresholdDistX)
        {

            if (playerPos[0] - oldValX > 0)
            {
                XwasPos = true;
            }
            else if (playerPos[0] - oldValX > 0)
            {
                XwasPos = false;
            }

            valXStarfield = playerPos[0] + Mathf.Sign(playerPos[0] - oldValX) * ((width / 2) + buffer);

            if (oldValY < valYStarfield)
            {
                starfieldPos = new Vector3(valXStarfield, valYStarfield, 0);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                starfieldPos = new Vector3(valXStarfield, valYStarfield - height, 0);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
            }
            else if (oldValY > valYStarfield)
            {
                starfieldPos = new Vector3(valXStarfield, valYStarfield, 0);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                starfieldPos = new Vector3(valXStarfield, valYStarfield + height, 0);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
            }

            thresholdDistX = width;
            previousSignX = Mathf.Sign(playerPos[0] - oldValX);
            oldValX = playerPos[0];
            firstX = false;
        }
    }

    private void VerticalSpawn()
    {
        playerPos = player.transform.position;

        if (!firstY && previousSignY == Mathf.Sign(playerPos[1] - oldValY))
        {
            thresholdDistY = height;
        }
        else if (!firstY && previousSignY != Mathf.Sign(playerPos[1] - oldValY))
        {
            thresholdDistY = height - 2 * buffer;
        }

        if (Mathf.Abs(playerPos[1] - oldValY) > thresholdDistY)
        {

            if (playerPos[1] - oldValY > 0)
            {
                YwasPos = true;
            }
            else if (playerPos[1] - oldValY > 0)
            {
                YwasPos = false;
            }

            if (playerPos[1] - oldValY > 0)
            {
                valYStarfield = playerPos[1] + height + buffer;
            }
            else if (playerPos[1] - oldValY < 0)
            {
                valYStarfield = playerPos[1] - buffer;
            }

            if (firstX)
            {
                starfieldPos = new Vector3(oldValX + (width / 2), valYStarfield, 0);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                starfieldPos = new Vector3(oldValX - (width / 2), valYStarfield, 0);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
            }
            else
            {
                if (oldValX < valXStarfield)
                {
                    starfieldPos = new Vector3(valXStarfield, valYStarfield, 0);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                    starfieldPos = new Vector3(valXStarfield - width, valYStarfield, 0);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
                }
                else if (oldValX > valXStarfield)
                {
                    starfieldPos = new Vector3(valXStarfield, valYStarfield, 0);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                    starfieldPos = new Vector3(valXStarfield + width, valYStarfield, 0);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
                }
            }

            thresholdDistY = height;
            previousSignY = Mathf.Sign(playerPos[1] - oldValY);
            oldValY = playerPos[1];
            firstY = false;
        }
    }

}
