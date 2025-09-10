using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnPause;
    public event EventHandler OnResume;

    public static int TotalScore => totalScore;
    public static int LevelIndex => levelIndex;

    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private static int levelIndex = 1;
    private static int totalScore = 0;

    private int score = 0;
    private float time = 0f;
    private bool isRunning = false;

    public static void ResetStaticData()
    {
        levelIndex = 1;
        totalScore = 0;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        LoadCurrentLevel();
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
        InputManager.Instance.OnPauseAction += (object sender, System.EventArgs e) =>
        {
            TogglePause();
        };
    }

    private void TogglePause()
    {
        if (Time.timeScale == 0f)
            ResumeGame();
        else
            PauseGame();
    }

    private void Update()
    {
        if (isRunning)
            time += Time.deltaTime;
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel newLevel = Instantiate(gameLevel);
        Lander.Instance.transform.position = newLevel.LanderStartTransform.position;
        cinemachineCamera.Target.TrackingTarget = newLevel.CameraStartTargetTransform;
        CinemachineCamZoom2D.Instance.targetOrthoSize = newLevel.ZoomOutOrthoSize;

    }
    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevelList)
        {
            if (gameLevel.LevelIndex == levelIndex)
            {
                return gameLevel;
            }
        }

        return null;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isRunning = e.state == Lander.State.Normal;
        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCamZoom2D.Instance.SetNormalOrthoSize();
        }
    }


    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        score += e.score;
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        score += 500;
    }

    public int GetScore() => score;
    public float GetTime() => time;

    public void GoNextLevel()
    {
        totalScore += score;
        levelIndex++;
        
        if(GetGameLevel() == null)
        {
            //no more levels
            SceneLoader.LoadScene(Scene.GameOverScene);
            return;
        }
        else
        {
            SceneLoader.LoadScene(Scene.GameScene);
        }
    }

    public void RestartLevel()
    {
        LoadCurrentLevel();
        SceneLoader.LoadScene(Scene.GameScene);
    }

    public void PauseGame()
    {
        OnPause?.Invoke(this, EventArgs.Empty);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        OnResume?.Invoke(this, EventArgs.Empty);
        Time.timeScale = 1f;
    }

}
