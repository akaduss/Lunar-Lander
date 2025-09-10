using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private TextMeshProUGUI soundTM;
    [SerializeField] private TextMeshProUGUI musicTM;

    private void Awake()
    {
        soundButton.onClick.AddListener( () =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundTM.text = "SOUND " + SoundManager.Instance.GetSoundVolume().ToString();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            musicTM.text = "MUSIC " + MusicManager.Instance.GetMusicVolume().ToString();
        });
        resumeButton.onClick.AddListener( () =>
        {
            GameManager.Instance.ResumeGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnResume += GameManager_OnResume;
        
        gameObject.SetActive(false);
    }

    private void GameManager_OnResume(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void GameManager_OnPause(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }


}
