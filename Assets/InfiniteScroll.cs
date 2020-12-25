using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    private readonly List<GameObject> _gameObjects = new List<GameObject>(10);
    
    [SerializeField] private GameObject _item;

    private ScrollRect _scrollRect;
    private Transform _content;
    private int _count;

    public Action<int, GameObject> FillItem = null;
    
    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _content = _scrollRect.content.transform;
    }

    public void Resize(int count)
    {
        _count = count;
        Refresh();
    }

    public void Clear()
    {
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            _gameObjects[i].SetActive(false);
        }
    }
    
    public void Refresh()
    {
        Clear();
        
        for (int i = 0; i < _count; i++)
        {
            FillItem(i, GetItem());
        }
        
        // for (int i = _count; i < _gameObjects.Count; i++)
        // {
        //     _gameObjects[i].SetActive(false);
        // }
    }

    private GameObject GetItem()
    {
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            if (_gameObjects[i].activeInHierarchy) continue;
            
            _gameObjects[i].SetActive(true);
            return _gameObjects[i];
        }

        return AddItem();
    }

    private GameObject AddItem()
    {
        var go = Instantiate(_item, _content);
        _gameObjects.Add(go);
        return go;
    }
}