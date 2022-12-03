using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum UIScreen { START_SCREEN, GAME_SCREEN, END_SCREEN }

public class Asteroids_UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject StartScreen;
    [SerializeField]
    private GameObject GameScreen;
    [SerializeField]
    private GameObject EndScreen;

    public event Action<UIScreen> OnScreenChanged;
    public UIScreen ScreenActive { get; private set; }
    public static Asteroids_UIManager Instance;

    private void Awake() => Instance = this;

    private void Start() => Asteroids_GameManager.Instance.OnGameOver += () => { ChangeScreen(UIScreen.END_SCREEN); };

    public void ChangeScreen(UIScreen newScreen)
    {
        ScreenActive = newScreen;

        switch (ScreenActive)
        {
            case UIScreen.START_SCREEN:
                StartScreen.SetActive(true);
                GameScreen.SetActive(false);
                EndScreen.SetActive(false);
                break;

            case UIScreen.GAME_SCREEN:
                StartScreen.SetActive(false);
                GameScreen.SetActive(true);
                EndScreen.SetActive(false);
                break;

            case UIScreen.END_SCREEN:
                StartScreen.SetActive(false);
                GameScreen.SetActive(false);
                EndScreen.SetActive(true);
                break;

            default:
                break;
        }

        OnScreenChanged?.Invoke(ScreenActive);
    }
}
