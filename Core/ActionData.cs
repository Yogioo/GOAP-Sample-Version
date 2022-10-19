using System;

namespace Deck.Core.GOAP.Core
{
    public class ActionData
    {
        private string m_ActionName;
        private FullData m_RequestData;
        private FullData m_EffectData;
        private Func<float> m_GetCost;
        private float CostValue;
        public FullData Request => m_RequestData;

        public delegate void OnAction();

        public OnAction ActiveAction;

        public ActionData(string actionName)
        {
            this.m_ActionName = actionName;
            InitAction(new FullData(),new FullData(),null);
        }
        public void InitAction(
            FullData request,
            FullData effects,
            OnAction action,
            Func<float> getCost = null,
            float costValue = 0)
        {
            this.m_RequestData = request;
            this.m_EffectData = effects;
            this.ActiveAction = action;
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

        public void SetRequest(Enum key,object val)
        {
            this.Request.Set(key,val);
        }

        public void SetEffect(Enum key, object val)
        {
            this.m_EffectData.Set(key,val);
        }

        public void SetAction(OnAction onAction)
        {
            ActiveAction = onAction;
        }
        
        public float GetCost()
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