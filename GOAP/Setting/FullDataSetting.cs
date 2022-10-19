using System;
using System.Collections.Generic;
using Deck.Core.GOAP.Core;

namespace Deck.Core.GOAP.Setting
{
    [Serializable]
    public class FullDataSetting
    {
        public List<InputSetting> Data;
        public FullData GetFullData()
        {
            var result = new FullData();
            foreach (var inputSetting in Data)
            {
                object value = null;
                string inputStr = inputSetting.InputValue;
                try
                {
                    switch (inputSetting.InputType)
                    {
                        case InputType.BOOLEAN:
                            value = inputStr == "true" || inputStr == "True" ||
                                    inputStr == "TRUE";
                            break;
                        case InputType.INT:
                            value = int.Parse(inputStr);
                            break;
                        case InputType.FLOAT:
                            value = float.Parse(inputStr);
                            break;
                        case InputType.STRING:
                            value = inputStr;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    result.Set(inputSetting.Key, value);
                }
                catch
                {
                    throw new Exception($"Key:{inputSetting.Key} Parse Error,ValueType:{inputSetting.InputType} and Value:{inputSetting.InputValue} Not Match!");
                }
            }

            return result;
        }
    }

    [System.Serializable]
    public class InputSetting
    {
        public string Key;
        public InputType InputType;
        public string InputValue;
    }

    public enum InputType
    {
        BOOLEAN,
        INT,
        FLOAT,
        STRING
    }
}