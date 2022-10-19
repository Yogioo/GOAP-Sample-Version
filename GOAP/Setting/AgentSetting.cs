using System.Collections.Generic;
using Deck.Core.GOAP.Core;

namespace Deck.Core.GOAP.Setting
{
    [System.Serializable]
    public class AgentSetting
    {
        public GoalSetting GoalSetting;
        public List<ActionSetting> ActionSetting;
        public AllSolution GetAllSolution()
        {
            var Solutions = new Dictionary<GoalData,Solution>();
            List<ActionData> allActions = new List<ActionData>();
            foreach (var actionSetting in this.ActionSetting)
            {
                allActions.Add(actionSetting.GetActionData());
            }

            foreach (var goal in this.GoalSetting.GetGoals())
            {
                var solution = PlannerAPI.BuildSolution(goal, allActions);
                Solutions.Add(goal, solution);
            }

            return new AllSolution(Solutions);
        }
    }
}