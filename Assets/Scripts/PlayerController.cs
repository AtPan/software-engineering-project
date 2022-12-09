using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public Rigidbody2D playerRb;

    public Tilemap walls;

    public float horizontal;
    public float vertical;

    public TextMeshProUGUI text;

    private int attack = 1;

    //private static bool playerExists = false;

    void Start()
    {
        // Global game settings
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // Player Initialization
        this.transform.position = GlobalState.GetPlayerCoords();
    }

    private void Update()
    {
        // Receive Input --------------------------------------------------
        horizontal = Input.GetAxisRaw("Horizontal" ) * speed; // [-speed, speed]
	    vertical = Input.GetAxisRaw("Vertical") * speed;      // [-speed, speed]
        // ----------------------------------------------------------------

        // Create new position vector -------------------------------------
        Vector2 position = transform.position;

        // Prioritizes vertical movement
        // Prevents movement on both axes simultaniously
        if (vertical != 0) position.y += vertical * speed;
        else if (horizontal != 0) position.x += horizontal * speed;
        // ---------------------------------------------------------------

        int tile_x = (int)Math.Round(position.x);
        int tile_y = (int)Math.Round(position.y);
        
        // Check if position is valid --------------------------------------------------------------------
        if (walls == null || !walls.GetTile(new Vector3Int(tile_x - 1, tile_y - 1, 0))) {
            // Handle player movement across non-blocking (non-wall) objects
            playerRb.position = position;
        }
        else {
            horizontal = vertical = 0;
        }
        // -----------------------------------------------------------------------------------------------

        /*
        Debug.Log($"({position.x}, {position.y}) -- ({(int)(position.x + playerRb.velocity.x + hInput)}, {(int)(position.y + playerRb.velocity.y + vInput)})");
        if(walls == null || !walls.GetTile(new Vector3Int((int)(position.x + playerRb.velocity.x + hInput), (int)(position.y + playerRb.velocity.y + vInput), 0))) {
            if((hInput > 0 || hInput < 0 ) && (vInput > 0))
            {
                playerRb.velocity = new Vector2(0, speed);
                hInput = 0;
            }
            else if ((hInput > 0 || hInput < 0) && (vInput < 0))
            {
                playerRb.velocity = new Vector2(0, -1 * speed);
                hInput = 0;
            }
            else if (vInput > 0 && hInput == 0)
            {
                playerRb.velocity = new Vector2(0, speed);
                hInput = 0;
            }
            else if (vInput < 0 && hInput == 0)
            {
                playerRb.velocity = new Vector2(0, -1 * speed);
                hInput = 0;
            }
            else if (hInput > 0 && vInput == 0)
            {
                playerRb.velocity = new Vector2(speed,0);
                vInput = 0;
            }
            else if (hInput < 0 && vInput == 0)
            {
                playerRb.velocity = new Vector2(-1 * speed, 0);
                vInput = 0;
            }
            else
            {
                playerRb.velocity = new Vector2(0, 0);
                hInput = 0;
                vInput = 0;
            }
        }
        else {
            playerRb.velocity = new Vector2(0, 0);
            hInput = 0;
            vInput = 0;
            Debug.Log("Hit wall");
        }
        */
        
    }

    public float getVInput()
    {
        return vertical;
    }

    public float getHInput()
    {
        return horizontal;
    }

    public void AttackEnemy(EnemyController e) {
        e.TakeDamage(attack);

        if(text != null) text.text = "";

        if(e.IsDead()) {
            // Switch Scenes
        }
    }
}