using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IOGame : MonoBehaviour
{
    private const int BUFFER_SIZE = 256;
    private const int MAX_SCENE_NAME_LENGTH = 20;

    public void LoadGame() {
        try {
            /* Opens a file stream of the save file, for now called test.sav */
            FileStream read = File.OpenRead(GlobalState.GetSaveFile());
            /* Creates a temp buffer to store information in */
            byte[] buffer = new byte[BUFFER_SIZE];

            /* Read Name of Scene to Load */
            int bytes_read = read.Read(buffer, 0, 1);
            bytes_read = read.Read(buffer, 0, buffer[0]);

            /* Load the Scene */
            SceneManager.LoadScene(Encoding.UTF8.GetString(buffer, 0, bytes_read).Trim(), LoadSceneMode.Single);

            /* Read Coordinates of player */
            bytes_read = read.Read(buffer, 0, 8);
            GlobalState.SetPlayerCoords(BitConverter.ToSingle(buffer[0..4]), BitConverter.ToSingle(buffer[4..8]));

            // Loading Enemies and/or items would go here

            read.Close();
            Time.timeScale = 1;
        }
        catch (Exception e) {
            Debug.Log(e);
        }
    }

    public void SaveGame() {
        try {
            FileStream write = File.Create(GlobalState.GetSaveFile());
            
            /* Save Name of Scene */
            string scene_name = SceneManager.GetActiveScene().name;
            byte[] buffer = new byte[BUFFER_SIZE];
            buffer[0] = (byte)scene_name.Length;
            write.Write(buffer, 0, 1);
            write.Write(Encoding.UTF8.GetBytes(scene_name), 0, buffer[0]);

            /* Save Position of Player */
            GameObject player = GameObject.Find("Player");
            write.Write(BitConverter.GetBytes(player.transform.position.x), 0, 4);
            write.Write(BitConverter.GetBytes(player.transform.position.y), 0, 4);

            // Saving Enemies and/or items would go here
            
            write.Close();
        }
        catch  (Exception e) {
            Debug.Log(e);
        }
    }
}