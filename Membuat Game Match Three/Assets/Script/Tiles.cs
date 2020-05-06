using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    private Vector3 firstPosition;
    private Vector3 finalPosition;
    private float swipeAngle;
    private Vector3 tempPosition;
    //Menampung data posisi tile
    public float xPosition;
    public float yPosition;
    public int column;
    public int row;
    private Grids grid;
    private GameObject otherTile;
    public bool isMatched = false;
    //Menyimpan indeks kolom dan row yang sebelumnya dipilih
    private int previousColumn;
    private int previousRow;


    // Start is called before the first frame update
    void Start()
    {
        //Menentukan posisi dari tile
        grid = FindObjectOfType<Grids>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        column = Mathf.RoundToInt((xPosition - grid.startPosition.x) / grid.offset.x);
        row = Mathf.RoundToInt((yPosition - grid.startPosition.y) / grid.offset.x);
    }
    // Update is called once per frame
    void Update()
    {
        CheckMatches();
        xPosition = (column * grid.offset.x) + grid.startPosition.x;
        yPosition = (row * grid.offset.y) + grid.startPosition.y;
        SwipeTile();
    }
    void OnMouseDown()
    {
        //Mendapatkan titik awal sentuhan jari  
        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void OnMouseUp()
    {
        //Mendapatkan titik akhir sentuhan jari
        finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }
    void CalculateAngle()
    {
        //Menghitung sudut antara posisi awal dan posisi akhir
        swipeAngle = Mathf.Atan2(finalPosition.y - firstPosition.y, finalPosition.x - firstPosition.x) * 180 / Mathf.PI;
        MoveTile();
    }
    void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = tempPosition;
            grid.tiles[column, row] = this.gameObject;
        }
        if (Mathf.Abs(yPosition - transform.position.y) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = tempPosition;
            grid.tiles[column, row] = this.gameObject;
        }

    }
    void MoveTile()
    {
        previousColumn = column;
        previousRow = row;

        if (swipeAngle > -45 && swipeAngle <= 45)
        {
            //Right swipe
            SwipeRightMove();
        }
        else if (swipeAngle > 45 && swipeAngle <= 135)
        {
            //Up swipe
            SwipeUpMove();
        }
        else if (swipeAngle > 135 || swipeAngle <= -135)
        {
            //Left swipe
            SwipeLeftMove();
        }
        else if (swipeAngle < -45 && swipeAngle >= -135)
        {
            //Down swipe
            SwipeDownMove();
        }
        StartCoroutine(checkMove());
    }
    void SwipeRightMove()
    {
        //Menukar posisi dengan sebelah kanan nya
        otherTile = grid.tiles[column + 1, row];
        otherTile.GetComponent<Tiles>().column -= 1;
        column += 1;
    }
    void SwipeUpMove()
    {
        //Menukar posisi dengan sebelah atasnya
        otherTile = grid.tiles[column, row + 1];
        otherTile.GetComponent<Tiles>().row -= 1;
        row += 1;
    }
    void SwipeLeftMove()
    {
        //Menukar posisi dengan sebelah kirinya
        otherTile = grid.tiles[column - 1, row];
        otherTile.GetComponent<Tiles>().column += 1;
        column -= 1;
    }
    void SwipeDownMove()
    {
        //Menukar posisi denhgan sebelah bawahnya
        otherTile = grid.tiles[column, row - 1];
        otherTile.GetComponent<Tiles>().row += 1;
        row -= 1;
    }

    void CheckMatches()
    {
        //Check horizontal matching
        if (column > 0 && column < grid.gridSizeX - 1)
        {
            //Check samping kiri dan kanan nya
            GameObject leftTile = grid.tiles[column - 1, row];
            GameObject rightTile = grid.tiles[column + 1, row];
            if (leftTile != null && rightTile != null)
            {
                if (leftTile.CompareTag(gameObject.tag) && rightTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    rightTile.GetComponent<Tiles>().isMatched = true;
                    leftTile.GetComponent<Tiles>().isMatched = true;
                }
            }
        }
        //Check vertical matching
        if (row > 0 && row < grid.gridSizeY - 1)
        {
            //Check samping atas dan bawahnya
            GameObject upTile = grid.tiles[column, row + 1];
            GameObject downTile = grid.tiles[column, row - 1];
            if (upTile != null && downTile != null)
            {
                if (upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    downTile.GetComponent<Tiles>().isMatched = true;
                    upTile.GetComponent<Tiles>().isMatched = true;
                }
            }
        }

        if (isMatched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.grey;
        }
    }

    IEnumerator checkMove()
    {
        yield return new WaitForSeconds(.3f);
        //Cek jika tile nya tidak sama kembalikan, jika ada yang sama panggil DestroyMatches
        if (otherTile != null)
        {
            if (!isMatched && !otherTile.GetComponent<Tiles>().isMatched)
            {
                GameManager.instance.combo = 1;
                otherTile.GetComponent<Tiles>().row = row;
                otherTile.GetComponent<Tiles>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            else
            {
                grid.DestroyMatches();
                GameManager.instance.combo++;
            }
        }
        otherTile = null;
    }

}
