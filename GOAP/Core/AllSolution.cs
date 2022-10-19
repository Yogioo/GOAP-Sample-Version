using System.Collections.Generic;

namespace Deck.Core.GOAP.Core
{
    public class AllSolution
    {
        private Dictionary<GoalData, Solution> Data;

        public AllSolution()
        {
            Data = new Dictionary<GoalData, Solution>();
        }

        public AllSolution(Dictionary<GoalData, Solution> data)
        {
            Data = data;
        }

        public void AddSolution(GoalData goal, Solution solution)
        {
            Data.Add(goal, solution);
        }

        public Solution GetSolution(GoalData goalData)
        {
            return Data[goalData];
        }

        public List<GoalData> GetAllGoalData()
        {
            List<GoalData> goals = new List<GoalData>();
            //1.获得并排序Goal
            foreach (var kvp in Data)
            {
                goals.Add(kvp.Key);
            }

            return goals;
        }
    }

    public class Solution
    {
        public List<Stack<ActionData>> Data;

        public Solution(List<Stack<ActionData>> data)
        {
            Data = data;
        }
    }
}