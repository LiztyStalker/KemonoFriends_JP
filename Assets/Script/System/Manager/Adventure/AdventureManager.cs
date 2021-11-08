using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AdventureManager : SingletonClass<AdventureManager>
{

    enum TYPE_ADVENTURE_DATA { KEY, NAME, CONTENTS, GOAL, COST, EVENTTIME, BGMKEY}

    Dictionary<string, Adventure> m_adventureDic = new Dictionary<string, Adventure>();

    Adventure[] m_adventureArray = null;

    public int ElementCount { get { return m_adventureDic.Count; } }

    public AdventureManager()
    {
        
        initParse();
    }

    void initParse()
    {


        Sprite[] adventureImage = Resources.LoadAll<Sprite>(PrepClass.adventureImagePath);

        TextAsset textAsset = Resources.Load<TextAsset>(PrepClass.adventureDataPath);

        if (textAsset != null)
        {
            string[] adventureRecord = textAsset.text.Split('\n');

            for (int i = 0; i < adventureRecord.Length; i++)
            {

                string[] adventureWord = adventureRecord[i].Split('\t');
                string key = adventureWord[(int)TYPE_ADVENTURE_DATA.KEY];
                key = key.Trim();


                if (adventureWord.Length == Enum.GetValues(typeof(TYPE_ADVENTURE_DATA)).Length)
                {

                    Sprite[] images = new Sprite[Enum.GetValues(typeof(Adventure.TYPE_ADVENTURE_IMAGE)).Length];

                    for (int j = 0; j < images.Length; j++)
                    {
                        string extKey = string.Format("{0}_{1}", (Adventure.TYPE_ADVENTURE_IMAGE)j, key);
                        Sprite image = adventureImage.Where(advImage => advImage.name == extKey).SingleOrDefault();
                        if (image == null) Debug.LogWarning(string.Format("{0} Adventure 이미지 없음", key));
                        images[j] = image;
                    }

                    int cost;
                    if(!int.TryParse(adventureWord[(int)TYPE_ADVENTURE_DATA.COST], out cost)){
                        cost = 100;
                    }

                    float eventTime;
                    if(!float.TryParse(adventureWord[(int)TYPE_ADVENTURE_DATA.EVENTTIME], out eventTime)){
                        eventTime = 1f;
                    }
                    

                                        
                    Adventure adventure = getAdventure(
                        key,
                        images,
                        adventureWord[(int)TYPE_ADVENTURE_DATA.NAME].Trim(),
                        adventureWord[(int)TYPE_ADVENTURE_DATA.CONTENTS].Trim(),
                        adventureWord[(int)TYPE_ADVENTURE_DATA.GOAL].Trim(),
                        cost,
                        eventTime,
                        adventureWord[(int)TYPE_ADVENTURE_DATA.BGMKEY].Trim()
                        );

                    if (adventure != null)
                        m_adventureDic.Add(key, adventure);
                    else
                        Debug.LogWarning(string.Format("{0} 모험을 등록하지 못했습니다. 객체가 생성되지 않았습니다", key));
                }
                else
                {
                    if(key != "")
                        Debug.LogError(string.Format("{0}의 데이터가 많거나 부족합니다. {1}", key, adventureWord.Length));
                }
            }
        }
        else{
            Debug.LogError(string.Format("{0} 데이터를 찾지 못했음", PrepClass.adventureDataPath));
        }




    
    }




    Adventure getAdventure(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey)
    {
        return new Adventure(key, images, name, contents, goal, cost, eventTime, bgmKey);
        //switch (key)
        //{
        //    case "MaterialHarvest":
        //        return new AdventureHarvest(key, images, name, contents, goal, cost, eventTime, bgmKey);
        //    case "CeruleanRepulse":
        //        return new AdventureRepulse(key, images, name, contents, goal, cost, eventTime, bgmKey);
        //    case "RuinsExplore":
        //        return new AdventureExplore(key, images, name, contents, goal, cost, eventTime, bgmKey);
        //    case "PPPDance":
        //        return new AdventureDance(key, images, name, contents, goal, cost, eventTime, bgmKey);
        //    case "BusDrive":
        //        return new AdventureDrive(key, images, name, contents, goal, cost, eventTime, bgmKey);
        //    default:
        //        Debug.LogError(string.Format("{0} 해당하는 키에 맞는 모험을 찾을 수 없습니다.", key));
        //        break;
        //}
        //return null;
    }

    /// <summary>
    /// 모험 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Adventure getAdventure(int index)
    {
        if (m_adventureDic.Count > 0)
        {
            if (m_adventureArray == null)
                m_adventureArray = m_adventureDic.Values.ToArray<Adventure>();

            if (m_adventureArray.Length > index)
            {
                return m_adventureArray[index];
            }
            else
            {
                Debug.LogError(string.Format("m_adventureArray 인덱스를 벗어났습니다. {0}", index));
            }
        }
        else
        {
            Debug.LogError("m_adventureDic가 비어있습니다.");
        }
        return null;
    }




    /// <summary>
    /// 모험 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Adventure getAdventure(string key)
    {
        if (m_adventureDic.ContainsKey(key))
        {
            return m_adventureDic[key];
        }
        Debug.LogError(string.Format("{0} 모험을 찾을 수 없습니다.", key));
        return null;
    }

}

