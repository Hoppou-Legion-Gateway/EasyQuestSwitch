#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PlatformSwitcher.Fields;

namespace PlatformSwitcher.Types
{
    [AddComponentMenu("")]
    public class Type_Material : Type_Base
    {
        [System.NonSerialized]
        private Material type;

        public SharedShader Shader = new SharedShader();
        public SharedColor MainColor = new SharedColor();
        [InspectorName("GPU Instancing")]
        public SharedBool GPUInstancing = new SharedBool();
        private SharedString ShaderPath = new SharedString();

        public override void Setup(Object type)
        {
            Material material = (Material)type;
            Shader.Setup(material.shader);
            ShaderPath.Setup(material.shader.name);
            MainColor.Setup(material.color);
            GPUInstancing.Setup(material.enableInstancing);
        }

        public override void Setup(Object type, int currentVersion)
        {
            Material material = (Material)type;
            if (currentVersion == 0) // 0 -> 131 upgrade
            {
                MainColor.Setup(material.color);
                GPUInstancing.Setup(material.enableInstancing);
            }
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            Material material = (Material)type;
            if (Shader.Get(buildTarget) != null)
            {
                if (Platform.IsPC(buildTarget)) ShaderPath.PC = Shader.Get(buildTarget).name;
                else if (Platform.IsMobile(buildTarget)) ShaderPath.Mobile = Shader.Get(buildTarget).name;
            }
            else if (Shader.Get(buildTarget) == null && !string.IsNullOrEmpty(ShaderPath.Get(buildTarget)))
            {
                if (Platform.IsPC(buildTarget)) Shader.PC = UnityEngine.Shader.Find(ShaderPath.PC);
                else if (Platform.IsMobile(buildTarget)) Shader.Mobile = UnityEngine.Shader.Find(ShaderPath.Mobile);
            }

            if (Shader.Get(buildTarget) == null)
            {
                throw new MissingReferenceException();
            }
            material.shader = Shader.Get(buildTarget);
            material.color = MainColor.Get(buildTarget);
            material.enableInstancing = GPUInstancing.Get(buildTarget);
        }
    }
}
#endif

