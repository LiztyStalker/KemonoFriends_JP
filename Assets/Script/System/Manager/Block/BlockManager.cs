
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockManager : SingletonClass<BlockManager> {

    enum TYPE_BLOCK_DATA { KEY, IMAGE, TYPE, VALUE, LOCATION, WEIGHT, SCORE, HEALTH }

    //전체 블록
    List<Block> m_blockList = new List<Block>();
//    int m_totalWeight = 0;

    //사용할 블록
    List<Block> m_useBlockList = null;
    string m_adventureKey;
    int m_useWeight = 0;

    //빈 블록
    Block m_emptyBlock = null;
    Block m_defaultBlock = null;
    Block m_typeBlock = null;


    public BlockManager(){
        initParse();
    }

    void initParse()
    {
        Sprite[] blockIcons = Resources.LoadAll<Sprite>(PrepClass.blockIconPath);

        TextAsset blockField = Resources.Load<TextAsset>(PrepClass.blockDataPath);

        if (blockField != null)
        {
            string[] blockRecord = blockField.text.Split('\n');

            for (int i = 0; i < blockRecord.Length; i++)
            {
                string[] blockWord = blockRecord[i].Split('\t');
                string key = blockWord[(int)TYPE_BLOCK_DATA.KEY];
                key = key.Trim();

                if (blockWord.Length == Enum.GetValues(typeof(TYPE_BLOCK_DATA)).Length)
                {
                    string imageKey = blockWord[(int)TYPE_BLOCK_DATA.IMAGE].Trim();
                    Sprite icon = blockIcons.Where(iconSp => iconSp.name == imageKey).SingleOrDefault();
                    if (icon == null) Debug.LogWarning(string.Format("{0} 아이콘 없음", key));

                    //타입
                    int typeIndex;
                    if (!int.TryParse(blockWord[(int)TYPE_BLOCK_DATA.TYPE], out typeIndex))
                        typeIndex = 1;

                    TYPE_BLOCK typeBlock = (TYPE_BLOCK)typeIndex;



                    //타입
                    if (!int.TryParse(blockWord[(int)TYPE_BLOCK_DATA.VALUE], out typeIndex))
                        typeIndex = 0;

                    TYPE_VALUE typeValue = (TYPE_VALUE)typeIndex;


                    //사용처
                    Dictionary<string, bool> locationFlag = new Dictionary<string, bool>();

                    for(int j = 0; j < AdventureManager.GetInstance.ElementCount; j++){
                        Adventure adventure = AdventureManager.GetInstance.getAdventure(j);
                        if (adventure != null)
                        {
                            locationFlag.Add(adventure.key, false);
                        }
                    }

                    string[] locations = blockWord[(int)TYPE_BLOCK_DATA.LOCATION].Split(',');

                    for (int j = 0; j < locations.Length; j++)
                    {
                        locations[j] = locations[j].Trim();

                        if (locationFlag.ContainsKey(locations[j]))
                        {
                            locationFlag[locations[j]] = true;
                        }
                    }

                    
                    //가중치
                    int weight;
                    if (!int.TryParse(blockWord[(int)TYPE_BLOCK_DATA.WEIGHT], out weight))
                        weight = 0;

                    int score;
                    if (!int.TryParse(blockWord[(int)TYPE_BLOCK_DATA.SCORE], out score))
                        score = 0;

                    int health;
                    if (!int.TryParse(blockWord[(int)TYPE_BLOCK_DATA.HEALTH], out health))
                        health = 0;

                    Block block = new Block(key, icon, typeBlock, typeValue, locationFlag, weight, score, health);
                    m_blockList.Add(block);

                }
                else
                {
                    if(key != "")
                        Debug.LogError(string.Format("{0}의 데이터가 많거나 부족합니다. {1}", key, blockWord.Length));
                }

            }
        }
        else
        {
            Debug.LogError(string.Format("{0}의 데이터를 찾지 못했습니다", PrepClass.blockDataPath));
        }

        m_blockList.Sort(blockSort);
//        m_totalWeight = m_blockList.Sum(block => block.weight);

//        Debug.LogWarning(m_blockList[0].icon.name);

    }
    
    /// <summary>
    /// 블록 정렬 - 낮은 숫자가 위로
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int blockSort(Block a, Block b)
    {
        return b.weight.CompareTo(a.weight);
    }

    /// <summary>
    /// 벽 블록 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <param name="typeValue"></param>
    /// <returns></returns>
    public Block getWallBlock(string key, TYPE_VALUE typeValue)
    {
        if (m_blockList.Count > 0)
        {
            return m_blockList.Where(block => block.isCheckLocation(key) && block.typeBlock == TYPE_BLOCK.WALL && block.typeValue == typeValue).SingleOrDefault();
        }
        return null;
    }

    /// <summary>
    /// 세룰리안 블록 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <param name="typeValue"></param>
    /// <returns></returns>
    public Block getCeruleanBlock(string key, TYPE_VALUE typeValue)
    {
        if (m_blockList.Count > 0)
        {
            return m_blockList.Where(block => block.isCheckLocation(key) && block.typeBlock == TYPE_BLOCK.CERULEAN && block.typeValue == typeValue).SingleOrDefault();
        }
        return null;
    }

    //재료 수집
    //유적지 탐사
    //PPP와 춤을
    //세룰리안 격퇴
    /// <summary>
    /// 해당하는 모험에 맞는 블록 가져오기
    /// 아무것도 해당되지 않으면 DefaultBlock을 가져옵니다.
    /// </summary>
    /// <param name="adventureKey"></param>
    /// <returns></returns>
    public Block getRandomBlock(string adventureKey, float rate)
    {

        //현재 가져오려는 어드벤쳐 키가 맞지 않으면
        if (m_adventureKey != adventureKey)
        {
            //어드벤쳐 블록 재 등록
            m_adventureKey = adventureKey;
            m_useBlockList = m_blockList.Where(block => block.isCheckLocation(adventureKey)).ToList<Block>();
            m_useWeight = m_useBlockList.Sum(block => block.weight);
        }


        int useWeight = m_useWeight;

        if (m_useBlockList.Count > 0)
        {
            for (int i = m_useBlockList.Count - 1; i >= 0; i--)
            {

                if (i == 0)
                {
                    return m_useBlockList[i];
                }

                else if (UnityEngine.Random.Range(0, useWeight) < (int)((float)(m_useBlockList[i].weight) * rate))
                {

                    return m_useBlockList[i];

                    //true이고 사료가 아니면 재루프
                    //if (isFood && m_blockList[i].typeBlock != TYPE_BLOCK.BLOCK)
                    //    continue;

                    //false이면 전체
                    
                }

                useWeight -= m_useBlockList[i].weight;
            }
        }
        return getDefaultBlock(adventureKey);
    }

    /// <summary>
    /// 빈 블록 가져오기
    /// </summary>
    /// <returns></returns>
    public Block getEmptyBlock()
    {
        if(m_emptyBlock == null)
            m_emptyBlock = m_blockList.Where(block => block.typeBlock == TYPE_BLOCK.NONE).SingleOrDefault();
        return m_emptyBlock;
    }

    /// <summary>
    /// 기본 블록 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Block getDefaultBlock(string key)
    {
        if (m_defaultBlock == null || !m_defaultBlock.isCheckLocation(key))
            m_defaultBlock = m_blockList.Where(block => block.isCheckLocation(key) && block.typeBlock == TYPE_BLOCK.BLOCK && block.typeValue == TYPE_VALUE.VALUE_00).SingleOrDefault();
        return m_defaultBlock;
    }

    /// <summary>
    /// 기본 블록 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Block getBallBlock(string key)
    {
        if (m_defaultBlock == null || !m_defaultBlock.isCheckLocation(key))
            m_defaultBlock = m_blockList.Where(block => block.isCheckLocation(key) && block.typeBlock == TYPE_BLOCK.BALL && block.typeValue == TYPE_VALUE.VALUE_00).SingleOrDefault();
        return m_defaultBlock;
    }


    /// <summary>
    /// 해당 블록 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <param name="typeBlock"></param>
    /// <param name="typeValue"></param>
    /// <returns></returns>
    public Block getTypeBlock(string key, TYPE_BLOCK typeBlock, TYPE_VALUE typeValue = TYPE_VALUE.VALUE_00)
    {
        Debug.Log(key + typeBlock);
        if (m_typeBlock == null || !m_typeBlock.isCheckLocation(key) || m_typeBlock.typeBlock != typeBlock || m_typeBlock.typeValue != typeValue)
//            m_typeBlock = 
            return m_blockList.Where(block => block.isCheckLocation(key) && block.typeBlock == typeBlock && block.typeValue == typeValue).SingleOrDefault();
        return null;
    }


    
    public Block getTypeBlock(string key, TYPE_BLOCK typeBlock, float rate = 1f, bool isEmpty = true)
    {
        List<Block> m_typeBlocks = m_blockList.Where(block => block.isCheckLocation(key) && block.typeBlock == typeBlock).ToList<Block>();
        int useWeight = m_typeBlocks.Sum(block => block.weight);

        if (m_typeBlocks.Count > 0)
        {
            for (int i = m_typeBlocks.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    if (isEmpty)
                    {
                        if (UnityEngine.Random.Range(0f, 100f) < 30f * rate)
                        {
                            return m_typeBlocks[i];
                        }
                        else
                        {
                            return m_emptyBlock;
                        }
                    }
                    else
                        return m_typeBlocks[i];


                }

                else if (UnityEngine.Random.Range(0, useWeight) < (int)((float)(m_typeBlocks[i].weight) * rate))
                {
                    return m_typeBlocks[i];


                    //true이고 사료가 아니면 재루프
                    //if (isFood && m_blockList[i].typeBlock != TYPE_BLOCK.BLOCK)
                    //    continue;

                    //false이면 전체

                }

                useWeight -= m_typeBlocks[i].weight;
            }
        }
        return getDefaultBlock(key);
    }


}
