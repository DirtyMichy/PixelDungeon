using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

public class Manager : MonoBehaviour
{
    private bool GameOver = false;
    public GameObject[] playableCharacters;     //Array of gameobjects which contain playable characters
    public GameObject[] playerCharactersAlive;  //Chosen characters by players
    public int[] playerChosenCharacter;         //index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
    public int playerCount = 1;                 //total number of players
    public GameObject currentCheckPoint;        //current checkpoint at which players can respawn
    public float zoomStart = 15f;

    void Awake()
    {
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        if (CountPlayersAlive() <= 0 && !GameOver)
        {
            GameOver = true;
            StartCoroutine(RespawnPlayers());
            Debug.Log("Respawning Players");
        }

        if (Input.GetKeyDown(KeyCode.Escape) || GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Any))
        {
            SceneManager.LoadScene("Menu");
        }

        //if the distance between the leftest and the rightest player gets greater than 15, the camera starts to zoom out
        if (CountPlayersAlive() > 1)
        {
            float leftestPos = 0;
            float rightestPos = 0;

            for (int i = 0; i < playerCharactersAlive.Length; i++)
            {
                if (playerCharactersAlive[i] != null)
                {
                    if (playerCharactersAlive[i].transform.position.x > rightestPos)
                    {
                        rightestPos = playerCharactersAlive[i].transform.position.x;
                    }

                    leftestPos = rightestPos;

                    if (playerCharactersAlive[i].transform.position.x < leftestPos)
                    {
                        leftestPos = playerCharactersAlive[i].transform.position.x;
                    }
                }
            }

            float distance = Mathf.Abs(leftestPos - rightestPos);
            float x = 0;

            //Debug.Log("Distance: " + distance + "Left: " + Mathf.Abs(leftestPos) + "Right: " + Mathf.Abs(rightestPos));

            if (distance > zoomStart)
            {
                Debug.Log("Scaling");

                Camera.main.orthographicSize = distance / (zoomStart / 5f);

                x = rightestPos - distance / (zoomStart / 7.5f);

                if (x > 0)
                    Camera.main.transform.position = new Vector3(x, (Camera.main.orthographicSize - 5f), -10f);
                else
                    Camera.main.transform.position = new Vector3(0f, (Camera.main.orthographicSize - 5f), -10f);

                Vector3 scale = new Vector3(distance / zoomStart, distance / zoomStart, 1f);

                Camera.main.transform.localScale = scale;
            }
            else
            {
                Camera.main.transform.localScale = new Vector3(1f, 1f, 1f);

                Camera.main.orthographicSize = 5f;

                x = rightestPos - distance / (zoomStart / 7.5f);

                if (x > 0)
                    Camera.main.transform.position = new Vector3(x, 0f, -10f);
                else
                    Camera.main.transform.position = new Vector3(0f, 0f, -10f);
            }

        }
        else
        {
            if (CountPlayersAlive() == 1)
            {
                Camera.main.orthographicSize = 5f;
                if (playerCharactersAlive[0].transform.position.x > 0f)
                    Camera.main.transform.position = new Vector3(playerCharactersAlive[0].transform.position.x, 0f, -10f);
                else
                    Camera.main.transform.position = new Vector3(0f, 0f, -10f);
            }
            else//zoom towards the last checkpoint if all players are dead
            if (currentCheckPoint.transform.position.x > 0f)
                Camera.main.transform.position = new Vector3(currentCheckPoint.transform.position.x, 0f, -10f);
            else
                Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        }
    }

    IEnumerator RespawnPlayers()
    {
        yield return new WaitForSeconds(3f);

        GameOver = false;

        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            Debug.Log("Respawning:" + playerID);
            GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
            temp.SendMessage("SetPlayerID", playerID);
        }
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
    }

    public void setCheckPoint(GameObject cp)
    {
        currentCheckPoint = cp;
        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            if (!playerCharactersAlive[playerID])
            {
                GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
                temp.SendMessage("SetPlayerID", playerID);
            }
        }
    }

    int CountPlayersAlive()
    {
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
        return playerCharactersAlive.Length;
    }
}