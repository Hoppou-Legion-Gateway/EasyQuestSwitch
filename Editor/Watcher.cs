using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformSwitcher
{
    public class Watcher : IActiveBuildTargetChanged
    {
        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            Data data = Data.FindInScene();
            data?.OnSceneOpened();
        }

        public int callbackOrder => 1;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            Data data = Data.FindInScene();
            data?.OnChangedBuildTarget(newTarget);
        }
    }
}

