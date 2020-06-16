﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    GameManager GM;
    public static PauseManager pauseManager;
    private Player player;
    public static bool IsGamePaused = false;
    [SerializeField] string sceneToLoad;
    [SerializeField] private GameObject pauseMenuUI;
    private void Start()
    {
        pauseManager = this;
        GM = GameManager.instance;
    }
    public void PauseButtonPressed()
    {
        if (IsGamePaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale=0f;
        IsGamePaused = true;
    }
    public void LoadMenu()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
        GM.RestoreCheckpointStart();
        SceneManager.LoadScene(sceneToLoad);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
