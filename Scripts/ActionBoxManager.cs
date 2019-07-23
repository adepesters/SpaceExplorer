using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBoxManager : MonoBehaviour
{
    Vector3 pos;

    [SerializeField] GameObject panel;
    [SerializeField] Text text;
    Vector3 offset;

    public Vector3 Offset { get => offset; set => offset = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        panel.GetComponent<RectTransform>().transform.position = Camera.main.WorldToScreenPoint(pos + Offset);
    }

    public void SetPos(Vector3 currentPos)
    {
        pos = currentPos;
    }

    public void SetText(string currentText)
    {
        text.text = currentText;
    }
}
