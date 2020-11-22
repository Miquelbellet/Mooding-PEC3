using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool activatePlayer;

    private LevelScript levelScript;
    private Vector3 playerPos;
    private float time;

    void Start()
    {
        levelScript = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelScript>();
        playerPos = transform.position;
    }

    void Update()
    {
        if (activatePlayer)
        {
            time += Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, playerPos, 0.5f * time);
            transform.position = playerPos;
            if (Input.GetKeyDown(KeyCode.W))
            {
                time = 0;
                playerPos += Vector3.up;
                if (!levelScript.MovePlayer(transform.position, playerPos)) playerPos -= Vector3.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                time = 0;
                playerPos += Vector3.down;
                if (!levelScript.MovePlayer(transform.position, playerPos)) playerPos -= Vector3.down;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                time = 0;
                playerPos += Vector3.right;
                if (!levelScript.MovePlayer(transform.position, playerPos)) playerPos -= Vector3.right;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                time = 0;
                playerPos += Vector3.left;
                if (!levelScript.MovePlayer(transform.position, playerPos)) playerPos -= Vector3.left;
            }
        }
    }
}
