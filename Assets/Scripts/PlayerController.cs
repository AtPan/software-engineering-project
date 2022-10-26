using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed;
    public Rigidbody2D playerRb;

    public float hInput;
    public float vInput;

    void Start()
    {

    }

    private void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

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

}