using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerScreen : Screen
{
    [SerializeField] private Button _joinServer;
    [SerializeField] private Button _directConnection;
    [SerializeField] private Button _addServer;
    [SerializeField] private Button _edit;
    [SerializeField] private Button _delete;
    [SerializeField] private Button _refresh;
    [SerializeField] private Button _cancel;

    private float _time = 0;
    private LanServerFinder _lanServerFinder;
    private LanServerList _lanServerList;
    private List<LanServerInfo> _servers = new List<LanServerInfo>(10);

    [SerializeField] private Text _progress;
    [SerializeField] private InfiniteScroll _serverList;

    protected override void Reset()
    {
        _lanServerFinder.Stop();
        _serverList.Clear();
        _servers.Clear();
        
        _joinServer.onClick.RemoveAllListeners();
        _directConnection.onClick.RemoveAllListeners();
        _addServer.onClick.RemoveAllListeners();
        _edit.onClick.RemoveAllListeners();
        _delete.onClick.RemoveAllListeners();
        _refresh.onClick.RemoveAllListeners();
        _cancel.onClick.RemoveAllListeners();
    }

    protected override void Setup()
    {
        _time = 0;
        _serverList.FillItem = FillServerInfo;

        _lanServerList = new LanServerList();
        _lanServerFinder = new LanServerFinder(_lanServerList);
        
        _cancel.onClick.AddListener(() =>
        {
            Client.Instance.ScreenManager.ShowMainMenu();
        });
    }

    
    protected override void Tick()
    {
        if (_lanServerList.WantUpdate)
        {
            _lanServerList.WantUpdate = false;
            
            _servers.Clear();
            _servers.AddRange(_lanServerList.GetServerList());
            
            _serverList.Resize(_servers.Count);
        }
        
        switch ((int)(_time * 3 % 4))
        {
            case 0:
                _progress.text = "O o o";
                break;
            case 1:
            case 3:
                _progress.text = "o O o";
                break;
            case 2:
                _progress.text = "o o O";
                break;
            default:
                _progress.text = "O o o";
                break;
        }
        
        _time += Time.deltaTime;
    }

    private void FillServerInfo(int index, GameObject go)
    {
        var server = _servers[index];
        
        var info = go.GetComponent<ServerInfoPrefab>();
        
        info.Init("LAN world", server.Description, $"{server.Address}:{server.Port}");
    }

    private void OnApplicationQuit()
    {
        _lanServerFinder.Stop();
    }
}
