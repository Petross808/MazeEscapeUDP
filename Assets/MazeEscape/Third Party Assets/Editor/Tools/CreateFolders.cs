using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GD
{
    /// <summary>
    /// Creates a consistent folder system under a user defined project name
    /// Access from "GD/Utils/Create Project Folders" in the main menu
    /// </summary>
    /// <see cref="https://unity.com/how-to/organizing-your-project"/>
    public class CreateFolders : EditorWindow
    {
        private static string projectName = "Type project name here...";
        private static string folderPath;
        private static bool generateMyAssets;
        private static bool generateThirdPartyAssets;

        [MenuItem("Tools/DkIT/Utils/Create project folders")]
        private static void ShowProjectPopup()
        {
            var window = GetWindow(typeof(CreateFolders));

            window.minSize = new Vector2(300, 100);
            window.maxSize = new Vector2(800, 100);

            var title = new GUIContent();
            title.text = "Create Project Folders";
            window.titleContent = title;
        }

        private static void CreateAllFolders()
        {
            List<string> folders = new List<string>
        {
            "Animations",
            "Audio/Diegetic",
            "Audio/Non Diegetic",
            "Data",
            "Editor",
            "Materials",
            "Meshes",
            "Prefabs",
            "Scripts",
            "Scenes/Start",
             "Scenes/InProgress",
              "Scenes/Completed",
            "Shaders",
            "Textures"
        };

            foreach (string folder in folders)
            {
                if (generateMyAssets)
                    folderPath = $"Assets/{projectName}/My Assets/{folder}";
                else
                    folderPath = $"Assets/{projectName}/{folder}";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (generateThirdPartyAssets)
                {
                    folderPath = $"Assets/{projectName}/Third Party Assets/{folder}";

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        private void OnEnable()
        {
            // Set the initial values to true
            generateMyAssets = true;
            generateThirdPartyAssets = true;

            folderPath = $"Assets/{projectName}";
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            // Begin a horizontal layout group for Project Name
            GUILayout.BeginHorizontal();
            GUILayout.Label("Project Name");
            // Add flexible space to push the text field to the right
            GUILayout.FlexibleSpace();
            projectName = EditorGUILayout.TextField(projectName);
            GUILayout.EndHorizontal();

            // Begin a horizontal layout group
            GUILayout.BeginHorizontal();
            // Create a label for the checkbox
            GUILayout.Label("Generate My Assets Folder");
            // Add flexible space to push the text field to the right
            // GUILayout.FlexibleSpace();
            // Checkbox for Third Party Assets
            generateMyAssets = EditorGUILayout.Toggle(generateMyAssets);
            // End the horizontal layout group
            GUILayout.EndHorizontal();

            // Begin a horizontal layout group
            GUILayout.BeginHorizontal();
            // Create a label for the checkbox
            GUILayout.Label("Generate Third Party Folder");
            // Checkbox for Third Party Assets
            generateThirdPartyAssets = EditorGUILayout.Toggle(generateThirdPartyAssets);
            // End the horizontal layout group
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Create Folders"))
                DoCreateAllFolders();
        }

        private void DoCreateAllFolders()
        {
            if (Directory.Exists($"Assets/{projectName}"))
            {
                // Show an error dialog if the project name already exists
                EditorUtility.DisplayDialog("Error", $"Assets/{projectName} already exists!", "OK");
            }
            else
            {
                CreateAllFolders();
                Close();
            }
        }
    }
}