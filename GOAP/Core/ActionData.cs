using System;

namespace Deck.Core.GOAP.Core
{
    public class ActionData
    {
        /// <summary>
        /// ActionName 可以用于解析成对应游戏事件,比如移动比如攻击
        /// </summary>
        private string m_ActionName;

        /// <summary>
        /// 需求数据
        /// </summary>
        private FullData m_RequestData;

        /// <summary>
        /// 影响数据
        /// </summary>
        private FullData m_EffectData;

        /// <summary>
        /// 获得消耗,可以在运行时不为空时调用 返回一个动态的消耗
        /// </summary>
        private Func<float> m_GetCost;

        /// <summary>
        /// 消耗 如果GetCost方法为空时,返回这个配置项
        /// </summary>
        private float CostValue;

        public FullData Request => m_RequestData;

        public delegate void OnAction();

        public ActionData(string actionName)
        {
            this.m_ActionName = actionName;
            InitAction(new FullData(), new FullData(), null);
        }

        public void InitAction(
            FullData request,
            FullData effects,
            Func<float> getCost = null,
            float costValue = 0)
        {
            this.m_RequestData = request;
            this.m_EffectData = effects;
            this.m_GetCost = getCost;
            this.CostValue = costValue;
        }

        public void InitCost(Func<float> getCost)
        {
            this.m_GetCost = getCost;
        }

        public void InitCost(float costValue)
        {
            this.CostValue = costValue;
        }

        public void SetRequest(Enum key, object val)
        {
            this.Request.Set(key, val);
        }

        public void SetEffect(Enum key, object val)
        {
            this.m_EffectData.Set(key, val);
        }

        public float GetCostValue()
        {
            if (this.m_GetCost != null)
            {
                return this.m_GetCost.Invoke();
            }
            else
            {
                return CostValue;
            }
        }

        /// <summary>
        /// 判读当前Action能不能解决GoalData的需求
        /// </summary>
        /// <param name="goalData"></param>
        /// <returns></returns>
        public bool IsMatchEffects(FullData goalData)
        {
            return this.m_EffectData.Contain(goalData);
        }

        public override string ToString()
        {
            return m_ActionName;
        }
    }
}