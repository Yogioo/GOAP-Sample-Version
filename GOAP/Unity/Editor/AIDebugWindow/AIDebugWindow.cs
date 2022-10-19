using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Deck.Core.GOAP.Unity.Editor
{
    public class AIDebugWindow : EditorWindow
    {
        [MenuItem("Tools/AI/AIDebugWindow")]
        public static AIDebugWindow Show()
        {
            AIDebugWindow wnd = GetWindow<AIDebugWindow>();
            wnd.titleContent = new GUIContent("AIDebugWindow");
            return wnd;
        }

        [OnOpenAsset(2)]
        public static bool step2(int instanceID, int line)
        {
            var aiDebugWindow = Show();
            var assetPath = AssetDatabase.GetAssetPath(instanceID);
            if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(AgentConfig))
            {
                var assets = AssetDatabase.LoadAssetAtPath<AgentConfig>(assetPath);
                if (assets != null)
                {
                    aiDebugWindow.Init(assets);
                }
            }

            return false;
        }

        public void Init(AgentConfig agentConfig)
        {
            Debug.Log("Init agentConfig");
        }

        public void OnEnable()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            //TODO: 使用UIBuilder写个编辑器
            // 1. OverView界面, 显示Goal Action 信息
            // 2. Action Stack界面, 显示出所有可行的任务链
            // 3. Runtime界面, 显示运行时, 各参数情况, 正在执行的任务链与任务

            
            // // Import UXML
            // var visualTree =
            //     AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            //         "Assets/Scripts/Deck/Core/GOAP/Unity/Editor/AIDebugWindow/AIDebugWindow.uxml");
            // VisualElement labelFromUXML = visualTree.CloneTree();
            // root.Add(labelFromUXML);
        }
    }
}