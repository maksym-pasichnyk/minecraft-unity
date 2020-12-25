using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : Screen
{
    [SerializeField] private Button _singleplayer;
    [SerializeField] private Button _multiplayer;
    [SerializeField] private Button _options;
    [SerializeField] private Button _quitGame;

    protected override void Reset()
    {
        _singleplayer.onClick.RemoveAllListeners();
        _multiplayer.onClick.RemoveAllListeners();
        _options.onClick.RemoveAllListeners();
        _quitGame.onClick.RemoveAllListeners();
    }
    
    protected override void Setup()
    {
        _singleplayer.onClick.AddListener(() =>
        {
            Client.Instance.LoadWorld();
        });
        
        _multiplayer.onClick.AddListener(() =>
        {
            Client.Instance.ScreenManager.ShowMultiplayerScreen();
        });
        
        _quitGame.onClick.AddListener(Application.Quit);
    }

    protected override void Tick()
    {
        
    }
}
