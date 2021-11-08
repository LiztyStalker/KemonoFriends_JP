using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIGarden : UIPanel
    {

        enum TYPE_GARDEN { MYFRIEND, WANTED, DIC }

        SoundPlay m_soundPlayer;

        [SerializeField]
        Image m_image;

        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_contentsText;

        [SerializeField]
        Button m_selectButton;

        [SerializeField]
        Button m_wantedButton;

        [SerializeField]
        Button m_backButton;

        [SerializeField]
        UIMyFriend m_uiMyFriend;

        [SerializeField]
        UIFriendWanted m_uiFriendWanted;

        [SerializeField]
        UIFriendDictionary m_uiFriendDictionary;

        [SerializeField]
        UIGardenToggle m_uiGardenToggle;

        [SerializeField]
        Transform m_gardenPanel;

        List<UIGardenToggle> m_gardenToggleList = new List<UIGardenToggle>();

        void Start()
        {
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;

            if (m_gardenToggleList.Count == 0)
            {

                for (int i = 0; i < GardenManager.GetInstance.ElementCount; i++)
                {
                    UIGardenToggle garToggle = Instantiate(m_uiGardenToggle);
                    garToggle.transform.SetParent(m_gardenPanel);
                    garToggle.setDelegate(setAdventureInfo);
                    garToggle.setToggle(GardenManager.GetInstance.getGarden(i));
                    garToggle.GetComponent<Toggle>().group = m_gardenPanel.GetComponent<ToggleGroup>();
                    garToggle.GetComponent<Toggle>().isOn = false;
                    garToggle.transform.localScale = Vector3.one;
                    m_gardenToggleList.Add(garToggle);
                }
                m_gardenToggleList[0].GetComponent<Toggle>().isOn = true;
                m_gardenToggleList[0].OnValueChanged(true);
            }
            uiUpdate();
        }

        void setAdventureInfo(Garden garden)
        {
            m_image.sprite = garden.image;            
            m_nameText.text = garden.name;
            m_contentsText.text = garden.contents;
        }

        //void OnValueChanged(bool isOn)
        //{
        //    m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

        //    if (isOn)
        //    {
        //        for (int i = 0; i < m_gardenToggleList.Count; i++)
        //        {
        //            //해당 토글에 대한 설명
        //        }
        //    }
        //}


        public override void uiUpdate()
        {

            base.uiUpdate();
            buttonView();
        }

        public void OnSelectClicked()
        {

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

            for (int i = 0; i < m_gardenToggleList.Count; i++)
            {
                if (m_gardenToggleList[i].GetComponent<Toggle>().isOn)
                {
                    switch ((TYPE_GARDEN)i)
                    {
                        case TYPE_GARDEN.MYFRIEND:
                            m_uiMyFriend.openPanel(this);
                            break;
                        case TYPE_GARDEN.WANTED:
                            m_uiFriendWanted.openPanel(this);
                            break;
                        case TYPE_GARDEN.DIC:
                            m_uiFriendDictionary.openPanel(this);
                            break;

                    }
                }
            }
            buttonView();

        }

        void buttonView()
        {
            string panelName = UIPanelManager.GetInstance.nowPanel().GetType().Name;

            m_wantedButton.gameObject.SetActive(false);

            switch (panelName)
            {
                case "UIGarden":
                    m_selectButton.gameObject.SetActive(true);
                    break;
                case "UIFriendDictionary":
                    m_selectButton.gameObject.SetActive(false);
                    break;
                case "UIFriendWanted":
                    m_selectButton.gameObject.SetActive(false);
                    if(FriendWantedManager.GetInstance.friendList.Count > 0)
                        m_wantedButton.gameObject.SetActive(true);
                    break;
                case "UIMyFriend":
                    m_selectButton.gameObject.SetActive(false);
                    break;
            }

            if (m_uiFriendDictionary.isActiveAndEnabled || m_uiFriendWanted.isActiveAndEnabled || m_uiMyFriend.isActiveAndEnabled)
            {
                m_backButton.gameObject.SetActive(true);
            }
            else
            {
                m_backButton.gameObject.SetActive(false);
            }

        }

        public void OnBackClicked()
        {
            ((UILobby)UIPanelManager.GetInstance.root).backPanel();
            buttonView();
        }
    }
}

