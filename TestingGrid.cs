// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour {

    // [SerializeField] private HeatMapVisual heatMapVisual;
    // [SerializeField] private HeatMapBoolVisual heatMapBoolVisual; 
    private Grid<HeatMapGridObject> grid;
    private Grid<StringGridObject> stringGrid; 

    private void Start() {
        // grid = new Grid<HeatMapGridObject>(20, 10, 8f, Vector3.zero, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        stringGrid = new Grid<StringGridObject>(20, 10, 8f, Vector3.zero, (Grid<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));
    }

    private void Update() {
        Vector3 position = UtilsClass.GetMouseWorldPosition(); 
        /* if (Input.inputString == "c") { 
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position); 
            if (heatMapGridObject != null) {
                heatMapGridObject.AddValue(5); 
            }
        } */ 
        if (Input.GetKeyDown(KeyCode.A)) { stringGrid.GetGridObject(position).AddLetter("A"); }
        if (Input.GetKeyDown(KeyCode.B)) { stringGrid.GetGridObject(position).AddLetter("B"); }
        if (Input.GetKeyDown(KeyCode.C)) { stringGrid.GetGridObject(position).AddLetter("C"); }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { stringGrid.GetGridObject(position).AddNumber("1"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { stringGrid.GetGridObject(position).AddNumber("2"); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { stringGrid.GetGridObject(position).AddNumber("3"); }
    }
} 

public class HeatMapGridObject {

    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value; 

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }


    public void AddValue(int addValue) {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);

        // trigger event handler
        grid.TriggerGridObjectChanged(x, y); 
    }


    public float GetValueNormalized() {
        return (float)value / MAX;      
    }


    public override string ToString() {
        return value.ToString(); 
    }
}

public class StringGridObject {
    private Grid<StringGridObject> grid;
    private int x;
    private int y;

    private string letters;
    private string numbers; 

    public StringGridObject(Grid<StringGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = ""; 
    }

    public void AddLetter(string letter) {
        letters += letter;
        grid.TriggerGridObjectChanged(x, y); 
    }
    
    public void AddNumber(string number) {
        numbers += number;
        grid.TriggerGridObjectChanged(x, y); 
    }

    public override string ToString() {
        return letters + "\n" + numbers; 

    }
}


