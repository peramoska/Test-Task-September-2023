using System.Collections;
using System.Collections.Generic;


using System.Linq;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class DijkstraAlgorithm
    {
        private Graph graph;

        bool isFindPath = false;
        List<Node> pathList = new List<Node>();
        List<Node> unvisitedNode = new List<Node>();

        Vector2Int finalPos;
        Dictionary<int, List<Node>> enablePaths = new Dictionary<int, List<Node>>();

        public DijkstraAlgorithm(Graph _graph, Vector2Int from, Vector2Int to)
        {
            graph = _graph;
            unvisitedNode = graph.Nodes;
            finalPos = to;

            foreach (Node nd in unvisitedNode)
            {
                nd.PrintNode();
            }

            //GetPath(from, to);
        }


        private void PrintPath()
        {
            Debug.Log("PRINT PATH ");
            foreach (Node nod in pathList)
            {
                Debug.Log(nod.PrintNode());
            }
        }

        private void PrintEnablePaths()
        {
            foreach (var pth in enablePaths)
            {
                string str = "path with " + pth.Key + " steps \n";
                foreach (Node nd in pth.Value)
                {
                    str = str + nd.PositionNode;
                }
                Debug.Log(str);
            }

        }



        private bool IsUnvisitedNode(Node node)
        {
            foreach (Node nd in unvisitedNode)
            {
                if (nd == node)
                {
                    return true;
                }
            }
            return false;
        }

        public void FindEndBranch()
        {
            List<Node> lastPathNodeChildrens = graph.GetChildrensNode(pathList.Last());

            for (int i = 0; i < lastPathNodeChildrens.Count; i++)
            {
                if (IsUnvisitedNode(lastPathNodeChildrens[i]) == true && lastPathNodeChildrens[i].IsParent(pathList.Last()))
                {
                    pathList.Add(lastPathNodeChildrens[i]);
                    Debug.Log(pathList.Count);
                    FindEndBranch();
                }
            }
        }

        public bool IsExsistPathWithCountSteps(int countSteps)
        {
            foreach (var path in enablePaths)
            {
                if (path.Key == countSteps)
                {
                    Debug.Log(path.Key);
                    return true;
                }

            }
            return false;
        }

        public void RemoveVisitedNodes(List<Node> path)
        {
            for (int i = 0; i < path.Count; i++)
            {



            }

        }


        public bool CheckBranch()
        {
            List<Node> path = new List<Node>();
            for (int i = 0; i < pathList.Count;)
            {
                if (pathList[i].PositionNode != finalPos)
                {
                    path.Add(pathList[i]);
                    i++;
                }
                else
                {
                    Debug.Log("find path");


                    enablePaths.Add(enablePaths.Count, path);
                    return true;
                    //unvisitedNode.Remove(path[path.Count - 1]);

                }

            }
            if (path.Last().PositionNode != finalPos)
            {
                pathList.Remove(path.Last());
                unvisitedNode.Remove(path.Last());
            }
            return false;
        }

        public Node Clear()
        {
            Node finalNode = graph.GetNode(finalPos);
            foreach (Node nod in unvisitedNode)
            {
                Debug.Log("node " + nod.PositionNode + "childrens count " + graph.GetChildrensNode(nod).Count);

                int count = graph.GetChildrensNode(nod).Count;

                if (count == 0 && nod!= finalNode)
                {
                    return nod;
                }


            }

            return null;

        }

        public void GetPath(Vector2Int from, Vector2Int to)
        {
            pathList.Add(graph.GetNode(from));
            Debug.Log("Unvisited Nodes " + unvisitedNode.Count);

            Node clearNode = Clear();
            while (clearNode != null)
            {
                unvisitedNode.Remove(clearNode);
                clearNode = Clear();
            }
            
            Debug.Log("Unvisited Nodes " + unvisitedNode.Count);


            graph.PrintNodes();

            Debug.Log("find childrens ");

            Debug.Log("Unvisited Nodes " + unvisitedNode.Count);
           while (isFindPath == false)
            {
                FindEndBranch();

                Debug.Log("branch \n");
                foreach (Node nd in pathList)
                {
                    Debug.Log(nd.PositionNode);
                }
                isFindPath = CheckBranch();
                Debug.Log(isFindPath);
            }
         

            PrintEnablePaths();
            Debug.Log("Unvisited Nodes " + unvisitedNode.Count);
        }

    }




}
