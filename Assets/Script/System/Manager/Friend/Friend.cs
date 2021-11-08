using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//게임시작, 게임종료, 보너스, 세루리안점령, 아이템사용, 야성해방
public enum TYPE_SPEECH {INFO, START, END, DEFEAT, BONAS, ITEM, FERAL }
public enum TYPE_ABILITY { FORCE, DEX, RANGE, FERAL, LUCK }

public class Friend : ICloneable {

    readonly char splitChar = '/';

    Sprite m_icon; //아이콘
    Sprite m_image; //이미지
    string m_key; //키
    string m_name; //이름
    string m_cretureGroup; //생물분류
    string m_contents; //설명
    int m_coin; //자파리코인

//    int m_maxScore; //최고 점수
    Dictionary<string, int> m_scoreDic = new Dictionary<string, int>();
//    int m_level;

    int[] m_abilities; //기본 능력치

//    int[] m_increaseAbilities; //능력치증가량 1.5~ 미사용

    //1.5 데이터값 변경에 의하여 추가
    int[] m_addAbilities; //추가 능력치 - 여기에다가 합산 필요

    string[] m_speeches; //대사

    int m_count; //참여 횟수
    
    public Sprite icon { get { return m_icon; } }
    public Sprite image { get { return m_image; } }
    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
//    public int maxExperiance { get { return m_maxExperiance; } }
//    public int experiance { get { return experiance; } }
//    public int level { get { return m_level; } }

    public string cretureGroup { get { return m_cretureGroup; } }
    public string orderGroup { get { return m_cretureGroup.Split(' ')[0]; } }
    public string familyGroup { get { return m_cretureGroup.Split(' ')[1]; } }
    public string genusGroup { get { return m_cretureGroup.Split(' ')[2]; } }
    public int count { get { return m_count; } }
    public int coin { get { return m_coin; } }
//    public int maxScore { get { return m_maxScore; } }
    public string contents { 
        get 
        {
            return m_contents;
//            return string.Format("\"{0}\"\n\n{1}", speeches[(int)TYPE_SPEECH.INFO], m_contents);
        } 
    }

//    public int[] abilities{get{return m_abilities;}}
//    public int[] increaseAbilities { get { return m_increaseAbilities; } }

    string[] speeches { get { return m_speeches; } }

    public int force { get { return getAbility(TYPE_ABILITY.FORCE);  /*m_abilities[(int)TYPE_ABILITY.FORCE]; */} }
    public int dex { get { return getAbility(TYPE_ABILITY.DEX); /*m_abilities[(int)TYPE_ABILITY.DEX];*/ } }
    public int range { get { return getAbility(TYPE_ABILITY.RANGE); /*m_abilities[(int)TYPE_ABILITY.RANGE];*/ } }
    public int feral { get { return getAbility(TYPE_ABILITY.FERAL); /*m_abilities[(int)TYPE_ABILITY.FERAL];*/ } }
    public int luck { get { return getAbility(TYPE_ABILITY.LUCK); /*m_abilities[(int)TYPE_ABILITY.LUCK]; */} }

    public int realForce { get { return (int)increaseValue(TYPE_ABILITY.FORCE, force);} }
    public float realDex { get { return increaseValue(TYPE_ABILITY.DEX, dex); } }
    public float realRange { get { return increaseValue(TYPE_ABILITY.RANGE, range); } }
    public float realFeral { get { return increaseValue(TYPE_ABILITY.FERAL, feral); } }
    public float realLuck { get { return increaseValue(TYPE_ABILITY.LUCK, luck); } }

    public int increaseForce { get { return getAbility(TYPE_ABILITY.FORCE) * PrepClass.defaultIncreaseCost; /*m_increaseAbilities[(int)TYPE_ABILITY.FORCE];*/ } }
    public int increaseDex { get { return getAbility(TYPE_ABILITY.DEX) * PrepClass.defaultIncreaseCost; /*(m_increaseAbilities[(int)TYPE_ABILITY.DEX];*/ } }
    public int increaseRange { get { return getAbility(TYPE_ABILITY.RANGE) * PrepClass.defaultIncreaseCost; /* m_increaseAbilities[(int)TYPE_ABILITY.RANGE];*/ } }
    public int increaseFeral { get { return getAbility(TYPE_ABILITY.FERAL) * PrepClass.defaultIncreaseCost; /* m_increaseAbilities[(int)TYPE_ABILITY.FERAL];*/ } }
    public int increaseLuck { get { return getAbility(TYPE_ABILITY.LUCK) * PrepClass.defaultIncreaseCost; /* m_increaseAbilities[(int)TYPE_ABILITY.LUCK];*/ } }

