using System.IO;
using UnityEditor;

namespace BaseArchitecture.Editor
{
    public enum UIComponentTypes
    {
        Screen,
        HUD
    }

    /// <summary>
    /// Generates the Model/View/Controller/Screen-or-HUD script skeleton for a new UI component,
    /// following the same layout and base-class conventions as the ErrorDialogScreen sample.
    /// </summary>
    public static class UIComponentGenerator
    {
        public static void Generate(string componentName, UIComponentTypes componentType, string folderPath, string namespaceName)
        {
            var componentFolder = Path.Combine(folderPath, componentName);
            Directory.CreateDirectory(componentFolder);

            var typeSuffix = componentType == UIComponentTypes.Screen ? "Screen" : "HUD";
            var addressableFolder = componentType == UIComponentTypes.Screen ? "Screens" : "HUD";
            var addressablePath = $"{addressableFolder}/{componentName}{typeSuffix}View";

            WriteFile(componentFolder, $"{componentName}Model.cs", BuildModel(componentName, namespaceName));
            WriteFile(componentFolder, $"{componentName}View.cs", BuildView(componentName, namespaceName, addressablePath));
            WriteFile(componentFolder, $"{componentName}Controller.cs", BuildController(componentName, namespaceName, typeSuffix));
            WriteFile(componentFolder, $"{componentName}{typeSuffix}.cs", BuildComponent(componentName, namespaceName, componentType, typeSuffix));

            AssetDatabase.Refresh();
        }

        private static void WriteFile(string folder, string fileName, string content)
        {
            File.WriteAllText(Path.Combine(folder, fileName), content);
        }

        private static string BuildModel(string name, string ns) =>
$@"using BaseArchitecture.Core;

namespace {ns}
{{
    public class {name}Model : Model
    {{
    }}
}}
";

        private static string BuildView(string name, string ns, string addressablePath) =>
$@"using BaseArchitecture.Core;
using UnityEngine;

namespace {ns}
{{
    [AddressablePath(""{addressablePath}"")]
    public class {name}View : View
    {{
    }}
}}
";

        private static string BuildController(string name, string ns, string typeSuffix) =>
$@"using BaseArchitecture.Core;

namespace {ns}
{{
    public class {name}Controller : Controller<{name}{typeSuffix}, {name}Model, {name}View>
    {{
        public {name}Controller({name}{typeSuffix} uiComponent, {name}Model model, {name}View view)
            : base(uiComponent, model, view)
        {{
        }}
    }}
}}
";

        private static string BuildComponent(string name, string ns, UIComponentTypes componentType, string typeSuffix)
        {
            var baseClass = componentType == UIComponentTypes.Screen ? "Screen" : "HUD";

            return
$@"using BaseArchitecture.Core;

namespace {ns}
{{
    public class {name}{typeSuffix} : {baseClass}<{name}Model, {name}View, {name}Controller>
    {{
    }}
}}
";
        }
    }
}
