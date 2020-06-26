using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    public GameObject dungeonPart;
    public int min, max;

    void Awake()
    {
        float x = 0;
        for (int i = 0; i < Random.Range(min, max); i++)
        {
            x += 19.2f;
            Vector3 pos = new Vector3(x, 0f, 0f);
            Instantiate(dungeonPart, pos, gameObject.transform.rotation);
        }
    }
}