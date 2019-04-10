using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceDialog : MonoBehaviour
{
    [SerializeField] [TextArea] List<string> choices;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<string> GetChoices()
    {
        return choices;
    }

}
