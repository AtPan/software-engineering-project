using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap walls;
    public Tilemap doors;
    private int can_move;
    private float waitTime;
    public float startWaitTime;
    public float speed;
    public Transform[] moveSpots;
    private int randomSpot;
    // public float minX;
    // public float maxX;
    // public float minY;
    //public float maxY;

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

        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
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

    void handle_movement(){
     transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
     if(Vector2.Distance(transform.position, moveSpots[randomSpot].position)< 0.2f)
     if(waitTime <=0){
        randomSpot = Random.Range(0,moveSpots.Length);
        waitTime = startWaitTime;
     } else {
        waitTime -= Time.deltaTime;
        // moveSpots.position = new Vector2(Random.Range(minX,maxX), Random.Range(minY, maxY));
     }
    }
}

