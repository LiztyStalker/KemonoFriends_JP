using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIFriendAbilityContents : MonoBehaviour
    {
        [SerializeField]
        Text m_contentsText;

        Coroutine m_coroutine = null;

        float m_timer = 0f;


        public void setContents(TYPE_ABILITY typeAbility)
        {
            gameObject.SetActive(true);


            switch (typeAbility)
            {
                case TYPE_ABILITY.FORCE:
                    m_contentsText.text = "완력 - 세룰리안에 대한 공격력";
                    break;
                case TYPE_ABILITY.DEX:
                    m_contentsText.text = "속도 - 프렌즈의 이동속도";
                    break;
                case TYPE_ABILITY.RANGE:
                    m_contentsText.text = "범위 - 세룰리안에 대한 범위공격";
                    break;
                case TYPE_ABILITY.FERAL:
                    m_contentsText.text = "야성 - 야생해방 지속시간 및 효과";
                    break;
                case TYPE_ABILITY.LUCK:
                    m_contentsText.text = "행운 - 좋은 블록이 나올 확률";
                    break;
            }

            if (m_coroutine == null)
                m_coroutine = StartCoroutine(contentsCoroutine());
            else
                m_timer = 1f;


        }

        IEnumerator contentsCoroutine()
        {
            m_timer = 1f;
            Debug.Log("start");
            while (m_timer > 0f)
            {
                m_timer -= PrepClass.frameTime;
                Debug.Log("end : " + m_timer);
                yield return new WaitForSeconds(PrepClass.frameTime);
            }
            Debug.Log("end : " + m_timer);

            gameObject.SetActive(false);
        }

        void OnDisable()
        {
            Debug.Log("end : " + m_timer);

            m_coroutine = null;
        }


    }
}

