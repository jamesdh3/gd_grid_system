using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestingHeatmap : MonoBehaviour {

    private void Start() {
        Grid grid = new Grid<PathNode>(20, 10, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y)); 
    }


}
