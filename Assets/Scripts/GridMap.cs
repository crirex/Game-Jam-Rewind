using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    //Incepe cu valori null, pune doar daca trebuie in spatiile care trebuie
    private GridItem[,] gridItems;
    private int width;
    private int height;

    public float CellSize { get; set; }

    public GridMap(int size, float cellSize)
    {
        gridItems = new GridItem[size, size];
        this.width = size;
        this.height = size;
        CellSize = cellSize;
    }

    public GridMap(int width, int height, float cellSize)
    {
        gridItems = new GridItem[width, height];
        this.width = width;
        this.height = height;
        CellSize = cellSize;
    }

    public void ResizeGridMap(int width, int height)
    {
        GridItem[,] newGridItems = new GridItem[width, height];
        for(int i = 0; i < Mathf.Min(width, this.width); ++i)
        {
            for (int j = 0; j < Mathf.Min(height, this.height); ++j)
            {
                newGridItems[i, j] = gridItems[i, j];
            }
        }
        gridItems = newGridItems;
        this.width = width;
        this.height = height;
    }

    public void ResizeGridMap(int size)
    {
        ResizeGridMap(size, size);
    }

    public GridItem GetItemFromPosition(float posX, float posY)
    {
        Debug.Log(posX / CellSize + CellSize / 2 + " " + posY / CellSize + CellSize / 2);
        return GetItemFromIndex(Mathf.RoundToInt(posX / CellSize - CellSize / 2), Mathf.RoundToInt(posY / CellSize - CellSize / 2));
    }

    public GridItem GetItemFromIndex(int i, int j)
    {
        if (0 <= i && i < width && 0 <= j && j < height)
        {
            GridItem gridItem = gridItems[i, j];
            if (gridItem == null)
            {
                gridItem = new GridItem(i * CellSize + CellSize/2, j * CellSize + CellSize / 2);
            }
            return gridItem;
        }
        return null;
    }
}
