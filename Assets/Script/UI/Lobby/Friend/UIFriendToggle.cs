using UnityEngine;
using UnityEngine.UI;

class UIFriendToggle : MonoBehaviour
{



    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_nameText;

    string m_friendKey;

    public string friendKey { get { return m_friendKey; } } 

    public void setFriend(Friend friend)
    {
        m_friendKey = friend.key;
        m_image.sprite = friend.image;
        m_nameText.text = friend.name;
    }


	


}

