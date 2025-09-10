using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTM;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        scoreTM.text = $"FINAL SCORE: {GameManager.TotalScore}";
    }
} 
