using UnityEngine;
using UnityEditor;
using System.IO;

namespace Lab5Games.Editor
{
    public class ProjectSettingsWindow : EditorWindow
    {
        [MenuItem("Lab5Games/Project Settings")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<ProjectSettingsWindow>();
            wnd.titleContent = new GUIContent("Project Settings");

            wnd.minSize = new Vector2(450, 250);
            wnd.maxSize = new Vector2(450, 250);
        }

        const string DEFAULT_PROJECT_NAME = "New Project";

        string _projectName = DEFAULT_PROJECT_NAME;

        private void OnGUI()
        {
            GUILayout.Label("Organization", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _projectName = EditorGUILayout.TextField("Project Name", _projectName);
            if (GUILayout.Button("Organize", GUILayout.Width(100))) OrganizeProject();
            EditorGUILayout.EndHorizontal();
        }

        private void OrganizeProject()
        {
            string[] folders = new string[]
            {
                "Assets/{0}/Art/Animations",
                "Assets/{0}/Art/Materials",
                "Assets/{0}/Art/Models",
                "Assets/{0}/Art/Textures",
                "Assets/{0}/Art/Sprites",
                "Assets/{0}/Art/Fonts",

                "Assets/{0}/Audio/Musics",
                "Assets/{0}/Audio/Sounds",

                "Assets/{0}/Code/Scripts",
                "Assets/{0}/Code/Shaders",

                "Assets/{0}/Content/Prefabs",
                "Assets/{0}/Content/Scenes",
                "Assets/{0}/Content/Scriptables",

                "Assets/Settings",
                "Assets/Others"
            };

            string name = _projectName;
            if (string.IsNullOrEmpty(name))
                name = DEFAULT_PROJECT_NAME;

            name = "_" + name.Trim();
            name = name.Replace(" ", "_");
            
            foreach(var folder in folders)
            {
                string path = string.Format(folder, name);

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
