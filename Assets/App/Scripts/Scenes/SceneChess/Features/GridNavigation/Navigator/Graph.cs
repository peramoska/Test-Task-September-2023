using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class Graph
    {
        private List<Node> nodes = new List<Node>();
        public List<Node> Nodes => nodes;
        List<Node> unvisitedNode = new List<Node>();
    
        public Graph(Vector2Int startPosition)
        {
            Node node = new Node(startPosition, startPosition);
            nodes.Add(node);
            unvisitedNode = Nodes;
        }

        public void AddNode(Vector2Int position, Vector2Int positionCameFrom)
        {
            bool isCreatedNode = false;
            foreach (Node nod in nodes)
            {
                if (nod.PositionNode == position)
                {
                    nod.UpdateNode(positionCameFrom);
                    isCreatedNode = true;
                }
            }

            if (isCreatedNode == false)
            {
                Node node = new Node(position, positionCameFrom);
                nodes.Add(node);
            }
        }

        public bool IsPositionOnGraph(Vector2Int vector)
        {

            foreach (Node n in Nodes)
            {
                if (n.PositionNode == vector)
                {
                    return true;
                }
            }
            return false;

        }

        public Node GetNode(Vector2Int position)
        {
            foreach (Node node in nodes)
            {
                if (node.PositionNode == position)
                {
                    return node;
                }
            }
            return null;
        }

        public Node GetNode(Node position)
        {
            foreach (Node node in nodes)
            {
                if (node == position)
                {
                    return node;
                }
            }
            return null;
        }


        public Node GetUnvisitedNode()
        {
            foreach (Node node in nodes)
            {
                if (node.IsVisited == false)
                {
                    return node;
                }
            }
            return null;
        }


        public bool IsPath(Vector2Int finalPos)
        {
            Node finalNode = GetNode(finalPos);
            if (finalNode != null)
            {
                return true;
            }
            return false;
        }

        public List<Node> GetChildrensNode(Node node)
        {
            List<Node> childrens = new List<Node>();

            foreach (Node nod in Nodes)
            {
                if (nod.IsParent(node) == true)
                {
                    childrens.Add(nod);
                }
            }

            return childrens;
        }

        public bool IsExsistPrevNode(Node node)
        {
            foreach (Node nd in nodes)
            {
                if (nd.Parents.Count != 0) return true;
            }
            return false;
        }

        public void PrintNodes()
        {
            foreach (Node node in nodes)
            {
                string nodeInfoString = "\n node " + node.PositionNode + "\n Parents: ";
                foreach (Vector2Int info in node.Parents)
                {
                    nodeInfoString = nodeInfoString + "  " + info + "\n";
                }

                Debug.Log(nodeInfoString);
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


        public bool IsEndNode(Node n, Vector2Int to)
        {
            Node toNode = GetNode(to);
            if (GetChildrensNode(n).Count == 0 && n!= toNode) return true;
            return false;

        }
    }



    public class Node
    {
        Vector2Int position;
        bool isVisited = false;
        List<Vector2Int> parentNodes = new List<Vector2Int>();
      

        public bool IsVisited => isVisited;
        public List<Vector2Int> Parents => parentNodes;
        public Vector2Int PositionNode => position;


        public Node(Vector2Int _pos, Vector2Int _cameFromNode)
        {
            position = _pos;
            parentNodes.Add(_cameFromNode);
        }

        public void UpdateNode(Vector2Int newCameFromPos)
        {
            parentNodes.Add(newCameFromPos);
        }

        public void ChangeVisitedStatus()
        {
            isVisited = true;
        }


        public void RemoveParent(Vector2Int removeThisParent)
        {
            foreach (Vector2Int parent in Parents)
            {
                if (parent == removeThisParent)
                {
                    Parents.Remove(parent);
                }
            }

        }

        public bool IsParent(Node nd)
        {
            foreach (Vector2Int parent in Parents)
            {
                if (nd.PositionNode != PositionNode && parent == nd.PositionNode)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsParent(Vector2Int nd)
        {
            foreach (Vector2Int parent in Parents)
            {
                if (nd != PositionNode && parent == nd)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanCameFromNode(Vector2Int pos)
        {
            foreach (Vector2Int vector in Parents)
            {
                if (vector == pos) return true;
            }

            return false;
        }

        public string PrintNode()
        {
            string info = "Node: " + PositionNode + "\n parents:";

            foreach (Vector2Int nodeParents in Parents)
            {
                info = info + nodeParents + "\n";
            }
            return info;
        }

    }
}
