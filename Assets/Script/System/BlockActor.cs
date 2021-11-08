using System.Collections;
using UnityEngine;

public class BlockActor : MonoBehaviour
{

    Field m_field;

    BlockActor m_innerBlockActor = null;

    BlockMover m_blockMover = null;

    Block m_block;

    int m_sortingOrder = 0;
    int m_depth = 0;

    int m_health;

    float m_time = 0f;
    const float m_viewTime = 1f;

    float m_sizeTimer = 0f;
    Coroutine m_sizeCoroutine = null;



    [SerializeField]
    SpriteRenderer m_spriteRenderer;

    [SerializeField]
    Transform m_healthBar;

    Vector2 m_defaultBarSize;
    Vector2 m_defaultSize;

    SoundPlay m_soundPlayer;

    int m_index;

    public int depth { get { return m_depth; } set { m_depth = value; } }
    public float time { get { return m_time; } }
    public void resetDepth() { m_depth = 0; }
    public Field field { get { return m_field; } }
    public BlockMover blockMover { get { return m_blockMover; } }

    public void increaseDepth(int sortingOrder)
    {
        m_sortingOrder = sortingOrder + 1;
    }

    public void setField(Field field)
    {
        m_field = field;
    }

    public int index {
        get
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.index;
            return m_index;
        } 

