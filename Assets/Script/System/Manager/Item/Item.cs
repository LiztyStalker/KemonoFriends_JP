using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item{

    string m_key;
    Sprite m_icon;
//    string m_location;
    string m_itemName;
    string m_contents;
    int m_cost;
    int m_value;
    bool m_isUse;

    Dictionary<string, bool> m_locationFlag;


    public string key { get { return m_key; } }
//    public string location { get { return m_location; } }
    public string itemName { get { return m_itemName; } }
    public Sprite icon { get { return m_icon; } }
    public string contents { get { return m_contents; } }
    public int cost { get { return m_cost; } }
    public int value { get { return m_value; } }
    public bool isUse { get { return m_isUse; } }

    public Item(string key, Sprite icon, string itemName, Dictionary<string, bool> locationFlag, string contents, int cost, int value, bool isUse)
    {
        m_key = key;
        m_itemName = itemName;
        m_locationFlag = locationFlag;
        m_icon = icon;
        m_contents = contents;
        m_cost = cost;
        m_value = value;
        m_isUse = isUse;
    }

    public bool isCheckLocation(string key)
    {
        if (m_locationFlag.ContainsKey(key))
        {
            return m_locationFlag[key];
        }
        return false;
    }

}
