using System;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private MainMenuScreen _mainMenuScreen;
    [SerializeField] private MultiplayerScreen _multiplayerScreen;

    private IScreen _currentScreen;

    private void SetScreen(IScreen screen)
    {
        _currentScreen?.Hide();
        _currentScreen = screen;
        _currentScreen?.Show();
    }

    public void ShowMainMenu()
    {
        SetScreen(_mainMenuScreen);
    }

    public void ShowGameMenu()
    {
        SetScreen(null);
    }

    public void ShowMultiplayerScreen()
    {
        SetScreen(_multiplayerScreen);
    }

    private void Update()
    {
        // if (_currentScreen == null)
        // {
        //     SetScreen(_mainMenuScreen);
        // }
    }
}
