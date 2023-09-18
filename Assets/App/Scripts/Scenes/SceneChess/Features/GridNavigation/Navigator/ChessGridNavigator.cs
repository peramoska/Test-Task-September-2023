using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;

using System.Linq;
using UnityEngine;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.View.ContainerUnits;
using System.IO;


namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            
            if (isFreeCell(grid, to) == true)
            {
                List<ChessUnit> units = new List<ChessUnit>();
                units = grid.Pieces.ToList();

                Direction dir = Direction.up;
                foreach (ChessUnit un in units)
                {
                    if (un.PieceModel.PieceType == unit && un.CellPosition == from)
                    {
                        switch (un.PieceModel.Color)
                        {
                            case ChessUnitColor.White:
                                dir = Direction.down;
                                break;
                            case ChessUnitColor.Black:
                                dir = Direction.up;
                                break;
                        }
                    }
                }

                List<Vector2Int> path = new List<Vector2Int>();
                switch (dir)
                {
                    case Direction.up:
                        if (from.y <= to.y && isFreeCell(grid, to) == true)
                        {
                            Graph graphPath = new Graph(from);

                            CreateSteps(graphPath, unit, to, from, dir, grid);

                            if (graphPath.IsPath(to) == true)
                            {

                                Vector2Int part = CreatePath(unit, to, from, dir, grid);
                                path.Add(part);

                                while (CreatePath(unit, path.Last(), from, dir, grid) != path.Last())
                                {
                                    path.Add(CreatePath(unit, path.Last(), from, dir, grid));
                                }

                                path.Reverse();
                                path.Add(to);



                                foreach (Vector2Int p in path)
                                {
                                    Debug.Log(p);
                                }

                            }
                            return path;
                        }
                        break;
                    case Direction.down:
                        if (from.y >= to.y && isFreeCell(grid, to) == true)
                        {
                            Graph graph1 = new Graph(from);
                            CreateSteps(graph1, unit, to, from, dir, grid);
                            Node fromNode = graph1.GetNode(from);

                            if (graph1.IsPath(to) == true)
                            {

                                Vector2Int part = CreatePath(unit, to, from, dir, grid);
                                path.Add(part);

                                while (CreatePath(unit, path.Last(), from, dir, grid) != path.Last())
                                {
                                    path.Add(CreatePath(unit, path.Last(), from, dir, grid));
                                }

                                path.Reverse();
                                path.Add(to);
                               


                                foreach (Vector2Int p in path)
                                {
                                    Debug.Log(p);
                                }

                            }
                            return path;
                        }
                        break;
                }
            }
            return null;
        }


        private Vector2Int CreatePath(ChessUnitType unit, Vector2Int to, Vector2Int from, Direction dir, ChessGrid grid)
        {
            Vector2Int path = new Vector2Int();
            Graph stepGrapf = new Graph(from);

            List<Vector2Int> unvisitedSteps = new List<Vector2Int>(0); // точки из графов детей
            List<Vector2Int> visitedSteps = new List<Vector2Int>(0); //посещенные

            foreach (Node ndd in stepGrapf.Nodes)
            {
                unvisitedSteps.Add(ndd.PositionNode);
            }

            while (unvisitedSteps.Count > 0)
            {
                stepGrapf = CreateGraph(unit, to, unvisitedSteps[0], dir, grid);

                Debug.Log(stepGrapf.GetNode(to) == null);
                if (stepGrapf.GetNode(to) == null)
                {
                    Node newNode = stepGrapf.GetNode(unvisitedSteps[0]);
                    List<Node> childrens = stepGrapf.GetChildrensNode(newNode);
                    foreach (Node nd in childrens)
                    {
                        if (unvisitedSteps.Contains(nd.PositionNode) == false && visitedSteps.Contains(nd.PositionNode) == false && stepGrapf.IsEndNode(nd, to) != false)
                        {
                            unvisitedSteps.Add(nd.PositionNode);
                        }
                    }
                    visitedSteps.Add(unvisitedSteps.First());
                    unvisitedSteps.Remove(unvisitedSteps.First());
                }
                else
                {
                    path = unvisitedSteps.First();
                    return path;
                }
            }
            return path;
        }

        private void CreateSteps(Graph graph, ChessUnitType unit, Vector2Int to, Vector2Int from, Direction dir, ChessGrid grid)
        {
            Step step = new Step(unit, from, to, dir, grid);
            Node fromNode = graph.GetNode(from);
            fromNode.ChangeVisitedStatus();
            foreach (Vector2Int pos in step.Steps)
            {
                graph.AddNode(pos, from);
            }
            Node nextNode = graph.GetUnvisitedNode();
            if (nextNode != null) CreateSteps(graph, unit, to, nextNode.PositionNode, dir, grid);
        }

        private Graph CreateGraph(ChessUnitType unit, Vector2Int to, Vector2Int from, Direction dir, ChessGrid grid)
        {
            Graph graph = new Graph(from);
            Step step = new Step(unit, from, to, dir, grid);
            Node fromNode = graph.GetNode(from);
            fromNode.ChangeVisitedStatus();
            foreach (Vector2Int pos in step.Steps)
            {
                graph.AddNode(pos, from);
            }
            return graph;

        }

        public bool isFreeCell(ChessGrid grid, Vector2Int cell)
        {
            ChessUnit piece = grid.Get(cell);
            if (piece == null) return true;
            else return false;
        }
    }

}

