using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FriendWantedManager : SingletonClass<FriendWantedManager>
{

    List<Friend> m_friendWantedList = null;
    int m_count = 5;
//    TimeSpan m_timer = new TimeSpan(0, 5, 0);

    public int ElementCount { get { return m_friendWantedList.Count; } }

    public FriendWantedManager()
    {
        listWanted();
    }

    public void listWanted()
    {
        m_friendWantedList = FriendManager.GetInstance.getRandomFriend(m_count);
    }

    public List<Friend> friendList
    {
        get 
        { 
            return m_friendWantedList; 
        }
    }

    public string wantedFriend(Friend friend)
    {
        if (m_friendWantedList.Contains(friend))
        {
            m_friendWantedList.Remove(friend);
            return friend.key;
        }
        return null;
    }


}

