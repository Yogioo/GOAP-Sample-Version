using System.Collections.Generic;
using System.Linq;

namespace Deck.Core.GOAP.Core
{
    public static class PlannerAPI
    {
        /// <summary>
        /// 规划节点, 构建所有计划的计划树
        /// </summary>
        /// <param name="allGoals"></param>
        /// <param name="allActions"></param>
        /// <returns></returns>
        public static AllSolution BuildAllSolution(List<GoalData> allGoals, List<ActionData> allActions)
        {
            AllSolution allSolution = new AllSolution();
            foreach (var goalData in allGoals)
            {
                var solution = BuildSolution(goalData, allActions);
                allSolution.AddSolution(goalData, solution);
            }

            return allSolution;
        }

        /// <summary>
        /// 规划阶段, 构建计划树
        /// </summary>
        /// <param name="goalData"></param>
        /// <param name="allActions"></param>
        /// <returns>返回当前目标的解决方案</returns>
        public static Solution BuildSolution(GoalData goalData, List<ActionData> allActions)
        {
            var solutionData = GetAllPlan(null, goalData.Data, allActions);
            return new Solution(solutionData);
        }

        /// <summary>
        /// 规划完毕后, 运行时,基于计划树判断AI下一个应该可以执行的行为
        /// </summary>
        /// <param name="worldData">世界信息</param>
        /// <param name="allSolution">AI规划数据</param>
        /// <param name="goalData">AI正在处理什么目标(测试用)</param>
        /// <param name="executeActionData">AI需要执行的链路(测试用)</param>
        /// <param name="nextAction">AI应该做的行为(执行用)</param>
        /// <returns></returns>
        public static bool GetAction(FullData worldData, AllSolution allSolution,
            out GoalData goalData,
            out Stack<ActionData> executeActionData,
            out ActionData nextAction)
        {
            //1.获得并排序Goal
            List<GoalData> goals = allSolution.GetAllGoalData();
            //降序排列
            goals.Sort((a, b) => -a.GoalPriority.CompareTo(b.GoalPriority));
            while (goals.Count > 0)
            {
                //2.选择最高优先级的目标
                GoalData currentGoal = goals[0];
                Solution currentSolution = allSolution.GetSolution(currentGoal);
                bool isExecuteAbleAny = false;
                float minCost = float.MaxValue;
                int minIndex = -1;
                int minExecuteAbleIndex = -1;
                for (var i = 0; i < currentSolution.Data.Count; i++)
                {
                    //3.从所有的行为链路中 判断是否可以执行 如果可以执行 返回Cost
                    int executeAbleIndex = -1;
                    Stack<ActionData> actionStack = currentSolution.Data[i];
                    bool isExecuteAble = false;
                    var actionArray = actionStack.ToArray();
                    for (var index = actionArray.Length - 1; index >= 0; index--)
                    {
                        var actionData = actionArray[index];
                        // 世界信息中, 满足了Action的执行条件, 说明此Action可以被执行, 那么后续的所有Action就不需要判断了, 因为都可以执行
                        isExecuteAble |= worldData.Contain(actionData.Request);
                        // executeAbleIndex++;
                        if (isExecuteAble)
                        {
                            executeAbleIndex = index;
                            isExecuteAbleAny = true;
                            break;
                        }
                    }


                    if (isExecuteAble)
                    {
                        var currentCost = actionStack.GetCost();
                        if (minCost > currentCost)
                        {
                            minCost = currentCost;
                            minIndex = i;
                            minExecuteAbleIndex = executeAbleIndex;
                        }
                    }
                }

                // 如果当前目标是无法被解决的, 那么判断下一个目标
                if (isExecuteAbleAny)
                {
                    //4.如果有可以执行 选择最小的Cost的行为链路
                    goalData = currentGoal;
                    executeActionData = currentSolution.Data[minIndex];
                    nextAction = null;
                    var actionArr = executeActionData.ToArray();
                    nextAction = actionArr[minExecuteAbleIndex];
                    return true;
                }
                else
                {
                    goals.RemoveAt(0);
                }
            }

            goalData = null;
            executeActionData = null;
            nextAction = null;
            return false;
        }

        public static Stack<ActionData> MakePlan(FullData worldData, GoalData goalData, List<ActionData> allActions)
        {
            var allPlan = GetAllPlan(worldData, goalData.Data, allActions);
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
                    currentTotalCost += action.GetCostValue();
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

        private static List<Stack<ActionData>> GetAllPlan(FullData worldData, FullData goalData,
            List<ActionData> allActions)
        {
            List<Stack<ActionData>> allPlan = new List<Stack<ActionData>>();
            foreach (ActionData action1 in allActions)
            {
                DeepLoop(allActions, action1, new Stack<ActionData>(), allPlan, worldData, goalData);
            }

            return allPlan;
        }

        private static void DeepLoop(List<ActionData> allActionsA, ActionData actionA, Stack<ActionData> plan,
            List<Stack<ActionData>> allPlan,
            FullData worldData, FullData goalData, int limit = 1000)
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


                // 其次匹配需求 (worldData 在构建的时候是为空的) (如果此Action没有Require那么可以返回解)
                if (worldData != null && worldData.Contain(requestA) || requestA.m_Data.Count == 0)
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
                        DeepLoop(allActionsB, actionB, copyPlan, allPlan, worldData, requestA, limit);
                    }

                    // 规划阶段, 把没解的也存起来
                    if (worldData == null)
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

        public static float GetCost(this Stack<ActionData> actionStack)
        {
            float currentTotalCost = 0;
            var plan = actionStack;
            foreach (var action in plan)
            {
                currentTotalCost += action.GetCostValue();
            }

            return currentTotalCost;
        }
    }
}