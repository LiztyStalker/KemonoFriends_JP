using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : SingletonClass<ItemManager> {

    enum TYPE_ITEM_DATA { KEY, NAME, LOCATION, CONTENTS, COST, VALUE, USE }

    Dictionary<string, Item> m_itemDic = new Dictionary<string,Item>();
    Item[] m_itemArray = null;

    public int ElementCount { get { return m_itemDic.Count; } }

    public ItemManager(){
        initParse();
    }

    void initParse(){

        Sprite[] itemIcon = Resources.LoadAll<Sprite>(PrepClass.itemIconPath);

        TextAsset textAsset = Resources.Load<TextAsset>(PrepClass.itemDataPath);

        if (textAsset != null)
        {
            string[] itemRecord = textAsset.text.Split('\n');

            for (int i = 0; i < itemRecord.Length; i++)
            {
                string[] itemWord = itemRecord[i].Split('\t');
                string key = itemWord[(int)TYPE_ITEM_DATA.KEY];
                key = key.Trim();


                if (itemWord.Length == Enum.GetValues(typeof(TYPE_ITEM_DATA)).Length)
                {

                    Sprite itemSprite = itemIcon.Where(itemSp => itemSp.name == key).SingleOrDefault();
                    if (itemSprite == null) Debug.LogWarning(string.Format("{0} 아이콘 없음", key));


                    int cost;
                    if (!int.TryParse(itemWord[(int)TYPE_ITEM_DATA.COST], out cost))
                        cost = 100;

                    int value;
                    if (!int.TryParse(itemWord[(int)TYPE_ITEM_DATA.VALUE], out value))
                        value = 0;

                    bool isUse;
                    if (!bool.TryParse(itemWord[(int)TYPE_ITEM_DATA.USE], out isUse))
                        isUse = false;


                    /////사용처
                    Dictionary<string, bool> locationFlag = new Dictionary<string, bool>();

                    for (int j = 0; j < AdventureManager.GetInstance.ElementCount; j++)
                    {
                        Adventure adventure = AdventureManager.GetInstance.getAdventure(j);
                        if (adventure != null)
                        {
                            locationFlag.Add(adventure.key, false);
                        }
                    }

                    string[] locations = itemWord[(int)TYPE_ITEM_DATA.LOCATION].Split(',');

                    for (int j = 0; j < locations.Length; j++)
                    {
                        locations[j] = locations[j].Trim();

                        if (locationFlag.ContainsKey(locations[j]))
                        {
                            locationFlag[locations[j]] = true;
                        }
                    }
                    /////



                    Item item = new Item(
                        key,
                        itemSprite,
                        itemWord[(int)TYPE_ITEM_DATA.NAME],
                        locationFlag,
                        itemWord[(int)TYPE_ITEM_DATA.CONTENTS],
                        cost,
                        value,
                        isUse
                        );

                    m_itemDic.Add(key, item);
                }
                else
                {
                    if (key != "")
                        Debug.LogError(string.Format("{0}의 데이터가 많거나 부족합니다. {1}", key, itemWord.Length));
                }


            }


        }
        else
        {
            Debug.LogError(string.Format("{0}의 데이터를 찾지 못했습니다", PrepClass.itemDataPath));
        }
    }


    /// <summary>
    /// 기본값 아이템 가져오기
    /// </summary>
    /// <returns></returns>
    public Item defaultItem()
    {
        return getItem("ItemNone");
    }

    /// <summary>
    /// 아이템 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Item getItem(string key)
    {
        if (m_itemDic.ContainsKey(key))
        {
            return m_itemDic[key];
        }

        Debug.LogError(string.Format("{0} 아이템을 찾을 수 없습니다.", key));
        return defaultItem();
    }

    /// <summary>
    /// 모험 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item getItem(int index)
    {
        if (m_itemDic.Count > 0)
        {
            if (m_itemArray == null)
                m_itemArray = m_itemDic.Values.ToArray<Item>();

            if (m_itemArray.Length > index)
            {
                return m_itemArray[index];
            }
            else
            {
                Debug.LogError(string.Format("m_itemArray 인덱스를 벗어났습니다. {0}", index));
            }
        }
        else
        {
            Debug.LogError("m_itemDic가 비어있습니다.");
        }
        return null;
    }
}
