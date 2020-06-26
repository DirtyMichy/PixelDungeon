using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour 
{
    [SerializeField]
    private GameObject enemy = default;

    [SerializeField]
    private GameObject powerUp = default;

    [SerializeField]
    private Transform[] spawnPoints;

	void Awake () 
    {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            spawnPoints[i] = transform.GetChild(i);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int rng = Random.Range(0, 2);
            if(rng == 0)
                Instantiate(enemy, spawnPoints[i].position, spawnPoints[i].rotation);
            if (rng == 1)
                Instantiate(powerUp, spawnPoints[i].position, spawnPoints[i].rotation);
        }
	}	
}