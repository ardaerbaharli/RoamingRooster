using System;
using System.Globalization;
using Controllers;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public enum AdType
{
    Rewarded,
    Interstitial
}

public class AdManager : MonoBehaviour
{
    [SerializeField] private GameObject adUI;
    [SerializeField] private Button closeButton;

    public bool IsAdsEnabled;
    public Action OnAdClosed, OnRewardedAdClosed;

    private AdType currentAdType;
    public static AdManager Instance;

    public int GamesPlayed
    {
        get => PlayerPrefs.GetInt("GamesPlayed", 0);
        set
        {
            PlayerPrefs.SetInt("GamesPlayed", value);
            if (value % 3 == 0)
            {
                ShowAd(AdType.Interstitial);
                PlayerPrefs.SetInt("GamesPlayed", 0);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        IsAdsEnabled = PlayerPrefsX.GetBool("IsAdsEnabled", true);
    }

    private void Start()
    {
        closeButton.onClick.AddListener(CloseAd);
    }

    private void CloseAd()
    {
        adUI.SetActive(false);
        GameManager.instance.SetGameState(GameManager.instance.PreviousState);
        if (currentAdType == AdType.Rewarded)
            OnRewardedAdClosed?.Invoke();
        else
            OnAdClosed?.Invoke();

        OnRewardedAdClosed = null;
        OnAdClosed = null;
    }

    public void ShowAd(AdType adType)
    {
        if (!IsAdsEnabled)
        {
            if (adType != AdType.Rewarded)
                return;
        }

        currentAdType = adType;

        adUI.SetActive(true);
        GameManager.instance.SetGameState(GameState.WatchingAd);
    }

    public void RemoveAds()
    {
        IsAdsEnabled = false;
        PlayerPrefsX.SetBool("IsAdsEnabled", false);
    }
}