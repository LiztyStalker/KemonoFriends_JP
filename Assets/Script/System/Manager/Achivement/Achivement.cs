using System;
using UnityEngine;

//1.5
public class Achivement
{

    public enum TYPE_ACHIVEMENT {RUN, FRIENDS, CERULEAN, JAPARIBREAD, COIN, SANDSTAR, ABILITY, TOTAL_ABILITY}

    string m_key;

    Sprite m_icon;

    TYPE_ACHIVEMENT m_typeAchivment;
    int m_value;

    string m_name;
    string m_contents;
    int m_award;



    public string key { get { return m_key; } }
    public Sprite icon { get { return m_icon; } }
    public TYPE_ACHIVEMENT typeAchivement { get { return m_typeAchivment; } }
    public int value { get { return m_value; } }
    public int award { get { return m_award; } }
    public string nameText { get { return m_name; } }
    public string contents { get { return m_contents; } }



    public Achivement(
        string key, 
        Sprite icon, 
        TYPE_ACHIVEMENT typeAchivment,
        int value,
        string name,
        string contents,
        int award
        )
    {
        m_key = key;
        m_icon = icon;
        m_typeAchivment = typeAchivment;
        m_value = value;
        m_name = name;
        m_contents = contents;
        m_award = award;
    }



}

