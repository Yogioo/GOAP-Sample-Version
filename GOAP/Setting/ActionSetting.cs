using System;
using Deck.Core.GOAP.Core;

namespace Deck.Core.GOAP.Setting
{
    [System.Serializable]
    public class ActionSetting
    {
        public string Name;
        public FullDataSetting Request, Effects;
        public float Cost;
        public Func<float> GetCost;
        public ActionData GetActionData()
        {
            var result = new ActionData(Name);
            result.InitAction(Request.GetFullData(), Effects.GetFullData(), GetCost, Cost);
            return result;
        }
    }
}