    float increaseValue(TYPE_ABILITY typeAbility, int value)
    {
        return ((float)PrepClass.defaultIncreaseAbility +
            (value - PrepClass.defaultIncreaseAbility) *
            PrepClass.increaseAbilities[(int)typeAbility]) *
            PrepClass.defaultAbilities[(int)typeAbility];
    }

    /// <summary>
    /// 능력치 비용 가져오기
    /// </summary>
    /// <param name="typeAbility"></param>
    /// <returns></returns>
    public int getIncreaseAbilityCost(int typeAbility){
        switch((TYPE_ABILITY)typeAbility){
            case TYPE_ABILITY.FORCE:
                return increaseForce;
            case TYPE_ABILITY.DEX:
                return increaseDex;
            case TYPE_ABILITY.RANGE:
                return increaseRange;
            case TYPE_ABILITY.FERAL:
                return increaseFeral;
            case TYPE_ABILITY.LUCK:
                return increaseLuck;
            default:
                Debug.LogWarning(string.Format("{0} 해당 능력치 비용을 가져올 수 없음", typeAbility));
                break;
        }
        return -1;
    }

    /// <summary>
    /// 대사 가져오기
    /// </summary>
    /// <param name="typeSpeech"></param>
    /// <returns></returns>
    public string getSpeech(TYPE_SPEECH typeSpeech)
    {
        if (speeches.Length > (int)typeSpeech)
        {
            return speeches[(int)typeSpeech];
        }
        return null;
    }

    /// <summary>
    /// 능력치 가져오기
    /// </summary>
    /// <param name="typeAbility"></param>
    /// <returns></returns>
    public int getAbility(TYPE_ABILITY typeAbility)
    {
        if (m_abilities.Length > (int)typeAbility)
        {
            return m_abilities[(int)typeAbility] + m_addAbilities[(int)typeAbility];
        }
        return -1;
    }

