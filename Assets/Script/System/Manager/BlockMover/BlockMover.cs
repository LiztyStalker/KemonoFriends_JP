using System.Collections;
using UnityEngine;


public enum TYPE_ARROW
{
    RIGHT = 1,
    DOWN = 2,
    LEFT = 4,
    UP = 8
}



public class BlockMover : MonoBehaviour
{

    public enum TYPE_USER { PLAYER, CPU }

    SoundPlay m_soundPlayer;

    [SerializeField]
    SpriteRenderer m_spriteRenderer;

    [SerializeField]
    SpriteRenderer m_coverSpriteRenderer;

    [SerializeField]
    ParticleSystem m_feralParticle;

    [SerializeField]
    ParticleSystem m_jumpParticle;
    
    Block m_block;

    Field m_field;

    IBlockMover m_iBlockMover;

    BlockActor m_nowBlockActor;
    BlockActor m_nextBlockActor;

    TYPE_ARROW m_typeArrow = TYPE_ARROW.RIGHT;
    TYPE_USER m_typeUser;

    bool m_isAuto = true;
    bool m_isStun = false;

    public bool isAuto { get { return m_isAuto; } }
    public bool isStun { get { return m_isStun; } }
    public TYPE_ARROW typeArrow { get { return m_typeArrow; } set { m_typeArrow = value; } }
    public BlockActor[][] blockActorField { get { return m_field.blockActorField; } }

    public TYPE_USER typeUser { get { return m_typeUser; } }
    public bool isFeral { get { return m_field.isFeral; } }
    public Friend friend { get { return m_field.friend; } }
    public Block block { get { return m_block; } }

    public BlockActor nowBlockActor { get { return m_nowBlockActor; } set { m_nowBlockActor = value; } }
    public BlockActor nextBlockActor { get { return m_nextBlockActor; } set { m_nextBlockActor = value; } }

    public ParticleSystem feralParticle { get { return m_feralParticle; } }
    public ParticleSystem jumpParticle { get { return m_jumpParticle; } }

    //블록 행동자 클래스 제작 - 해당 클래스에 따라서 움직임

