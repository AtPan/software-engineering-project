using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class Enemy
{
    public int speed;
    public int health;
    public int attack;
    public string enemyName;
    public Vector2[] movementPoints;
    public int uid = UID_COUNT++;
    public bool canFly = false;

    public bool canMove = true;
    private int randomPosition;

    private static int UID_COUNT = 0;

    public void AddMovementPoint(Vector2 point)
    {
        for(int i = 0; i < movementPoints.Length; i++)
        {
            if(movementPoints[i] == null)
            {
                movementPoints[i] = point;
                break;
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetAttack()
    {
        return attack;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public bool IsDead()
    {
        return health == 0;
    }

    public void StopMoving()
    {
        this.canMove = false;
    }

    public void AllowMoving()
    {
        this.canMove = true;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public Vector2 RandomPosition()
    {
        if(randomPosition < 0 || randomPosition >= movementPoints.Length)
        {
            ChangeRandomPosition();
        }

        if(randomPosition < 0 || randomPosition >= movementPoints.Length)
        {
            return Vector2.negativeInfinity;
        }
        return movementPoints[randomPosition];
    }

    public void ChangeRandomPosition()
    {
        if (movementPoints.Length == 0) return;
        if (movementPoints.Length == 1)
        {
            randomPosition = 0;
            return;
        }

        int oldPos = this.randomPosition;
        while (oldPos == this.randomPosition) this.randomPosition = Random.Range(0, movementPoints.Length);
    }
}
