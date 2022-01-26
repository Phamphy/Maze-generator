using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Graph
{
    public int height;
    public int width;
    public List<Node> nodes;
    public List<Edge> edges;


    public List<Node> findNeighboors(Node node)
    {
        //Outputs a list containing the neighboors of a given node
        List<Node> Neighboors = new List<Node>();
        foreach(Edge edge in edges)
        {
            if (edge.node1 == node)
            {
                Neighboors.Add(edge.node2);
            }
            else if (edge.node2 == node)
            {
                Neighboors.Add(edge.node1);
            }
        }

        return Neighboors;
    }

    public bool areNeighboors(Node node1, Node node2)
    {
        //Checks if two nodes are neighboors
        foreach(Edge edge in edges)
        {
            if ((edge.node1 == node1 && edge.node2 == node2) || (edge.node1 == node2 && edge.node2 == node1))
            {
                return true;
            }
        }

        return false;
    }

    public List<Edge> removeDoubles(List<Edge> inputList)
    {
        //Ensures that edges "node1-node2" and "node2-node1" are not both in the list
        List<Edge> newEdges = new List<Edge>();
        foreach (Edge edge in inputList)
        {
            if (edge.node1.id < edge.node2.id)
            {
                newEdges.Add(edge);
            }
        }

        return newEdges;
    }
}
