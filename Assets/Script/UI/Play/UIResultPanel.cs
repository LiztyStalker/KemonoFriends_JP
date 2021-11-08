using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIResultPanel : MonoBehaviour
{

    const float m_countTime = 1f;

    //증가량
    int m_increaseFood;
    int m_increaseCombo;
    int m_increaseCoin;
    int m_increaseCerulean;

    //얻을 데이터
    int m_addBread;

//    bool m_isRun = true;

    public int addBread { get { return m_addBread; } }

    //얻은 데이터
    int m_food;
    int m_combo;
    int m_coin;
    int m_cerulean;


    //실제 얻는 데이터
    int m_bonasCombo;
    int m_bonasFood;
    int m_bonasCerulean;
    int m_bonasCoin;

    [SerializeField]
    Text m_addBreadText;

    [SerializeField]
    Text m_totalFoodText;

    [SerializeField]
    Text m_totalComboText;

    [SerializeField]
    Text m_totalCoinText;

    [SerializeField]
    Text m_totalCeruleanText;

    [SerializeField]
    Text m_bonasFoodText;
    
    [SerializeField]
    Text m_bonasComboText;
    
    [SerializeField]
    Text m_bonasCoinText;

    [SerializeField]
    Text m_bonasCeruleanText;




    public void uiUpdate(Field field)
    {


        m_food = field.food;
        m_combo = field.maxCombo;
        m_coin = field.coin;
        m_cerulean = field.cerulean;


        //각 어드벤처에 따라 다름
        m_bonasCoin = field.adventure.getJapariBread(AdventureActor.TYPE_JAPARIRATE.COIN, m_coin);
        m_bonasFood = field.adventure.getJapariBread(AdventureActor.TYPE_JAPARIRATE.BREAD, m_food);
        m_bonasCombo = field.adventure.getJapariBread(AdventureActor.TYPE_JAPARIRATE.COMBO, m_combo);
        m_bonasCerulean = field.adventure.getJapariBread(AdventureActor.TYPE_JAPARIRATE.CERULEAN, m_cerulean);

        //패배시 보너스 절반
        if(field.isDefeat){
            m_bonasCoin /= 2;
            m_bonasFood /= 2;
            m_bonasCombo /= 2;
            m_bonasCerulean /= 2;
        }


        m_addBread = m_bonasCoin + m_bonasFood + m_bonasCombo + m_bonasCerulean;

        m_totalComboText.text = string.Format("{0}", m_combo);
        m_totalFoodText.text = string.Format("{0}", m_food);
        m_totalCoinText.text = string.Format("{0}", m_coin);
        m_totalCeruleanText.text = string.Format("{0}", m_cerulean);

        m_bonasComboText.text = string.Format("{0}", m_bonasCombo);
        m_bonasFoodText.text = string.Format("{0}", m_bonasFood);
        m_bonasCoinText.text = string.Format("{0}", m_bonasCoin);
        m_bonasCeruleanText.text = string.Format("{0}", m_bonasCerulean);

        m_addBreadText.text = string.Format("{0}", m_addBread);

        //m_increaseCoin = (int)((float)field.coin / m_countTime * 0.1f);
        //m_increaseFood = (int)((float)field.food / m_countTime * 0.1f);
        //m_increaseCombo = (int)((float)field.combo / m_countTime * 0.1f);
        //m_increaseCerulean = (int)((float)field.cerulean / m_countTime * 0.1f);

        //StartCoroutine(resultCoroutine(field));
    }



    IEnumerator resultCoroutine(Field field)
    {
        float time = m_countTime;
        while (time > 0f)
        {
            setValue(m_totalComboText, m_combo);
            m_combo += m_increaseCombo;

            setValue(m_totalFoodText, m_food);
            m_food += m_increaseFood;

            setValue(m_totalCoinText, m_coin);
            m_coin += m_increaseCoin;

            setValue(m_totalCeruleanText, m_cerulean);
            m_cerulean += m_increaseCerulean;


            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        setValue(m_totalComboText, m_combo);
        setValue(m_totalFoodText, field.food);
        setValue(m_totalCoinText, field.coin);
        setValue(m_totalCeruleanText, field.cerulean);



        ///////////////////////////////////////////////////////////////////




        m_bonasCoin = (int)((float)field.coin * 10);
        m_bonasFood = (int)((float)(field.food) * 0.1f);
        m_bonasCombo = field.combo;
        m_bonasCerulean = (int)((float)(field.cerulean) * 0.5f);


        m_increaseCoin = (int)((float)m_bonasCoin / m_countTime * 0.1f);
        m_increaseCerulean = (int)((float)m_bonasCerulean / m_countTime * 0.1f);
        m_increaseFood = (int)((float)m_bonasFood / m_countTime * 0.1f);
        m_increaseCombo = (int)((float)m_bonasCombo / m_countTime * 0.1f);



        setValue(m_bonasComboText, m_bonasCombo);
        yield return new WaitForSeconds(0.5f);

        setValue(m_bonasFoodText, m_bonasFood);
        yield return new WaitForSeconds(0.5f);

        setValue(m_bonasCoinText, m_bonasCoin);
        yield return new WaitForSeconds(0.5f);

        setValue(m_bonasCeruleanText, m_bonasCerulean);
        yield return new WaitForSeconds(0.5f);
    

        time = m_countTime;

        while (time > 0f)
        {

            if (m_bonasCerulean - m_increaseCerulean < 0)
            {
                m_bonasFood += m_bonasCerulean;
                m_bonasCerulean = 0;
            }
            else
            {
                m_bonasCerulean -= m_increaseCerulean;
                m_bonasFood += m_increaseCerulean;
            }

            setValue(m_bonasCeruleanText, m_bonasCerulean);
            setValue(m_bonasFoodText, m_bonasFood);

            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }


        if (m_bonasCerulean - m_increaseCerulean < 0)
        {
            m_bonasFood += m_bonasCerulean;
            m_bonasCerulean = 0;
        }
        else
        {
            m_bonasCerulean -= m_increaseCerulean;
            m_bonasFood += m_increaseCerulean;
        }
        setValue(m_bonasCeruleanText, m_bonasCerulean);
        setValue(m_bonasFoodText, m_bonasFood);


        time = m_countTime;
        
        while (time > 0f)
        {

            if (m_bonasCerulean - m_increaseCerulean < 0)
            {
                m_bonasFood += m_increaseCoin;
                m_bonasCoin = 0;
            }
            else
            {
                m_bonasFood += m_increaseCoin;
                m_bonasCoin -= m_increaseCoin;
            }


            setValue(m_bonasCoinText, m_bonasCoin);
            setValue(m_bonasFoodText, m_bonasFood);
            
            time -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }

        if (m_bonasCerulean - m_increaseCerulean < 0)
        {
            m_bonasFood += m_increaseCoin;
            m_bonasCoin = 0;
        }
        else
        {
            m_bonasFood += m_increaseCoin;
            m_bonasCoin -= m_increaseCoin;
        }


        setValue(m_bonasCoinText, m_bonasCoin);
        setValue(m_bonasFoodText, m_bonasFood);



        /////////////////////////////////////////

        //
//        m_isRun = false;
   
    }

    void setValue(Text text, int maxValue)
    {
        text.text = string.Format("{0}", maxValue);
    }



}






