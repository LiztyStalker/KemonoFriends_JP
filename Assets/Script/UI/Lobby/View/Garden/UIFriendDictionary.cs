using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIFriendDictionary : UIFriendView
    {
        [SerializeField]
        Text m_conditionText;

        protected override void OnEnable()
        {
            base.OnEnable();
            index = 0;
            maxIndex = FriendManager.GetInstance.ElementCount;
            setFriendInfo(FriendManager.GetInstance.friendList[index]);
        }

        public override void setFriendInfo(Friend friend)
        {
            base.setFriendInfo(friend);
            m_conditionText.text = "";
        }

        public override void setFriend(int index)
        {
            this.index = index;
            setFriend(FriendManager.GetInstance.friendList[index]);
        }

    }
}