    /// <summary>
    /// 능력치 상승하기
    /// </summary>
    /// <param name="typeAbility"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool upgradeAbility(TYPE_ABILITY typeAbility, int count = 1){
        if (m_abilities.Length > (int)typeAbility)
        {
            m_addAbilities[(int)typeAbility] += count;
//            m_increaseAbilities[(int)typeAbility] = m_abilities[(int)typeAbility] * PrepClass.defaultIncreaseCost;
            return true;
        }
        return false;

    }

    public Friend(float version, FriendSerial friendSerial)
    {
        Friend friend = FriendManager.GetInstance.getFriend(friendSerial.key);
        m_icon = friend.icon;
        m_image = friend.image;
        m_key = friend.key;
        m_name = friend.name;
        m_cretureGroup = friend.cretureGroup;
        m_contents = friend.contents;
        m_coin = friend.coin;

        m_addAbilities = (int[])friendSerial.abilityArray.Clone();

        if (versionSynchronize(version, friend))
        {
            Debug.Log(string.Format("{0} 동기화 완료", version));
        }

        m_abilities = (int[])friend.m_abilities.Clone();

        //for (int i = 0; i < m_increaseAbilities.Length; i++)
        //{
        //    m_increaseAbilities[i] = (m_abilities[i] + 1) * PrepClass.defaultIncreaseCost;
        //}

        for(int i = 0; i < friendSerial.scoreArray.Length; i++){
            string[] serialData = friendSerial.scoreArray[i].Split(splitChar);
            if (serialData.Length > 0)
            {
                //1.6버전 미만은 리더보드를 위한 최고점수 초기화
                if(version < 1.6f)
                    m_scoreDic.Add(serialData[0], 0);
                else
                    m_scoreDic.Add(serialData[0], int.Parse(serialData[1]));
            }
        }

        //1.4
        //모험 개수가 다르면
        if (m_scoreDic.Keys.Count != AdventureManager.GetInstance.ElementCount)
        {
            for (int i = 0; i < AdventureManager.GetInstance.ElementCount; i++)
            {
                Adventure adventure = AdventureManager.GetInstance.getAdventure(i);
                //모험키가 없으면
                if (!m_scoreDic.ContainsKey(adventure.key))
                {
                    //새 모험키 등록
                    m_scoreDic.Add(adventure.key, 0);
                }
            }
        }
        
        m_speeches = friend.speeches;
        m_count = friendSerial.count;
    }

    public Friend(Sprite icon, Sprite image, string key, string name, string cretureGroup, int coin, string contents, int[] ability, string[] speeches)
    {
        m_icon = icon;
        m_image = image;
        m_key = key;
        m_name = name;
        m_cretureGroup = cretureGroup;
        m_contents = contents;
        m_coin = coin;

        m_addAbilities = new int[Enum.GetValues(typeof(TYPE_ABILITY)).Length];

        m_abilities = (int[])ability.Clone();
//        m_increaseAbilities = new int[Enum.GetValues(typeof(TYPE_ABILITY)).Length];


        for (int i = 0; i < AdventureManager.GetInstance.ElementCount; i++)
        {
            m_scoreDic.Add(AdventureManager.GetInstance.getAdventure(i).key, 0);
        }

        //for (int i = 0; i < m_increaseAbilities.Length; i++)
        //{
        //    m_increaseAbilities[i] = (m_abilities[i] + 1) * PrepClass.defaultIncreaseCost;
        //}

        m_speeches = (string[])speeches.Clone();
        m_count = 0;
//        m_maxExperiance = PrepClass.defaultExperiance;
//        m_experiance = 0;
//        m_level = 1;
    }

    bool versionSynchronize(float version, Friend friend)
    {
        bool isSync = false;
        //능력치를 합산해야 함
        //1.4 이하 버전은 능력치값 수정 필요
        //1.4은 능력치가 100이상으로 되어있음
        //캐릭터 능력치를 빼고 남은 값을 삽입해야 함
        if (1.4f >= version)
        {
            for (int i = 0; i < Enum.GetValues(typeof(TYPE_ABILITY)).Length; i++)
            {
                //현재 어빌리티와 프렌즈 어빌리티를 차감하여 삽입
                //더한 프렌즈 어빌리티 결과가 도출
                m_addAbilities[i] = m_addAbilities[i] - friend.m_abilities[i];
            }
            isSync = true;
        }

        return isSync;
    }

    public void addCount(){
        m_count++;
    }

    /// <summary>
    /// 가장 높은 능력치 가져오기
    /// </summary>
    /// <returns></returns>
    public int getMaxAbility()
    {

        int[] totalAbilities = new int[Enum.GetValues(typeof(TYPE_ABILITY)).Length];

        for (int i = 0; i < totalAbilities.Length; i++)
        {
            totalAbilities[i] = m_abilities[i] + m_addAbilities[i];
        }

        return totalAbilities.Max(ability => ability);
    }

    /// <summary>
    /// 업그레이드한 능력치 총 합
    /// </summary>
    /// <returns></returns>
    public int getSumAbility()
    {
        return m_addAbilities.Sum();
    }

    public int getMaxScore(string adventureKey)
    {
        if (m_scoreDic.ContainsKey(adventureKey))
        {
            return m_scoreDic[adventureKey];
        }
        return -1;
    }

    public bool setMaxScore(string adventureKey, int score)
    {
        if (m_scoreDic.ContainsKey(adventureKey))
        {

            if (m_scoreDic[adventureKey] < score)
            {
                m_scoreDic[adventureKey] = score;
                return true;
            }
        }
        return false;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public FriendSerial getSerialize()
    {
        FriendSerial friendSerial = new FriendSerial();
        friendSerial.key = key;
        friendSerial.count = count;
        friendSerial.abilityArray = (int[])m_addAbilities.Clone();


        string[] scoreArray = new string[m_scoreDic.Count];
        string[] scoreKeyArray = m_scoreDic.Keys.ToArray<string>();
        int[] scoreValueArray = m_scoreDic.Values.ToArray<int>();

        for(int i = 0; i < m_scoreDic.Count; i++){
            scoreArray[i] = string.Format("{0}{1}{2}", scoreKeyArray[i], splitChar, scoreValueArray[i]);
        }

        friendSerial.scoreArray = scoreArray;

        return friendSerial;

    }


    

}
