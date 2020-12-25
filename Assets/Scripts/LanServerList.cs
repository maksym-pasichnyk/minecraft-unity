using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using UnityEngine;

public class LanServerList
{
    private List<LanServerInfo> _servers = new List<LanServerInfo>(10);
    public bool WantUpdate;
    
    public void AddServer(string response, IPAddress address)
    {
        var port = ParsePort(response);
        
        for (int i = 0; i < _servers.Count; i++)
        {
            if (_servers[i].Port == port)
            {
                return;
            }
        }
        
        var description = ParseMotd(response);
        
        _servers.Add(new LanServerInfo
        {
            Description = description,
            Address = address,
            Port = port
        });
        
        WantUpdate = true;
    }

    private static int ParsePort(string response)
    {
        var i1 = response.IndexOf("[AD]");
        var i2 = response.IndexOf("[/AD]");

        if (i1 != -1 && i2 != -1)
        {
            return int.Parse(response.Substring(i1 + 4, i2 - (i1 + 4)));
        }
        
        return 0;
    }

    private static string ParseMotd(string response)
    {
        var i1 = response.IndexOf("[MOTD]");
        var i2 = response.IndexOf("[/MOTD]");

        if (i1 != -1 && i2 != -1)
        {
            return response.Substring(i1 + 6, i2 - (i1 + 6));
        }
        
        return "";
    }

    public LanServerInfo[] GetServerList()
    {
        return _servers.ToArray();
    }
}
