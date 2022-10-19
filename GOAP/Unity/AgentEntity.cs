using System.Collections.Generic;
using Deck.Core.GOAP.Core;
using Deck.Core.GOAP.Setting;
using UnityEngine;

namespace Deck.Core.GOAP.Unity
{
    public class AgentEntity : MonoBehaviour
    {
        public AgentConfig ConfigData;
        public FullDataSetting WorldDataSetting = new FullDataSetting(); //测试用
        public FullData worldData; //测试用

        private AllSolution m_AllSolution;

        private void Awake()
        {
            m_AllSolution = ConfigData.Setting.GetAllSolution();
        }

        public void Update()
        {
            worldData = WorldDataSetting.GetFullData(); //测试用
            if (PlannerAPI.GetAction(worldData, m_AllSolution,
                    out GoalData goal,
                    out Stack<ActionData> executeActionStack,
                    out ActionData nextAct))
            {
                Debug.Log($"this AI Should Do:{nextAct}");
            }
        }
    }
}