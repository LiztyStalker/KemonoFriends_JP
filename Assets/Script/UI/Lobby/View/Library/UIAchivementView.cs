using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//1.5
namespace GamePanel{
	public class UIAchivementView : MonoBehaviour
	{

//        Queue<Achivement> m_achivementQueue = new Queue<Achivement>();

        [SerializeField]
        Transform m_achivementPanel;
        
        [SerializeField]
        Image m_icon;

        [SerializeField]
        Text m_contentsText;

        SoundPlay m_soundPlayer;

        const float closeMax = 175f;
        const float closeMin = 195f;

        const float openMax = 0f;
        const float openMin = 20f;

        Vector2 closeMaxVector2;
        Vector2 closeMinVector2;

        Vector2 openMaxVector2;
        Vector2 openMinVector2;


        int count = 10;

        Vector2 nowOffsetMax;
        Vector2 nowOffsetMin;

        Coroutine m_coroutine = null;

        RectTransform m_rectTransform;

        float m_timer = 0f;

        void Awake()
        {
            m_rectTransform = m_achivementPanel.GetComponent<RectTransform>();

            m_soundPlayer = GetComponent<SoundPlay>();
            if (m_soundPlayer == null)
                m_soundPlayer = gameObject.AddComponent<SoundPlay>();

            closeMaxVector2 = new Vector2(m_rectTransform.offsetMax.x, closeMax);
            closeMinVector2 = new Vector2(m_rectTransform.offsetMin.x, closeMin);

            openMaxVector2 = new Vector2(m_rectTransform.offsetMax.x, openMax);
            openMinVector2 = new Vector2(m_rectTransform.offsetMax.x, openMin);

            m_rectTransform.offsetMax = closeMaxVector2;
            m_rectTransform.offsetMin = closeMinVector2;



//            nowOffsetMax = m_achivementPanel.GetComponent<RectTransform>().offsetMax;
//            nowOffsetMin = m_achivementPanel.GetComponent<RectTransform>().offsetMin;

            m_coroutine = null;
        }


        void setAchivement(Achivement achivement)
        {


            m_icon.sprite = achivement.icon;
            m_contentsText.text = achivement.nameText;


            if (m_rectTransform == null)
            {
                Awake();
            }

//            m_rectTransform.offsetMax = closeMaxVector2;
//            m_rectTransform.offsetMin = closeMinVector2;


            if (m_coroutine != null)
                StopCoroutine(m_coroutine);

            m_coroutine = StartCoroutine(achivementCoroutine());

            m_soundPlayer.audioPlay("EffectSuccess", TYPE_SOUND.EFFECT);
           

        }

        //1.5
        //WaitForSeconds가 안 먹히는 현상으로 인해 null로 교체
        IEnumerator achivementCoroutine() {

            m_rectTransform.offsetMax = closeMaxVector2;
            m_rectTransform.offsetMin = closeMinVector2;


            while (m_timer < 3f)
            {
                //아래로 내려가야 함

                m_rectTransform.offsetMax = Vector2.Lerp(m_rectTransform.offsetMax, openMaxVector2, 0.1f);
                m_rectTransform.offsetMin = Vector2.Lerp(m_rectTransform.offsetMax, openMinVector2, 0.1f);


                m_timer += PrepClass.frameTime;
                yield return null;

            }


            m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, openMax);
            m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, openMin);


            m_timer = 0f;
            yield return null;

            while (m_timer < 3f)
            {
                m_rectTransform.offsetMax = Vector2.Lerp(m_rectTransform.offsetMax, closeMaxVector2, 0.05f);
                m_rectTransform.offsetMin = Vector2.Lerp(m_rectTransform.offsetMax, closeMinVector2, 0.05f);
                
                //위로 올라가야 함
                m_timer += PrepClass.frameTime;
                yield return null;

            }

            m_rectTransform.offsetMax = new Vector2(m_rectTransform.offsetMax.x, closeMax);
            m_rectTransform.offsetMin = new Vector2(m_rectTransform.offsetMin.x, closeMin);

            m_timer = 0f;

            m_coroutine = null;
            yield return null;
        }

        /// <summary>
        /// 업적 데이터 갱신하기
        /// 업적 등록
        /// </summary>
        public void achivementDataUpdate()
        {
            //모든 업적 구분하고
            //업적이 달성했으면 업적 달성 보이기

            for (int i = 0; i < Enum.GetValues(typeof(Achivement.TYPE_ACHIVEMENT)).Length;)
            {
                //true 업적 달성 - false 업적 미달성
                //달성한 업적 가져오기
                Achivement achivement = AchivementManager.GetInstance.getSuccessAchivement((Achivement.TYPE_ACHIVEMENT)i);

                //키가 empty가 아니면
                if (achivement != null)
                {
                    //업적 애니메이션
                    setAchivement(achivement);
                    //업적 삽입
                    Account.GetInstance.setAchivement(achivement);
                    //이펙트 발생

                }
                else
                {
                    //같은 값이 없으면 한칸 상승
                    i++;
                }
            }

        }


	}
}
