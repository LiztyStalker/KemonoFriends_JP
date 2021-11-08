using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {

    //모험 이벤트가 벌어지는 시간
    const float m_eventTime = 1.5f;


//    [SerializeField]
    UIPlayer m_uiPlayer;

    [SerializeField]
    BlockActor m_blockActor;

//    [SerializeField]
//    BlockMover m_blockMover;

    BlockActor[][] m_blockActorField = new BlockActor[PrepClass.yCnt][];

    Friend m_friend;

    Item m_item;

    Adventure m_adventure;

    BlockMover m_blockPlayer;
    List<BlockMover> m_blockCPU = new List<BlockMover>();

    int m_score = 0; //현재 점수
    int m_coin = 0; //현재 자파리코인
    int m_feral = 0; //야성해방

    int m_maxCombo = 0;
    int m_combo = 0; //섭취 콤보 (야성해방이 아닐 때 세루리안 건드리면 초기화)
    int m_cerulean = 0; //세루리안 사냥 수
    int m_maxFeral = PrepClass.defaultFeral;
    int m_food = 0; //식량 섭취량
    int m_count = 0; //식량 섭취 카운트
    int m_sandstar = 0; //샌드스타

    bool m_isItemCombo = false;
    bool m_isFeral = false;
    bool m_isGameRun = false;

    float m_nowFeralTime = 0f;
    const float m_maxFeralTime = 8f;

    float m_nowComboTime = 0f;
    const float m_maxComboTime = 1f;
    
    float m_gameTime = 0f;
    const float m_maxGameTime = 60f;

    bool m_isPause = false;
    bool m_isDefeat = false;

    int m_defeatCount = 0;

    Coroutine m_feralCoroutine = null;

    SoundPlay m_soundPlayer = null;

    public BlockActor[][] blockActorField { get { return m_blockActorField; } }

    public int score { get { return m_score; } }
    public int coin { get { return m_coin; } }
    public int food { get { return m_food; } }
    public int cerulean { get { return m_cerulean; } }
    public int sandstar { get { return m_sandstar; } }

    public int nowFeral { get { return m_feral; } }
    public int maxFeral { get { return m_maxFeral; } }
    public int combo { get { return m_combo; } }
    public int maxCombo { get { return m_maxCombo; } }

    public bool isGameRun { get { return m_isGameRun; } }
    public bool isDefeat { get { return m_isDefeat; } }
    public bool isFeral { get { return m_isFeral; } }

    public List<BlockMover> cpuList { get { return m_blockCPU; } }

    float nowFeralTime { get { return m_nowFeralTime; } set { m_nowFeralTime = value; } }
    float maxFeralTime { 
        get {
            if (m_friend == null)
                return m_maxFeralTime;
            return m_maxFeralTime * m_friend.realFeral; 
        } 
    }

    public float feralRate
    {
        get
        {
            if (!isFeral)
                return (float)nowFeral / (float)maxFeral;
            else
                return nowFeralTime / maxFeralTime;
        }
    }

    public float timeRate
    {
        get
        {
            return m_gameTime / m_maxGameTime;
        }
    }

    /// <summary>
    /// 60s → 0s으로 이동하는 다운 타임
    /// </summary>
    public float gameTimeDown { get { return m_gameTime; } }

    /// <summary>
    /// 0s → 60s으로 이동하는 업 타임
    /// </summary>
    public float gameTimeUp { get { return m_maxGameTime - m_gameTime; } }


    readonly Vector2 startPos = new Vector2(-2.25f, 2.9f);
    readonly Vector2 lengthVector = new Vector2(0.75f, 0.75f);


    public void setPause(){m_isPause = true;}
    public void resetPause(){m_isPause = false;}

    public Friend friend { get { return m_friend; } }
    public Item item { get { return m_item; } }
    public Adventure adventure { get { return m_adventure; } }


	// Use this for initialization
	void Awake () {


        m_uiPlayer = GameObject.Find("Game@Play").GetComponent<UIPlayer>();


        //        initTest();
        initPlay();

        m_uiPlayer.setField(this);
        StartCoroutine(gameRun());

	}

    /// <summary>
    /// 사운드 초기화
    /// </summary>
    void initSound(){
        m_soundPlayer = GetComponent<SoundPlay>();
        if (m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();
        m_soundPlayer.audioPlay(adventure.bgmKey, TYPE_SOUND.BGM);
    }

    /// <summary>
    /// 테스트용
    /// </summary>
    void initTest()
    {
        m_friend = Account.GetInstance.getFriend("Serval");
        m_item = ItemManager.GetInstance.defaultItem();
        m_adventure = AdventureManager.GetInstance.getAdventure("BusDrive");

    }

    /// <summary>
    /// 플레이용
    /// </summary>
    void initPlay()
    {
        m_friend = Account.GetInstance.playFriend();
        m_item = ItemManager.GetInstance.getItem(Account.GetInstance.itemKey);
        m_adventure = AdventureManager.GetInstance.getAdventure(Account.GetInstance.adventureKey);
        GetComponent<SpriteRenderer>().sprite = m_adventure.BGImage;
    }

    /// <summary>
    /// 필드 초기화
    /// </summary>
    void initBlock()
    {

        if (PrepClass.yCnt >= PrepClass.maxCnt || PrepClass.xCnt >= PrepClass.maxCnt)
        {
            throw new FieldIndexOutException(string.Format("xCnt 또는 yCnt는 {2}이상으로 설정할 수 없습니다 - X:{0} Y{1}", PrepClass.xCnt, PrepClass.yCnt, PrepClass.maxCnt));
        }
        else if (PrepClass.yCnt <= 0 || PrepClass.xCnt < 0)
        {
            throw new FieldIndexOutException(string.Format("xCnt 또는 yCnt는 0 이하로 설정할 수 없습니다 - X:{0} Y{1}", PrepClass.xCnt, PrepClass.yCnt));
        }
            

        else
        {

            //일단 블록 배치
            for (int y = 0; y < PrepClass.yCnt; y++)
            {
                m_blockActorField[y] = new BlockActor[PrepClass.yCnt];

                for (int x = 0; x < PrepClass.xCnt; x++)
                    m_blockActorField[y][x] = createBlockActor(x, y);
            }
            
            //어드벤처 알고리즘으로 블록 초기화
            m_adventure.batchBlock(m_blockActorField, friend.realLuck, createBlockActor);
            m_adventure.createMover(createBlockMover);

//            if(m_adventure is AdventureExplore)
//            if(m_adventure.adventureType == typeof(AdventureExplore))
//                createBlockMover(BlockMover.TYPE_USER.PLAYER, 0, 0);
////            else if(m_adventure is AdventureDrive)
//            else if (m_adventure.adventureType == typeof(AdventureDrive))
//                createBlockMover(BlockMover.TYPE_USER.PLAYER, 3, 5);
        }
    }

    /// <summary>
    /// 블록 행동자 생성하기
    /// </summary>
    /// <param name="blockMover">복사할 블록 행동자</param>
    /// <param name="typeUser">유저 타입</param>
    /// <param name="x">X 위치</param>
    /// <param name="y">Y 위치</param>
    /// <returns></returns>
    public BlockMover createBlockMover(BlockMover blockMover, BlockMover.TYPE_USER typeUser, int x, int y, float size = 1f)
    {

        BlockMover tmpBlockMover = (BlockMover)Instantiate(blockMover);
        tmpBlockMover.setField(this);
        tmpBlockMover.initUser(typeUser);
        tmpBlockMover.initPosition(m_blockActorField[y][x]);

        //1.4
        tmpBlockMover.setSize(size);

        //
        if (typeUser == BlockMover.TYPE_USER.PLAYER)
        {
            tmpBlockMover.setIcon(m_friend.icon);
            m_blockPlayer = tmpBlockMover;
        }
        else
        {
            tmpBlockMover.setBlock(m_adventure.getCPUBlock());

            //풋볼만 콜라이더 사이즈 조절
            if (m_adventure.adventureType == typeof(AdventureFootball))
                tmpBlockMover.GetComponent<BoxCollider2D>().size = Vector2.one * 0.8f;

//            tmpBlockMover.setBlock(BlockManager.GetInstance.getCeruleanBlock(adventure.key, TYPE_VALUE.VALUE_00));
            m_blockCPU.Add(tmpBlockMover);
        }

        return tmpBlockMover;
        //
    }


    /// <summary>
    /// 빈 블록 생성하기
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    BlockActor createBlockActor(int x, int y)
    {
        BlockActor blockActor = (BlockActor)Instantiate(m_blockActor);
        blockActor.setField(this);
        blockActor.transform.position = new Vector2(startPos.x + x * lengthVector.x, startPos.y - y * lengthVector.y);
        blockActor.index = y * PrepClass.maxCnt + x;
        return blockActor;
    }

    /// <summary>
    /// 게임 초반
    /// </summary>
    /// <returns></returns>
    IEnumerator gamePreview()
    {
        float time = 2f;
        m_uiPlayer.gamePreivew(time);
        yield return new WaitForSeconds(time); 
    }

    /// <summary>
    /// 준비
    /// </summary>
    /// <returns></returns>
    IEnumerator gameReady()
    {
        m_uiPlayer.gameReady();

        m_gameTime = 1f;
        while (m_gameTime > 0f)
        {
            m_gameTime -= PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }

        m_uiPlayer.gameStart();
        m_isGameRun = true;
    }

    /// <summary>
    /// 게임 진행
    /// </summary>
    /// <returns></returns>
    IEnumerator gameRun()
    {

        initBlock();

        //초반
        yield return StartCoroutine(gamePreview());

        //준비
        yield return StartCoroutine(gameReady());


        initSound();

        StartCoroutine(comboCoroutine());


        m_gameTime = m_maxGameTime;
        m_uiPlayer.uiUpdate();

        float eventTimer = 0f;
        float adventureTime = adventure.eventTime;

        while (m_gameTime > 0)
        {



            m_uiPlayer.timerUpdate();

            m_gameTime -= PrepClass.frameTime;
            eventTimer += PrepClass.frameTime;

            if (eventTimer >= adventureTime)
            {
                adventureTime = m_adventure.adventureEvent(this);
                eventTimer = 0f;

                if (m_adventure.isDefeat(m_blockActorField))
                {
                    setDefeat();
                }
            }




            yield return new WaitForSeconds(PrepClass.frameTime);


        }

        //게임 종료

        //야성해방 강제 종료
        m_nowFeralTime = 0f;
        m_feral = 0;
        m_isFeral = false;
        if (m_blockPlayer != null)
            m_blockPlayer.setFeral(m_isFeral);
        m_uiPlayer.feralEnd();
        m_feralCoroutine = null;

        if (m_maxCombo < m_combo)
            m_maxCombo = m_combo;
        m_combo = 0;
        
        yield return StartCoroutine(gameEnd());

        //아이템 추가 유무
        if(isSandStar()) yield return StartCoroutine(gameBonas());

        m_uiPlayer.gameFinish();
    }

    /// <summary>
    /// 패배 설정
    /// </summary>
    public void setDefeat()
    {
        m_gameTime = 0f;
        m_isDefeat = true;

        if(m_blockPlayer != null)
            m_blockPlayer.setDefeat();

        foreach (BlockMover mover in cpuList)
            mover.setDefeat();
    }

    /// <summary>
    /// 게임 끝
    /// </summary>
    /// <returns></returns>
    IEnumerator gameEnd()
    {
        //게임 패배가 아니면 코인 1개 증정
        if (!m_isDefeat)
            m_coin++;

        m_soundPlayer.audioStop();
        m_isGameRun = false;
        m_uiPlayer.gameEnd(3f, m_isDefeat);

        
        yield return new WaitForSeconds(3f);
    }

    /// <summary>
    /// 추가보너스
    /// </summary>
    /// <returns></returns>
    IEnumerator gameBonas()
    {
        float bonasTime = 3f;
        m_uiPlayer.gameBonas(bonasTime);
        yield return new WaitForSeconds(bonasTime);
        
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if (m_blockActorField[y][x].typeBlock == TYPE_BLOCK.SANDSTAR)
                {
                    indexAction(m_blockActorField[y][x], 0);
                    yield return new WaitForSeconds(0.5f);
                }

            }
        }

        yield return new WaitForSeconds(bonasTime);
    }

    /// <summary>
    /// 보너스시 남은 샌드스타 여부
    /// </summary>
    /// <returns></returns>
    bool isSandStar()
    {
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if (m_blockActorField[y][x].block.typeBlock == TYPE_BLOCK.SANDSTAR)
                    return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_isGameRun && !m_isPause)
        {

            //블록
            //입력 - 유적지만 화살표


            if (Input.GetMouseButtonDown(0))
            {
//                if (adventure is AdventureRepulse || adventure is AdventureHarvest)

                    //어드벤처 - 아케이드이면
//                RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                RaycastHit2D[] rays = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                foreach (RaycastHit2D ray in rays)
                {
                    if (ray.collider != null)
                    {
                        //Debug.Log("ray : " + ray);
                        if (ray.collider.tag == PrepClass.blockTag)
                        {
                            BlockActor blockActor = ray.collider.GetComponent<BlockActor>();
                            blockActor = m_adventure.checkBlock(blockActor, 0, m_isFeral);

                            Debug.Log("blockActor " + blockActor);

                            if (blockActor != null)
                            {
                                if (adventure.adventureType.BaseType == typeof(AdventureBlock))
                                {


                                    //if (blockActor.typeBlock == TYPE_BLOCK.SANDSTAR && !(blockActor.typeValue == TYPE_VALUE.VALUE_01))
                                    //    indexAction(blockActor.indexX, blockActor.indexY, 0, 0);
                                    //else
                                    indexAction(blockActor, 0);

                                    if (m_adventure.isDefeat(m_blockActorField))
                                    {
                                        m_gameTime = 0f;
                                        m_isDefeat = true;
                                    }
                                }
                                else
                                {
                                    //블록을 기준으로 무버 움직이게 하기
                                    if (blockActor.blockMover != null)
                                    {
                                        indexAction(blockActor.blockMover, blockActor);
                                    }
                                }

                            }
                        }
                    }

                }
                m_uiPlayer.uiUpdate();
            }

        }
	}

    /// <summary>
    /// 방향 전환 이벤트
    /// </summary>
    /// <param name="selector"></param>
    public void setArrowChange(int selector)
    {
        m_blockPlayer.setArrowChange(selector);
    }

    /// <summary>
    /// 1.4
    /// 터치 버튼 이벤트
    /// </summary>
    public void setOneEvent()
    {
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if (blockActorField[y][x].blockMover != null)
                {
                    indexAction(blockActorField[y][x].blockMover, blockActorField[y][x]);
                }
            }
        }
    }

    //public void setArrowChange(TYPE_ARROW typeArrow)
    //{
    //    m_blockPlayer.setArrowChange(typeArrow);
    //}

    //해당 모험에 대한 아이템 변경 필요
    public void useItem()
    {
        if (m_item != ItemManager.GetInstance.defaultItem())
        {

            switch (m_item.key)
            {
                case "ItemFeral":
                    //야성해방
                    m_feral = maxFeral;
                    if (m_feralCoroutine == null && feralCheck(null))
                        m_feralCoroutine = StartCoroutine(feralCoroutine());
                    else
                        nowFeralTime = maxFeralTime;

                    break;
                case "ItemCerulean":
                    //모든 세룰리안 퇴치
                    ceruleanRepulse();
                    break;
                case "ItemCombo":
                    //콤보 유지
                    StartCoroutine(itemComboCoroutine(5f));
                    break;
                case "ItemHelp":
                    //자동 터치
                    break;
                case "ItemTime":
                    //시간 증가
                    if (m_gameTime + 5f > m_maxGameTime)
                        m_gameTime = m_maxGameTime;
                    else
                        m_gameTime += 5f;
                    break;
            }


            m_item = ItemManager.GetInstance.defaultItem();
        }

        m_uiPlayer.uiUpdate();
    }

    /// <summary>
    /// 아이템 콤보 지속기간
    /// </summary>
    /// <param name="timer"></param>
    /// <returns></returns>
    IEnumerator itemComboCoroutine(float timer)
    {
        float m_timer = timer;

        m_isItemCombo = true;
        while (m_timer > 0f)
        {
            m_timer -= PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }
        m_isItemCombo = false;

    }

    /// <summary>
    /// 세룰리안 모두 제거하기
    /// </summary>
    void ceruleanRepulse()
    {

        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if (m_blockActorField[y][x].typeBlock == TYPE_BLOCK.CERULEAN)
                {

                    if (m_blockActorField[y][x].hitBlock(int.MaxValue))
                    {
                        m_score += m_blockActorField[y][x].score;
                        m_adventure.createBlock(m_blockActorField[y][x], friend.realLuck);
                    }
                }
            }
        }
    }






    //해당하는 모험에 따라서 알고리즘 달라짐 
    //수확 - 주변 수집
    //습격 - 세룰리안 공격 가능 - 1인 공격
    //탐험 - 사용하지 않음
    //댄스 - 1인 공격
    //void indexAction(int nowX, int nowY, int rangeX = 1, int rangeY = 1)
    //{

    //    //야성해방 샌드스타 먹으면 야성해방으로 전환

    //    List<BlockActor> blockList = m_adventure.indexAction(m_blockActorField, nowX, nowY, rangeX, rangeY, m_isFeral);

    //    if (blockList != null)
    //    {
    //        m_count = blockList.Count;

    //        if (m_count > 0)
    //        {
    //            m_combo++;

    //            if (m_feralCoroutine == null && feralCheck(m_count))
    //            {
    //                m_feralCoroutine = StartCoroutine(feralCoroutine());
    //            }

    //            for (int i = m_count - 1; i >= 0; i--)
    //            {
    //                indexAction(blockList[i], i);
    //            }
    //        }

    //        m_adventure.clearBlockList();
    //    }
    //}

    /// <summary>
    /// 블록 누를 시 행동 이벤트
    /// </summary>
    /// <param name="blockActor"></param>
    /// <param name="depth"></param>
    public void indexAction(BlockActor blockActor, int depth = 0)
    {



        if (m_adventure.isDefeat(blockActor, m_isFeral))
            setDefeat();

        List<BlockActor> blockList = adventure.indexAction(blockActorField, blockActor, m_isFeral, depth);


        //연속으로 먹어야 콤보가 끊기지 않음
        if (blockList != null)
        {
            m_count = blockList.Count;

            if (m_count > 0)
            {
//                m_nowComboTime = m_maxComboTime;
//                m_combo++;


//                int blockCnt = 0;
                bool isFirstBlock = false;

                for (int i = m_count - 1; i >= 0; i--)
                {
                    //공격
                    int damage = adventure.calculateDamage(m_friend.realForce, isFeral);

                    if (blockList[i].hitBlock(damage))
                    {


                        //첫번째 블록 콤보 조정
                        if (!isFirstBlock)
                        {
                            m_nowComboTime = m_maxComboTime;
                            m_combo++;
                            isFirstBlock = true;
                        }

                        //야성해방 스코어 대폭 증가 - 야성해방 도중이면 1초 추가
                        if (blockList[i].typeBlock == TYPE_BLOCK.SANDSTAR && blockList[i].typeValue == TYPE_VALUE.VALUE_01)
                        {
                            m_feral += (int)((float)m_maxFeral * 0.25f * m_friend.realFeral);
                            if (m_feralCoroutine != null)
                            {
//                            if (!(m_feralCoroutine == null && feralCheck(null))){
//                                m_feralCoroutine = StartCoroutine(feralCoroutine());
//                            else
//                            {
                                if (nowFeralTime + 1f > maxFeralTime)
                                    nowFeralTime = maxFeralTime;
                                else
                                    nowFeralTime += 1f;
                            }
                        }


                        if (m_feralCoroutine == null && feralCheck(blockList[i].block))
                        {
                            m_feralCoroutine = StartCoroutine(feralCoroutine());
                        }


                        countCalculate(blockList[i].typeBlock);
                        m_score += blockList[i].score;
                        m_adventure.createBlock(blockList[i], friend.realLuck);
//                        blockCnt++;




                    }
                }




            }

            m_adventure.clearBlockList();
        }


        m_uiPlayer.uiUpdate();
    }


    /// <summary>
    /// 블록에 닿으면 행동 이벤트
    /// </summary>
    /// <param name="blockMover"></param>
    /// <param name="blockActor"></param>
    public void indexAction(BlockMover blockMover, BlockActor blockActor)
    {
        //블록무버 커졌다 작아짐


        blockActor.setSize(1.25f, 0.1f);
        
        m_nowComboTime = m_maxComboTime;
        m_combo++;

        m_adventure.indexAction(blockMover, blockActor);

        if (m_feralCoroutine == null && feralCheck(blockActor.blockMover.block))
        {
            m_feralCoroutine = StartCoroutine(feralCoroutine());
        }

        countCalculate(TYPE_BLOCK.BLOCK);
        m_score += m_combo * 50;
    }

    /// <summary>
    /// 세룰리안 잡기
    /// </summary>
    /// <param name="block"></param>
    public void catchCerulean(Block block)
    {
        countCalculate(block.typeBlock);
        m_score += block.score;
    }

    /// <summary>
    /// 숫자 세기
    /// </summary>
    /// <param name="typeBlock"></param>
    void countCalculate(TYPE_BLOCK typeBlock)
    {
        switch (typeBlock)
        {
            case TYPE_BLOCK.BLOCK:
                m_food++;
                break;
            case TYPE_BLOCK.COIN:
                m_coin++;
                break;
            case TYPE_BLOCK.CERULEAN:
                m_cerulean++;
                break;
            case TYPE_BLOCK.SANDSTAR:
                m_combo++;
                m_count++;
                m_sandstar++;
                break;
        }
        
    }

    /// <summary>
    /// 야생해방 여부 체크
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    bool feralCheck(Block block)
    {
        if (isGameRun)
        {

            int feral;
            if (block == null)
                feral = 0;
            else
                feral = adventure.calculateFeral(block, m_combo);


            if (!isFeral)
            {
                m_feral += feral;
                if (maxFeral <= m_feral)
                {
                    m_isFeral = true;
                    m_feral = 0;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 야생해방 지속시간
    /// </summary>
    /// <returns></returns>
    IEnumerator feralCoroutine()
    {

        m_uiPlayer.feralStart();

        m_adventure.feralStart(this);

        nowFeralTime = maxFeralTime;
        while (nowFeralTime > 0f)
        {
            m_uiPlayer.feverUpdate();
            nowFeralTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        m_maxFeral = (int)((float)m_maxFeral * 1.25f);
        m_feral = 0;
        m_isFeral = false;
        m_uiPlayer.feralEnd();
        m_adventure.feralEnd(this);
        m_feralCoroutine = null;


    }

    /// <summary>
    /// 콤보 지속시간
    /// </summary>
    /// <returns></returns>
    IEnumerator comboCoroutine()
    {
        while(isGameRun){
            m_nowComboTime -= PrepClass.frameTime;
            if (m_nowComboTime <= 0f && !m_isItemCombo)
            {
                if (m_maxCombo < m_combo)
                    m_maxCombo = m_combo;

                m_combo = 0;
            }
            yield return new WaitForSeconds(PrepClass.frameTime);
        }
    }

}
