using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

using System;
using System.Reflection;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    [NonSerialized]
    public Tilemap walls;
    public float horizontal;
    public float vertical;
    public TextMeshProUGUI text;

    public EnemyController targetedEnemy;

    public int attack = 1;
    public int health = 10;
    public GameObject inventory;
    private TextMeshProUGUI inventoryText;

    private Rigidbody2D body;
    private bool canMove;
    private SpriteRenderer sprite;
    private BattleController battleController;
    private PauseGame pause;

    void Awake()
    {
        // Player Initialization
        PlayerController pc = GlobalState.RetreivePlayer();
        this.body = this.gameObject.GetComponent<Rigidbody2D>();
        this.sprite = this.gameObject.GetComponent<SpriteRenderer>();
        this.battleController = this.gameObject.GetComponent<BattleController>();
        this.pause = this.gameObject.GetComponent<PauseGame>();
        this.canMove = true;

        if (inventory != null)
        {
            this.inventoryText = inventory.GetComponentInChildren<TextMeshProUGUI>();
            this.inventory.SetActive(false);
        }

        if (pc == null) return;
        
        this.attack = pc.attack;
        this.health = pc.health;
        this.speed = pc.speed;
        this.sprite.sprite = pc.sprite.sprite;
        this.sprite.color = pc.sprite.color;
        this.sprite.flipX = pc.sprite.flipX;
        this.sprite.flipY = pc.sprite.flipY;
    }

    private void Start()
    {
        this.transform.position = GlobalState.GetPlayerCoords();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && this.inventory != null && (battleController.battleScreen == null || !battleController.battleScreen.activeSelf) && !this.pause.GetPaused())
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            this.pause.canPause = inventory.activeSelf;
            inventory.SetActive(!inventory.activeSelf);
            inventoryText.text = $"Inventory\nHealth: {this.health}\nAttack: {this.attack}\nSpeed: {this.speed}";
        }

        if(this.canMove) {
            // Receive Input --------------------------------------------------
            this.horizontal = Input.GetAxisRaw("Horizontal" ) * speed;   // [-speed, speed]
            this.vertical = Input.GetAxisRaw("Vertical") * speed;      // [-speed, speed]
            // ----------------------------------------------------------------
            
            this.body.velocity = new Vector3(horizontal, vertical, 0f);
        }
    }

    public SpriteRenderer GetSprite()
    {
        return sprite;
    }

    public float getVInput()
    {
        return vertical;
    }

    public float getHInput()
    {
        return horizontal;
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
        if(this.health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void AttackEnemy() {
        if (this.targetedEnemy == null) return;
        if(text != null) text.text = "";

        this.targetedEnemy.enemy.TakeDamage(attack);

        if(this.targetedEnemy.enemy.IsDead()) {
            Destroy(this.targetedEnemy.gameObject);
            this.targetedEnemy = null;
            battleController.EndBattleScene();
            return;
        }

        this.TakeDamage(this.targetedEnemy.enemy.attack);

        if(text != null)
        {
            text.text = $"Attacked {this.targetedEnemy.enemy.enemyName} for {this.attack} damage\n";
            text.text += $"{this.targetedEnemy.enemy.enemyName} attacked for {this.targetedEnemy.enemy.attack} damage";
        }
    }

    public void Analyze() {
        if (this.text != null) {
            this.text.text = this.targetedEnemy == null ? "Enemy is already dead" : $"Enemy {this.targetedEnemy.enemy.enemyName}:\nRemaining Health: {this.targetedEnemy.enemy.health}";
        }
    }

    public void StopMoving() {
        this.canMove = false;
    }

    public void AllowMoving() {
        this.canMove = true;
    }

    public PauseGame GetPauseMenuController()
    {
        return this.pause;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Find gameobject collided with
        GameObject target = collision.gameObject;

        // If that object is a wall ---------------------------------
        if(target.tag == "Wall") {
            // Remove all velocity, halting the player
            this.body.velocity = Vector3.zero;
            this.body.angularVelocity = 0.0f;

            // Bounce player back a step, removing them from the wall
            this.body.AddForce(new Vector3(-horizontal * speed, -vertical * speed, 0.0f));
        }
        // ----------------------------------------------------------

        // If that object is an enemy -------------------------------
        else if(target.tag == "Enemy") {
            EnemyController e = target.GetComponent<EnemyController>();
            this.targetedEnemy = e;
            e.enemy.StopMoving();
            battleController.StartBattleScene(this, new EnemyController[] { e });
        }
        // ----------------------------------------------------------
    }
}