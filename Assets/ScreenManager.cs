using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private Screen _mainMenuScreen;
    [SerializeField] private Screen _multiplayerScreen;

    private IScreen _currentScreen;

    private void SetScreen(IScreen screen)
    {
        _currentScreen?.Hide();
        _currentScreen = screen;
        _currentScreen?.Show();
    }

    public void ShowMainMenu() => SetScreen(_mainMenuScreen);

    public void ShowGameMenu() => SetScreen(null);

    public void ShowMultiplayerScreen() => SetScreen(_multiplayerScreen);
}
