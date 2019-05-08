using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceDialog : MonoBehaviour
{
    [SerializeField] [TextArea] List<string> choices;


    public List<string> GetChoices()
    {
        return choices;
    }

}
