using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Effect effect;
    private void Start()
    {
        this.effect = this.gameObject.GetComponent<Effect>();
        this.effect.EnableMessage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if (target.tag != "Player") return;

        effect.ApplyEffect(target.GetComponent<PlayerController>());
        Destroy(this.gameObject);
    }
}
