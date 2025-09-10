using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrow;
    [SerializeField] private GameObject speedDownArrow;
    [SerializeField] private GameObject speedLeftArrow;
    [SerializeField] private GameObject speedRightArrow;
    [SerializeField] private Image fuelBarFillImage;

    private void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        speedUpArrow.SetActive(Lander.Instance.GetY_Speed() > 0);
        speedDownArrow.SetActive(Lander.Instance.GetY_Speed() <= 0);
        speedLeftArrow.SetActive(Lander.Instance.GetX_Speed() < 0);
        speedRightArrow.SetActive(Lander.Instance.GetX_Speed() > 0);

        statsTextMesh.text =
            GameManager.LevelIndex + "\n" +
            GameManager.Instance.GetScore() + "\n" +
            GameManager.Instance.GetTime().ToString("F1") + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetX_Speed() * 10)) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetY_Speed() * 10)) + "\n";

        fuelBarFillImage.fillAmount = Lander.Instance.GetNormalizedFuel();
    
        
    }

}
