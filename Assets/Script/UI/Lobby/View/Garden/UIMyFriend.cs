using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamePanel
{
    public class UIMyFriend : UIFriendView
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
    }
}

