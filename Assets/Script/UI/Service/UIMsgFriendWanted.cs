using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIMsgFriendWanted : MonoBehaviour
    {
        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_coinText;

        [SerializeField]
        Image m_image;

        [SerializeField]
        Text m_contentsText;

        public void setFriendView(Friend friend)
        {
            m_nameText.text = friend.name;
            m_coinText.text = string.Format("{0}", friend.coin);
            m_image.sprite = friend.image;
            m_contentsText.text = "모집하시겠습니까?";
        }
    }
}

