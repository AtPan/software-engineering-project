using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using System;

public class PlayerController : MonoBehaviour
{
    public GameObject tileParentObject;

    private int can_move = 6;
    private TileController tc;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, 0, -0.5f);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        tc = tileParentObject.GetComponent<TileController>();
        this.GetComponent<SpriteRenderer>().material.color = new Color(0.8f, 0.4f, 0.2f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(can_move == 0) {
            handle_movement();
            can_move = 4;
        }
        can_move--;
    }

    void handle_movement() {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
	    int vertical = (int)Input.GetAxisRaw("Vertical");

        Vector2 position = transform.position;
	    position.x += horizontal;
	    position.y += vertical;
        
        // Check if position is valid

        if(tc.isTilePassable((int)position.x, (int)position.y)) {
            GetComponent<Rigidbody2D>().position = position;
        }
    }
}
