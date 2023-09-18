using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public enum Direction
    {
        up,
        down
    }

    public enum StepType
    {
        up, //pon
        nextField,//king
        diagonal, //queen, bishop
        vertical, //queen, rook
        horizontal, // queen, rook
        theLetterG //knight
    }

    public class Step
    {
        private Vector2Int startCell;
        private Vector2Int finalCell;

        private List<Vector2Int> stepsFromRule;

        public List<Vector2Int> Steps => stepsFromRule;

        public Step(ChessUnitType type, Vector2Int unitCell, Vector2Int unitEndCell, Direction dir, ChessGrid grid)
        {
            startCell = unitCell;
            finalCell = unitEndCell;

            stepsFromRule = new List<Vector2Int>();
            List<StepType> unitStepRules = new List<StepType>();
            unitStepRules = UnitStepRules.Instance.GetRules(type);

            for (int i = 0; i < unitStepRules.Count; i++)
            {
                List<Vector2Int> list = new List<Vector2Int>();
                list = GetPositionsFromRule(unitStepRules[i], startCell, dir, grid);

                foreach (Vector2Int pos in list)
                {
                    //Debug.Log(pos);
                    stepsFromRule.Add(pos);
                }
            }

        }

        private List<Vector2Int> GetPositionsFromRule(StepType rule, Vector2Int from, Direction dir, ChessGrid grid) //direction
        {
            List<Vector2Int> list = new List<Vector2Int>();

            switch (rule)
            {
                case StepType.up:
                    UpPattern upPattern = new UpPattern(from, dir, grid);
                    list = upPattern.PatternPositions;
                    break;
                case StepType.nextField:
                    NextFieldPattern nextFieldPattern = new NextFieldPattern(from, dir, grid);
                    list = nextFieldPattern.PatternPositions;
                    break;
                case StepType.horizontal:
                    HorizontalPattern horizontalPattern = new HorizontalPattern(from, dir, grid);
                    list = horizontalPattern.PatternPositions;
                    break;
                case StepType.vertical:
                    VerticalPattern verticalPattern = new VerticalPattern(from, dir, grid);
                    list = verticalPattern.PatternPositions;
                    break;
                case StepType.theLetterG:

                    TheLetterGPattern theLetterGPattern = new TheLetterGPattern(from, dir, grid);
                    list = theLetterGPattern.PatternPositions;
                    break;
                case StepType.diagonal:
                    DiagonalPattern diagonalPattern = new DiagonalPattern(from, dir, grid);
                    list = diagonalPattern.PatternPositions;
                    break;
            }

            if (list != null) return list;
            else return null;
        }


    }


}
