using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class BattleController : MonoBehaviour
{
    public GameObject battleScreen;

    public EnemyController[] enemies;
    public PlayerController player;
    public PauseGame pause;
    public float timeScale;
    public Vector2 oldPlayerPosition;

    private static readonly Vector2[] enemy_battle_positions = new Vector2[] { 
        new Vector2(3, 3),
        new Vector2(1.5f, 3),
        new Vector2(0, 3),
        new Vector2(-1.5f, 3)
    };
    private static readonly Vector2 player_battle_position = new Vector2(-3, 0);

    private void Awake()
    {
        enemies = new EnemyController[4];
    }

    public void StartBattleScene(PlayerController player, EnemyController[] enemyList)
    {
        if (battleScreen == null)
        {
            Debug.Log("No battle scene available");
            return;
        }

        timeScale = Time.timeScale;
        Time.timeScale = 0;
        battleScreen.SetActive(true);

        enemies = enemyList;

        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].enemy.StopMoving();
            enemyList[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
            enemyList[i].transform.position = enemy_battle_positions[i];
        }

        this.player = player;
        player.StopMoving();
        player.GetSprite().sortingOrder = 5;
        oldPlayerPosition = player.transform.position;
        player.transform.position = player_battle_position;

        this.pause = player.gameObject.GetComponent<PauseGame>();
        this.pause.canPause = false;
    }

    public void EndBattleScene()
    {
        Time.timeScale = timeScale;
        this.enemies = null;
        this.battleScreen.SetActive(false);
        this.player.AllowMoving();
        this.player.GetSprite().sortingOrder = 4;
        this.player.gameObject.transform.position = this.oldPlayerPosition;
        this.player = null;
        this.pause.canPause = true;
        this.pause = null;
    }
}
