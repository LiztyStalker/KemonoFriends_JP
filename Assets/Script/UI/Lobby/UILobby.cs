using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GamePanel
{
    public class UILobby : UIPanel
    {

        bool isSave = false;
        bool isQuit = false;

        SoundPlay m_soundPlayer;
        
        [SerializeField]
        UILowMenu m_uiLowMenu;

        [SerializeField]
        UIView m_uiView;

        [SerializeField]
        UIUpMenu m_uiUpMenu;

        [SerializeField]
        UIOption m_uiOption;

        [SerializeField]
        UIMsg m_uiMsg;

        [SerializeField]
        UIFriendList m_friendList;

        [SerializeField]
        UIAchivementView m_achivementView;

        [SerializeField]
        UnityEngine.UI.Text m_saveText;

        [SerializeField]
        
        UIWait m_uiWait;

        Coroutine m_coroutine = null;


        public UIFriendList uiFriendList { get { return m_friendList; } }
        public UIMsg uiMsg { get { return m_uiMsg; } }
        public UIWait uiWait { get { return m_uiWait; } }
        public SoundPlay soundPlayer { get { return m_soundPlayer; } }
        public UIAchivementView achivementView { get { return m_achivementView; } }

        void Awake()
        {
            uiWait.close();
            m_saveText.gameObject.SetActive(false);

            UIPanelManager.GetInstance.setRoot(this);

//            FriendManager.GetInstance.initInstance();

            m_uiLowMenu.setDelegate(m_uiView.setView);

            uiUpdate();
            //        for(int i = 0; i < FriendManager.GetInstance.FriendList.Length; i++){
            //            Account.GetInstance.addFriend(FriendManager.GetInstance.FriendList[i]);
            //        }

            m_soundPlayer = GetComponent<SoundPlay>();
            if (m_soundPlayer == null)
                m_soundPlayer = gameObject.AddComponent<SoundPlay>();


        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                backPanel();
            }
        }

        public override void uiUpdate()
        {
            m_uiUpMenu.uiUpdate();
            m_achivementView.achivementDataUpdate();
        }

        public void backPanel()
        {


//            if (UIPanelManager.GetInstance.nowPanel() != this)
            //대기 창이 뜨면 아무것도 하지 못함
            if (m_uiWait.gameObject.activeSelf)
                return;

            if(UIPanelManager.GetInstance.nowPanel() is UIAdventure ||
                UIPanelManager.GetInstance.nowPanel() is UIGarden ||
                UIPanelManager.GetInstance.nowPanel() is UILibrary)
            {
                OnQuitClicked();
            }
            else
            {
                m_soundPlayer.audioPlay("EffectCancel", TYPE_SOUND.EFFECT);
                UIPanelManager.GetInstance.nowPanel().closePanel();
                UIPanelManager.GetInstance.nowPanel().uiUpdate();

                //                if (UIPanelManager.GetInstance.nowPanel() == this)
                //                    m_uiLowMenu.initToggle();
            }
        }

        public void OnOptionClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            m_uiOption.openPanel(this);
        }

        public void OnQuitClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            uiMsg.setMsg("종료하시겠습니까?", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK_CANCEL, GameQuit);
        }

        void GameQuit()
        {
            OnApplicationQuit();
        }


        void OnApplicationFocus(bool isFocus)
        {
            Debug.Log("Focus : " + isFocus);
            //저장 필요
            OnApplicationPause(isFocus);
        }

        void OnApplicationPause(bool isFocus)
        {
            if (!isFocus)
            {
                Time.timeScale = 0f;
                saveData();
            }
            else
                Time.timeScale = 1f;
        }

        void OnApplicationQuit()
        {
            if (!isQuit)
            {
                m_uiWait.setContents("종료중...");
                Application.CancelQuit();
                isQuit = true;
                saveData();
            }
//            GoogleApi.GetInstance.setLogOut();
//            Account.GetInstance.saveData();
        }

        void saveData()
        {
            //저장 요청
            //저장이 시작되면 끝날 때까지 저장하지 못함



            Account.GetInstance.saveAccount(PrepClass.isOffline, saveCallBack, null);
           
            if (PrepClass.isOffline)
            {
//                Account.GetInstance.saveAccount(PrepClass.isOffline, 
                isQuit = true;
                Application.Quit();
            }
            else
            {
                if (m_coroutine == null)
                {
//                    GoogleApi.GetInstance.openSavedGame("friend", TYPE_FILE_ACCESS.Save, saveCallBack, null);
                    isSave = false;
                    m_coroutine = StartCoroutine(saveCoroutine());
                }
            }

        }

        //모든 데이터가 저장되면 자동 로그아웃

        void saveCallBack(bool success)
        {
            isSave = success;
        }

        IEnumerator saveCoroutine()
        {
            //저장 로딩 패널 열기
//            if (isQuit)
            //종료중 패널 열기
//            else
            //저장중 패널 열기
            m_saveText.gameObject.SetActive(true);

            int cnt = 0;
            bool isFlip = false;
            float time = 0f;
            float maxTime = 5f;
            while (maxTime >= time)
            {
                if (isSave)
                    break;

                m_saveText.text = "저장중";
                for (int i = 0; i < cnt; i++)
                    m_saveText.text += ".";

                if (!isFlip)
                {
                    cnt++;
                    if (cnt >= 3)
                        isFlip = !isFlip;
                }
                else
                {
                    cnt--;
                    if (cnt <= 0)
                        isFlip = !isFlip;
                }


                
                time += PrepClass.frameTime;
                yield return null;
                //저장 여부 확인 - 저장이 되지 않았으면 파일 저장
                //
            }

            m_saveText.gameObject.SetActive(false);

            if (isSave)
            {
                Debug.Log("구글 저장 완료");
            }
            else
            {
//                Account.GetInstance.saveAccount(PrepClass.isOffline, saveCallBack, null);
                Debug.Log("파일 저장 완료");
            }

            m_coroutine = null;

            if(isQuit) Application.Quit();

        }

    }
}
