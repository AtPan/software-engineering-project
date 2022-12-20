using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    GameObject pauseScreen = null;
    bool isPaused = false;
    public bool canPause = true;

    private void Start()
    {
        pauseScreen = GameObject.Find("PauseMenu");
        if(pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
    }

    void Update()
    {
        //Debug.Log($"CanPause: {canPause} -- Escape: {Input.GetKeyDown(KeyCode.Escape)}");
        if (pauseScreen != null && canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            pauseScreen.SetActive(isPaused);
        }
    }
}
