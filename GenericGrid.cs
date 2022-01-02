// grid_generic.cs 

/*
 * Description: Class and methods for building a generic grid system 
 * Contents: 
 * 
 * Contributors: Code Monkey, James Harvey 
*/ 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid<TGridObject> { // NOTE:  this will allow us to pass in any type when instantiating Grid class


    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; // handles events when the state of our grid changes 
    
    // class that looks at what needs to change when events get triggered 
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y; 
    }
     
    // properties of a grid 
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;

    // grid and debugging values 
    private TGridObject[,] gridArray; // NOTE: how we assign a multi-dimensional array 
    private TextMesh[,] debugTextArray;


    // instantiating Grid Object 
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y); 
            }
        }

        bool showDebug = true; 
        if (showDebug) { 

            // loop and create the grid 
            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {

                    // insert text in center of cells 
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);

                    // drawing useful grid lines 
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            // now draw the the top and most right borders of the grid 
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            // subscribe to event 
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }


    public int GetWidth() {
        return width;
    }


    public int GetHeight() {
        return height;
    }


    public float GetCellSize() {
        return cellSize;
    }

    private Vector3 GetWorldPosition(int x, int y) {
    /* 
     * Returns the position of where user clicks 
     */ 
        return new Vector3(x, y) * cellSize + originPosition;
    }


    public void GetXY(Vector3 worldPosition, out int x, out int y) {
    /* 
     * Returns the x and y coordinates of position of object 
     */
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }


    public void SetGridObject(int x, int y, TGridObject value)
    {
        // only make changes if user input is within grid boundaries 
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });  //debugTextArray[x, y].text = gridArray[x, y].ToString();

        }
    }


    public void TriggerGridObjectChanged(int x, int y) {
    /* 
    * subscriber method that changes grid when event is triggered
    */ 
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }


    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject) ;
        }
    }


    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}