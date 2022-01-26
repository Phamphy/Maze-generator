using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;

public class DFS
{
    public List<Edge> DFS_Iter(Graph graph)
    {
        //Outputs the edges forming the spanning tree (the path in the maze) from the inital graph
        Stack<Node> nextNodes = new Stack<Node>(); //Each time a node is visited, its unvisited neighboors get placed in this stack
        List<Edge> newEdges = new List<Edge>();
        List<Node> visitedOrder = new List<Node>(); //List that contains all nodes in the order in which they are visited
        Dictionary<Node, bool> visited = new Dictionary<Node, bool>();
        foreach (Node node in graph.nodes)
        {
            visited.Add(node, false); //Each node starts unvisited
        }

        int index = Random.Range(0, graph.nodes.Count);
        Node s = graph.nodes[index]; //The node from which the DFS algorithm is started is chosen randomly
        nextNodes.Push(s);

        while (nextNodes.Count != 0)
        {
            //DFS algorithm : each time a node is visited, its unvisited neighboors are added to a stack
            //Thus when a dead-end is reached, the algorithm backtracks to the last unvisited neighboor
            Node n = nextNodes.Pop();
            if (!visited[n])
            {
                foreach (Node neighboor in Shuffle(graph.findNeighboors(n)))
                {
                    //The shuffle function makes the graph traversal random to avoid repeating patterns in the maze
                    if (!visited[neighboor])
                    {
                        nextNodes.Push(neighboor);
                    }
                }

                visited[n] = true;
                visitedOrder.Add(n);
            }
        }
        
        //The DFS algorithm doesn't directly output the edges forming the path in the maze
        //It instead ouputs the order in which the nodes are visited

        for (int i = 0; i < visitedOrder.Count - 1; i++)
        {
            //Reconstruction of the edges from the order in which the nodes were visited
            Node currentNode = visitedOrder[i];
            Node nextNode = visitedOrder[i + 1];
            //If two consecutive visited nodes are neighboors, they form an edge of the path
            if (graph.areNeighboors(currentNode, nextNode))
            {
                Edge edge = new Edge();
                edge.node1 = currentNode;
                edge.node2 = nextNode;
                edge.id = edge.node1.id + "-" + edge.node2.id;
                newEdges.Add(edge);
            }
            //If they are not, find the last node in the already visited ones that was a neighboor of the considered node
            else
            {
                for (int j = 0; j < i; j++)
                {
                    if (graph.areNeighboors(visitedOrder[j], nextNode))
                    {
                        currentNode = visitedOrder[j];
                    }
                }

                Edge edge = new Edge();
                edge.node1 = currentNode;
                edge.node2 = nextNode;
                edge.id = edge.node1.id + "-" + edge.node2.id;
                newEdges.Add(edge);

            }
        }

        return newEdges;
    }

    public List<Node> Shuffle(List<Node> list)
    {
        //Outputs a list containing the nodes in the input list in a random order
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Node node = list[k];
            list[k] = list[n];
            list[n] = node;
        }

        return list;
    }
    
}
