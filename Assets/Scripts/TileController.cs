using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    // Tile Prefab to duplicate and use as background
    // TODO: Change to array of tiles, as to select one from many for a dynamic background
    public GameObject tile;

    // Number of pixels that coorespond to one world unit
    private float SCALE_X;
    private float SCALE_Y;

    // Width and Height of screen, in tiles
    private int WIDTH;
    private int HEIGHT;

    // Takes Player Coordinates (World Units) and returns the cooresponding Tile object
    public GameObject getTile(int x, int y) {
        x += (WIDTH / 2);
        y += (HEIGHT / 2);

        return GameObject.Find($"Tile[{x}][{y}]");
    }

    public bool isTilePassable(int x, int y) {
        GameObject tile = getTile(x, y);
        if(tile == null) return false;
        else return tile.GetComponent<Tile>().isPassable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 increment = Camera.main.ScreenToWorldPoint(new Vector3(1, 1, 0));
        SCALE_X = Math.Abs(increment.x - origin.x);
        SCALE_Y = Math.Abs(increment.y - origin.y);

        renderTiles();
    }

    void renderTiles() {
        WIDTH = 1 + (int)(Camera.main.pixelWidth * SCALE_X);
        HEIGHT = 1 + (int)(Camera.main.pixelHeight * SCALE_Y);
        //Debug.Log($"X Scale: {SCALE_X} -- Y Scale: {SCALE_Y}");
        //Debug.Log($"Width (in tiles): {WIDTH} -- Height (in tiles): {HEIGHT}");

        GameObject tileHolder = GameObject.FindGameObjectsWithTag("TileController")[0];
        for(int x = 0; x <= WIDTH; x++) {
            for(int y = 0; y <= HEIGHT; y++) {

                GameObject obj = Instantiate(tile, new Vector2(x - (WIDTH / 2), y - (HEIGHT / 2)), Quaternion.identity) as GameObject;
                obj.GetComponent<SpriteRenderer>().material.color = new Color(Math.Abs(x) / (float)WIDTH, Math.Abs(y) / (float)HEIGHT, 0.45f, 1.0f);
                obj.name = $"Tile[{x}][{y}]";
                obj.transform.parent = tileHolder.transform;
            }
        }

        for(int x = 0; x <= WIDTH; x++) {
            GameObject border_tile = GameObject.Find($"Tile[{x}][0]");
            border_tile.GetComponent<Tile>().makeImpassable();
            border_tile = GameObject.Find($"Tile[{x}][{HEIGHT}]");
            border_tile.GetComponent<Tile>().makeImpassable();
        }
        for(int y = 0; y <= HEIGHT; y++) {
            GameObject border_tile = GameObject.Find($"Tile[0][{y}]");
            border_tile.GetComponent<Tile>().makeImpassable();
            border_tile = GameObject.Find($"Tile[{WIDTH}][{y}]");
            border_tile.GetComponent<Tile>().makeImpassable();
        }
    }
}
