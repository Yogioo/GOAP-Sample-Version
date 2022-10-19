using System;
using System.Collections.Generic;

namespace Deck.Core.GOAP.Core
{
    public class FullData
    {
        public Dictionary<string, object> m_Data;

        public FullData()
        {
            m_Data = new Dictionary<string, object>();
        }

        /// <summary>
        /// 判断当前Data是否包含目标Data (当前结果是否能解决目标)
        /// </summary>
        /// <param name="targetData"></param>
        /// <returns></returns>
        public bool Contain(FullData targetData)
        {
            foreach (var targetKvp in targetData.m_Data)
            {
                if (m_Data.TryGetValue(targetKvp.Key, out var globalKvpValue))
                {
                    bool isContain = false;
                    isContain |= targetKvp.Value is bool && globalKvpValue.Equals(targetKvp.Value);
                    isContain |= targetKvp.Value is float && (float)globalKvpValue >= (float)targetKvp.Value;
                    isContain |= targetKvp.Value is int && (int)globalKvpValue >= (int)targetKvp.Value;
                    isContain |= targetKvp.Value is string && globalKvpValue.Equals(targetKvp.Value);
                    if (isContain)
                    {
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public FullData DeepCopy()
        {
            var newData = new FullData
            {
                m_Data = new Dictionary<string, object>(this.m_Data)
            };
            return newData;
        }

        public void ModifyFromOther(FullData target)
        {
            var localData = (target as FullData);
            foreach (var localKvp in localData.m_Data)
            {
                m_Data[localKvp.Key] = localKvp.Value;
            }
        }

        public void Set(Enum enumKey, object value)
        {
            Set(enumKey.ToString(), value);
        }

        public void Set(string strKey, object value)
        {
            m_Data[strKey] = value;
        }

        public bool GetBool(Enum enumKey)
        {
            if (TryGet(enumKey, out var value))
            {
                return (bool)value;
            }

            return false;
        }

        public bool TryGet(Enum enumKey, out object value)
        {
            if (m_Data.TryGetValue(enumKey.ToString(), out value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}