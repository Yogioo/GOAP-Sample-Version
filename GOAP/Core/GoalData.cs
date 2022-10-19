using System;

namespace Deck.Core.GOAP.Core
{
    public class GoalData
    {
        public GoalData()
        {
            Data = new FullData();
        }

        public FullData Data { get; private set; }
        public int GoalPriority;

        public void SetGoal(Enum enumKey, object value)
        {
            Data.Set(enumKey, value);
        }

        public void SetGoal(FullData data)
        {
            Data = data;
        }
    }
}