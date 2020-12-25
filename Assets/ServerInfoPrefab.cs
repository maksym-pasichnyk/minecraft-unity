using UnityEngine;
using UnityEngine.UI;

public class ServerInfoPrefab : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Text _address;

    public void Init(string title, string description, string address)
    {
        _title.text = title;
        _description.text = description;
        _address.text = address;
    }
}
