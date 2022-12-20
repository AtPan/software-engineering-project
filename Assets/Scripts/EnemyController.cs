using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Reflection;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Awake()
    {
        this.body = this.gameObject.GetComponent<Rigidbody2D>();
        this.sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.enemy.CanMove()) {
            Vector3 position = this.transform.position;
            Vector2 goal = this.enemy.RandomPosition();
            if (!goal.Equals(Vector2.negativeInfinity)) this.transform.position = Vector2.MoveTowards(position, goal, this.enemy.speed * Time.deltaTime);
            
            if((double)Vector2.Distance(this.transform.position, this.enemy.RandomPosition()) <= Double.Epsilon) {
                this.enemy.ChangeRandomPosition();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject target = collision.gameObject;

        if (target.tag == "Wall") {
            Vector3 position = this.transform.position;
            int steps = 1;
            position *= (steps * this.enemy.speed * Time.deltaTime);
            this.transform.position -= position;

            this.enemy.ChangeRandomPosition();
            this.enemy.AllowMoving();
        }
    }

    public SpriteRenderer GetSprite()
    {
        return sprite;
    }
}
