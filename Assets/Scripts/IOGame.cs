using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class IOGame : MonoBehaviour
{
    public GameObject enemyPrefab;

    private string sceneName;

    private const int BUFFER_SIZE = 256;
    private const int MAX_SCENE_NAME_LENGTH = 20;
    private int bytesRead;

    private void LoadEnemiesInScene(Scene oldScene, Scene newScene)
    {
        SceneManager.activeSceneChanged -= LoadEnemiesInScene;
        SceneManager.SetActiveScene(newScene);

        try
        {
            FileStream read = File.OpenRead(GlobalState.GetSaveFile());
            byte[] buffer = new byte[BUFFER_SIZE];

            for(int i = 0; i < bytesRead / BUFFER_SIZE; i++)
            {
                read.Read(buffer, 0, BUFFER_SIZE);
            }
            read.Read(buffer, 0, bytesRead % BUFFER_SIZE);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            /* Read Coordinates of player */
            read.Read(buffer, 0, 8);
            GlobalState.SetPlayerCoords(BitConverter.ToSingle(buffer[0..4]), BitConverter.ToSingle(buffer[4..8]));

            // Read Number of enemies
            read.Read(buffer, 0, sizeof(int));
            int n = BitConverter.ToInt32(buffer[0..sizeof(int)]);
            Debug.Log($"Read {n} enemies from save file");

            // Read n Enemies
            for (int i = 0; i < n; i++)
            {
                // Retreive x,y coords of enemy
                read.Read(buffer, 0, 2 * sizeof(float));
                float x = BitConverter.ToSingle(buffer[0..sizeof(float)]);
                float y = BitConverter.ToSingle(buffer[sizeof(float)..(2 * sizeof(float))]);

                // Retreive length of enemy stats
                read.Read(buffer, 0, sizeof(int));
                int statLen = BitConverter.ToInt32(buffer[0..sizeof(int)]);

                // Retreive enemy stats
                read.Read(buffer, 0, statLen);
                Enemy enemyStats = JsonUtility.FromJson<Enemy>(Encoding.UTF8.GetString(buffer[0..statLen]));

                // Retreive enemy scaling
                read.Read(buffer, 0, 2 * sizeof(float));
                Vector3 scaling = new Vector3(
                    BitConverter.ToSingle(buffer[0..sizeof(float)]),
                    BitConverter.ToSingle(buffer[sizeof(float)..(2 * sizeof(float))]),
                    0.0f
                );

                // Populate and spawn enemy object
                EnemyController newEnemy = Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity).GetComponent<EnemyController>();

                newEnemy.gameObject.transform.position = new Vector2(x, y);
                newEnemy.gameObject.transform.localScale = scaling;
                newEnemy.enemy = enemyStats;
                SpriteRenderer sprite = newEnemy.GetSprite();
                sprite.sortingOrder = 4;
                
                foreach(GameObject e in enemies)
                {
                    //e.tag = "OldEnemy__ToDelete";
                    EnemyController eControl = e.GetComponent<EnemyController>();
                    Debug.Log($"1. Scene UID: {eControl.enemy.uid} -- Loaded UID: {newEnemy.enemy.uid}");
                    if (eControl.enemy.uid == newEnemy.enemy.uid)
                    {
                        sprite.sprite = eControl.GetSprite().sprite;
                        sprite.flipX = eControl.GetSprite().flipX;
                        sprite.flipY = eControl.GetSprite().flipY;
                        sprite.color = eControl.GetSprite().color;
                        break;
                    }
                }
            }

            foreach(GameObject e in enemies)
            {
                //e.SetActive(false);
                Destroy(e);
            }

            read.Close();
            Time.timeScale = 1;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void LoadGame() {
        try {
            bytesRead = 0;
            SceneManager.activeSceneChanged += LoadEnemiesInScene;
            /* Opens a file stream of the save file, for now called test.sav */
            FileStream read = File.OpenRead(GlobalState.GetSaveFile());
            /* Creates a temp buffer to store information in */
            byte[] buffer = new byte[BUFFER_SIZE];

            /* Read Name of Scene to Load */
            bytesRead += read.Read(buffer, 0, sizeof(int));
            int sceneNameLen = BitConverter.ToInt32(buffer[0..sizeof(int)]);
            bytesRead += read.Read(buffer, 0, sceneNameLen);

            /* Load the Scene */
            string sceneName = Encoding.UTF8.GetString(buffer, 0, sceneNameLen).Trim();
            Debug.Log($"Scene to change to: {sceneName}");

            read.Close();
            SceneManager.LoadScene(sceneName);
        }
        catch (Exception e) {
            Debug.Log(e);
        }
    }

    public void SaveGame() {
        try {
            FileStream write = File.Create(GlobalState.GetSaveFile());
            
            /* Save Name of Scene */
            string scene_name = this.sceneName != null ? this.sceneName : SceneManager.GetActiveScene().name;
            byte[] buffer;
            write.Write(BitConverter.GetBytes(scene_name.Length), 0, sizeof(int));
            write.Write(Encoding.UTF8.GetBytes(scene_name), 0, scene_name.Length);

            /* Save Position of Player */
            GameObject player = GameObject.Find("Player");
            write.Write(BitConverter.GetBytes(player.transform.position.x), 0, 4);
            write.Write(BitConverter.GetBytes(player.transform.position.y), 0, 4);

            // Saving All Enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // Saving Number of Enemies
            write.Write(BitConverter.GetBytes(enemies.Length), 0, sizeof(int));
            foreach (GameObject e in enemies)
            {
                EnemyController ec = e.GetComponent<EnemyController>();

                // Save x,y coords of enemy
                write.Write(BitConverter.GetBytes(e.transform.position.x), 0, sizeof(float));
                write.Write(BitConverter.GetBytes(e.transform.position.y), 0, sizeof(float));

                // Save length of enemy stats
                buffer = Encoding.UTF8.GetBytes(JsonUtility.ToJson(ec.enemy));
                write.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int));

                // Save enemy stats
                write.Write(buffer, 0, buffer.Length);

                // Save enemy scaling
                write.Write(BitConverter.GetBytes(e.transform.localScale.x), 0, sizeof(float));
                write.Write(BitConverter.GetBytes(e.transform.localScale.y), 0, sizeof(float));
            }

            write.Close();
        }
        catch  (Exception e) {
            Debug.Log(e);
        }
    }

    public void SetSceneName(string name)
    {
        this.sceneName = name;
    }
}