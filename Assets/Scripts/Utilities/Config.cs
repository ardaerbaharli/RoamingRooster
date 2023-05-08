using UnityEngine;

namespace Utilities
{
        public static class Config
        {
                public static string ScorePref = "Score";
                public static string CoinPref = "Coins";
                public static string ActiveLanguage
                {
                        get => PlayerPrefs.GetString("ActiveLanguage", "en");
                        set => PlayerPrefs.SetString("ActiveLanguage", value);
                }
                public static bool IsSoundOn
                {
                        get => PlayerPrefsX.GetBool("IsVolumeOn", true);
                        set => PlayerPrefsX.SetBool("IsVolumeOn", value);
                }
                public static bool IsVibrationOn
                {
                        get => PlayerPrefsX.GetBool("IsVibrationOn", true);
                        set => PlayerPrefsX.SetBool("IsVibrationOn", value);
                }
        }
}