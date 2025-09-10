using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTM;
    [SerializeField] private TextMeshProUGUI statsTM;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI nextButtonTM;

    private Action nextButtonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => { nextButtonClickAction(); });
    }

    private void Start()
    {
        Lander.Instance.OnLanded += Instance_OnLanded;

        gameObject.SetActive(false);
    }

    private void Instance_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            titleTM.text = "SUCCESSFUL LANDING!";
            nextButtonTM.text = "NEXT";
            nextButtonClickAction = GameManager.Instance.GoNextLevel;
        }
        else
        {
            titleTM.text = "<color=#ff0000>CRASH!</color>";
            nextButtonClickAction = GameManager.Instance.RestartLevel;
        }

        statsTM.text =
            Mathf.Round( e.speed) + "\n"
            + Mathf.Round( e.dotProduct) + "\n"
            + "x" + e.scoreMultiplier + "\n"
            + GameManager.Instance.GetScore();

        gameObject.SetActive(true);
        nextButton.Select();

    }

}
