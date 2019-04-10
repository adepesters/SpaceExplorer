using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBoss : MonoBehaviour
{
    public float barDisplay; //current progress
    public Vector2 pos = new Vector2(5, 300);
    public Vector2 size = new Vector2(500, 10);
    public Texture2D emptyTex;
    public Texture2D fullTex;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnGUI()
    {
        //draw the background:
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), emptyTex);

        //draw the filled-in part:
        GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), fullTex);
        GUI.color = Color.green;
        GUI.EndGroup();
        GUI.EndGroup();
    }

    // Update is called once per frame
    void Update()
    {
        barDisplay = Time.time * 0.05f;
    }
}
