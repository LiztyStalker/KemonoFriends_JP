using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIFriendWanted : UIFriendView
    {

        [SerializeField]
        Text m_wantedText;

        [SerializeField]
        Transform m_emptyFriendPanel;

        [SerializeField]
        Transform m_wantedBtn;


        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;
            initFriendWanted();
        }

        void initFriendWanted()
        {
            index = 0;
            if (FriendWantedManager.GetInstance.friendList.Count > 0)
            {
                maxIndex = FriendWantedManager.GetInstance.ElementCount;
                setFriendInfo(FriendWantedManager.GetInstance.friendList[index]);
                m_emptyFriendPanel.gameObject.SetActive(false);
                m_wantedBtn.gameObject.SetActive(true);
            }
            else
            {
                m_emptyFriendPanel.gameObject.SetActive(true);
                m_wantedBtn.gameObject.SetActive(false);
            }
        }

        public override void OnSelectClicked()
        {

            if (Account.GetInstance.checkCoin(100))
            {
                //고용하시겠습니까?
                m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
                ((UILobby)(UIPanelManager.GetInstance.root)).uiMsg.setMsg(m_friend, TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, friendWanted);
            }
            else
            {
                //고용 불가 자파리코인 더 필요
                m_soundPlayer.audioPlay("EffectWarning", TYPE_SOUND.EFFECT);
                ((UILobby)(UIPanelManager.GetInstance.root)).uiMsg.setMsg("자파리코인이 부족합니다.", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
            }
        }

        public override void setFriend(int index)
        {
            this.index = index;
            setFriend(FriendWantedManager.GetInstance.friendList[index]);
            m_wantedText.text = string.Format("{0}", m_friend.coin);
        }

        void friendWanted()
        {
            //고용완료
            string key = FriendWantedManager.GetInstance.wantedFriend(m_friend);

            if (key != null)
            {
                m_soundPlayer.audioPlay("EffectSuccess", TYPE_SOUND.EFFECT);

                Account.GetInstance.addFriend(m_friend.key);
                Account.GetInstance.useCoin(m_friend.coin);
                UIPanelManager.GetInstance.root.uiUpdate();
                //리스트 갱신
//                Account.GetInstance.saveData();
                initFriendWanted();

            }
            else
            {
                ((UILobby)(UIPanelManager.GetInstance.root)).uiMsg.setMsg(string.Format("{0} : 알 수 없는 오류가 나타났습니다.", m_friend.name), TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
                Debug.LogError(string.Format("{0} : 고용하는 도중 프렌즈를 찾을 수 없습니다.", m_friend.key));
            }
        }
    }
}

