using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kruskal
{
    public List<Edge> kruskalEdges(Graph graph)
    {
        //Kruskal algorithm : takes a graph as input and outputs a set of edges that form the path of the maze
        //It works by adding at each step a random edge from the graph that doesn't create a cycle in the maze path
        List<Edge> newEdges = new List<Edge>();
        List<Edge> walls = new List<Edge>();

        //In the beginning, each cell of the maze is surrounded by walls
        walls = graph.removeDoubles(graph.edges);
        List<List<Node>> nodeSets = new List<List<Node>>();
        int counter = 0;
        foreach (Node node in graph.nodes)
        {
            //At the start, all node subsets are singletons
            List<Node> nodeSet = new List<Node>();
            nodeSet.Add(node);
            nodeSets.Add(nodeSet);
            counter++;
        }

        foreach(Edge wall in Shuffle(walls))
        {
            int setIndex1 = setNumber(nodeSets, wall.node1);
            int setIndex2 = setNumber(nodeSets, wall.node2);

            if (setIndex1 != setIndex2)
            {
                //If the nodes from the randomly selected edge are in different subsets, 
                //destroys the wall separating them and merges the subsets
                newEdges.Add(wall);
                mergeSubsets(nodeSets, setIndex1, setIndex2);
            }
        }

        return newEdges;
    }

    int setNumber(List<List<Node>> nodeLists, Node node)
    {
        //Outputs the index of the subset in which the given node is
        int counter = 0;
        foreach (List<Node> nodeList in nodeLists)
        {
            if (nodeList.Contains(node))
            {
                return counter;
            }

            counter++;
        }

        return -1;
    }

    public List<List<Node>> mergeSubsets(List<List<Node>> nodeLists, int index1, int index2)
    {
        //Returns the set of subsets of nodes in which the two subsets given as an input are merged
        List<Node> nodeList1 = nodeLists[index1];
        List<Node> nodeList2 = nodeLists[index2];

        nodeLists.Remove(nodeList1);
        nodeLists.Remove(nodeList2);

        nodeList1.AddRange(nodeList2);

        nodeLists.Add(nodeList1);

        return nodeLists;
    }


    public List<Edge> Shuffle(List<Edge> list)
    {
        //Outputs a list containing the edges in the input list in a random order
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Edge edge = list[k];
            list[k] = list[n];
            list[n] = edge;
        }

        return list;
    }
}
