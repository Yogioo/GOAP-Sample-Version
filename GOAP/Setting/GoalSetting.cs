using System.Collections.Generic;
using Deck.Core.GOAP.Core;

namespace Deck.Core.GOAP.Setting
{
    [System.Serializable]
    public class GoalSetting
    {
        public List<FullDataSetting> Goals;

        public List<GoalData> GetGoals()
        {
            var result = new List<GoalData>();
            foreach (var fullDataSetting in Goals)
            {
                var goal=new GoalData();
                goal.SetGoal(fullDataSetting.GetFullData());
                result.Add(goal);
            }

            return result;
        }
    }
}