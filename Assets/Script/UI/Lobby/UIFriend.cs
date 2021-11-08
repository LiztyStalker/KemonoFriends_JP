using System;
using UnityEngine;
using UnityEngine.UI;


namespace GamePanel
{
    public class UIFriend : UIPanel
    {



        [SerializeField]
        Toggle[] m_toggleArray;

        [SerializeField]
        UIFriendList m_friendList;

        [SerializeField]
        UIFriendView m_friendView;

        [SerializeField]
        UIFriendView m_friendWantedView;

        //[SerializeField]
        //Image m_image;

        //[SerializeField]
        //Text m_contents;

        //[SerializeField]
        //Text m_nameText;

        //    int m_index = 0;

        public UIFriendList friendList { get { return m_friendList; } }

        void Start()
        {
            if (m_toggleArray.Length > 0)
            {
                for (int i = 0; i < m_toggleArray.Length; i++)
                    m_toggleArray[i].onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
            }
        }


        public void OnFriendWantedClicked()
        {
            m_friendWantedView.gameObject.SetActive(true);
        }

        public void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                for (int i = 0; i < m_toggleArray.Length; i++)
                {
                    if (m_toggleArray[i].isOn)
                    {
                        friendPanelView((TYPE_LOBBY)i);
                        break;
                    }
                }
            }
        }

        void friendPanelView(TYPE_LOBBY typeLobby)
        {

            m_friendList.gameObject.SetActive(false);
            m_friendWantedView.gameObject.SetActive(false);
            m_friendView.gameObject.SetActive(false);


            //switch (typeLobby)
            //{
            //    case TYPE_LOBBY.ADVENTURE:
            //        m_friendView.gameObject.SetActive(true);
            //        break;
            //    case TYPE_LOBBY.WANTED:
            //        m_friendWantedView.gameObject.SetActive(true);
            //        break;
            //    case TYPE_LOBBY.DIC:
            //        break;
            //    case TYPE_LOBBY.PLAYGROUND:
            //        break;
            //    case TYPE_LOBBY.OPT:
            //        break;
            //}


        }

        //void setFriend(int index)
        //{
        //    m_index = index;
        //    setFriend(Account.GetInstance.getFriend(m_index));
        //}

        //void setFriend(Friend friend)
        //{
        //    m_friendView.setFriendInfo(friend);
        //}



        //public void OnLeftClicked()
        //{
        //    if (m_index == 0)
        //        m_index = Account.GetInstance.friendCount;
        //    m_index--;
        //    setFriend(Account.GetInstance.getFriend(m_index));
        //}

        //public void OnRightClicked()
        //{
        //    if (m_index >= Account.GetInstance.friendCount - 1)
        //        m_index = -1;
        //    m_index++;
        //    setFriend(Account.GetInstance.getFriend(m_index));
        //}

        //public void OnListClicked()
        //{
        //    m_friendList.setIndex(m_index);
        //    m_friendList.gameObject.SetActive(true);
        //}




    }
}

