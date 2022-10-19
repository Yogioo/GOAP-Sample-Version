using System.Collections.Generic;

namespace Deck.Core.GOAP.Core.Example
{
    public class ExamplePlanner
    {
        public void Main()
        {
            // 游戏信息
            var worldData = new FullData();
            worldData.Set(GameEnum.IsHungry, true);
            worldData.Set(GameEnum.HasIngredients, true);
            worldData.Set(GameEnum.HasPhoneNumber, true);
            worldData.Set(GameEnum.Money, 10);
            // 目标设定
            GoalData goal = new GoalData();
            goal.SetGoal(GameEnum.IsHungry, false);
            goal.GoalPriority = 100;

            GoalData goal2 = new GoalData();
            goal2.SetGoal(GameEnum.HasFood, true);
            goal.GoalPriority = 1;

            // 初始化所有行为
            List<ActionData> actions = new List<ActionData>();
            ActionData.OnAction onAction = null;
            ActionData eat = AddAction("eat");
            eat.SetRequest(GameEnum.HasFood, true);
            eat.SetEffect(GameEnum.IsHungry, false);

            ActionData phoneForApple = AddAction("phoneForApple");
            phoneForApple.SetRequest(GameEnum.HasPhoneNumber, true);
            phoneForApple.SetRequest(GameEnum.Money, 11);
            phoneForApple.SetEffect(GameEnum.AppleOnRoute, true);

            ActionData waitForDelivery = AddAction("waitForDelivery");
            waitForDelivery.SetRequest(GameEnum.AppleOnRoute, true);
            waitForDelivery.SetEffect(GameEnum.HasFood, true);

            ActionData bake = AddAction("bake");
            bake.SetRequest(GameEnum.FoodMixed, true);
            bake.SetEffect(GameEnum.FoodCooked, true);

            ActionData mix = AddAction("mix");
            mix.SetRequest(GameEnum.HasIngredients, true);
            mix.SetEffect(GameEnum.FoodMixed, true);

            ActionData serve = AddAction("serve");
            serve.SetRequest(GameEnum.FoodCooked, true);
            serve.SetEffect(GameEnum.HasFood, true);

            ActionData AddAction(string actionName)
            {
                var newAction = new ActionData(actionName);
                actions.Add(newAction);
                return newAction;
            }


            // 举例:离线构建整个AI的解决方案
            var allGoals = new List<GoalData>
            {
                goal,
                goal2
            };
            var buildAllSolution = PlannerAPI.BuildAllSolution(allGoals, actions);

            // 举例:实时构建计划
            var plan = PlannerAPI.MakePlan(worldData, goal, actions);
        }
    }
}