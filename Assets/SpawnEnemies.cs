using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour 
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GameObject[] spawnPoint;

	void Awake () 
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            int rng = Random.Range(0, 2);
            if(rng == 1)
                Instantiate(enemy, spawnPoint[i].transform.position, spawnPoint[i].transform.rotation);
        }
	}	
}