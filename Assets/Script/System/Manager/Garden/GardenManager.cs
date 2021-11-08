using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : SingletonClass<GardenManager>
{

    enum TYPE_GARDEN_DATA { KEY, NAME, CONTENTS }

    List<Garden> m_gardenList = new List<Garden>();

    public int ElementCount { get { return m_gardenList.Count; } }

    public GardenManager()
    {
        initParse();
    }

    void initParse()
    {
        Sprite[] gardenIcon = Resources.LoadAll<Sprite>(PrepClass.gardenIconPath);
//        Sprite[] gardenImage = Resources.LoadAll<Sprite>(PrepClass.gardenImagePath);

        TextAsset textAsset = Resources.Load<TextAsset>(PrepClass.gardenDataPath);

        if (textAsset != null)
        {
            string[] gardenRecord = textAsset.text.Split('\n');

            for (int i = 0; i < gardenRecord.Length; i++)
            {
                string[] gardenWord = gardenRecord[i].Split('\t');
                string key = gardenWord[(int)TYPE_GARDEN_DATA.KEY].Trim();


                if (gardenWord.Length == Enum.GetValues(typeof(TYPE_GARDEN_DATA)).Length)
                {

                    string name = gardenWord[(int)TYPE_GARDEN_DATA.NAME].Trim();

                    string contents = gardenWord[(int)TYPE_GARDEN_DATA.CONTENTS].Trim();

                    Sprite icon = gardenIcon.Where(garIcon => garIcon.name == string.Format("Icon_{0}", key)).SingleOrDefault();
                    if (icon == null) Debug.LogWarning(string.Format("{0} 아이콘 없음", key));

//                    Sprite image = gardenImage.Where(garImage => garImage.name == string.Format("Image_{0}", key)).SingleOrDefault();
//                    if (image == null) Debug.LogWarning(string.Format("{0} 아이콘 없음", key));

                    Garden garden = new Garden(key, name, contents, icon, null);
                    m_gardenList.Add(garden);
                }
                else
                {
                    if (key != "")
                        Debug.LogError(string.Format("{0}의 데이터가 많거나 부족합니다. {1}", key, gardenWord.Length));
                }
            }
        }
    }

    public Garden getGarden(int index)
    {
        if (m_gardenList.Count > 0)
        {
            return m_gardenList[index];
        }
        return null;
    }

}

