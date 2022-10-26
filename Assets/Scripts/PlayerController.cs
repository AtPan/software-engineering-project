using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    public Tilemap walls;
    public Tilemap doors;

    private int can_move;

    // Start is called before the first frame update
    void Start()
    {
        // Global game settings
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // Player Initialization
        this.GetComponent<SpriteRenderer>().material.color = new Color(0.8f, 0.4f, 0.2f, 1.0f);
        this.transform.position = new Vector3(0, 0, -0.5f);
        this.can_move = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if(can_move == 0) {
            handle_movement();
            can_move = 5;
        }
        can_move--;
    }

    // Handles Player Movement
    void handle_movement() {
        // Receive Input --------------------------------------------------
        int horizontal = (int)Input.GetAxisRaw("Horizontal"); // -1, 0, 1
	    int vertical = (int)Input.GetAxisRaw("Vertical");     // -1, 0, 1
        // ----------------------------------------------------------------

        // Create new position vector -------------------------------------
        Vector2 position = transform.position;

        // Prioritizes vertical movement
        // Prevents movement on both axes simultaniously
        if (vertical != 0) position.y += vertical;
        else if (horizontal != 0) position.x += horizontal;
        // ---------------------------------------------------------------
        
        // Check if position is valid --------------------------------------------------------------------
        if (doors.GetTile(new Vector3Int((int)position.x - 1, (int)position.y - 1, 0))) {
            // Handle player interaction with a door
            Debug.Log($"Player Hit Door At Coords: ({(int)position.x - 1}, {(int)position.y - 1})");
        }
        else if (!walls.GetTile(new Vector3Int((int)position.x - 1, (int)position.y - 1, 0))) {
            // Handle player movement across non-blocking (non-wall) objects
            GetComponent<Rigidbody2D>().position = position;
        }
        // -----------------------------------------------------------------------------------------------
    }
}
