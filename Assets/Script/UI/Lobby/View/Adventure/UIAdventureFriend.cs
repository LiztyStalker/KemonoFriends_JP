using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIAdventureFriend : UIFriendView
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            index = 0;
            maxIndex = Account.GetInstance.friendCount;
            setFriendInfo(Account.GetInstance.getFriend(index));
        }

        public override void setFriend(int index)
        {
            this.index = index;
            setFriend(Account.GetInstance.getFriend(index));
        }

        public void playFriend()
        {
            Account.GetInstance.friendKey = m_friend.key;
        }


    }
}

