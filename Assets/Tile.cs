using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private bool canPass = true;

    public bool isPassable() {
        return canPass;
    }

    public void makeImpassable() {
        canPass = false;
    } 
}
