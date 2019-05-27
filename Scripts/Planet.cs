using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Planet : MonoBehaviour
{
    int planetID;

    public int PlanetID { get => planetID; set => planetID = value; }

    void Awake()
    {
        string numbersOnly = Regex.Replace(this.gameObject.name, "[^0-9]", "");
        PlanetID = int.Parse(numbersOnly);
    }

}
