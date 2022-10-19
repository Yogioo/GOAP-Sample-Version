using System.Collections.Generic;
using Deck.Core.GOAP.Core;

namespace Deck.Core.GOAP.Example
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

            // 初始化所有行为
            List<ActionData> actions = new List<ActionData>();
            ActionData.OnAction onAction = null;
            ActionData eat = AddAction("eat");
            eat.SetRequest(GameEnum.HasFood, true);
            eat.SetEffect(GameEnum.IsHungry, false);
            eat.SetAction(() =>
            {
                //TODO: 游戏通信,通知吃饭功能系统触发吃 其他类似这里就不写了
            });

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

            // 离线构建计划 TODO:实时计算计划的Cost 与可行性 然后执行
            var buildPlan = PlannerAPI.BuildPlan(goal, actions);
            // 实时构建计划
            var plan = PlannerAPI.MakePlan(worldData, goal, actions);
        }
    }
}