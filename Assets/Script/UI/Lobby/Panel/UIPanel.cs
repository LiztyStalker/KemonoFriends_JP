using UnityEngine;

namespace GamePanel
{
    public class UIPanel : MonoBehaviour
    {


//        UIPanelManager m_uiPanelManager;
        UIPanel m_parent;

        protected UIPanel parent { get { return m_parent; } }

        protected virtual void OnEnable()
        {
            UIPanelManager.GetInstance.pushPanel(this);
        }

        protected virtual void OnDisable()
        {
            UIPanelManager.GetInstance.popPanel();
            uiUpdate();
        }
        

        //protected void setPanelManager(UIPanelManager panelManager)
        //{
        //    m_uiPanelManager = panelManager;
        //}
       
        public virtual void uiUpdate()
        {
            if (parent != null)
                parent.uiUpdate();
        }


        public virtual void openPanel(UIPanel parent)
        {
            m_parent = parent;
            gameObject.SetActive(true);
        }

        public virtual void closePanel()
        {
            gameObject.SetActive(false);
        }

    }
}

