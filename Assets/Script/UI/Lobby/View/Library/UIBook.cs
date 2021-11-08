using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIBook : UIPanel
    {
        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Image m_image;

        [SerializeField]
        Transform m_numPanel;


        List<Toggle> m_numToggleList = new List<Toggle>();

        int index;






        public void OnBackClicked()
        {
            ((UILobby)UIPanelManager.GetInstance.root).backPanel();
        }

    }
}

