using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIFriendList : UIPanel
    {

        SoundPlay m_soundPlayer;


        [SerializeField]
        UIFriendToggle m_friendToggle;

        [SerializeField]
        Transform m_friendListPanel;

        List<UIFriendToggle> m_friendToggleList = new List<UIFriendToggle>();

        int m_Index = -1;

        string m_friendViewName;


        public void friendListView(UIFriendView uiFriendView, int index)
        {
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;


            m_Index = index;
            string friendViewName = uiFriendView.GetType().Name;

            switch (friendViewName)
            {
                case "UIAdventureFriend":
                    adventureView();
                    break;
                case "UIFriendDictionary":
                    dictionaryView();
                    break;
                case "UIFriendWanted":
                    wantedView();
                    break;
                case "UIMyFriend":
                    myFriendView();
                    break;
            }




        }




        void adventureView()
        {
            for (int i = 0; i < Account.GetInstance.friendCount; i++)
            {
                friendCreate(Account.GetInstance.getFriend(i));
            }
            initToggleButton();
        }

        void wantedView()
        {

            for (int i = 0; i < FriendWantedManager.GetInstance.ElementCount; i++)
            {
                friendCreate(FriendWantedManager.GetInstance.friendList[i]);
            }
            initToggleButton();
        }

        void dictionaryView()
        {
            for (int i = 0; i < FriendManager.GetInstance.ElementCount; i++)
            {
                friendCreate(FriendManager.GetInstance.friendList[i]);
            }
            initToggleButton();
        }

        void myFriendView()
        {
            for (int i = 0; i < Account.GetInstance.friendCount; i++)
            {
                friendCreate(Account.GetInstance.getFriend(i));
            }
            initToggleButton();
        }

        void initToggleButton()
        {

            for (int i = 0; i < m_friendToggleList.Count; i++)
                m_friendToggleList[i].GetComponent<Toggle>().isOn = false;


            if (m_Index == -1)
                m_Index = 0;

            m_friendToggleList[m_Index].GetComponent<Toggle>().isOn = true;
        }

        void friendCreate(Friend friend)
        {
            UIFriendToggle friendToggle = Instantiate(m_friendToggle);
            friendToggle.setFriend(friend);
            friendToggle.transform.SetParent(m_friendListPanel);
            friendToggle.GetComponent<Toggle>().isOn = false;
            friendToggle.GetComponent<Toggle>().onValueChanged.AddListener((isOn) => OnToggleClicked(isOn));
            friendToggle.GetComponent<Toggle>().group = m_friendListPanel.GetComponent<ToggleGroup>();
            friendToggle.transform.localScale = Vector2.one;
            m_friendToggleList.Add(friendToggle);
        }


        public void OnToggleClicked(bool isOn)
        {

            if (isOn)
            {
                m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
                for (int i = 0; i < m_friendToggleList.Count; i++)
                    if (m_friendToggleList[i].GetComponent<Toggle>().isOn)
                    {
                        m_Index = i;
                    }
            }
        }


        public void OnSelectClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            if (parent != null)
                ((UIFriendView)parent).setFriend(m_Index);
            closePanel();
        }

        public void OnCloseClicked()
        {
            closePanel();
        }

        protected override void OnDisable()
        {
            int count = m_friendToggleList.Count - 1;
            for(int i = count; i >= 0; i--){
                Destroy(m_friendToggleList[i].gameObject);
            }
            m_friendToggleList.Clear();
            base.OnDisable();
        }


    }
}

