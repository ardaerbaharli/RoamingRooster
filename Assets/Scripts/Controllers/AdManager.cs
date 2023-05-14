using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    [SerializeField] private GameObject adUI;
    [SerializeField] private Button closeButton;

    public static AdManager Instance;
    
    public int GamesPlayed
    {
        get => PlayerPrefs.GetInt("GamesPlayed", 0);
        set
        {
            PlayerPrefs.SetInt("GamesPlayed", value);
            if (value % 3 == 0)
            {
                ShowAd();
                PlayerPrefs.SetInt("GamesPlayed", 0);
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        closeButton.onClick.AddListener(CloseAd);
    }
    private void CloseAd()
    {
        adUI.SetActive(false);
    }
    public void ShowAd()
    {
        adUI.SetActive(true);
    }
    public void RemoveAds()
    {
       //TODO remove ads
    }
}
