using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    private Effect effect;
    private void Start()
    {
        this.effect = this.gameObject.GetComponent<Effect>();
        this.effect.DisableMessage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if(target.tag == "Player")
        {
            effect.ApplyEffect(target.GetComponent<PlayerController>());
        }
        else if (target.tag == "Enemy")
        {
            EnemyController ec = target.GetComponent<EnemyController>();
            if(!ec.enemy.canFly) effect.ApplyEffect(ec);
        }
    }
}
