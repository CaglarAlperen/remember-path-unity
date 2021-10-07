using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    [SerializeField] GameObject tile;
    [SerializeField] float showTime = 3f;

    int[,] grid = new int[8, 6] { { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 },
                                  { 0,0,0,0,0,0 } };
    bool[,] visited = new bool[8, 6] { { false,false,false,false,false,false },
                                       { false,false,false,false,false,false },
                                       { false,false,false,false,false,false },
                                       { false,false,false,false,false,false },
                                       { false,false,false,false,false,false },
                                       { false,false,false,false,false,false },
                                       { false,false,false,false,false,false },
                                       { false,false,false,false,false,false } };
    int startingX;
    (int y, int x) curPoint;

    // Start is called before the first frame update
    void Start()
    {
        ChooseStartPoint();
        StartCoroutine(ShowPathAndHide());
    }

    IEnumerator ShowPathAndHide()
    {
        GenerateRandomPath();
        yield return new WaitForSeconds(showTime);
        HideTiles();
    }

    private void ChooseStartPoint()
    {
        startingX = Random.Range(0, 6);
        grid[0, startingX] = 1;
        curPoint = (0, startingX);
        visited[0, startingX] = true;
    }

    private void GenerateRandomPath()
    {
        ClearTiles();
        while (curPoint.y != 7)
        {
            Move();
        }
        DrawPath();
    }

    private void Move()
    {
        int direction = Random.Range(0,3); // 0 -> right, 1 -> down, 2 -> left

        if (direction == 0 && curPoint.x < 5)
        {
            if (!visited[curPoint.y,curPoint.x+1])
            {
                curPoint.x++;
                grid[curPoint.y, curPoint.x] = 1;
                visited[curPoint.y, curPoint.x] = true;
            }
        }
        else if (direction == 1 && curPoint.y < 7)
        {
            if (!visited[curPoint.y+1, curPoint.x])
            {
                curPoint.y++;
                grid[curPoint.y, curPoint.x] = 1;
                visited[curPoint.y, curPoint.x] = true;
            }
        }
        else if (direction == 2 && curPoint.x > 0)
        {
            if (!visited[curPoint.y, curPoint.x-1])
            {
                curPoint.x--;
                grid[curPoint.y, curPoint.x] = 1;
                visited[curPoint.y, curPoint.x] = true;
            }
        }
    }

    private void DrawPath()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (grid[i,j] == 1)
                {
                    Instantiate(tile, new Vector3(j+1,i+1,i+1), Quaternion.identity);
                }
            }
        }
    }

    private void HideTiles()
    {
        var tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.gameObject.transform.position.x == startingX + 1 && tile.gameObject.transform.position.y == 1)
                continue;
            tile.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ShowAllTiles()
    {
        var tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            tile.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void ShowTile(Vector3 playerPos)
    {
        (int y, int x) posInGrid = ((int)playerPos.y, (int)playerPos.x);
        var tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.gameObject.transform.position.x == posInGrid.x && tile.gameObject.transform.position.y == posInGrid.y)
            {
                tile.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void ClearTiles()
    {
        var tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }
    }

    public int GetStartingX()
    {
        return startingX;
    }

    public float GetShowTime()
    {
        return showTime;
    }
}
