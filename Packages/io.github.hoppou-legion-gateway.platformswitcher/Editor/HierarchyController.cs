using UnityEditor;
using UnityEngine;

namespace PlatformSwitcher
{
    public static class HierarchyController
    {
        private static GUIStyle entryStyle;
        private static Data data;
        private static bool initialized;
        private static Texture2D logo;
        private static float sideOffset = 0f;
        private static bool showHierarchyIcon = true;

        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
        }

        public static void InitializeHierarchy()
        {
            if (logo == null)
            {
                logo = (Texture2D)Resources.Load("PlatformSwitcher/Logo_Crop", typeof(Texture2D));
            }
            if (data == null)
            {
                data = Data.FindInScene();
            }

            sideOffset = EditorPrefs.GetFloat(Prefs.HierarchySideOffset, 0f);
            showHierarchyIcon = EditorPrefs.GetBool(Prefs.ShowHierarchyIcon, true);
            if (!showHierarchyIcon)
            {
                EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItemGUI;
            }
            else if (initialized)
            { // only re-add the callback if the settings were edited
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
            }

            initialized = true;
        }


        private static void OnHierarchyItemGUI(int id, Rect rect)
        {
            var instance = EditorUtility.InstanceIDToObject(id) as GameObject;
            if (instance == null) return;

            if (!initialized)
            {
                InitializeHierarchy();
            }

            if (!showHierarchyIcon)
            {
                return;
            }

            if (entryStyle == null)
            {
                entryStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleRight,
                    fixedHeight = 18,
                    fixedWidth = 18
                };
            }

            if (data == null || data.Objects == null) return;

            var targetIndex = data.Objects.FindIndex(i =>
                i.Target != null && ((i.Target as Component)?.gameObject.GetHashCode() == instance.GetHashCode() ||
                                     (i.Target as GameObject)?.GetHashCode() == instance.GetHashCode()));

            if (targetIndex == -1)
            {
                return;
            }

            var newRect = rect;
            newRect.xMax += sideOffset;
            newRect.xMin = newRect.xMax - 18;
            EditorGUI.LabelField(newRect, new GUIContent(logo), entryStyle);
            var evt = Event.current;
            if (evt.type == EventType.MouseUp && newRect.Contains(evt.mousePosition))
            {
                EditorWindow.GetWindow<Window>(false, "Platform Switcher");
                var tSo = new SerializedObject(data);
                tSo.FindProperty("Objects").GetArrayElementAtIndex(targetIndex).FindPropertyRelative("Foldout").boolValue =
                  true;
                tSo.ApplyModifiedProperties();
            }
        }
    }
}
