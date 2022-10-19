using System.Collections.Generic;
using System.Linq;

namespace Deck.Core.GOAP.Core
{
    public class PlannerAPI
    {
        public static List<Stack<ActionData>> BuildPlan(GoalData goalData, List<ActionData> allActions)
        {
            var allPlan = GetAllPlan(null, goalData.Data, allActions);
            return allPlan;
        }

        public static Stack<ActionData> MakePlan(FullData worldData, GoalData goalData, List<ActionData> allActions)
        {
            FullData planeData = worldData.DeepCopy();
            var allPlan = GetAllPlan(planeData, goalData.Data, allActions);
            // 基于Cost排序 选择最优解
            return GetPlanByMinCost(allPlan);
        }

        private static Stack<ActionData> GetPlanByMinCost(List<Stack<ActionData>> allPlan)
        {
            float minTotalCost = float.MaxValue;
            int minCostIndex = -1;
            for (var i = 0; i < allPlan.Count; i++)
            {
                float currentTotalCost = 0;
                var plan = allPlan[i];
                foreach (var action in plan)
                {
                    currentTotalCost += action.GetCost();
                }

                if (minTotalCost > currentTotalCost)
                {
                    minTotalCost = currentTotalCost;
                    minCostIndex = i;
                }
            }

            if (minCostIndex != -1)
            {
                return allPlan[minCostIndex];
            }
            else
            {
                return null;
            }
        }


        private static List<Stack<ActionData>> GetAllPlan(FullData planData, FullData goalData,
            List<ActionData> allActions)
        {
            List<Stack<ActionData>> allPlan = new List<Stack<ActionData>>();
            foreach (ActionData action1 in allActions)
            {
                DeepLoop(allActions, action1, new Stack<ActionData>(), allPlan, planData, goalData);
            }

            return allPlan;
        }

        private static void DeepLoop(List<ActionData> allActionsA, ActionData actionA, Stack<ActionData> plan,
            List<Stack<ActionData>> allPlan,
            FullData planData, FullData goalData, int limit = 1000)
        {
            limit--;
            if (limit < 0)
            {
                return;
            }

            var allActionsB = new List<ActionData>(allActionsA);
            allActionsB.Remove(actionA);
            //首先匹配结果
            if (actionA.IsMatchEffects(goalData))
            {
                plan.Push(actionA);
                var requestA = actionA.Request;


                // 其次匹配需求 (planData在构建的时候是为空的) (如果此Action没有Require那么可以返回解)
                if (planData != null && planData.Contain(requestA) || requestA.m_Data.Count == 0)
                {
                    // 需求与结果都能满足 那么就是一个解
                    allPlan.Add(plan);
                }
                else
                {
                    // 如果解不满足 那么继续寻找解
                    foreach (var actionB in allActionsB)
                    {
                        // 刷新结果为当前行为的需求
                        Stack<ActionData> copyPlan = new Stack<ActionData>(plan.Distinct().Reverse());
                        DeepLoop(allActionsB, actionB, copyPlan, allPlan, planData, requestA, limit);
                    }

                    // 规划阶段, 把没解的也存起来
                    if (planData == null)
                    {
                        allPlan.Add(plan);
                    }
                }
            }
            else
            {
                // 如果结果不匹配， 说明此Action不属于解的一环
            }
        }
    }
}