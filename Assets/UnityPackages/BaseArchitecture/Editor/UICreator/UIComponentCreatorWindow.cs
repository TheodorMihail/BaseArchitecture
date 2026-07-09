using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BaseArchitecture.Editor
{
    /// <summary>
    /// Editor window that scaffolds the Model/View/Controller/Screen-or-HUD scripts
    /// for a new UI component, so the four files don't have to be hand-authored each time.
    /// </summary>
    public class UIComponentCreatorWindow : EditorWindow
    {
        private UIComponentType _componentType = UIComponentType.Screen;
        private string _componentName = "";
        private string _folderPath = "Assets";
        private string _namespaceName = "Game";
        private bool _namespaceEditedManually;

        [MenuItem("BaseArchitecture/Create UI Component...")]
        private static void Open()
        {
            var window = GetWindow<UIComponentCreatorWindow>(true, "Create UI Component");
            window.minSize = new Vector2(440, 210);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("UI Component Creator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _componentType = (UIComponentType)EditorGUILayout.EnumPopup("Type", _componentType);
            _componentName = EditorGUILayout.TextField("Name", _componentName);

            EditorGUILayout.BeginHorizontal();
            _folderPath = EditorGUILayout.TextField("Folder", _folderPath);
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                BrowseForFolder();
            }
            EditorGUILayout.EndHorizontal();

            if (!_namespaceEditedManually)
            {
                _namespaceName = SuggestNamespace(_folderPath);
            }

            EditorGUI.BeginChangeCheck();
            var editedNamespace = EditorGUILayout.TextField("Namespace", _namespaceName);
            if (EditorGUI.EndChangeCheck())
            {
                _namespaceName = editedNamespace;
                _namespaceEditedManually = true;
            }

            EditorGUILayout.Space();

            var validationError = Validate();
            if (!string.IsNullOrEmpty(validationError))
            {
                EditorGUILayout.HelpBox(validationError, MessageType.Warning);
            }

            using (new EditorGUI.DisabledScope(!string.IsNullOrEmpty(validationError)))
            {
                if (GUILayout.Button("Create", GUILayout.Height(28)))
                {
                    Create();
                }
            }
        }

        private void BrowseForFolder()
        {
            var absolutePath = EditorUtility.OpenFolderPanel("Select Destination Folder", _folderPath, "");
            if (string.IsNullOrEmpty(absolutePath)) return;

            var dataPath = Application.dataPath;
            if (!absolutePath.StartsWith(dataPath))
            {
                EditorUtility.DisplayDialog("Invalid Folder", "Please choose a folder inside this project's Assets folder.", "OK");
                return;
            }

            _folderPath = "Assets" + absolutePath.Substring(dataPath.Length).Replace('\\', '/');
            if (!_namespaceEditedManually)
            {
                _namespaceName = SuggestNamespace(_folderPath);
            }
        }

        private static string SuggestNamespace(string folderPath)
        {
            var segments = folderPath.Replace('\\', '/').Split('/');
            var parts = new List<string>();

            foreach (var segment in segments)
            {
                if (string.IsNullOrEmpty(segment) || segment == "Assets") continue;
                parts.Add(ToPascalCase(segment));
            }

            return parts.Count > 0 ? string.Join(".", parts) : "Game";
        }

        private static string ToPascalCase(string value)
        {
            var cleaned = Regex.Replace(value, "[^A-Za-z0-9]", "");
            if (string.IsNullOrEmpty(cleaned)) return "Game";
            return char.ToUpperInvariant(cleaned[0]) + cleaned.Substring(1);
        }

        private string Validate()
        {
            if (string.IsNullOrWhiteSpace(_componentName))
                return "Name is required.";

            if (!Regex.IsMatch(_componentName, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                return "Name must be a valid identifier (letters, digits, underscore; can't start with a digit).";

            if (string.IsNullOrWhiteSpace(_folderPath) || !_folderPath.StartsWith("Assets"))
                return "Folder must be inside Assets.";

            if (string.IsNullOrWhiteSpace(_namespaceName) ||
                !Regex.IsMatch(_namespaceName, @"^[A-Za-z_][A-Za-z0-9_]*(\.[A-Za-z_][A-Za-z0-9_]*)*$"))
                return "Namespace must be a valid dotted identifier.";

            var typeSuffix = _componentType == UIComponentType.Screen ? "Screen" : "HUD";
            var targetFile = Path.Combine(_folderPath, _componentName, $"{_componentName}{typeSuffix}.cs");
            if (File.Exists(targetFile))
                return $"{_componentName}{typeSuffix} already exists at that location.";

            return null;
        }

        private void Create()
        {
            UIComponentGenerator.Generate(_componentName, _componentType, _folderPath, _namespaceName);

            var typeSuffix = _componentType == UIComponentType.Screen ? "Screen" : "HUD";
            EditorUtility.DisplayDialog(
                "UI Component Created",
                $"{_componentName}{typeSuffix} created at {_folderPath}/{_componentName}",
                "OK");

            Close();
        }
    }
}
