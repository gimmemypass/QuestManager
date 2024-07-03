#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using QuestSystem.BluePrints;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuestSystem
{
    public class AddQuestStepWindow : OdinEditorWindow
    {
        [Searchable, HideReferenceObjectPicker]
        [SerializeField]
        private List<Node> bluePrints = new();

        public void Init(Object parent, Action<BaseQuestStep> addToQuest)
        {
            bluePrints.Clear();
            var allSteps = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(BaseQuestStep)) && !t.IsAbstract);
            foreach (var step in allSteps)
            {
                bluePrints.Add(new Node(step.Name, step, parent, addToQuest)); 
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AssetDatabase.SaveAssets();
        }

        private class Node
        {
            [HideLabel, ReadOnly]
            public string Name;
            
            private readonly Type neededType;
            private readonly Object parent;
            private readonly Action<BaseQuestStep> addToQuest;

            public Node(string name, Type neededType, Object parent, Action<BaseQuestStep> addToQuest)
            {
                Name = name;
                this.neededType = neededType;
                this.parent = parent;
                this.addToQuest = addToQuest;
            }

            [Button]
            private void Add()
            {
                var asset = ScriptableObject.CreateInstance(neededType) as BaseQuestStep;
                AssetDatabase.AddObjectToAsset(asset, parent);
                asset.name = neededType.Name;
                addToQuest.Invoke(asset);
                EditorUtility.SetDirty(parent);
                GetWindow<AddQuestStepWindow>().Close();
            }
        }
    }
}
#endif