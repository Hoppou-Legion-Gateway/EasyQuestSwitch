#if UNITY_EDITOR
using UnityEditor;

namespace PlatformSwitcher
{
    // EditorPrefs are shared by every project, so keys keep a product prefix
    public static class Prefs
    {
        private const string Prefix = "PlatformSwitcher.";
        // Prefix used before the rename
        private const string LegacyPrefix = "EQS_";

        public const string Language = Prefix + "Language";
        public const string ListFormat = Prefix + "ListFormat";
        public const string HierarchySideOffset = Prefix + "HierarchySideOffset";
        public const string ShowHierarchyIcon = Prefix + "ShowHierarchyIcon";
        public const string PromptForPlatformChange = Prefix + "PromptForPlatformChange";

        // Copy settings saved under legacy keys; legacy keys are left untouched
        [InitializeOnLoadMethod]
        private static void MigrateLegacy()
        {
            string legacyLanguage = Legacy(Language);
            if (NeedsMigration(Language))
            {
                // Before 1.2 the language was stored as an index, afterwards as a code
                string code = EditorPrefs.GetString(legacyLanguage, string.Empty);
                if (!string.IsNullOrEmpty(code)) EditorPrefs.SetString(Language, code);
                else EditorPrefs.SetInt(Language, EditorPrefs.GetInt(legacyLanguage, 0));
            }
            if (NeedsMigration(ListFormat))
                EditorPrefs.SetInt(ListFormat, EditorPrefs.GetInt(Legacy(ListFormat), 0));
            if (NeedsMigration(HierarchySideOffset))
                EditorPrefs.SetFloat(HierarchySideOffset, EditorPrefs.GetFloat(Legacy(HierarchySideOffset), 0f));
            if (NeedsMigration(ShowHierarchyIcon))
                EditorPrefs.SetBool(ShowHierarchyIcon, EditorPrefs.GetBool(Legacy(ShowHierarchyIcon), true));
            if (NeedsMigration(PromptForPlatformChange))
                EditorPrefs.SetBool(PromptForPlatformChange, EditorPrefs.GetBool(Legacy(PromptForPlatformChange), true));
        }

        private static string Legacy(string key) => LegacyPrefix + key.Substring(Prefix.Length);

        private static bool NeedsMigration(string key) =>
            !EditorPrefs.HasKey(key) && EditorPrefs.HasKey(Legacy(key));
    }
}
#endif