    void Start()
    {

        m_soundPlayer = gameObject.GetComponent<SoundPlay>();
        if (m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();

        setFeral(false);
        setJump(false);
//        m_defaultSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (m_typeUser == TYPE_USER.PLAYER) 
            angleView(m_typeArrow);
    }


    public void setDefeat()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void setBlock(Block block)
    {
        m_block = block;
        m_coverSpriteRenderer.sprite = block.icon;
//        setIcon(block.icon);
    }

    public void setIcon(Sprite sprite)
    {
        m_spriteRenderer.sprite = sprite;
    }

    public void setField(Field field)
    {
        m_field = field;

        m_iBlockMover = field.adventure.createBlockMover();
        m_isAuto = m_iBlockMover.autoCheck();
        //if (field.adventure.adventureType == typeof(AdventureDrive))
        //    isAuto = false;

        //해당 어드벤처에 대하여 행동자 클래스 생성
    }

    public void setSize(float size)
    {
        m_iBlockMover.setSize(size);
    }

    public void initUser(TYPE_USER typeUser)
    {
        m_typeUser = typeUser;
    }

    public void initPosition(BlockActor blockActor)
    {
        m_nowBlockActor = blockActor;
        transform.position = blockActor.transform.position;
    }



    public void setArrowChange(int selector)
    {


        m_iBlockMover.setArrowChange(this, selector, m_typeArrow);
        angleView(m_typeArrow);
//        setArrowChange(m_iBlockMover.setArrowChange(selector, m_typeArrow));

//        if (m_field.adventure is AdventureDrive)

        //행동자에 따라서 움직이기
        //if(m_field.adventure.adventureType == typeof(AdventureDrive))
        //{
        //    if (selector > 0)
        //        setArrowChange(TYPE_ARROW.RIGHT);
        //    else if (selector < 0)
        //        setArrowChange(TYPE_ARROW.LEFT);

        //}
        //else if (m_field.adventure.adventureType == typeof(AdventureExplore))
        //{
        //    setArrowChange(getArrow(selector));
        //}
    }


    /// <summary>
    /// 이동 변경
    /// </summary>
    //void setArrowChange(TYPE_ARROW typeArrow)
    //{
    //    if (m_typeUser == TYPE_USER.PLAYER)
    //    {
    //        m_typeArrow = typeArrow;

    //        angleView(typeArrow);
           
    //    }

    //    if (!m_isAuto)
    //    {
    //        m_iBlockMover.getNextBlockActor(this, true);
    //    }

    //}

    void angleView(TYPE_ARROW typeArrow)
    {
        transform.eulerAngles = new Vector3(0f, 0f, m_iBlockMover.angleView(typeArrow));


        //행동자에 따라서 움직임 
////        if (m_field.adventure is AdventureExplore)
        //if (m_field.adventure.adventureType == typeof(AdventureExplore))
        //{
        //    switch (m_typeArrow)
        //    {
        //        case TYPE_ARROW.RIGHT:
        //            transform.eulerAngles = new Vector3(0f, 0f, 270f);
        //            break;
        //        case TYPE_ARROW.DOWN:
        //            transform.eulerAngles = new Vector3(0f, 0f, 180f);
        //            break;
        //        case TYPE_ARROW.LEFT:
        //            transform.eulerAngles = new Vector3(0f, 0f, 90f);
        //            break;
        //        case TYPE_ARROW.UP:
        //            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //            break;

        //    }
        //}
    }

    //TYPE_ARROW getArrow(int selector)
    //{

    //    int typeArrow = (int)m_typeArrow;
    //    //오른쪽
    //    if (selector > 0)
    //    {
    //        typeArrow = typeArrow * 2;
    //    }
    //    //왼쪽
    //    else if (selector < 0)
    //    {
    //        typeArrow = typeArrow / 2;
    //    }

    //    if (typeArrow == 0)
    //        typeArrow = (int)TYPE_ARROW.UP;
    //    else if (typeArrow > (int)TYPE_ARROW.UP)
    //        typeArrow = (int)TYPE_ARROW.RIGHT;

    //    return (TYPE_ARROW)typeArrow;
    //}
    

    //TYPE_ARROW getReverseArrow(TYPE_ARROW typeArrow)
    //{
    //    int revArrow = (int)typeArrow * 4;
    //    if (revArrow > (int)TYPE_ARROW.UP)
    //        revArrow /= 16;
    //    return (TYPE_ARROW)revArrow;
    //}



    void FixedUpdate()
    {
        if (m_field.isGameRun)
        {
            m_iBlockMover.moverController(m_field, this);

            //if (m_field.adventure.adventureType == typeof(AdventureFootball))
            //{

            //    //가속도
            //    Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

            //    //줄여나가야 함

            //    //최소 속도까지




            //    //스크린 밖으로 나가면 게임 오버
            //    //
            //    Vector3 nowPos = Camera.main.WorldToViewportPoint(transform.position);

            //    //        Debug.Log(nowPos);

            //    if (!m_field.isFeral)
            //    {
            //        if (nowPos.x <= -0.1f || nowPos.x >= 1.1f)
            //        {
            //            m_field.setDefeat();
            //        }
            //        if (nowPos.y <= 0.15f || nowPos.y >= 0.95f)
            //        {
            //            m_field.setDefeat();
            //        }
            //    }
            //    else
            //    {
            //        Vector2 dir = GetComponent<Rigidbody2D>().velocity;

            //        if (nowPos.x <= 0f || nowPos.x >= 1f)
            //            dir.x = -dir.x;

            //        if (nowPos.y <= 0.2f || nowPos.y >= 0.9f)
            //            dir.y = -dir.y;

            //        GetComponent<Rigidbody2D>().velocity = dir;
            //    }
            //}

            //else
            //{

            //    setFeral(m_field.isFeral);



                //다음 블록이 있는가?
                //없으면
                //현재 블록에서 진행하는 방향에 벽이 있는가?
                //있으면 다음 블록 null
                //없으면
                //해당 방향이 맵 끝인가?
                //아니면
                //다음 블록 임시로 가져오기
                //다음 블록에서 진행하는 방향 반대쪽에 벽이 있는가?
                //있으면 다음블록 null
                //없으면 다음블록 등록


                //다음 블록이 있으면
                //다음 블록과 가까워졌는가?
                //가까우면 - 현재 블록으로 교체
                //다음 블록 null
                //가깝지 않으면 반복

                //if (m_typeUser == TYPE_USER.CPU)
                //{
                //    m_typeArrow = randomArrow();
                //}


                ////아이템 습득 - 충돌체에서 이벤트 발생
                //catchBlock(m_nowBlockActor, m_nextBlockActor);


                //if (m_iBlockMover.getNextBlockActor(this, isAuto) != null)
                //{
                //    m_iBlockMover.moveAction(this);
                //}






                    //    if (m_typeUser == TYPE_USER.PLAYER)
                    //    {
                    //        float speed = (m_field.isFeral) ? 0.1f * m_field.friend.realDex * 1.5f : 0.1f * m_field.friend.realDex;
                    //        transform.position = Vector3.Lerp(transform.position, m_nextBlockActor.transform.position, speed);

                    //    }
                    //    else
                    //        transform.position = Vector3.Lerp(transform.position, m_nextBlockActor.transform.position, 0.05f);

                    //    //점프 상태이면
                    //    if (m_jumpParticle.gameObject.activeSelf)
                    //    {

                    //        float nowDistance = Vector2.Distance(m_nowBlockActor.transform.position, transform.position);
                    //        float nextDistance = Vector2.Distance(m_nextBlockActor.transform.position, transform.position);
                    //        float totalDistance = Vector2.Distance(m_nextBlockActor.transform.position, m_nowBlockActor.transform.position);
                    //        float rate;

                    //        if (nextDistance > nowDistance)
                    //        {
                    //            //확대
                    //            rate = (nowDistance / totalDistance) * 1.5f + 1f;

                    //        }
                    //        else
                    //        {
                    //            //축소
                    //            rate = (nextDistance / totalDistance) * 1.5f + 1f;
                    //        }

                    //        transform.localScale = new Vector3(m_defaultSize.x * rate, m_defaultSize.y * rate, 1f);
                    //    }


                    //    //목표에 도착했으면
                    //    if (Vector2.Distance(transform.position, m_nextBlockActor.transform.position) < 0.075f)
                    //    {
                    //        transform.position = m_nextBlockActor.transform.position;
                    //        GetComponent<Collider2D>().enabled = true;
                    //        m_nowBlockActor = m_nextBlockActor;
                    //        m_nextBlockActor = null;

                    //        if(m_jumpParticle.gameObject.activeSelf)
                    //            setJump(false);

                    //    }
            
        }
    }

   


    public void setFeral(bool isFeral)
    {

        if (m_typeUser == TYPE_USER.PLAYER)
        {
            m_iBlockMover.setFeral(this, isFeral);


            //                Debug.LogWarning(string.Format("{0}{1}", gameObject.name, m_field));

            //                if (m_field.adventure is AdventureDrive)
            //if(m_field.adventure.adventureType == typeof(AdventureDrive))
            //{
            //    if (isFeral)
            //    {
            //        transform.localScale = new Vector3(2f, 2f, 2f);
            //        var particle = m_feralParticle.main;
            //        particle.startSizeMultiplier = 4f;
            //    }

            //    else
            //    {
            //        transform.localScale = m_defaultSize;
            //        var particle = m_feralParticle.main;
            //        particle.startSizeMultiplier = 2f;
            //    }
            //}
            //            }
            //        }
        }
    }

    void setJump(bool isJump)
    {
        m_iBlockMover.setJump(this, isJump);
    //    if (m_jumpParticle != null)
    //    {
    //        m_jumpParticle.gameObject.SetActive(isJump);

    //        if (isJump)
    //            m_soundPlayer.audioPlay("EffectJump", TYPE_SOUND.EFFECT);
    //        else
    //            transform.localScale = m_iBlockMover.defaultSize;

    //    }
    }

    //BlockActor getNextBlockActor(bool isAuto)
    //{
    //    if (!isAuto)
    //        return m_nextBlockActor;

    //    if (isStun)
    //    {
    //        return m_nextBlockActor;
    //    }

    //    if (m_nextBlockActor == null)
    //    {
    //        //현재 블록에서 해당 방향으로 벽 유무
    //        if (isCheckMove(m_nowBlockActor, m_typeArrow))
    //        {

    //            //벽이 없으면 현재 인덱스 가져오기
    //            int indexX = m_nowBlockActor.indexX;
    //            int indexY = m_nowBlockActor.indexY;



    //            switch (m_typeArrow)
    //            {
    //                case TYPE_ARROW.UP:
    //                    //도약
    //                    if ((int)m_nowBlockActor.typeTopValue == 13)
    //                    {
    //                        setJump(true);
    //                        GetComponent<Collider2D>().enabled = false;
    //                        m_nextBlockActor = m_field.blockActorField[6][m_nowBlockActor.indexX];
    //                        return m_nextBlockActor;
    //                    }

    //                    indexY--;
    //                    break;
    //                case TYPE_ARROW.DOWN:
    //                    //도약
    //                    if ((int)m_nowBlockActor.typeTopValue == 14)
    //                    {
    //                        setJump(true);
    //                        GetComponent<Collider2D>().enabled = false;
    //                        m_nextBlockActor = m_field.blockActorField[0][m_nowBlockActor.indexX];
    //                        return m_nextBlockActor;
    //                    }

    //                    indexY++;
    //                    break;
    //                case TYPE_ARROW.LEFT:
    //                    indexX--;
    //                    break;
    //                case TYPE_ARROW.RIGHT:
    //                    indexX++;
    //                    break;
    //            }


    //            //인덱스가 넘어가지 않으면
    //            if (indexX >= 0 && indexX < PrepClass.xCnt && indexY >= 0 && indexY < PrepClass.yCnt)
    //            {

    //                BlockActor tmpBlockActor = m_field.blockActorField[indexY][indexX];


    //                //다음 방향의 벽에도 해당하는 방향 반대쪽에 벽이 없으면
    //                if (isCheckMove(tmpBlockActor, getReverseArrow(m_typeArrow)))
    //                {
    //                    //다음으로 향할 벽 등록
    //                    m_nextBlockActor = tmpBlockActor;
    //                    return m_nextBlockActor;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        return m_nextBlockActor;
    //    }

    //    m_nextBlockActor = null;
    //    return m_nextBlockActor;
//    }

    //void catchBlock(BlockActor nowBlockActor, BlockActor nextBlockActor)
    //{
    //    if (m_typeUser == TYPE_USER.PLAYER)
    //    {
    //        if (nextBlockActor == null)
    //        {
    //            m_field.indexAction(m_nowBlockActor, 1);
    //        }
    //        else
    //        {
    //            if (Vector2.Distance(transform.position, m_nextBlockActor.transform.position) <
    //                 Vector2.Distance(transform.position, m_nowBlockActor.transform.position))
    //            {
    //                m_field.indexAction(m_nextBlockActor, 1);
    //            }
    //            else
    //                m_field.indexAction(m_nowBlockActor, 1);
    //        }
    //    }
    //}


    //TYPE_ARROW randomArrow()
    //{
    //    int cnt = Random.Range(0, 4);
    //    return (TYPE_ARROW)pow(cnt);
    //}


    //int pow(int cnt)
    //{
    //    int pw = 1;
    //    while (cnt > 0)
    //    {
    //        pw *= 2;
    //        cnt--;
    //    }
    //    return pw;
    //}

    public void soundPlay(string key, TYPE_SOUND typeSound)
    {
        m_soundPlayer.audioPlay(key, typeSound);
    }

    ///// <summary>
    ///// 지나갈 수 있는지 여부
    ///// </summary>
    ///// <param name="blockActor"></param>
    ///// <returns></returns>
    //bool isCheckMove(BlockActor blockActor, TYPE_ARROW typeArrow)
    //{


    //    if (m_typeUser == TYPE_USER.CPU && (int)m_nowBlockActor.typeTopValue == 15)
    //    {
    //        return true;
    //    }

    //    //통로이면
    //    if ((int)blockActor.typeTopValue == 13 || (int)blockActor.typeTopValue == 14)
    //    {
    //        return true;
    //    }

    //    //해당하는 방향에 벽이 있으면
    //    if (((int)blockActor.typeTopValue & (int)typeArrow) == (int)typeArrow)
    //    {
    //        //이동 못함
            
    //        return false;
    //    }
    //    //없으면 이동 가능
    //    return true;

    //}


    public void createParticle()
    {
        ParticleSystem particle = (ParticleSystem)Instantiate(m_block.getParticle(), transform.position, Quaternion.identity);
        particle.gameObject.AddComponent<ParticleLife>();
        particle.transform.SetParent(transform);
    }


    void OnTriggerEnter2D(Collider2D col)
    {

        
        //축구이면
        //블록에 닿으면 indexAction 실행


        //행동자에 따라서 움직임
        if (col.tag == PrepClass.moverTag)
        {
            if (col.GetComponent<BlockMover>().typeUser == BlockMover.TYPE_USER.PLAYER)
            {

                if (m_iBlockMover.crashAction(m_field, col.GetComponent<BlockMover>(), this))
                {
                    StartCoroutine(readyCoroutine());
                    createParticle();
                }
                else
                    m_field.setDefeat();
            }


            //if (col.GetComponent<BlockMover>().m_typeUser == TYPE_USER.PLAYER)
            //{


            //    //야생해방상태이면 반대로 잡아먹힘
            //    if (m_field.isFeral)
            //    {
            //        //중앙으로 이동
            //        m_nextBlockActor = m_field.blockActorField[3][3];
            //        //8초 후에 부활
            //        StartCoroutine(readyCoroutine());
            //        //스코어 올리기
            //        m_field.catchCerulean(m_block);

            //        //
            //        ParticleSystem particle = (ParticleSystem)Instantiate(m_block.getParticle(), transform.position, Quaternion.identity);
            //        particle.gameObject.AddComponent<ParticleLife>();
            //        particle.transform.SetParent(transform);

            //        m_soundPlayer.audioPlay("EffectHit", TYPE_SOUND.EFFECT);

            //    }
            //    else
            //        m_field.setDefeat();
            
        }


        //if (col.tag == "Block")
        //{
        //    if (col.GetComponent<BlockActor>().typeBlock == TYPE_BLOCK.WALL)
        //    {
        //        //블록이 등록
        //    }
        //}
    }

    IEnumerator readyCoroutine()
    {
        m_isStun = true;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(8f);
        GetComponent<Collider2D>().enabled = true;
        m_isStun = false;
    }
}

