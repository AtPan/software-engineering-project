using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour

{
    public string levelToLoad;
    public bool verticalFacing;
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        if ((verticalFacing && player.getVInput() != 0) || (!verticalFacing && player.getHInput() != 0))
        {
            GlobalState.TrackPlayer(player);
            SceneManager.LoadScene(levelToLoad);
        }
        
    }
}
