using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGraphGenerator
{
    public Graph InitializeRectGraph(int width, int height)
    {
        //Creates a rectangular graph in which nodes are disposed on a grid
        //Each node is connected to its upper, lower, right and left neighboors
        Graph graph = new Graph();
        graph.height = height;
        graph.width = width;
        graph.nodes = new List<Node>();
        graph.edges = new List<Edge>();
        int counter = 1;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node node = new Node();
                node.id = counter; //Nodes are numbered from the right to the left, from the top to the bottom
                node.coords[0] = i;
                node.coords[1] = j;
                graph.nodes.Add(node);
                counter++;
            }
        }

        foreach (Node node1 in graph.nodes)
        {
            foreach (Node node2 in graph.nodes)
            {
                if (System.Math.Abs(node1.coords[0] - node2.coords[0]) == 1 && System.Math.Abs(node1.coords[1] - node2.coords[1]) == 0)
                {
                    //If two nodes are adjacent in the x coordinate, add an edge to the graph
                    Edge edge = new Edge();
                    edge.node1 = node1;
                    edge.node2 = node2;
                    edge.id = node1.id + "-" + node2.id;
                    graph.edges.Add(edge);
                }

                else if (System.Math.Abs(node1.coords[1] - node2.coords[1]) == 1 && System.Math.Abs(node1.coords[0] - node2.coords[0]) == 0)
                {
                    //If two nodes are adjacent in the y coordinate, add an edge to the graph
                    Edge edge = new Edge();
                    edge.node1 = node1;
                    edge.node2 = node2;
                    edge.id = node1.id + "-" + node2.id;
                    graph.edges.Add(edge);
                }
            }
        }

        return graph;
    }
}
