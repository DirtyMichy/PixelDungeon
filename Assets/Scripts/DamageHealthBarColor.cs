using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHealthBarColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f);
        GetComponent<Image>().color = new Color(1f, 0f, 0f);
    }
}