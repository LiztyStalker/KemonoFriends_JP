using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IOPackage;
using GoogleAPI;

public delegate void MsgDelegate(string msg);

public class Account : SingletonClass<Account>{
    
   
    
    //계정이름
    string m_nickname;
    //세룰리안 처치
    int m_cerulean;
    int m_totalCerulean;
    //자파리코인
    int m_coin;
    int m_totalCoin;
    //자파리빵
    int m_japariBread;
    int m_totalJapariBread;
    //샌드스타
    int m_sandstar;
    int m_totalSandstar;

    //프렌즈들
    Dictionary<string, Friend> m_friendDic = new Dictionary<string, Friend>();

    //도움말
//    Dictionary<string, bool> m_adventureHelpDic = new Dictionary<string, bool>();

    //책
    List<string> m_bookList = new List<string>();

    //업적
    List<string> m_achivementList = new List<string>();

    //다음씬 이동
    string m_nextScene;
    //플레이중인 프렌즈 키
    string m_friendKey = "";
    //모험 키
    string m_adventureKey = "";
    //아이템 키
    string m_itemKey = "";


    public string nickname { get { return m_nickname; } }
    public int celurean { get { return m_cerulean; } }
    public int coin { get { return m_coin; } }
    public int japariBread { get { return m_japariBread; } }
    public int sandstar { get { return m_sandstar; } }

    public string nextScene { get { return m_nextScene; } set { m_nextScene = value; /*Debug.Log("checkSce : " + m_nextScene);*/ } }
    public string friendKey { get { return m_friendKey; } set { m_friendKey = value; /*Debug.Log("checkFri : " + m_friendKey);*/ } }
    public string adventureKey { get { return m_adventureKey; } set { m_adventureKey = value; /*Debug.Log("checkAdv : " + m_adventureKey);*/ } }
    public string itemKey { get { return m_itemKey; } set { m_itemKey = value; /*Debug.Log("checkAdv : " + m_adventureKey);*/ } }

    public int friendCount { get { return m_friendDic.Keys.Count; } }

    /// <summary>
    /// 계정 불러오기
    /// </summary>
    /// <param name="isOffline"></param>
    /// <param name="eventDel"></param>
    /// <param name="msgDel"></param>
    public void loadAccount(bool isOffline, GoogleApi.EventDelegate eventDel, MsgDelegate msgDel)
    {
        if (!isOffline)
        {
            //구글에서 불러오기 
            GoogleApi.GetInstance.openSavedGame(PrepClass.fileName, TYPE_FILE_ACCESS.Load, eventDel, msgDel);   

            //파일 데이터와 비교 - 파일 데이터가 최신이면 파일 데이터를 불러와야 함
        }
        else
        {
            //파일에 저장된 데이터 가져오기
            if (loadData())
            {
                if(eventDel != null) eventDel(true);
                if (msgDel != null) msgDel("불러오기 완료!");
            }
            else
            {
                //로드실패 - 새 데이터 쓰기
                initAccount();
                if (eventDel != null) eventDel(false);
                if (msgDel != null) msgDel("새 데이터 작성");
            }
            
        }
    }

    /// <summary>
    /// 계정 저장하기
    /// </summary>
    /// <param name="isOffline"></param>
    /// <param name="eventDel"></param>
    /// <param name="msgDel"></param>
    public void saveAccount(bool isOffline, GoogleApi.EventDelegate eventDel, MsgDelegate msgDel)
    {


        if (saveData(PrepClass.fileName))
        {
            if (eventDel != null) eventDel(true);
            if (msgDel != null) msgDel("저장 완료");
        }
        else
        {
            if (eventDel != null) eventDel(false);
            if (msgDel != null) msgDel("저장 실패");
        }

        if (!isOffline)
        {
            GoogleApi.GetInstance.openSavedGame(PrepClass.fileName, TYPE_FILE_ACCESS.Save, eventDel, msgDel);
        }
    }




