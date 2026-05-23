#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateNewFolders : EditorWindow
{
    string rootFolderName = "NewProject";

    [MenuItem("Tools/Create New Folder Structure")]
    static void Init()
    {
        CreateNewFolders window =
            (CreateNewFolders)GetWindow(typeof(CreateNewFolders));

        window.titleContent = new GUIContent("Folder Creator");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Unity Folder Structure Generator", EditorStyles.boldLabel);

        rootFolderName =
            EditorGUILayout.TextField("Root Folder Name", rootFolderName);

        GUILayout.Space(10);

        if (GUILayout.Button("Create Folders"))
        {
            CreateFolderStructure(rootFolderName);
        }
    }

    void CreateFolderStructure(string rootFolderName)
    {
        string basePath = Path.Combine("Assets", rootFolderName);

        string[] folders =
        {
            // Assets
            "Art",
            "Art/Materials",
            "Art/Models",
            "Art/Textures",

            "Audio",
            "Audio/Music",
            "Audio/Sound",

            "Code",
            "Code/Scripts",
            "Code/Shaders",

            "Docs",

            "Level",
            "Level/Prefabs",
            "Level/Scenes",
            "Level/UI"
        };

        foreach (string folder in folders)
        {
            string path = Path.Combine(basePath, folder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log($"Created Folder : {path}");
            }
        }

        AssetDatabase.Refresh();

        Debug.Log("Folder structure creation completed.");
    }
}
#endif