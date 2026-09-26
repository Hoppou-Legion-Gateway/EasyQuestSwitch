#if UNITY_EDITOR
namespace PlatformSwitcher
{
    // EditorPrefs are shared by every project, so keys keep a product prefix
    public static class Prefs
    {
        private const string Prefix = "PlatformSwitcher.";

        public const string Language = Prefix + "Language";
        public const string ListFormat = Prefix + "ListFormat";
        public const string HierarchySideOffset = Prefix + "HierarchySideOffset";
        public const string ShowHierarchyIcon = Prefix + "ShowHierarchyIcon";
        public const string PromptForPlatformChange = Prefix + "PromptForPlatformChange";
    }
}
#endif
