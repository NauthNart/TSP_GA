using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Name : MonoBehaviour
{
    public GameObject city;
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        string a = city.name.ToString().Substring(6, city.name.Length - 1 - 6);
        text.text = a;
    }

    
    void Update()
    {
        
    }
}
