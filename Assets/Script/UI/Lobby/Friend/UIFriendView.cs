using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public abstract class UIFriendView : UIPanel
    {

        protected Friend m_friend;

        protected SoundPlay m_soundPlayer;

        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_contentsText;

        [SerializeField]
        Image m_image;

        [SerializeField]
        Text m_orderText;

        [SerializeField]
        Text m_familyText;

        [SerializeField]
        Text m_genusText;

        [SerializeField]
        Text m_countText;

        [SerializeField]
        UIFriendAbility m_friendAbility;

        [SerializeField]
        Button m_toggleBtn;

        int m_index = 0;
        int m_maxIndex = 0;


        protected int index { get { return m_index; } set { m_index = value; } }
        protected int maxIndex { get { return m_maxIndex; } set { m_maxIndex = value; } }

        public abstract void setFriend(int index);

        void Start()
        {
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;

        }

        protected void setFriend(Friend friend)
        {
            setFriendInfo(friend);
        }
                
        public void OnLeftClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            if (m_index == 0)
                index = maxIndex;
            index--;
            setFriend(index);
        }

        public void OnRightClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            if (index >= maxIndex - 1)
                index = -1;
            index++;
            setFriend(index);
        }

        public virtual void OnListClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            ((UILobby)(UIPanelManager.GetInstance.root)).uiFriendList.friendListView(this, index);
            ((UILobby)(UIPanelManager.GetInstance.root)).uiFriendList.openPanel(this);
        }

        public virtual void setFriendInfo(Friend friend)
        {
            this.m_friend = friend;

            if(m_friendAbility != null)
                m_friendAbility.setFriendInfo(friend);

            m_nameText.text = friend.name;
            m_image.sprite = friend.image;

            m_contentsText.text = friend.contents;
            m_orderText.text = friend.orderGroup;
            m_familyText.text = friend.familyGroup;
            m_genusText.text = friend.genusGroup;

            if (m_countText != null)
                m_countText.text = string.Format("{0:d4}", friend.count);

            //if (m_toggleBtn != null && m_typeLobby != TYPE_LOBBY.WANTED)
            //{
            //    m_toggleBtn.GetComponentInChildren<Text>().text = "능력치";
            //    m_frieldAbility.gameObject.SetActive(false);
            //}

        }

        public void OnAbilityClicked()
        {

            if (m_friendAbility != null)
            {
                m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

                if (!m_friendAbility.isActiveAndEnabled)
                {
                    m_toggleBtn.GetComponentInChildren<Text>().text = "능력치";
                    m_friendAbility.setFriendInfo(m_friend);
                    m_friendAbility.gameObject.SetActive(true);
                }
                else
                {
                    m_toggleBtn.GetComponentInChildren<Text>().text = "정보";
                    m_friendAbility.gameObject.SetActive(false);
                }
            }
        }

        public virtual void OnSelectClicked(){}



    }
}

