#if UNITY_EDITOR
using UnityEditor;

namespace PlatformSwitcher
{
    // Maps build targets onto the two value sets every field stores (PC and Mobile)
    public static class Platform
    {
        public static bool IsPC(BuildTarget target) => target == BuildTarget.StandaloneWindows64;

        // Android (Quest and phones) and iOS share the Mobile values
        public static bool IsMobile(BuildTarget target) =>
            target == BuildTarget.Android || target == BuildTarget.iOS;

        public static bool IsSupported(BuildTarget target) => IsPC(target) || IsMobile(target);

        // True when switching between the targets would apply the same values
        public static bool SharesValues(BuildTarget a, BuildTarget b) =>
            a == b || (IsPC(a) && IsPC(b)) || (IsMobile(a) && IsMobile(b));

        // Active target if it is mobile, Android otherwise
        public static BuildTarget CurrentOrDefaultMobile()
        {
            BuildTarget active = EditorUserBuildSettings.activeBuildTarget;
            return IsMobile(active) ? active : BuildTarget.Android;
        }
    }
}
#endif