        set 
        {
            m_index = value;
            if (m_innerBlockActor != null)
                m_innerBlockActor.index = m_index;
        } 
    }


    public int indexX { 
        get 
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.indexX;
            return m_index % PrepClass.maxCnt; 
        } 
    }
    
    public int indexY { 
        get
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.indexY;
            return m_index / PrepClass.maxCnt; 
        } 
    }

    public Block block { 
        get {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.block;
            return m_block; 
        } 
    }

    public TYPE_BLOCK typeBlock { 
        get 
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.typeBlock;
            return m_block.typeBlock; 
        } 
    }

    public TYPE_VALUE typeTopValue{
        get
        {
            return m_block.typeValue;
        }
    }

    public TYPE_VALUE typeValue { 
        get 
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.typeValue;
            return m_block.typeValue; 
        } 
    }

    public int weight { 
        get 
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.weight;
            return m_block.weight; 
        } 
    }

    public int score { 
        get 
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.score;
            return m_block.score; 
        } 
    }

    public int health 
    { 
        get 
        {
            if (m_innerBlockActor != null)
                return m_innerBlockActor.health;
            return m_health; 
        } 
    }



    void Awake()
    {
        m_defaultBarSize = new Vector2(m_healthBar.localScale.x, m_healthBar.localScale.y);
        m_defaultSize = new Vector2(transform.localScale.x, transform.localScale.y);
        m_healthBar.gameObject.SetActive(false);


        StartCoroutine(healthCoroutine());
        //if(m_field.adventure is AdventureExplore)
        //    StartCoroutine(blockCoroutine(5f));
        //else if (m_field.adventure is AdventureDance)
        //    StartCoroutine(blockRemoveCoroutine(1f));

    }

    void Start()
    {
        m_soundPlayer = GetComponent<SoundPlay>();

        if (m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();
    }

    /// <summary>
    /// 블록 설정
    /// </summary>
    /// <param name="block"></param>
    public void setBlock(Block block){

        if (m_innerBlockActor != null)
            m_innerBlockActor.setBlock(block);
        else
        {
            m_block = block;
            m_health = block.health;
            m_spriteRenderer.sprite = block.icon;
            m_spriteRenderer.sortingOrder = m_sortingOrder;
            m_time = 0f;
            healthView();
        }
    }

    /// <summary>
    /// 블록 복제
    /// </summary>
    /// <param name="blockActor"></param>
    public void copyBlock(BlockActor blockActor)
    {
        if (m_innerBlockActor != null)
        {
            m_innerBlockActor.copyBlock(blockActor);
        }
        else
        {
            setBlock(blockActor.block);
            m_health = blockActor.health;
            m_time = blockActor.time;
            healthView();
        }
    }

    /// <summary>
    /// 블록 행동자 포함
    /// </summary>
    /// <param name="blockActor"></param>
    public void setInnerBlock(BlockActor blockActor)
    {
        if (m_innerBlockActor == null)
        {
            //최상위 블록은 콜라이더 사용 안함
            GetComponent<Collider2D>().enabled = false;
            m_innerBlockActor = blockActor;
            m_innerBlockActor.increaseDepth(m_sortingOrder);
            m_innerBlockActor.transform.position = transform.position;
            m_innerBlockActor.transform.localScale *= 0.8f;
        }
        else
            m_innerBlockActor.setInnerBlock(blockActor);
    }
    
    /// <summary>
    /// 체력 보이기
    /// </summary>
    void healthView()
    {
        float rate = (float)m_health / (float)block.health;
        m_healthBar.localScale = new Vector2(rate * m_defaultBarSize.x, m_defaultBarSize.y);
        healthBarView();
    }

    /// <summary>
    /// 블록 데미지 입기
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool hitBlock(int damage)
    {

        if (m_innerBlockActor != null)
        {
            return m_innerBlockActor.hitBlock(damage);
        }
        else
        {

            if (typeBlock == TYPE_BLOCK.NONE)
                return false;

            if (m_health >= 0)
            {
                if(depth == 0)
                    m_health -= damage;
                else
                    m_health -= (int)((float)damage * Mathf.Pow(m_field.friend.realRange, depth + 1));

                if (typeBlock == TYPE_BLOCK.CERULEAN)
                    m_time = m_viewTime;

                healthView();
                createSoundPlay(block, m_health);


                if (typeBlock == TYPE_BLOCK.CERULEAN)
                    createParticleSystem(block.getParticle("Particle@Hit_00"));

                if (m_health <= 0)
                {
                    //파티클
                    createParticleSystem(block.getParticle());
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 사운드 재생하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="health"></param>
    void createSoundPlay(Block block, int health)
    {
        switch (block.typeBlock)
        {
            case TYPE_BLOCK.CERULEAN:
                if (health > 0)
                    m_soundPlayer.audioPlay("EffectHit", TYPE_SOUND.EFFECT);
                else
                    m_soundPlayer.audioPlay("EffectHit", TYPE_SOUND.EFFECT);
                break;
            case TYPE_BLOCK.BLOCK:
                m_soundPlayer.audioPlay("EffectBread", TYPE_SOUND.EFFECT);
                break;
            case TYPE_BLOCK.COIN:
                m_soundPlayer.audioPlay("EffectCoin", TYPE_SOUND.EFFECT);
                break;
            case TYPE_BLOCK.SANDSTAR:
                switch (block.typeValue)
                {
//                    case TYPE_VALUE.VALUE_01:
//                        m_soundPlayer.audioPlay("EffectFeralSandStar", TYPE_SOUND.EFFECT);
//                        break;
                    case TYPE_VALUE.VALUE_05:
                        m_soundPlayer.audioPlay("EffectAllBread", TYPE_SOUND.EFFECT);
                        break;
                    case TYPE_VALUE.VALUE_06:
                        m_soundPlayer.audioPlay("EffectChange", TYPE_SOUND.EFFECT);
                        break;
                    default:
                        m_soundPlayer.audioPlay("EffectSandStar", TYPE_SOUND.EFFECT);
                        break;
                }
                break;
        }

    }

    /// <summary>
    /// 파티클 생성하기
    /// </summary>
    /// <param name="particle"></param>
    void createParticleSystem(ParticleSystem particle)
    {
        if (particle != null)
        {
            particle = (ParticleSystem)Instantiate(particle, transform.position, Quaternion.identity);
            particle.gameObject.AddComponent<ParticleLife>();
            particle.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning("파티클 없음");
        }

    }

    /// <summary>
    /// 체력바 보이기
    /// </summary>
    void healthBarView()
    {
        if (m_time > 0f)
            m_healthBar.gameObject.SetActive(true);
        else
            m_healthBar.gameObject.SetActive(false);
    }


    /// <summary>
    /// 사이즈 설정하기
    /// </summary>
    /// <param name="size">사이즈 크기</param>
    /// <param name="timer">변하는 시간 timer가 0 이하이면 1로 고정</param>
    public void setSize(float size, float timer)
    {
        if (timer <= 0f)
            timer = 1f;

        m_sizeTimer = timer;

        if (m_sizeCoroutine == null){
            transform.localScale *= size;
            m_sizeCoroutine = StartCoroutine(sizeCoroutine());
        }
    }


    IEnumerator sizeCoroutine()
    {

        while (m_sizeTimer > 0f)
        {
            m_sizeTimer -= PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }

        m_sizeCoroutine = null;
        transform.localScale = m_defaultSize;
    }

    /// <summary>
    /// 체력바 보이기
    /// </summary>
    /// <returns></returns>
    IEnumerator healthCoroutine()
    {
        while (gameObject.activeSelf)
        {
            m_time -= PrepClass.frameTime;
            healthBarView();
            yield return new WaitForSeconds(PrepClass.frameTime);
        }
    }
    
    /// <summary>
    /// 충돌 여부
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {



        //축구이면
        //공에 닿으면
        if(m_field.isGameRun){

            if (block != null)
            {
                if (block.typeBlock == TYPE_BLOCK.FRIEND)
                {
                    if (col.tag == PrepClass.moverTag)
                    {
                        m_blockMover = col.GetComponent<BlockMover>();

                    }
                }
                else
                {

                    //프렌즈 블록이 아니면
                    switch (typeBlock)
                    {
                        case TYPE_BLOCK.BALL:
                            break;
                        case TYPE_BLOCK.NONE:
                            break;
                        case TYPE_BLOCK.WALL:
                            break;
                        default:
                            Debug.Log("hit");
                            //프리무버일 경우만 사용
                            if (m_field.adventure.adventureType == typeof(AdventureFootball))
                            {
                                //블록을 주변에 있는것까지 먹음
                                //닿은것 1개만 먹어야 함
                                m_field.indexAction(this);
                            }
                            //세룰리안은 반대로 튕겨내야 함
                            //야성해방이면 자동으로 축구를 함 - 속도 매우 빠름
                            break;
                    }
                }
            }

            
        }



        //무버이면 블록을 먹은 무버에게 주기
        //

        //블록이면 먹기
        //세룰리안이면 다른 곳으로 튕기기
        
    }

    /// <summary>
    /// 충돌 빠져나오면
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerExit2D(Collider2D col)
    {
        //축구이면
        //공이 나가면
        if (col.tag == PrepClass.moverTag)
            m_blockMover = null;
    }

}

