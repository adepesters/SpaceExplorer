using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldGeneratorFast : MonoBehaviour
{
    [SerializeField] GameObject starfieldPrefab;

    float speed;

    Player player;
    Vector3 playerPos;

    float oldValX;
    float oldValY;

    Vector3 starfieldPos;

    [SerializeField] int width = 300;
    [SerializeField] int height = 250;
    [SerializeField] int buffer = 100;

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

    bool canSpawn = true;

    [SerializeField] float Zdepth;

    // Start is called before the first frame update
    void Start()
    {
        thresholdDistX = width - buffer;
        thresholdDistY = height - buffer;

        player = FindObjectOfType<Player>();
        playerPos = player.transform.position;
        oldValX = playerPos[0];
        oldValY = playerPos[1];

        valXStarfield = oldValX;
        valYStarfield = oldValY;

        starfieldPos = new Vector3(width / 2, height, Zdepth) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
        starfieldPos = new Vector3(width / 2, 0, Zdepth) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
        starfieldPos = new Vector3(-width / 2, height, Zdepth) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
        starfieldPos = new Vector3(-width / 2, 0, Zdepth) + playerPos;
        Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

        speed = starfieldPrefab.GetComponent<StarfieldSpeed>().GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            HorizontalSpawn();
            VerticalSpawn();
        }

        //Debug.Log("valXStarfield: " + valXStarfield);
        //Debug.Log("valYStarfield: " + valYStarfield);
        //Debug.Log("oldValX: " + oldValX);
        //Debug.Log("oldValY: " + oldValY);
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

        //float actualThreshold = 0;

        //if (speed < 0)
        //{
        //    actualThreshold = Mathf.Abs(thresholdDistX * speed);
        //}
        //else if (speed > 0)
        //{
        //    actualThreshold = thresholdDistX / speed;
        //}
        //else if (speed == 0)
        //{
        //    actualThreshold = thresholdDistX;
        //}

        if (Mathf.Abs(playerPos[0] - oldValX) > thresholdDistX * (1 / Mathf.Pow(2, speed)))
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

            if (firstY)
            {
                starfieldPos = new Vector3(valXStarfield, oldValY, Zdepth);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                starfieldPos = new Vector3(valXStarfield, oldValY + height, Zdepth);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
            }
            else
            {
                if (oldValY < valYStarfield)
                {
                    starfieldPos = new Vector3(valXStarfield, valYStarfield, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                    starfieldPos = new Vector3(valXStarfield, valYStarfield - height, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
                }
                else if (oldValY > valYStarfield)
                {
                    starfieldPos = new Vector3(valXStarfield, valYStarfield, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                    starfieldPos = new Vector3(valXStarfield, valYStarfield + height, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
                }
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

        //float actualThreshold = 0;

        //if (speed < 0)
        //{
        //    actualThreshold = Mathf.Abs(thresholdDistY * speed);
        //}
        //else if (speed > 0)
        //{
        //    actualThreshold = thresholdDistY / speed;
        //}
        //else if (speed == 0)
        //{
        //    actualThreshold = thresholdDistY;
        //}

        if (Mathf.Abs(playerPos[1] - oldValY) > thresholdDistY * (1 / Mathf.Pow(2, speed)))
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
                starfieldPos = new Vector3(oldValX + (width / 2), valYStarfield, Zdepth);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                starfieldPos = new Vector3(oldValX - (width / 2), valYStarfield, Zdepth);
                Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
            }
            else
            {
                if (oldValX < valXStarfield)
                {
                    starfieldPos = new Vector3(valXStarfield, valYStarfield, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                    starfieldPos = new Vector3(valXStarfield - width, valYStarfield, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
                }
                else if (oldValX > valXStarfield)
                {
                    starfieldPos = new Vector3(valXStarfield, valYStarfield, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);

                    starfieldPos = new Vector3(valXStarfield + width, valYStarfield, Zdepth);
                    Instantiate(starfieldPrefab, starfieldPos, Quaternion.Euler(-270, -90, 90), transform);
                }
            }

            thresholdDistY = height;
            previousSignY = Mathf.Sign(playerPos[1] - oldValY);
            oldValY = playerPos[1];
            firstY = false;
        }
    }

    public void SetCanSpawn(bool currentCanSpawn)
    {
        canSpawn = currentCanSpawn;
    }

    public float GetSpeed()
    {
        return speed;
    }

}
