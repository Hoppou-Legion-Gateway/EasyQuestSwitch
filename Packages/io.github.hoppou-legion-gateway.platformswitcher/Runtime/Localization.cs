#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlatformSwitcher
{
    public static class Localization
    {
        private static LocalizedLanguage _current;
        public static LocalizedLanguage Current
        {
            set => _current = value;
            get
            {
                if (_current == null)
                {
                    if (!IsLoaded && !LoadLanguages())
                        throw new InvalidOperationException("Platform Switcher localization files have not been imported yet.");
                    // Since 1.2 the language is stored as a code, before that as an index
                    string code = EditorPrefs.GetString(Prefs.Language, string.Empty);
                    if (string.IsNullOrEmpty(code)) SetLanguage(EditorPrefs.GetInt(Prefs.Language, 0));
                    else SetLanguage(code);
                }
                return _current;
            }
        }
        public static LocalizedLanguage[] AvailableLanguages;

        public static bool IsLoaded => AvailableLanguages != null && AvailableLanguages.Length > 0;

        [Serializable]
        public class LocalizedLanguage
        {
            public string Code;
            public string DisplayName;
            public string Author;

            public string SettingsButton;
            public string SettingsLanguage;
            public string SettingsListFormat;
            public string[] SettingsListFormatArray;
            public string SettingsApplyPC;
            public string SettingsApplyMobile;
            public string SettingsRemoveFromScene;
            public string SettingsCacheWarning;
            public string SettingsAssetPipelineV2;
            public string SettingsCacheButton;
            public string SettingsExplanation;
            public string SettingsDebugOptions;
            public string SettingsDebugReveal;
            public string SettingsFeedback;
            public string SettingsGithub;
            public string SettingsTwitter;
            public string SettingsHierarchy;
            public string SettingsHierarchyIconShow;
            public string SettingsHierarchyIconOffset;
            public string SettingsPromptForPlatformChange;

            public string PopupDeleteWarning;
            public string PopupTargetChanged;
            public string PopupAccept;
            public string PopupDecline;

            public string ListSetupInScene;
            public string ListExpand;
            public string ListFold;
            public string ListDragAndDrop;

            public string LogPrefix = "[Platform Switcher] ";
            public string LogUnsupportedComponent;
            public string LogComponentExists;
            public string LogSwitchMissing;
            public string LogSwitchFailure;
            public string LogSwitchSuccess;
            public string LogSwitchUnsupported;
        }

        public static bool LoadLanguages()
        {
            UnityEngine.Object[] JSONlanguages = Resources.LoadAll("PlatformSwitcher/Localizations", typeof(TextAsset));
            if (JSONlanguages.Length == 0) return false;

            LocalizedLanguage[] languages = new LocalizedLanguage[JSONlanguages.Length];
            for (int i = 0; i < JSONlanguages.Length; i++)
            {
                languages[i] = JsonUtility.FromJson<LocalizedLanguage>(JSONlanguages[i].ToString());
                languages[i].Code = JSONlanguages[i].name;
            }
            AvailableLanguages = languages;
            Array.Sort(AvailableLanguages, (x, y) =>
            {
                if (x.Code == "en") return -1;
                if (y.Code == "en") return 1;
                return x.DisplayName.CompareTo(y.DisplayName);
            });
            Current = AvailableLanguages[0];
            return true;
        }

        public static string[] GetLanguages()
        {
            string[] languagesArray = new string[AvailableLanguages.Length];
            for (int i = 0; i < AvailableLanguages.Length; i++)
            {
                languagesArray[i] = AvailableLanguages[i].DisplayName;
            }
            return languagesArray;
        }

        public static int SetLanguage(string str)
        {
            for (int i = 0; i < AvailableLanguages.Length; i++)
            {
                if (AvailableLanguages[i].Code == str.ToLower())
                {
                    Current = AvailableLanguages[i];
                    return i;
                }
            }
            Current = AvailableLanguages[0];
            return 0;
        }

        public static int SetLanguage(int i) =>
            SetLanguage(i >= 0 && i < AvailableLanguages.Length ? AvailableLanguages[i].Code : "en");


    }
}
#endif