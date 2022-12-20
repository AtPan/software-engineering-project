using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class that helps communicate data between scenes.
 * Used on startup when loading a save file from the main menu.
 *
 * Will communicate the player position,
 * all enemy positions,
 * all item positions,
 * and anything else that must be communicated.
 */
public class GlobalState : MonoBehaviour
{
    public float player_x;
    public float player_y;
    public string file_save;
    public int max_enemies;

    private PlayerController pc;

    private static GlobalState gs = null;
    
    private void Awake() {
        if(GlobalState.gs == null) {
            // Global game setting
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            GlobalState.gs = this;
            file_save = "test.sav";
            DontDestroyOnLoad(this);
        }
    }

    public static void TrackPlayer(PlayerController pc)
    {
        gs.pc = pc;
    }

    public static PlayerController RetreivePlayer()
    {
        var pc = gs.pc;
        gs.pc = null;
        return pc;
    }

    public static GlobalState GetSingleton() {
        return gs;
    }

    public static void SetPlayerCoords(float x, float y) {
        gs.player_x = x;
        gs.player_y = y;
    }

    public static void SetPlayerCoords(Vector3 v) {
        gs.player_x = v.x;
        gs.player_y = v.y;
    }

    public static void SetPlayerCoords(Vector2 v) {
        gs.player_x = v.x;
        gs.player_y = v.y;
    }

    public static Vector3 GetPlayerCoords() {
        return new Vector3(gs.player_x, gs.player_y, -0.5f);
    }

    public static string GetSaveFile() {
        return gs.file_save;
    }
}
