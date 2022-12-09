using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public int health;
    public int attack;
    public string enemy_name;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetHealth() {
        return health;
    }

    public int GetAttack() {
        return attack;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health < 0) health = 0;
    }

    public bool IsDead() {
        return health == 0;
    }

    public void Analyze() {
        if (text != null) {
            text.text = $"Enemy {enemy_name}:\nRemaining Health: {health}";
        }
    }
}
