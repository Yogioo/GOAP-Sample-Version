using System;

namespace Deck.Core.GOAP.Core
{
    public class GoalData
    {
        public GoalData()
        {
            Data = new FullData();
        }

        public readonly FullData Data;

        public void SetGoal(Enum enumKey, object value)
        {
            Data.Set(enumKey, value);
        }
    }
}