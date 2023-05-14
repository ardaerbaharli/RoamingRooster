namespace Utilities
{
        public static class Config
        {
                public static string ScorePref = "Score";
                public static string CoinPref = "Coins";
                public static string TopScorePref = "TopScore";
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