    void initAccount()
    {

        //개발자용
        //m_friendDic.Add("Serval", FriendManager.GetInstance.getFriend("Serval"));
        //m_friendDic.Add("Hippo", FriendManager.GetInstance.getFriend("Hippo"));
        //m_friendDic.Add("Alpaca", FriendManager.GetInstance.getFriend("Alpaca"));
        //m_friendDic.Add("Ibis", FriendManager.GetInstance.getFriend("Ibis"));
        //m_friendDic.Add("Raccoon", FriendManager.GetInstance.getFriend("Raccoon"));
        //m_friendDic.Add("Fennec", FriendManager.GetInstance.getFriend("Fennec"));
        //m_friendDic.Add("RoyalPenguin", FriendManager.GetInstance.getFriend("RoyalPenguin"));
        //m_friendDic.Add("EmperorPenguin", FriendManager.GetInstance.getFriend("EmperorPenguin"));
        //m_friendDic.Add("GentooPenguin", FriendManager.GetInstance.getFriend("GentooPenguin"));
        //m_friendDic.Add("SouthernRockhopperPenguin", FriendManager.GetInstance.getFriend("SouthernRockhopperPenguin"));
        //m_friendDic.Add("HumboldtPenguin", FriendManager.GetInstance.getFriend("HumboldtPenguin"));
        //m_friendDic.Add("SmallClawedOtter", FriendManager.GetInstance.getFriend("SmallClawedOtter"));
        //m_friendDic.Add("Jaquar", FriendManager.GetInstance.getFriend("Jaquar"));
        //m_friendDic.Add("SandCat", FriendManager.GetInstance.getFriend("SandCat"));
        //m_friendDic.Add("WhitefacedScopsOwl", FriendManager.GetInstance.getFriend("WhitefacedScopsOwl"));
        //m_friendDic.Add("EurasianEagleOwl", FriendManager.GetInstance.getFriend("EurasianEagleOwl"));
        //m_friendDic.Add("Tsuchinoko", FriendManager.GetInstance.getFriend("Tsuchinoko"));
        //m_friendDic.Add("AmericanBeaver", FriendManager.GetInstance.getFriend("AmericanBeaver"));
        //m_friendDic.Add("BlackTailedPrairieDog", FriendManager.GetInstance.getFriend("BlackTailedPrairieDog"));


        //데이터 로드, 파일이 없으면 새로 시작

        m_friendDic.Add("Serval", FriendManager.GetInstance.getFriend("Serval"));

        for (int i = 0; i < AdventureManager.GetInstance.ElementCount; i++)
        {
            //도움말 활성화 초기화
            PlayerPrefs.SetString(AdventureManager.GetInstance.getAdventure(i).key, "true");
        }

        m_japariBread = 1000;
        m_coin = 100;
        Debug.Log("새 데이터 작성 완료");
    }

    /// <summary>
    /// 사용중인 프렌즈 가져오기
    /// </summary>
    /// <returns></returns>
    public Friend playFriend()
    {
        return getFriend(m_friendKey);
    }

    /// <summary>
    /// 최고의 프렌즈 가져오기
    /// </summary>
    /// <param name="adventureKey"></param>
    /// <returns></returns>
    public Friend topFriend(string adventureKey)
    {
        return m_friendDic.Values.OrderByDescending(friend => friend.getMaxScore(adventureKey)).FirstOrDefault();
    }
    
    /// <summary>
    /// 프렌즈 삽입
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool addFriend(string key)
    {
        return addFriend(FriendManager.GetInstance.getFriend(key));
    }

