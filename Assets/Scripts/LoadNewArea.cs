using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour

{
    public string levelToLoad;
    public bool verticalFacing;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (verticalFacing && (player.getVInput() > 0 || player.getVInput() < 0))
            SceneManager.LoadScene(levelToLoad);
        else if (!verticalFacing && (player.getHInput() > 0 || player.getHInput() < 0))
            SceneManager.LoadScene(levelToLoad);
        
    }
}
