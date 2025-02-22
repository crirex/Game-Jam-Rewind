﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    //Incepe cu valori null, pune doar daca trebuie in spatiile care trebuie
    private GridItem[,] gridItems;

    public float CellSize { get; set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public int TotalSize => Height * Width;

    Vector2 possitionOffset;

    public GridMap(int size, float cellSize, Vector2 offset)
    {
        gridItems = new GridItem[size, size];
        this.Width = size;
        this.Height = size;
        CellSize = cellSize;
        possitionOffset = offset;
    }

    public GridMap(int width, int height, float cellSize, Vector2 offset)
    {
        gridItems = new GridItem[width, height];
        this.Width = width;
        this.Height = height;
        CellSize = cellSize;
        possitionOffset = offset;
    }

    public void ResizeGridMap(int width, int height)
    {
        GridItem[,] newGridItems = new GridItem[width, height];
        for(int i = 0; i < Mathf.Min(width, this.Width); ++i)
        {
            for (int j = 0; j < Mathf.Min(height, this.Height); ++j)
            {
                newGridItems[i, j] = gridItems[i, j];
            }
        }
        gridItems = newGridItems;
        this.Width = width;
        this.Height = height;
    }

    public bool isIndexValid(Vector2 index) => (0 <= index.x && index.x < Width && 0 <= index.y && index.y < Height);

    public Vector2Int getIndexFromPosition(Vector2 position) => new Vector2Int(
        Mathf.RoundToInt(position.x / CellSize - CellSize / 2 - possitionOffset.x), 
        Mathf.RoundToInt(position.y / CellSize - CellSize / 2 - possitionOffset.y));

    public void ResizeGridMap(int size)
    {
        ResizeGridMap(size, size);
    }

    public GridItem GetItemFromPosition(float posX, float posY)
    {
        return GetItemFromPosition(new Vector2(posX, posY));
    }

    public GridItem GetItemFromPosition(Vector2 position)
    {
        Vector2Int index = getIndexFromPosition(position);
        return GetItemFromIndex(index);
    }

    public GridItem GetItemFromIndex(Vector2Int index)
    {
        if (isIndexValid(index))
        {
            if (gridItems[index.x, index.y] == null)
            {
                gridItems[index.x, index.y] = new GridItem(index.x * CellSize + CellSize/2 + possitionOffset.x, index.y * CellSize + CellSize / 2 + possitionOffset.y);
            }
            return gridItems[index.x, index.y];
        }
        return null;
    }
}
