using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour 
{
    private bool exited = false;
    private int[] campaignCollectedMuffins;

    void OnTriggerEnter2D(Collider2D c)
    {
        if(!exited && c.tag == "Player")
        {
            exited = true;

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !exited)
        {
            exited = true;            

            SceneManager.LoadScene("Menu");
        }  
    }
}