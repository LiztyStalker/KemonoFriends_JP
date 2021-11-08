using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamePanel
{

    public class UIAdventure : UIPanel
    {

        SoundPlay m_soundPlayer = null;

        [SerializeField]
        Image m_costImage;

        [SerializeField]
        Text m_costText;

        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_contentsText;

        [SerializeField]
        Text m_conditionText;

        [SerializeField]
        Text m_topScoreText;

        [SerializeField]
        Image m_topFriendImage;

        [SerializeField]
        UIAdventureFriend m_uiAdventureFriend;

        [SerializeField]
        UIAdventureReady m_uiAdventureReady;

        [SerializeField]
        Button m_selectButton;

        [SerializeField]
        Button m_startButton;

        [SerializeField]
        Button m_backButton;

        [SerializeField]
        Text m_startButtonCostText;

        [SerializeField]
        UIAdventureToggle m_adventureToggle;

        [SerializeField]
        Transform m_adventureTogglePanel;

        List<UIAdventureToggle> m_adventureToggleList = new List<UIAdventureToggle>();

        int m_cost = 0;

        int calculateCost { get { return m_cost + m_uiAdventureReady.cost; } }

        void Awake()
        {

            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;

            m_uiAdventureReady.setDelegate(buttonView);

            if (m_adventureToggleList.Count == 0)
            {

                for (int i = 0; i < AdventureManager.GetInstance.ElementCount; i++)
                {
                    UIAdventureToggle advToggle = Instantiate(m_adventureToggle);
                    advToggle.transform.SetParent(m_adventureTogglePanel);
                    advToggle.setDelegate(setAdventureInfo);
                    advToggle.setToggle(AdventureManager.GetInstance.getAdventure(i));
                    advToggle.GetComponent<Toggle>().group = m_adventureTogglePanel.GetComponent<ToggleGroup>();
                    advToggle.GetComponent<Toggle>().isOn = false;
                    advToggle.transform.localScale = Vector3.one;
                    m_adventureToggleList.Add(advToggle);
                }
                m_adventureToggleList[0].GetComponent<Toggle>().isOn = true;
                m_adventureToggleList[0].OnValueChanged(true);
            }
            uiUpdate();
        }


        void setAdventureInfo(Adventure adventure)
        {
            m_nameText.text = adventure.name;
            m_contentsText.text = adventure.contents;
            m_conditionText.text = string.Format("목표 - {0}", adventure.goal);
            m_costText.text = string.Format("{0}", -(adventure.cost));
            m_cost = adventure.cost;

            Friend topFriend = Account.GetInstance.topFriend(adventure.key);

            m_topFriendImage.sprite = topFriend.icon;
            m_topScoreText.text = string.Format("{0:d6}", topFriend.getMaxScore(adventure.key));
        }
        

        public void OnSelectClicked()
        {

            string panelName = UIPanelManager.GetInstance.nowPanel().GetType().Name;

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

            switch (panelName)
            {
                case "UIAdventure":
                    m_uiAdventureFriend.openPanel(this);

                    break;
                case "UIAdventureFriend":
                    m_uiAdventureFriend.playFriend();
                    m_uiAdventureReady.openPanel(this);
                    break;
                default:
                    m_selectButton.GetComponentInChildren<Text>().text = "프렌즈 선택";
                    break;
            }
            uiUpdate();
        }

        public void OnBackClicked()
        {
            ((UILobby)UIPanelManager.GetInstance.root).backPanel();
        }

        public override void uiUpdate()
        {
            base.uiUpdate();

            string panelName = UIPanelManager.GetInstance.nowPanel().GetType().Name;
            switch (panelName)
            {
                case "UIAdventure":
                    m_selectButton.GetComponentInChildren<Text>().text = "프렌즈 선택";
                    break;
                case "UIAdventureFriend":
                    m_selectButton.GetComponentInChildren<Text>().text = "준비";
                    break;
                case "UIAdventureReady":
                    break;
            }
            buttonView();
        }

        public void buttonView()
        {

            if (m_uiAdventureFriend.isActiveAndEnabled || m_uiAdventureReady.isActiveAndEnabled)
            {
                m_backButton.gameObject.SetActive(true);
            }
            else
            {
                m_backButton.gameObject.SetActive(false);
            }


            if (m_uiAdventureReady.isActiveAndEnabled)
            {
                m_selectButton.gameObject.SetActive(false);
                m_startButton.gameObject.SetActive(true);
                //자파리 빵 소모 삽입
                m_startButtonCostText.text = string.Format("{0}", -(calculateCost));
            }
            else
            {
                m_selectButton.gameObject.SetActive(true);
                m_startButton.gameObject.SetActive(false);
            }

        }

        public void OnStartClicked()
        {


            if (Account.GetInstance.checkJapariBread(calculateCost))
            {
                ///1~2초 있다가 씬 전환
                ///저장 후 게임 시작
                


                m_soundPlayer.audioPlay("EffectStart", TYPE_SOUND.EFFECT);
//                Account.GetInstance.getFriend(Account.GetInstance.friendKey).addCount();
                Account.GetInstance.useJapariBread(calculateCost);

                saveData();
            }
            else
            {
                m_soundPlayer.audioPlay("EffectWarning", TYPE_SOUND.EFFECT);
                ((UILobby)UIPanelManager.GetInstance.root).uiMsg.setMsg("자파리빵이 부족합니다.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
            }

        }


        void saveData()
        {

            Account.GetInstance.saveAccount(PrepClass.isOffline, saveCallback, null);

            if (PrepClass.isOffline)
            {
//                Account.GetInstance.saveData();
                nextLevel();
            }
            else
            {
                ((UILobby)UIPanelManager.GetInstance.root).uiWait.setContents("실행중");
//                GoogleApi.GetInstance.openSavedGame("friend", TYPE_FILE_ACCESS.Save, saveCallback, null);
            }
        }



        void saveCallback(bool isSave)
        {

            if (isSave)
            {
                Debug.Log("저장 성공");
                nextLevel();
            }
            else
            {
                Debug.LogError("저장 실패");
                ((UILobby)UIPanelManager.GetInstance.root).uiWait.close();
            }
        }

        void nextLevel()
        {
            Account.GetInstance.nextScene = "Game@Play";
            SceneManager.LoadScene("Game@Load");
        }

    }
}

