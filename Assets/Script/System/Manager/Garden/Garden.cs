using UnityEngine;

public class Garden
{
    string m_key;
    string m_name;
    string m_contents;
    Sprite m_icon;
    Sprite m_image;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public string contents { get { return m_contents; } }
    public Sprite icon { get { return m_icon; } }
    public Sprite image { get { return m_image; } }

    public Garden(string key, string name, string contents, Sprite icon, Sprite image)
    {
        m_key = key;
        m_name = name;
        m_contents = contents;
        m_icon = icon;
        m_image = image;
    }



}

