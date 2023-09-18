using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;


namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class UnitStepRules
    {
        public static readonly UnitStepRules Instance = new UnitStepRules();
            private Dictionary<ChessUnitType, List<StepType>> rules = new Dictionary<ChessUnitType, List<StepType>>();

            public UnitStepRules()
            {
                InitializeUnitStepRules();
            }

            private void InitializeUnitStepRules()
            {
                List<StepType> ponStep = new List<StepType>();
                ponStep.Add(StepType.up);
                rules.Add(ChessUnitType.Pon, ponStep);

                List<StepType> kingStep = new List<StepType>();
                kingStep.Add(StepType.nextField);
                rules.Add(ChessUnitType.King, kingStep);

                List<StepType> queenStep = new List<StepType>();
                queenStep.Add(StepType.vertical);
                queenStep.Add(StepType.horizontal);
                queenStep.Add(StepType.diagonal);
                rules.Add(ChessUnitType.Queen, queenStep);

                List<StepType> rookStep = new List<StepType>();
                rookStep.Add(StepType.horizontal);
               rookStep.Add(StepType.vertical);
                rules.Add(ChessUnitType.Rook, rookStep);

                List<StepType> knightStep = new List<StepType>();
                knightStep.Add(StepType.theLetterG);
                rules.Add(ChessUnitType.Knight, knightStep);

                List<StepType> bishopStep = new List<StepType>();
                bishopStep.Add(StepType.diagonal);
                rules.Add(ChessUnitType.Bishop, bishopStep);

            }

            public List<StepType> GetRules(ChessUnitType unitType)
            {
                List<StepType> unitRulesList = new List<StepType>();
                foreach (var unitRule in rules)
                {
                    if (unitRule.Key == unitType)
                    {
                        int countRules = 0;
                        foreach (StepType step in unitRule.Value)
                        {

                            unitRulesList.Add(step);
                            countRules++;
                        }

                    }
                }
                return unitRulesList;

            }
     
    }
}
