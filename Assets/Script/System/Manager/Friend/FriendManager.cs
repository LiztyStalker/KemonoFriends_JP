using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendManager : SingletonClass<FriendManager>{

    enum TYPE_FRIEND_DATA { KEY, NAME, GROUP, ABILITY00, ABILITY01, ABILITY02, ABILITY03, ABILITY04, COIN, CONTENTS }

    const int m_friendLimit = 50;

    Dictionary<string, Friend> m_friendDic = new Dictionary<string, Friend>();
    Friend[] m_friendList = null;

    public Friend[] friendList { 
        get 
        {
            if (m_friendList == null)
            {
                m_friendList = new Friend[m_friendDic.Values.Count];
                m_friendDic.Values.CopyTo(m_friendList, 0);
            }
            return m_friendList;
        } 
    }

    public int ElementCount { get { return friendList.Length; } }


    public FriendManager()
    {
        initParse();
    }

    void initParse()
    {
        Sprite[] friendIcons = Resources.LoadAll<Sprite>(PrepClass.friendIconPath);
        Sprite[] friendImages = Resources.LoadAll<Sprite>(PrepClass.friendImagePath);

        TextAsset friendData = Resources.Load<TextAsset>(PrepClass.friendDataPath);
        TextAsset friendScripts = Resources.Load<TextAsset>(PrepClass.friendScriptsPath);
        Dictionary<string, string[]> m_friendScriptsDic = new Dictionary<string, string[]>();



        //프렌즈 대화내역 가져오기
        if (friendScripts != null){

            string[] friendScriptsRecord = friendScripts.text.Split('\n');
            for (int i = 0; i < friendScriptsRecord.Length; i++)
            {
                List<string> friendScriptsData = friendScriptsRecord[i].Split('\t').ToList<string>();
                string key = friendScriptsData[0];
                friendScriptsData.Remove(key);

                if (friendScriptsData.Count == Enum.GetValues(typeof(TYPE_SPEECH)).Length)
                {
                    string[] friendScriptsDataArray = friendScriptsData.ToArray<string>();
                    m_friendScriptsDic.Add(key, friendScriptsDataArray);
                }
                else
                {
                    if(key != "")
                        Debug.LogWarning(string.Format("{0} 프렌즈의 대사가 부족하거나 넘칩니다. {1}개", key, friendScriptsData.Count));
                }

            }
        }
        else
        {
            Debug.LogError(string.Format("{0}의 데이터를 찾지 못했습니다", PrepClass.friendScriptsPath));
        }


        //프렌즈 정보 가져오기
        if (friendData != null)
        {
            string[] friendRecord = friendData.text.Split('\n');
            for (int i = 0; i < friendRecord.Length; i++)
            {
                string[] friendWord = friendRecord[i].Split('\t');

                if(friendWord.Length == Enum.GetValues(typeof(TYPE_FRIEND_DATA)).Length){
                    //키 저장
                    string key = friendWord[(int)TYPE_FRIEND_DATA.KEY];

                    //코인 초기화
                    int coin = 100;

                    //코인 데이터 삽입
                    int.TryParse(friendWord[(int)TYPE_FRIEND_DATA.COIN], out coin);

                    //능력치 길이 조절
                    int[] ability = new int[Enum.GetValues(typeof(TYPE_ABILITY)).Length];

                    //능력치 삽입. 없으면 초기화 1
                    for(int j = 0; j < ability.Length; j++){
                        if (!int.TryParse(friendWord[(int)TYPE_FRIEND_DATA.ABILITY00 + j], out ability[j])){
                            Debug.LogError(string.Format("{0} 프렌즈 능력치 숫자 아님 {1} : 1로 초기화", key, friendWord[(int)TYPE_FRIEND_DATA.ABILITY00 + j]));   
                            ability[j] = 1;
                        }
                    }

                    //대사틀 준비
                    string[] speeches = new string[Enum.GetValues(typeof(TYPE_SPEECH)).Length];

                    //대사 유무 여부
                    if (m_friendScriptsDic.ContainsKey(key))
                    {
                        speeches = m_friendScriptsDic[key];
                    }
                    else
                    {
                        Debug.LogError(string.Format("{0} 프렌즈 대사 없음", key));
                    }

                    //프렌즈 아이콘 이미지 찾기
                    Sprite friendImage = friendImages.Where(friendName => friendName.name == friendWord[0]).SingleOrDefault();
                    if (friendImage == null) Debug.LogWarning(string.Format("{0} 프렌즈 이미지 없음", key));

                    Sprite friendIcon = friendIcons.Where(friendName => friendName.name == ("Icon_" + friendWord[0])).SingleOrDefault();
                    if (friendIcon == null) Debug.LogWarning(string.Format("{0} 프렌즈 아이콘 없음", key));

                    //프렌즈 데이터 삽입
                    Friend newFriend = new Friend(friendIcon, friendImage, friendWord[0], friendWord[1], friendWord[2], coin, friendWord[(int)TYPE_FRIEND_DATA.CONTENTS], ability, speeches);
                    m_friendDic.Add(key, newFriend);
                }
            }
        }
        else
        {
            Debug.LogError(string.Format("{0}의 데이터를 찾지 못했습니다", PrepClass.friendDataPath));
        }

    }


    public Friend getFriend(string key)
    {
        if (m_friendDic.ContainsKey(key))
        {
            return (Friend)m_friendDic[key].Clone();
        }
        return null;
    }

    /// <summary>
    /// 랜덤 프렌즈 가져오기
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <param name="isOverlap"> true 중복이어도 가져옴 - false 중복되면 가져오지 않음 1.31</param>
    /// <returns></returns>
    public List<Friend> getRandomFriend(int count = 1, bool isOverlap = false)
    {
        List<Friend> randFriendList = new List<Friend>();
        if (friendList.Length > 0)
        {
            int maxCount = m_friendLimit;
            for (int i = 0; i < count; i++)
            {
                Friend friend = friendList[UnityEngine.Random.Range(0, friendList.Length)];

                //중복여부

                if (isOverlap || (Account.GetInstance.getFriend(friend.key) == null && !randFriendList.Contains(friend)))
                {
                    randFriendList.Add(friend);
//                    Debug.Log("Wanted : " + friend.key);
                    maxCount = m_friendLimit;
                }
                else
                {
                    i--;
                    maxCount--;
                }

                if (maxCount <= 0)
                    break;
            }
            return randFriendList;
        }

        return null;
    }

    
}
