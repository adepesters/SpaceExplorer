using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColorClassifier : MonoBehaviour
{
    Vector3 colorSprite;

    Vector3 red = new Vector3(0.8f, 0f, 0f);
    Vector3 green = new Vector3(0f, 0.8f, 0f);
    Vector3 blue = new Vector3(0f, 0f, 0.8f);
    Vector3 cyan = new Vector3(0f, 1f, 1f);
    Vector3 pink = new Vector3(1f, 0f, 1f);
    Vector3 grey = new Vector3(0.5f, 0.5f, 0.5f);
    Vector3 black = new Vector3(0f, 0f, 0f);
    Vector3 white = new Vector3(1f, 1f, 1f);
    Vector3 yellow = new Vector3(1f, 0.92f, 0.016f);
    Vector3 purple = new Vector3(0.6f, 0.2f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] colors = new Vector3[] { red, green, blue, cyan, pink, yellow, purple };

        colorSprite = new Vector3(GetComponent<SpriteRenderer>().color.r,
                             GetComponent<SpriteRenderer>().color.g,
                             GetComponent<SpriteRenderer>().color.b);

        if (colorSprite[0] < 0.3f && colorSprite[1] < 0.3f && colorSprite[2] < 0.3f)
        {
            Debug.Log("black");
        }
        else if (colorSprite[0] > 0.85f && colorSprite[1] > 0.85f && colorSprite[2] > 0.85f)
        {
            Debug.Log("white");
        }
        else if (Mathf.Abs(colorSprite[0] - colorSprite[1]) < 0.05f
        && Mathf.Abs(colorSprite[0] - colorSprite[2]) < 0.05f
            && Mathf.Abs(colorSprite[1] - colorSprite[2]) < 0.05f)
        {
            Debug.Log("grey");
        }
        else
        {
            float[] distances = new float[colors.Length];

            int i = 0;
            int indexColor = 1000;
            float min = 1000f;
            foreach (Vector3 colorRef in colors)
            {
                distances[i] = Vector3.Distance(colorRef, colorSprite);
                if (distances[i] < min)
                {
                    min = distances[i];
                    indexColor = i;
                }
                i++;
            }

            //Debug.Log(indexColor);
            switch (indexColor)
            {
                case 0:
                    if (Mathf.Abs(colorSprite[0] - colorSprite[2]) < 0.12f)
                    {
                        Debug.Log("purple");
                    }
                    else
                    {
                        Debug.Log("red");
                    }
                    break;
                case 1:
                    Debug.Log("green");
                    break;
                case 2:
                    if (Mathf.Abs(colorSprite[0] - colorSprite[2]) < 0.12f)
                    {
                        Debug.Log("purple");
                    }
                    else if (Mathf.Abs(colorSprite[1] - colorSprite[2]) < 0.05f)
                    {
                        Debug.Log("green");
                    }
                    else
                    {
                        Debug.Log("blue");
                    }
                    break;
                case 3:
                    if (colorSprite[1] > colorSprite[2])
                    {
                        Debug.Log("green");
                    }
                    else
                    {
                        Debug.Log("blue");
                    }
                    break;
                case 4:
                    Debug.Log("pink");
                    break;
                //case 5:
                //    Debug.Log("grey");
                //    break;
                //case 5:
                //    Debug.Log("black");
                //    break;
                //case 5:
                //    Debug.Log("white");
                //    break;
                case 5:
                    if (colorSprite[1] > colorSprite[0])
                    {
                        Debug.Log("green");
                    }
                    else
                    {
                        Debug.Log("yellow");
                    }
                    break;
                case 6:
                    if (colorSprite[1] > colorSprite[0])
                    {
                        if (colorSprite[2] > colorSprite[1])
                        {
                            Debug.Log("blue");
                        }
                        else
                        {
                            Debug.Log("green");
                        }
                    }
                    //else if (colorSprite[2] > colorSprite[0])
                    //{
                    //    Debug.Log("blue");
                    //}
                    else
                    {
                        Debug.Log("purple");
                    }
                    break;
            }
        }

    }

}
