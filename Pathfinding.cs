/* Pathfinding.csv
 * 
 * Description: Utility Class that implements different Pathfinding Methods. The contents of this script was created with developing grid-based games in mind. 
 * Contents:
 *  Pathfinding methods implemented: 
 *      A* (fastest)  
 *      NOTE: I may want to just separate all the pathfinding methods..? I could utilize a enemy AI class that determines what path to use, and when. 
 *  
 *  Notes on A* Algorithm: 
 *      Gcost = walking cost from the start node 
 *      HCost = Heuristic Cost to reach end node 
 *      FCost = Fcost + HCost 
 *      DistanceCost = Manhattan distance 
 *      Choose the lowest FCost neighboring node 
 *      
 *  Contributors: James Harvey, Code Monkey 
 */


// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    // arbitrary constants for movement costs 
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; 
    private Grid<PathNode> grid;

    // List of nodes on queue 
    private List<PathNode> openList;

    // List of nodes already searched 
    private List<PathNode> closedList;

    public Pathfinding(int width, int height) {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y)); 
    }

    public Grid<PathNode> GetGrid() {
        return grid; 
    } 

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY); 

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();
        
        // initialize and calculate values for first node 
        for (int x=0; x < grid.GetWidth(); x++) { 
            for (int y =0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost(); 

        // check if we still have nodes to search 
        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList); 
            if (currentNode == endNode) {
                // reached final node 
                return CalculatePath(endNode); 
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode); 

            // cycle through neighbors of current node 
            foreach (PathNode neighborNode in GetNeighborList(currentNode)) {

                // already searched Node 
                if (closedList.Contains(neighborNode)) continue;
               
                // check if node is walkable 
                if (!neighborNode.isWalkable) {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGcost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode); 
                if (tentativeGcost < neighborNode.gCost) {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGcost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();
                    
                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode); 
                    }
                }
            }
        }

        // no more nodes 
        return null; 
    }


    private List<PathNode>  GetNeighborList(PathNode currentNode) {
        /*
         * NOTE: order of neighbors getted added doesn't matter 
         */
        List<PathNode> neighborList = new List<PathNode>(); 

        // check boundaries; then add neighbors to list 
        if (currentNode.x - 1 >= 0) {
            // left 
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.y)); 

            // left down 
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));

            // left up 
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }

        // check boundaries; then add neighbors to list 
        if (currentNode.x + 1 < grid.GetWidth()) {
            // right 
            neighborList.Add(GetNode(currentNode.x + 1, currentNode.y));

            // right down 
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));

            // right up
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // down
        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));

        // up 
        if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighborList;

    } 

    
    
    private PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y); 
    }


    private List<PathNode> CalculatePath(PathNode endNode) {
        /*
         * 
         */
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode; 
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode; 
        }
        path.Reverse();
        return path; 
    }


    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining; 
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)  { 
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i]; 
            }
        }
        return lowestFCostNode;
    }


}
