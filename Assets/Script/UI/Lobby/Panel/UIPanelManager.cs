using System.Collections.Generic;
using UnityEngine;

namespace GamePanel
{
    public class UIPanelManager : SingletonClass<UIPanelManager>
    {
        UIPanel m_root;

        Stack<UIPanel> m_panelStack = new Stack<UIPanel>();
        
        public UIPanel root { get { return m_root; } }

        public void setRoot(UIPanel panel)
        {
            m_root = panel;
        }

        public int panelCount()
        {
            return m_panelStack.Count;
        }

        public UIPanel nowPanel()
        {
            return m_panelStack.Peek();
        }

        public void pushPanel(UIPanel panel)
        {
            m_panelStack.Push(panel);
//            Debug.Log("push : " + panel + " " + m_panelStack.Peek());
        }

        public void popPanel()
        {
            if(m_panelStack.Count > 0)
                m_panelStack.Pop();
        }


        /// <summary>
        /// 해당 패널로 이동하기
        /// null은 처음으로
        /// </summary>
        /// <param name="uiPanel"></param>
        /// <returns></returns>
        public UIPanel moveIndex(UIPanel uiPanel)
        {

            if (uiPanel == null)
                uiPanel = root;

            while(panelCount() > 0){
                if (uiPanel.GetType() == m_panelStack.Peek().GetType())
                {
                    return m_panelStack.Peek();
                }
                m_panelStack.Peek().closePanel();
            }
            return null;

        }
    }
}

