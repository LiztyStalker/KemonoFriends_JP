using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_BLOCK {NONE, BLOCK, COIN, SANDSTAR, CERULEAN, WALL, BALL, FRIEND}
public enum TYPE_VALUE { 
    VALUE_00, 
    VALUE_01, 
    VALUE_02, 
    VALUE_03, 
    VALUE_04, 
    VALUE_05, 
    VALUE_06, 
    VALUE_07, 
    VALUE_08, 
    VALUE_09, 
    VALUE_10, 
    VALUE_11, 
    VALUE_12, 
    VALUE_13, 
    VALUE_14, 
    VALUE_15
}

public class Block {

//    Block m_innerBlock = null;

    string m_key;

    Sprite m_icon;

//    [SerializeField]
//    int m_score;

    TYPE_BLOCK m_typeBlock;
    TYPE_VALUE m_typeValue;
    
    //대체 예정
    //Hashtable m_loationHash = new Hashtable();
    Dictionary<string, bool> m_locationFlag;


    int m_weight;
    int m_score;
    int m_health;

//    int m_maxHealth;
//    int m_nowHealth;

    public Sprite icon { get { return m_icon; } }
    public int score { get { return m_score; } }
    public TYPE_BLOCK typeBlock { get { return m_typeBlock; } }
    public TYPE_VALUE typeValue { get { return m_typeValue; } }
    public int weight { get { return m_weight; } }
    public int health { get { return m_health; } }

    public Block(
        string key, 
        Sprite icon, 
        TYPE_BLOCK typeBlock, 
        TYPE_VALUE typeValue, 
        Dictionary<string, bool> locationFlag, 
        int weight, 
        int score, 
        int health
        )
    {
        m_key = key;
        m_icon = icon;
        m_typeBlock = typeBlock;
        m_typeValue = typeValue;
        m_locationFlag = new Dictionary<string, bool>(locationFlag);
        m_score = score;
        m_weight = weight;
        m_health = health;
    }


    public bool isCheckLocation(string key)
    {
        if (m_locationFlag.ContainsKey(key))
        {
            return m_locationFlag[key];
        }
        return false;
    }

    public ParticleSystem getParticle()
    {
        switch (typeBlock)
        {
            case TYPE_BLOCK.BALL:
                return getParticle("Particle@CeruleanDead");
            case TYPE_BLOCK.COIN:
                return getParticle("Particle@Coin");
            case TYPE_BLOCK.CERULEAN:
                return getParticle("Particle@CeruleanDead");
            case TYPE_BLOCK.BLOCK:
                return getParticle("Particle@Eat_00");
            case TYPE_BLOCK.SANDSTAR:
                switch (typeValue)
                {
                    case TYPE_VALUE.VALUE_00:
                        return getParticle("Particle@SandStar_00");
                    case TYPE_VALUE.VALUE_01:
                        return getParticle("Particle@SandStar_01");
                    case TYPE_VALUE.VALUE_02:
                        return getParticle("Particle@Horizental");
                    case TYPE_VALUE.VALUE_03:
                        return getParticle("Particle@Vertical");
                    case TYPE_VALUE.VALUE_04:
                        return getParticle("Particle@Cross");
                    case TYPE_VALUE.VALUE_06:
                        return getParticle("Particle@SandStar_02");
                    default:
                        return getParticle("Particle@SandStar");
                }
            default:
                return null;
        }





    }

    public ParticleSystem getParticle(string key)
    {
        return ParticleManager.GetInstance.getParticle(key);
    }



}
