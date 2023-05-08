using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText, proceedButtonText, cancelButtonText;
    [SerializeField] private Image blackout;
    [SerializeField] private GameObject background;

    [SerializeField] private float blackoutDuration = 0.5f;

    public Action OnProceedButtonClicked, OnCancelButtonClicked;

    public void Configure(string description, string proceedButtonText, string cancelButtonText,
        Action onProceedButtonClicked, Action onCancelButtonClicked)
    {
        SetDescriptionText(description);
        SetProceedButtonText(proceedButtonText);
        SetCancelButtonText(cancelButtonText);
        OnProceedButtonClicked = onProceedButtonClicked;
        OnCancelButtonClicked = onCancelButtonClicked;
        StartCoroutine(Enable());
    }

    private IEnumerator Enable()
    {
        // make the blackout color from transparent to black in blackoutDuration seconds and in the middle set the background active
        var t = 0f;
        while (t < blackoutDuration)
        {
            t += Time.deltaTime;
            var a = Mathf.Lerp(0, 1, t / blackoutDuration);
            blackout.color = new Color(0, 0, 0, a);
            if (t > blackoutDuration / 2)
            {
                background.SetActive(true);
            }

            yield return null;
        }
    }

    private IEnumerator Disable()
    {
        // make the blackout color from black to transparent in blackoutDuration seconds and in the middle set the background deactive
        var t = 0f;
        while (t < blackoutDuration)
        {
            t += Time.deltaTime;
            var a = Mathf.Lerp(1, 0, t / blackoutDuration);
            blackout.color = new Color(0, 0, 0, a);
            if (t > blackoutDuration / 2)
            {
                background.SetActive(false);
            }

            yield return null;
        }

    }

    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }

    public void SetProceedButtonText(string text)
    {
        proceedButtonText.text = text;
    }

    public void SetCancelButtonText(string text)
    {
        cancelButtonText.text = text;
    }

    public void ProceedButton()
    {
        OnProceedButtonClicked?.Invoke();
        OnProceedButtonClicked = null;
        StartCoroutine(Disable());
    }

    public void CancelButton()
    {
        OnCancelButtonClicked?.Invoke();
        OnCancelButtonClicked = null;
        StartCoroutine(Disable());
    }
}