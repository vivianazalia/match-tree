using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector3 firstPosition;
    private Vector3 finalPosition;
    private float swipeAngle;
    private Vector3 tempPosition;

    //menampung data posisi tile
    public float xPosition;
    public float yPosition;
    public int column;          //sumbu y
    public int row;             //sumbu x
    private Grid grid;
    private GameObject otherTile;

    public bool isMatched = false;

    //menyimpan indeks column dan row yang sebelumnya dipilih
    private int previousColumn;
    private int previousRow;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        row = Mathf.RoundToInt((xPosition - grid.startPos.x) / grid.offset.x);
        column = Mathf.RoundToInt((yPosition - grid.startPos.y) / grid.offset.y);
    }

    void Update()
    {
        CheckMatches();

        xPosition = (row * grid.offset.x) + grid.startPos.x;
        yPosition = (column * grid.offset.y) + grid.startPos.y;
        SwipeTile();
    }

    private void OnMouseDown()
    {
        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalPosition.y - firstPosition.y, finalPosition.x - firstPosition.x) * 180 / Mathf.PI;
        MoveTile();
    }

    void SwipeRightMove()
    {
        if (row + 1 < grid.gridSizeX)
        {
            otherTile = grid.tiles[row + 1, column];
            otherTile.GetComponent<Tile>().row = row;
            row += 1;
        }
    }

    void SwipeUpMove()
    {
        if (column + 1 < grid.gridSizeY)
        {
            otherTile = grid.tiles[row, column + 1];
            otherTile.GetComponent<Tile>().column = column;
            column += 1;
        }
    }

    void SwipeLeftMove()
    {
        if (row - 1 >= 0)
        {
            otherTile = grid.tiles[row - 1, column];
            otherTile.GetComponent<Tile>().row = row;
            row -= 1;
        }
    }

    void SwipeDownMove()
    {
        if (column - 1 >= 0)
        {
            otherTile = grid.tiles[row, column - 1];
            otherTile.GetComponent<Tile>().column = column;
            column -= 1;
        }
    }

    void MoveTile()
    {
        previousColumn = column;
        previousRow = row;

        if (swipeAngle > -45 && swipeAngle <= 45 && column < grid.gridSizeX)
        {
            SwipeRightMove();
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < grid.gridSizeY)
        {
            SwipeUpMove();
        }
        else if (swipeAngle > 135 || swipeAngle <= -135 && column > 0)
        {
            SwipeLeftMove();
        }
        else if (swipeAngle <= -45 && swipeAngle >= -135 && row > 0)
        {
            SwipeDownMove();
        }

        StartCoroutine(CheckMove());
    }

    void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > .1)
        {
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = tempPosition;
            grid.tiles[row, column] = this.gameObject;
        }

        if (Mathf.Abs(yPosition - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = tempPosition;
            grid.tiles[row, column] = this.gameObject;
        }
    }

    void CheckMatches()
    {
        //check horizontal matching 
        if (row > 0 && row < grid.gridSizeX - 1)
        {
            //check samping kiri kanan
            GameObject leftTile = grid.tiles[row - 1, column];
            Debug.Log(gameObject.name + " LeftTile [" + (row - 1) + ", " + column + "]");
            GameObject rigthTile = grid.tiles[row + 1, column];
            Debug.Log(gameObject.name + "RightTile [" + (row + 1) + ", " + column + "]");

            if (leftTile != null && rigthTile != null)
            {
                if(leftTile.CompareTag(gameObject.tag) && rigthTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    leftTile.GetComponent<Tile>().isMatched = true;
                    rigthTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }

        //check vertikal matching
        if (column > 0 && column < grid.gridSizeY - 1)
        {
            //check samping kiri kanan
            GameObject upTile = grid.tiles[row, column + 1];
            GameObject downTile = grid.tiles[row, column - 1];

            if (upTile != null && downTile != null)
            {
                if (upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    upTile.GetComponent<Tile>().isMatched = true;
                    downTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }

        if (isMatched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.grey;
        }
    }

    IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);

        if(otherTile != null)
        {
            //jika tile tidak sama, kembalikan
            if(!isMatched && !otherTile.GetComponent<Tile>().isMatched)
            {
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            else
            {
                Debug.Log("Panggil CheckMove Method");
                grid.DestroyMatches();
            }
        }
        otherTile = null;
    }
}
