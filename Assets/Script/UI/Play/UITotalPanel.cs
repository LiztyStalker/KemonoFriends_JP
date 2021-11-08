using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITotalPanel : MonoBehaviour
{
    SoundPlay m_soundPlayer;

    [SerializeField]
    Text m_addFoodText;

    [SerializeField]
    Text m_addCoinText;

    [SerializeField]
    Text m_addCeruleanText;

    [SerializeField]
    Text m_accFoodText;

    [SerializeField]
    Text m_accCoinText;

    [SerializeField]
    Text m_accCeruleanText;

    [SerializeField]
    Image m_topFriendImage;

    [SerializeField]
    Text m_topScoreText;

    [SerializeField]
    Transform m_newScoreText;

    Field m_field;

    public void uiUpdate(Field field, int addBread)
    {


        m_field = field;

        m_addFoodText.text = string.Format("+{0}", addBread);
        m_addCoinText.text = string.Format("+{0}", field.coin);
        m_addCeruleanText.text = string.Format("+{0}", field.cerulean);


        //개인 플레이 최고 점수 가져오기
        string adventureKey = field.adventure.key;
        Friend topFriend = Account.GetInstance.topFriend(adventureKey);

        m_newScoreText.gameObject.SetActive(false);

        //게임에 실패했으면 점수가 기록되지 않음
        if (!m_field.isDefeat)
        {
            if (topFriend.getMaxScore(adventureKey) < field.score)
            {
                StartCoroutine(newScoreCoroutine());
                topFriend = field.friend;
            }

            //플레이한 프렌즈에 최고 점수 삽입하기
            Friend nowFriend = Account.GetInstance.getFriend(field.friend.key);
            nowFriend.setMaxScore(adventureKey, field.score);
        }
        


        m_topFriendImage.sprite = topFriend.icon;
        m_topScoreText.text = string.Format("{0:d6}", topFriend.getMaxScore(adventureKey));



        Account.GetInstance.addJapariBread(addBread);
        Account.GetInstance.addCoin (field.coin);
        Account.GetInstance.addCerulean(field.cerulean);
        Account.GetInstance.addSandstar(field.sandstar);
        
        m_accCoinText.text = string.Format("{0}", Account.GetInstance.coin);
        m_accFoodText.text = string.Format("{0}", Account.GetInstance.japariBread);
        m_accCeruleanText.text = string.Format("{0}", Account.GetInstance.celurean);

        Debug.Log("is saving");
        // 저장하기
       

    }

     


    /// <summary>
    /// 리더보드 콜백 
    /// 1.6 생성
    /// </summary>
    /// <param name="success"></param>
    void leaderboardCallback(bool success)
    {
        Debug.Log("leaderboard : " + success);
        
        //1.6 리더보드 보기
        GoogleAPI.GoogleApi.GetInstance.showLeaderBoard(m_field.adventure.leaderboardKey);

        //if(success)
        //    //삽입 완료
        //else
        //    //삽입 실패

    }

    IEnumerator newScoreCoroutine()
    {

        float height = 2 * Camera.main.orthographicSize * 0.5f;
        float width = height * Camera.main.aspect * 0.5f;
        Debug.Log("new : " + height + " " + width);

        int count = 5;

        m_soundPlayer = GetComponent<SoundPlay>();
        if (m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();
        
        m_soundPlayer.audioPlay("EffectNewScore", TYPE_SOUND.EFFECT);
        //축하 사운드

        m_newScoreText.gameObject.SetActive(true);

        while (count > 0)
        {
            ParticleSystem particle = (ParticleSystem)Instantiate(ParticleManager.GetInstance.getParticle("Particle@NewScore"));
            SoundPlay parSoundPlay = particle.gameObject.AddComponent<SoundPlay>();
            parSoundPlay.audioPlay("EffectFireWorks", TYPE_SOUND.EFFECT);

//            parSoundPlay.audioPlay("EffectTime", TYPE_SOUND.EFFECT);
            
            float posX = Random.RandomRange(-width, width);
            float posY = Random.RandomRange(0f, height);



            particle.transform.position = new Vector3(posX, posY, 0f);
//            Debug.Log("newScore : " + particle.transform.position);
            count--;
            yield return new WaitForSeconds(0.33f);
        }


        //1.6 리더보드에 삽입
        GoogleAPI.GoogleApi.GetInstance.reportScore(m_field.adventure.leaderboardKey, (long)m_field.score, leaderboardCallback);


        yield return new WaitForSeconds(1f);
    }


//    IEnumerator totalResultCoroutine()
//    {

//        //Friend topFriend = Account.GetInstance.topFriend();

//        int addFood = Account.GetInstance.japariBread;
//        int addCoin = Account.GetInstance.coin;
//        int addCerulean = Account.GetInstance.celurean;

//        //if (topFriend != null)
//        //{
//        //    m_topFriendImage.sprite = topFriend.icon;
//        //    m_topScoreText.text = string.Format("{0}", topFriend.maxScore);
//        //}

//        m_accFoodText.text = string.Format("{0}", addFood);
//        m_accCoinText.text = string.Format("{0}", addCoin);
//        m_accCeruleanText.text = string.Format("{0}", addCerulean);



//        m_addFoodText.text = string.Format("{0}", m_bonasFood);
//        m_addCoinText.text = string.Format("{0}", m_coin);
//        m_addCeruleanText.text = string.Format("{0}", m_cerulean);




//        m_increaseCoin = (int)((float)m_bonasFood / m_countTime * 0.1f);
//        m_increaseFood = (int)((float)m_coin / m_countTime * 0.1f);
//        m_increaseCerulean = (int)((float)m_cerulean / m_countTime * 0.1f);




//        float time = m_countTime;
//        while (time > 0f)
//        {


//            addFood += m_increaseFood;
//            setValue(m_addFoodText, m_food);

//            addCoin += m_increaseCoin;
//            setValue(m_addCoinText, m_coin);

//            addCerulean += m_increaseCerulean;
//            setValue(m_addCeruleanText, m_cerulean);


//            time -= 0.1f;
//            yield return new WaitForSeconds(0.1f);
//        }


////        Account.GetInstance.addJapariBread(m_bonasFood);
////        Account.GetInstance.addCoin(m_coin);
////        Account.GetInstance.addCerulean(m_cerulean);

//        yield return new WaitForSeconds(0.1f);
//    }
}

