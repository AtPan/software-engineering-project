using System.IO;
using System.Text;
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
            bytes_read = read.Read(buffer, 0, 2);
            GlobalState.SetPlayerCoords((int)buffer[0], (int)buffer[1]);

            // Loading Enemies and/or items would go here

            read.Close();
        }
        catch (System.Exception e) {
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
            if (player != null) {
                buffer[1] = (byte)((int)player.transform.position.x);
                buffer[2] = (byte)((int)player.transform.position.y);
                write.Write(buffer, 1, 2);
            }

            // Saving Enemies and/or items would go here
            
            write.Close();
        }
        catch  (System.Exception e) {
            Debug.Log(e);
        }
    }
}