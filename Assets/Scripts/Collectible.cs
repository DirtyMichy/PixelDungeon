using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int healValue = 1;
    public bool collected = false;

    void Update()
    {
        iTween.PunchPosition(gameObject, iTween.Hash("y", 2f));
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!collected && (c.tag == "Player" || c.tag == "Enemy"))
        {
            collected = true;

            c.GetComponent<UnitObject>().Heal(healValue);

            GetComponent<AudioSource>().Play();
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        iTween.ScaleTo(gameObject, new Vector3(0f, 0f, 0f), 1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}