using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerIndicatorManager : MonoBehaviour
{
    GameObject layerIndicator1;
    GameObject layerIndicator2;
    GameObject layerIndicator3;

    Player player;

    Color activeLayer = new Color(1f, 0.399f, 0f, 1f);
    Color inactiveLayer = new Color(1, 1, 1, 0.8f);

    // Start is called before the first frame update
    void Start()
    {
        layerIndicator1 = GameObject.FindWithTag("LayerIndicator1");
        layerIndicator2 = GameObject.FindWithTag("LayerIndicator2");
        layerIndicator3 = GameObject.FindWithTag("LayerIndicator3");

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.CurrentLayer)
        {
            case 1:
                layerIndicator1.GetComponent<Image>().color = activeLayer;
                layerIndicator2.GetComponent<Image>().color = inactiveLayer;
                layerIndicator3.GetComponent<Image>().color = inactiveLayer;
                break;
            case 2:
                layerIndicator1.GetComponent<Image>().color = inactiveLayer;
                layerIndicator2.GetComponent<Image>().color = activeLayer;
                layerIndicator3.GetComponent<Image>().color = inactiveLayer;
                break;
            case 3:
                layerIndicator1.GetComponent<Image>().color = inactiveLayer;
                layerIndicator2.GetComponent<Image>().color = inactiveLayer;
                layerIndicator3.GetComponent<Image>().color = activeLayer;
                break;
        }

    }
}
