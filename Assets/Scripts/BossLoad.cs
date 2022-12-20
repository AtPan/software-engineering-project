using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLoad : MonoBehaviour
{
    public PlayerController player;
    public Vector2 startPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = startPlayerPosition;
    }
}