    /// <summary>
    /// 프렌즈 삽입하기
    /// </summary>
    /// <param name="friend"></param>
    /// <returns></returns>
    public bool addFriend(Friend friend)
    {
        if (friend != null)
        {
            m_friendDic.Add(friend.key, friend);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 프렌즈 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Friend getFriend(int index)
    {
        return m_friendDic.Values.ElementAt<Friend>(index);
    }

    /// <summary>
    /// 프렌즈 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Friend getFriend(string key)
    {
        if (m_friendDic.ContainsKey(key))
        {
            return m_friendDic[key];
        }
        return null;
    }

    /// <summary>
    /// 코인 사용 여부
    /// true 사용 가능
    /// </summary>
    /// <param name="coin"></param>
    /// <returns></returns>
    public bool checkCoin(int coin) {
        return (m_coin - coin < 0) ? false : true;
    }

    /// <summary>
    /// 코인 사용
    /// </summary>
    /// <param name="coin"></param>
    public void useCoin(int coin)
    {
        m_coin -= coin;
    }

    /// <summary>
    /// 코인 삽입
    /// </summary>
    /// <param name="coin"></param>
    public void addCoin(int coin)
    {
        m_coin += coin;
        m_totalCoin += coin;
    }

    /// <summary>
    /// 샌드스타 삽입
    /// </summary>
    /// <param name="sandstar"></param>
    public void addSandstar(int sandstar)
    {
        m_sandstar += sandstar;
        m_totalSandstar += sandstar;
    }

    /// <summary>
    /// 자파리빵 부족 여부
    /// true 사용 가능
    /// </summary>
    /// <param name="japariBread"></param>
    /// <returns></returns>
    public bool checkJapariBread(int japariBread){
        return (m_japariBread - japariBread < 0) ? false : true;
    }

    /// <summary>
    /// 자파리빵 사용
    /// </summary>
    /// <param name="japariBread"></param>
    public void useJapariBread(int japariBread)
    {
        m_japariBread -= japariBread;
    }

    /// <summary>
    /// 자파리빵 삽입
    /// </summary>
    /// <param name="japariBread"></param>
    public void addJapariBread(int japariBread)
    {
        m_japariBread += japariBread;
        m_totalJapariBread += japariBread;
    }

    /// <summary>
    /// 세룰리안 사용 유무 체크
    /// true 사용 가능
    /// </summary>
    /// <param name="cerulean"></param>
    /// <returns></returns>
    public bool checkCerulean(int cerulean)
    {
        return (m_cerulean - cerulean < 0) ? false : true;
    }

    /// <summary>
    /// 세룰리안 사용
    /// </summary>
    /// <param name="cerulean"></param>
    public void useCerulean(int cerulean)
    {
        m_cerulean -= cerulean;
    }

    /// <summary>
    /// 세룰리안 삽입
    /// </summary>
    /// <param name="cerulean"></param>
    public void addCerulean(int cerulean)
    {
        m_cerulean += cerulean;
        m_totalCerulean += celurean;
    }

    /// <summary>
    /// 닉네임 삽입
    /// </summary>
    /// <param name="nickname"></param>
    public void setNickname(string nickname)
    {
        m_nickname = nickname;
    }

    /// <summary>
    /// 도움말 활성화 유무
    /// true 사용
    /// </summary>
    /// <param name="adventureKey"></param>
    /// <returns></returns>
    public bool isAdventureHelp(string adventureKey)
    {
        bool isAdventureHelp = true;
        bool.TryParse(PlayerPrefs.GetString(adventureKey, "true"), out isAdventureHelp);
        return isAdventureHelp;
    }

    /// <summary>
    /// 도움말 설정하기
    /// </summary>
    /// <param name="adventureKey"></param>
    /// <param name="isHelp"></param>
    public void setAdventureHelp(string adventureKey, bool isHelp)
    {
        PlayerPrefs.SetString(adventureKey, isHelp.ToString());
    }

    /// <summary>
    /// 업적 유무
    /// true 업적 있음
    /// </summary>
    /// <param name="achivement"></param>
    /// <returns></returns>
    public bool isAchivement(Achivement achivement){
        return m_achivementList.Contains(achivement.key);
    }

    /// <summary>
    /// 업적 등록 및 보상 삽입
    /// </summary>
    /// <param name="achivement"></param>
    /// <returns></returns>
    public void setAchivement(Achivement achivement){
        m_achivementList.Add(achivement.key);
        addCoin(achivement.award);
    }

    //데이터 저장
    //프렌즈 모집, 게임 시작, 게임 종료, 책 구입, 종료시 저장

    /// <summary>
    /// 저장하기
    /// </summary>
    bool saveData(string fileName)
    {
        AccountSerial accountSerial = convertSeriel();
        return IOData.GetInstance.saveData(accountSerial, fileName);
    }

    /// <summary>
    /// 불러오기
    /// </summary>
    /// <returns></returns>
    bool loadData()
    {
        AccountSerial s_account = IOData.GetInstance.loadData(PrepClass.fileName);
        if (s_account != null)
        {
            
            //버전에 따라서 불러오기 다름

            m_bookList = s_account.bookList;
            m_japariBread = s_account.japariBread;
            m_coin = s_account.japariCoin;
            m_cerulean = s_account.cerulean;
            m_nickname = s_account.name;
            m_sandstar = s_account.sandstar;

            m_totalCerulean = s_account.totalCerulean;
            m_totalCoin  = s_account.totalCoin;
            m_totalJapariBread = s_account.totalJapariBread;
            m_totalSandstar = s_account.totalSandstar;

//            Debug.Log("sandstar : " + m_sandstar);

            foreach (FriendSerial friendSerial in s_account.friendList)
            {
                
                Friend friend = new Friend(s_account.version(), friendSerial);
                m_friendDic.Add(friend.key, friend);
            }

            m_bookList = s_account.bookList.ToList<string>();


            //1.4 버전 이하에서 업적이 없어 에러가 날 수 있음
            try
            {
                m_achivementList = s_account.achivementList.ToList<string>();

                //업적 테스트용
                //foreach(Achivement achive in AchivementManager.GetInstance.achivementDic.Values){
                //    if(!m_achivementList.Contains(achive.key)){
                //        m_achivementList.Add(achive.key);
                //    }
                //}
            }
            catch
            {
                m_achivementList = new List<string>();
            }


            //1.4 버전 이하는 토탈값을 기본적으로 가진 데이터로 초기화
            if (1.4f >= s_account.version())
            {
                m_totalCerulean = m_cerulean;
                m_totalCoin = m_coin;
                m_totalJapariBread = m_japariBread;
                m_totalSandstar = 0;
            }

            //1.5 버전 이하는 코인 25 증정
            if (1.5f >= s_account.version())
            {
                addCoin(25);
            }

            //1.6 버전 미만은 코인 100 증정 
            if (1.6f > s_account.version())
            {
                addCoin(100);
            }

            //전 버전 백업파일 1회 저장
            if (Application.version.CompareTo(s_account.version().ToString()) > 0)
            {
                IOData.GetInstance.saveData(s_account, PrepClass.fileName + "_" + s_account.version().ToString());
                Debug.Log("백업파일 저장");
            }



            return true;
        }
        return false;
    }


    /// <summary>
    /// 업적 타입에 대한 값 가져오기
    /// </summary>
    /// <param name="typeAchivement"></param>
    /// <returns></returns>
    public int getTypeAchivement(Achivement.TYPE_ACHIVEMENT typeAchivement)
    {
        switch (typeAchivement)
        {
            case Achivement.TYPE_ACHIVEMENT.RUN:
                return m_friendDic.Values.Sum(friend => friend.count);
            case Achivement.TYPE_ACHIVEMENT.JAPARIBREAD:
                return m_totalJapariBread;
            case Achivement.TYPE_ACHIVEMENT.SANDSTAR:
                return m_totalSandstar;
            case Achivement.TYPE_ACHIVEMENT.COIN:
                return m_totalCoin;
            case Achivement.TYPE_ACHIVEMENT.CERULEAN:
                return m_totalCerulean;
            case Achivement.TYPE_ACHIVEMENT.ABILITY:
                //각 프렌즈 능력치 중 최대값 가져오기
                return m_friendDic.Values.Max(friend => friend.getMaxAbility());
            case Achivement.TYPE_ACHIVEMENT.TOTAL_ABILITY:
                return m_friendDic.Values.Sum(friend => friend.getSumAbility());
            case Achivement.TYPE_ACHIVEMENT.FRIENDS:
                return m_friendDic.Count;
            default:
                Debug.LogWarning(string.Format("{0} 은 업적이 제작되지 않았습니다.", typeAchivement));
                break;
        }
        return 0;
    }


    public AccountSerial convertSeriel()
    {
        AccountSerial accountSerial = new AccountSerial();

        accountSerial.japariBread = japariBread;
        accountSerial.japariCoin = coin;
        accountSerial.cerulean = celurean;
        accountSerial.sandstar = sandstar;
        accountSerial.name = nickname;

        accountSerial.totalCerulean = m_totalCerulean;
        accountSerial.totalCoin = m_totalCoin;
        accountSerial.totalJapariBread = m_totalJapariBread;
        accountSerial.totalSandstar = m_totalSandstar;

        accountSerial.saveVersion = Application.version;

        accountSerial.friendList.Clear();

        foreach (Friend friend in m_friendDic.Values.ToList<Friend>())
        {
            FriendSerial friendSeral = friend.getSerialize();
            accountSerial.friendList.Add(friendSeral);
        }

        accountSerial.bookList.Clear();
        accountSerial.bookList = m_bookList.ToList<string>();

        accountSerial.achivementList.Clear();
        accountSerial.achivementList = m_achivementList.ToList<string>();

        return accountSerial;
    }

    /// <summary>
    /// 시리얼을 계정에 삽입
    /// </summary>
    /// <param name="accountSerial"></param>
    /// <returns></returns>
    public bool setAccount(AccountSerial accountSerial)
    {
        if (accountSerial != null)
        {

            //버전에 따라서 불러오기 다름

            m_bookList = accountSerial.bookList;
            m_japariBread = accountSerial.japariBread;
            m_coin = accountSerial.japariCoin;
            m_cerulean = accountSerial.cerulean;
            m_nickname = accountSerial.name;
            m_sandstar = accountSerial.sandstar;

            m_totalCerulean = accountSerial.totalCerulean;
            m_totalCoin = accountSerial.totalCoin;
            m_totalJapariBread = accountSerial.totalJapariBread;
            m_totalSandstar = accountSerial.totalSandstar;

            //            Debug.Log("sandstar : " + m_sandstar);

            foreach (FriendSerial friendSerial in accountSerial.friendList)
            {
                Friend friend = new Friend(accountSerial.version(), friendSerial);
                m_friendDic.Add(friend.key, friend);
            }

            m_bookList = accountSerial.bookList.ToList<string>();


            //1.4 버전 이하에서 업적이 없어 에러가 날 수 있음
            try
            {
                m_achivementList = accountSerial.achivementList.ToList<string>();

                //업적 테스트용
                //foreach(Achivement achive in AchivementManager.GetInstance.achivementDic.Values){
                //    if(!m_achivementList.Contains(achive.key)){
                //        m_achivementList.Add(achive.key);
                //    }
                //}
            }
            catch
            {
                m_achivementList = new List<string>();
            }


            //1.4 버전 이하는 토탈값을 기본적으로 가진 데이터로 초기화
            if (1.4f >= accountSerial.version())
            {
                m_totalCerulean = m_cerulean;
                m_totalCoin = m_coin;
                m_totalJapariBread = m_japariBread;
                m_totalSandstar = 0;
            }

            //1.5 버전 이하는 코인 25 증정
            if (1.5f >= accountSerial.version())
            {
                addCoin(25);
            }

            //1.6버전은 백업파일 저장


            return true;
        }
        return false;
    }
  
}
