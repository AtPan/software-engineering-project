using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Effect : MonoBehaviour
{
    public int healthChange;
    public int attackChange;
    public int speedChange;
    public string effectName;
    public Sprite sprite;
    public Color color;
    private bool messageEnable;
    private TextMeshProUGUI text;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("MessageBoard");
        this.messageEnable = false;
        if (go != null)
        {
            text = go.GetComponent<TextMeshProUGUI>();
            this.messageEnable = true;
        }
    }

    public void EnableMessage()
    {
        if (this.text == null) return;
        this.messageEnable = true;
    }

    public void DisableMessage()
    {
        this.messageEnable = false;
    }

    public void ApplyEffect(PlayerController pc)
    {
        pc.attack += attackChange;
        pc.speed += speedChange;

        pc.TakeDamage(-healthChange);

        if (this.messageEnable)
        {
            text.text = $"You have been afflicted with {effectName}";
        }

        if(this.sprite != null)
        {
            SpriteRenderer sr = pc.gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = this.sprite;
            sr.color = this.color;
        }
    }

    public void ApplyEffect(EnemyController ec)
    {
        ec.enemy.attack += attackChange;
        ec.enemy.speed += speedChange;

        ec.enemy.TakeDamage(-healthChange);
        if(ec.enemy.IsDead())
        {
            Destroy(ec.gameObject);
        }

        if (this.messageEnable)
        {
            text.text = "";
        }
    }
}
