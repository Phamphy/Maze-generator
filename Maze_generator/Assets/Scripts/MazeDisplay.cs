using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeDisplay : MonoBehaviour
{
    public GameObject verticalWall, horizontalWall;

    public GameObject Floor; //Floor of the maze

    public GameObject wallParent; //Empty that is a parent of the walls

    public int offsetFromBorders;

    private float wallThickness = 0.1f;

    public TMP_InputField widthField, heightField;

    public TMP_Dropdown dropdown;

    public Camera cam;

    public void GenerateMaze()
    {
        //Function associated to a button that generates the maze
        int height = int.Parse(heightField.text);
        int width = int.Parse(widthField.text);

        //If a maze was already generated, destroys it
        if (wallParent.transform.childCount != 0)
        {
            foreach(Transform child in wallParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        SquareGraphGenerator squareGraphGenerator = new SquareGraphGenerator();
        displayMaze(squareGraphGenerator.InitializeRectGraph(height, width));
    }


    void displayMaze(Graph graph)
    {
        Vector4 stepScale = stepAndScale(graph);
        //size of the wall in world coordinates
        float scaleX = stepScale[0];
        float scaleY = stepScale[1];

        //size of the wall in screen coordinates
        float stepX = stepScale[2];
        float stepY = stepScale[3];

        List<Edge> noDoubles = new List<Edge>();
        List<Edge> mazeFloor = new List<Edge>();
        if (dropdown.value == 1)
        {
            Kruskal kruskal = new Kruskal();
            //The path of the maze
            mazeFloor = kruskal.kruskalEdges(graph); 
        }

        else if (dropdown.value == 0)
        {
            DFS dfs = new DFS();
            //The path of the maze
            mazeFloor = dfs.DFS_Iter(graph); 
        }

        //All edges of the initial graph
        noDoubles = graph.removeDoubles(graph.edges); 

        foreach (Edge edge in noDoubles)
        {
            if (!inListEdges(mazeFloor, edge))
            {
                //If an edge is not a part of the path, generates a wall that separates the two connected nodes
                if (edge.node1.coords[0] == edge.node2.coords[0]) 
                {
                    //if two nodes have the same x coordinate, creates a vertical wall to separate them
                    GameObject wall = Instantiate(horizontalWall);

                    //Coordinates of the wall computed from the node coordinates
                    float[] coordWall = new float[2];

                    //x position of the wall
                    coordWall[0] = (float)edge.node1.coords[0];

                    //y position of the wall
                    coordWall[1] = (float)(edge.node1.coords[1] + edge.node2.coords[1]) / 2;

                    //Scale of the wall in world coordinates, with a slight offset so that two walls meet at their edges
                    wall.transform.localScale = new Vector3(scaleX + wallThickness, wallThickness, 1f);
                    Vector3 adjustedPos = new Vector3((coordWall[0]+0.5f) * stepX, (coordWall[1]+0.5f) * stepY, 1f); //Since walls are anchored in the middle, there's a 0.5 offset
                    wall.transform.position = cam.ScreenToWorldPoint(adjustedPos);
                    wall.transform.parent = wallParent.transform;
                }

               else if (edge.node1.coords[1] == edge.node2.coords[1])
                {
                    //if two nodes have the same y coordinate, creates a horizontal wall to separate them
                    GameObject wall = Instantiate(verticalWall);
                    float[] coordWall = new float[2];
                    coordWall[0] = (float)(edge.node1.coords[0] + edge.node2.coords[0]) / 2;
                    coordWall[1] = (float)edge.node1.coords[1];
                    wall.transform.localScale = new Vector3(wallThickness, scaleY+wallThickness, 1f);
                    Vector3 adjustedPos = new Vector3((coordWall[0]+0.5f) * stepX, (coordWall[1]+0.5f) * stepY, 1f);
                    wall.transform.position = cam.ScreenToWorldPoint(adjustedPos);
                    wall.transform.parent = wallParent.transform;
                }
            }
        }
    }

    Vector4 stepAndScale(Graph graph)
    {
        //Instantiates the outside walls and returns the step between two walls in screen space and in world space
        float camHeight = cam.orthographicSize * 2.0f; //Height and width of the camera for scaling purposes
        float camWidth = camHeight * Screen.width / Screen.height;

        //Creation of the outside walls of the maze, placed on the edges of the screen
        //Left wall
        GameObject leftWall = Instantiate(verticalWall);
        leftWall.transform.localScale = new Vector3(wallThickness, camHeight, 1f);

        //Adjusted position in screen coordinates
        Vector3 leftAdjustedPos = new Vector3(offsetFromBorders, Screen.height * 0.5f, 1f);

        //Transforms screen coordinates into world coordinates
        leftWall.transform.position = cam.ScreenToWorldPoint(leftAdjustedPos);

        //All walls are children of this empty to make them easier to destroy
        leftWall.transform.parent = wallParent.transform; 

        //Right wall
        GameObject rightWall = Instantiate(verticalWall);
        rightWall.transform.localScale = new Vector3(wallThickness, camHeight, 1f);
        Vector3 rightAdjustedPos = new Vector3(Screen.width - offsetFromBorders, Screen.height * 0.5f, 1f);
        rightWall.transform.position = cam.ScreenToWorldPoint(rightAdjustedPos);
        rightWall.transform.parent = wallParent.transform;

        //Upper wall
        GameObject upperWall = Instantiate(horizontalWall);
        upperWall.transform.localScale = new Vector3(camWidth, wallThickness, 1f);
        Vector3 upperAdjustedPos = new Vector3(Screen.width * 0.5f, Screen.height - offsetFromBorders, 1f);
        upperWall.transform.position = cam.ScreenToWorldPoint(upperAdjustedPos);
        upperWall.transform.parent = wallParent.transform;

        //Lower wall
        GameObject lowerWall = Instantiate(horizontalWall);
        lowerWall.transform.localScale = new Vector3(camWidth, wallThickness, 1f);
        Vector3 lowerAdjustedPos = new Vector3(Screen.width * 0.5f, offsetFromBorders, 1f);
        lowerWall.transform.position = cam.ScreenToWorldPoint(lowerAdjustedPos);
        lowerWall.transform.parent = wallParent.transform;

        //Floor of the maze
        GameObject floor = Instantiate(Floor);
        floor.transform.localScale = new Vector3(camWidth, camHeight, 1f);
        Vector3 floorAdjustedPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 2f);
        floor.transform.position = cam.ScreenToWorldPoint(floorAdjustedPos);
        floor.transform.parent = wallParent.transform;

        //step in world coordinates for scaling purposes
        float scaleX = ((rightWall.transform.position - leftWall.transform.position) / graph.width).x; 
        float scaleY = ((upperWall.transform.position - lowerWall.transform.position) / graph.height).y;

        //step in screen coordinates for positionning purposes
        float stepX = (float)Screen.width / graph.width; 
        float stepY = (float)Screen.height / graph.height;

        return new Vector4(scaleX, scaleY, stepX, stepY);
    }

    bool inListEdges(List<Edge> listEdges, Edge edge)
    {
        //Checks if an edge is in a given list of edges, independently from the order of the nodes
        foreach(Edge potentialMatch in listEdges)
        {
            if (edge.node1.id == potentialMatch.node1.id && edge.node2.id == potentialMatch.node2.id)
            {
                return true;
            }
            else if (edge.node2.id == potentialMatch.node1.id && edge.node1.id == potentialMatch.node2.id)
            {
                return true;
            }
        }
        return false;
    }

}
