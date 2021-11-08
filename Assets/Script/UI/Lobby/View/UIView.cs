using UnityEngine;
using UnityEngine.UI;

public enum TYPE_LOBBY {ADVENTURE, GARDEN, DIC}

namespace GamePanel
{
    public class UIView : MonoBehaviour
    {
        //모험하기
        [SerializeField]
        UIAdventure m_uiAdventure;

        //동산
        [SerializeField]
        UIGarden m_uiGarden;

        //책
        [SerializeField]
        UILibrary m_uiLibrary;


        public void setView(TYPE_LOBBY typeLobby)
        {

            UIPanelManager.GetInstance.moveIndex(null);

            switch (typeLobby)
            {
                case TYPE_LOBBY.ADVENTURE:
                    m_uiAdventure.openPanel(UIPanelManager.GetInstance.root);
                    break;

                case TYPE_LOBBY.GARDEN:
                    m_uiGarden.openPanel(UIPanelManager.GetInstance.root);
                    break;

                case TYPE_LOBBY.DIC:
                    m_uiLibrary.openPanel(UIPanelManager.GetInstance.root);
                    break;
                default:
                    Debug.LogWarning(string.Format("{0} 선택된 패널이 없습니다.", typeLobby));
                    break;
            }
        }


    }
}
