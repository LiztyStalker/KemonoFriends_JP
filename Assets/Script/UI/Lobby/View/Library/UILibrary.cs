using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UILibrary : UIPanel
    {

        enum TYPE_LIBRARY { Achievement, LeaderBoard }

        List<UIBookToggle> m_bookToggleList = new List<UIBookToggle>();

        [SerializeField]
        UIBook m_uiBook;

        [SerializeField]
        Transform m_libraryList;

        [SerializeField]
        UIBookToggle m_uiBookToggle;

        [SerializeField]
        UIAchivement m_uiAchivementPanel;
        

        [SerializeField]
        Button m_selectButton;

        [SerializeField]
        Button m_cancelButton;

        [SerializeField]
        Toggle m_achivementToggle;

        [SerializeField]
        Toggle m_leaderboardToggle;


        SoundPlay m_soundPlayer;

        List<Toggle> m_toggleList = new List<Toggle>();


        void Awake()
        {

            Toggle[] toggles = m_libraryList.transform.GetComponentsInChildren<Toggle>();

            if (toggles != null)
            {
                m_toggleList.AddRange(toggles);
            }

//            m_toggleList.Add(m_achivementToggle);

//            m_achivementToggle.isOn = true;

            //오프라인이면 리더보드 보이지 않기
            if (PrepClass.isOffline)
                m_leaderboardToggle.gameObject.SetActive(false);

            uiUpdate();
        }

        
        public void OnSelectClicked()
        {

            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

            //if (m_achivementToggle.isOn)
            //{
            //    m_uiAchivementPanel.openPanel(this);
            //}
            
            
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                if (m_toggleList[i].isOn)
                {
                    try
                    {
                        openLibrary((TYPE_LIBRARY)i);
                    }
                    catch (ArgumentException e)
                    {
                        Debug.LogError("라이브러리를 찾을 수 없습니다. " + i + e.Message);
                    }
                }
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

            //string panelName = UIPanelManager.GetInstance.nowPanel().GetType().Name;
            //switch (panelName)
            //{
            //    case "UILibrary":
            //        m_selectButton.GetComponentInChildren<Text>().text = "프렌즈 선택";
            //        break;
            //    case "UIAdventureFriend":
            //        m_selectButton.GetComponentInChildren<Text>().text = "준비";
            //        break;
            //    case "UIAdventureReady":
            //        break;
            //}
            buttonView();
        }

        public void buttonView()
        {

            if(m_uiAchivementPanel.isActiveAndEnabled)
            {
                m_selectButton.gameObject.SetActive(false);
                m_cancelButton.gameObject.SetActive(true);
            }
            else
            {
                m_selectButton.gameObject.SetActive(true);
                m_cancelButton.gameObject.SetActive(false);
            }

        }


        void openLibrary(TYPE_LIBRARY typeLibrary)
        {
            switch (typeLibrary)
            {
                case TYPE_LIBRARY.Achievement:
                    if (m_achivementToggle.isOn)
                    {
                        m_uiAchivementPanel.openPanel(this);
                    }
                    break;
                case TYPE_LIBRARY.LeaderBoard:
                    GoogleAPI.GoogleApi.GetInstance.showLeaderBoard();
                    break;
            }
        }

    }
}

