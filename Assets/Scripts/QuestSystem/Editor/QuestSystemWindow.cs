using System;
using System.IO;
using QuestSystem.BluePrints;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace QuestSystem.Editor
{
    public class QuestSystemWindow : OdinMenuEditorWindow
    {
        private const string SaveKey = "questSystemConfigSaveKey";
        
        public Config config;
        
        [MenuItem("Tools/QuestSystem")]
        public static void ShowWindow()
        {
            GetWindow<QuestSystemWindow>().Show();
        }

        private void Awake()
        {
            config = JsonConvert.DeserializeObject<Config>(PlayerPrefs.GetString(SaveKey));
            config ??= new Config();
        }

        protected override void OnDestroy()
        {
            var json = JsonConvert.SerializeObject(config);
            PlayerPrefs.SetString(SaveKey, json);
            base.OnDestroy();
        }

        protected override void OnGUI()
        {
            
            var rect = GUILayoutUtility.GetRect(100, 200, 20,20);
            var updateButtonName = "UpdateWindow";
            if(GUI.Button(rect, updateButtonName))
                ForceMenuTreeRebuild();
            base.OnGUI();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("Config", config);
            if (config.QuestsPath != null)
            {
                tree.Add("Create Quest", new CreateQuest(config.QuestsPath));
                tree.AddAllAssetsAtPath("StoryQuests", config.QuestsPath, typeof(Quest), true);
            }
            return tree;
        }

        [Serializable]
        public class CreateQuest : IValidate
        {
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public Quest quest;
            
            private readonly string folderPath;

            public CreateQuest(string folderPath)
            {
                this.folderPath = folderPath;
                quest = CreateInstance<Quest>();
                quest.Construct();
            }


            [Button("Add new SO")]
            [InfoBox("Fill all information", InfoMessageType.Warning, "@!IsValid()")]
            [EnableIf(nameof(IsValid))]
            private void CreateNewData()
            {
                var path = Path.Combine(folderPath, $"{quest.Name}_{quest.ID}.asset");
                
                AssetDatabase.CreateAsset(quest, path);
                AssetDatabase.SaveAssets();
            }

            public bool IsValid()
            {
                if (string.IsNullOrEmpty(quest.Name))
                    return false;
                if (string.IsNullOrEmpty(quest.Description))
                    return false;
                
                return true;
            }
        }

        [Serializable]
        public class Config
        {
            [FolderPath]
            public string QuestsPath;
        }
    }
}
