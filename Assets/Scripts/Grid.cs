using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //properti untuk ukuran grid
    public int gridSizeX, gridSizeY;
    public Vector2 startPos, offset;

    //background grid menggunakan prefab
    public GameObject tilePrefab;

    //array 2 dimensi untuk membuat tile
    public GameObject[,] tiles;
    public GameObject[] candies;

    void Start()
    {
        CreateGrid();
        Debug.Log("Start Grid Script");
    }

    void Update()
    {
        
    }

    void CreateGrid()
    {
        Debug.Log("Start CreateGrid Method");
        tiles = new GameObject[gridSizeX, gridSizeY];

        offset = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

        startPos = transform.position + (Vector3.left * (offset.x * gridSizeX / 2)) + (Vector3.down * (offset.y * gridSizeY / 3));

        //looping untuk membuat grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                int index = Random.Range(0, candies.Length);

                int MAX_ITERATION = 0;

                while(MatchesAt(x,y, candies[index]) && MAX_ITERATION < 100)
                {
                    index = Random.Range(0, candies.Length);
                    MAX_ITERATION++;
                }
                MAX_ITERATION = 0;

                Vector2 pos = new Vector3(startPos.x + (x * offset.x), startPos.y + (y * offset.y));

                GameObject backgroundTile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);

                backgroundTile.transform.parent = transform;
                backgroundTile.name = "(" + x + "," + y + ")";

                //GameObject candy = Instantiate(candies[index], pos, Quaternion.identity);
                GameObject candy = ObjectPooler.instance.SpawnFromPool(index.ToString(), pos, Quaternion.identity);

                candy.name = "(" + x + "," + y + ")";
                tiles[x, y] = candy;
            }
        }
    }

    private bool MatchesAt(int row, int column, GameObject pieces)
    {
        //cek jika ada tile yang sama di bawah dan sampingnya
        if(column > 1 && row > 1)
        {
            if(tiles[row, column - 1].tag == pieces.tag && tiles[row, column - 2].tag == pieces.tag)
            {
                return true;
            }

            if (tiles[row - 1, column].tag == pieces.tag && tiles[row - 2, column].tag == pieces.tag)
            {
                return true;
            }
        }
        //cek jika ada tile yang sama di atas dan sampingnya
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(tiles[row - 1, column].tag == pieces.tag && tiles[row - 2, column].tag == pieces.tag)
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (tiles[row, column - 1].tag == pieces.tag && tiles[row, column - 2].tag == pieces.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int row, int column)
    {
        if(tiles[row, column].GetComponent<Tile>().isMatched)
        {
            GameManager.instance.GetScore(10);
            GameObject gm = tiles[row, column];
            gm.SetActive(false);
            tiles[row, column] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < gridSizeX; i++)
        {
            for(int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        StartCoroutine(DecreaseRow());
    }

    private IEnumerator DecreaseRow()
    {
        int nullCount = 0;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    tiles[i, j].GetComponent<Tile>().column -= nullCount;
                    tiles[i, j] = null;
                }
            }

            nullCount = 0;
        }

        yield return new WaitForSeconds(.1f);

        StartCoroutine(FillBoard());
    }

    private void RefillBoard()
    {
        Debug.Log("Call RefillBoard");
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                if(tiles[x, y] == null)
                {
                    Vector2 tempPosition = new Vector3(startPos.x + (x * offset.x), startPos.y + (y * offset.y));

                    int candyToUse = Random.Range(0, candies.Length);

                    //GameObject tileToRefill = Instantiate(candies[candyToUse], tempPosition, Quaternion.identity);
                    GameObject tileToRefill = ObjectPooler.instance.SpawnFromPool(candyToUse.ToString(), tempPosition, Quaternion.identity);
                    tiles[x, y] = tileToRefill;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for(int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if(tiles[i, j] != null)
                {
                    if(tiles[i, j].GetComponent<Tile>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}
