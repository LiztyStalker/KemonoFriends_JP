using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccountSerial
{
    public string saveVersion; //저장된 버전
    public string name; //프렌즈 이름

    public int runDate; //현재까지 진행한 날짜
    public int lastSaveDate; //마지막 저장날짜

    public int japariBread; //자파리빵
    public int japariCoin; //자파리코인
    public int cerulean; //세룰리안
    public int sandstar; //샌드스타

    public int totalJapariBread; //자파리빵
    public int totalCoin; //자파리코인
    public int totalCerulean; //세룰리안
    public int totalSandstar; //샌드스타


//    프렌즈 모집
    public List<FriendSerial> friendList = new List<FriendSerial>();

//    구입한 책 키
    public List<string> bookList = new List<string>();

    //업적 달성
    public List<string> achivementList = new List<string>();

    public float version()
    {
        try
        {
            float version = float.Parse(Application.version);
            float.TryParse(saveVersion, out version);
            return version;
        }
        catch
        {
            return -1f;
        }
    }
}