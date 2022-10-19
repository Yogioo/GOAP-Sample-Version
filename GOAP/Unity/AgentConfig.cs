using Deck.Core.GOAP.Setting;
using UnityEngine;

namespace Deck.Core.GOAP.Unity
{
    [CreateAssetMenu(fileName = "NewAgentConfigName", menuName = "AI/AgentConfig", order = 0)]
    public class AgentConfig : ScriptableObject
    {
        public AgentSetting Setting;
    }
}