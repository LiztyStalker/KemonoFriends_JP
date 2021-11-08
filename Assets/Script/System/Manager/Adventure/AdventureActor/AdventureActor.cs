using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class AdventureActor
{

    public enum TYPE_JAPARIRATE { BREAD, COIN, CERULEAN, COMBO }

    public delegate BlockMover BlockMoverDelegate(BlockMover blockMover, BlockMover.TYPE_USER typeUser, int x, int y, float size = 1f);
    public delegate BlockActor BlockActorDelegate(int x, int y);

    protected float[] m_japariRate;
    List<BlockActor> m_blockList = new List<BlockActor>();
    string m_key;

    protected List<BlockActor> blockList { get { return m_blockList; } }
    protected string key { get { return m_key; } }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="key"></param>
    public AdventureActor(string key)
    {
        m_key = key;
    }

    /// <summary>
    /// 무버 생성하기 - null이면 없음 1.4
    /// </summary>
    /// <param name="blockMoverDel"></param>
    /// <returns></returns>
    public virtual BlockMover createMover(BlockMoverDelegate blockMoverDel)
    {
        return null;
    }

    /// <summary>
    /// 블록 배치 초기화
    /// </summary>
    /// <param name="blockActorArray"></param>
    public abstract void batchBlock(BlockActor[][] blockActorArray, float rate, BlockActorDelegate blockActorDel);

    /// <summary>
    /// 행동 이벤트
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <param name="nowX"></param>
    /// <param name="nowY"></param> 
    /// <param name="rangeX"></param>
    /// <param name="rangeY"></param>
    /// <param name="isCerulean"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    protected virtual List<BlockActor> indexAction(
        BlockActor[][] blockActorField,
        int nowX,
        int nowY,
        int depth = 0,
        bool isFeral = false,
        int rangeX = 1,
        int rangeY = 1
        )
    {
        int indexY = nowY;
        int indexX = nowX;

        for (int y = -rangeY; y <= rangeY; y++)
        {
            indexY = nowY + y;

            if (indexY < 0)
            {
                indexY = 0;
            }
            else if (indexY >= PrepClass.yCnt)
                break;

            for (int x = -rangeX; x <= rangeX; x++)
            {
                indexX = nowX + x;

                if (indexX < 0)
                {
                    indexX = 0;
                }
                else if (indexX >= PrepClass.xCnt)
                    break;


                indexAction(blockActorField, blockActorField[indexY][indexX], isFeral, depth + 1);

            }
        }

        return blockList;
    }


    /// <summary>
    /// 행동 이벤트 - 시작
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <param name="blockActor"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    public virtual List<BlockActor> indexAction(
        BlockActor[][] blockActorField,
        BlockActor blockActor,
        bool isFeral,
        int depth = 0
        )
    {

        blockActor = checkBlock(blockActor, depth, isFeral);
        if (blockActor != null)
        {
            blockActor.depth = depth;
            blockList.Add(blockActor);

            if (depth == 0)
                indexAction(blockActorField, blockActor.indexX, blockActor.indexY, depth, isFeral);

            if (blockActor.typeBlock == TYPE_BLOCK.SANDSTAR)
                checkSandStar(blockActorField, blockActor, isFeral, depth);
        }
        return blockList;
    }


    /// <summary>
    /// 블록 무버 행동 이벤트
    /// </summary>
    /// <param name="blockMover"></param>
    public virtual void indexAction(BlockMover blockMover, BlockActor blockActor) { }


    /// <summary>
    /// 블록 상태 체크
    /// </summary>
    /// <param name="blockActor"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    public abstract BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false);

    /// <summary>
    /// 모험 이벤트 발생하기 - eventTime 주기마다
    /// </summary>
    /// <param name="blockActorField"></param>
    public abstract float adventureEvent(Field field, float eventTime);

    /// <summary>
    /// 데미지 계산하기
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    public virtual int calculateDamage(int damage, bool isFeral)
    {
        return damage;
    }

    /// <summary>
    /// 샌드스타 체크하기
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <param name="blockActor"></param>
    /// <param name="isFeral"></param>
    protected abstract void checkSandStar(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral, int depth);


    /// <summary>
    /// 등록되어있는 모든 블록 없애기
    /// </summary>
    public void clearBlockList()
    {

        foreach (BlockActor blockActor in blockList)
            blockActor.resetDepth();

        blockList.Clear();
    }

    /// <summary>
    /// 블록 생성하기
    /// </summary>
    /// <param name="blockActor"></param>
    public virtual Block createBlock(BlockActor blockActor, float rate, bool isEmpty = true)
    {
        Block block = (isEmpty) ?
            BlockManager.GetInstance.getEmptyBlock() :
            BlockManager.GetInstance.getRandomBlock(key, rate);

        if (block != null)
            blockActor.setBlock(block);

        return block;
    }


    /// <summary>
    /// 게임 패배 조건 가져오기
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <returns></returns>
    public virtual bool isDefeat(BlockActor[][] blockActorField)
    {
        return false;
    }

    /// <summary>
    /// 게임 패배 조건 가져오기
    /// </summary>
    /// <param name="blockActor"></param>
    /// <returns></returns>
    public virtual bool isDefeat(BlockActor blockActor, bool isFeral)
    {
        return false;
    }

    /// <summary>
    /// 게임 패배 조건 가져오기
    /// </summary>
    /// <param name="loseCount"></param>
    /// <returns></returns>
    public virtual bool isDefeat(int loseCount)
    {
        return false;
    }

    /// <summary>
    /// 블록 모두 채우기
    /// </summary>
    /// <param name="blockActorArray">블록배열</param>
    /// <param name="toTypeBlock">변경할 블록</param>
    /// <param name="toTypeValue">블록 값</param>
    protected void fieldFillBread(BlockActor[][] blockActorArray, TYPE_BLOCK toTypeBlock, TYPE_VALUE toTypeValue = TYPE_VALUE.VALUE_00)
    {
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                Block block = BlockManager.GetInstance.getTypeBlock(key, toTypeBlock, toTypeValue);

                if (block != null)
                {
                    if (blockActorArray[y][x].typeBlock == TYPE_BLOCK.NONE)
                    {
                        blockActorArray[y][x].setBlock(block);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 블록 변경하기
    /// </summary>
    /// <param name="blockActorArray">블록배열</param>
    /// <param name="fromTypeBlock">변경 당하는 블록</param>
    /// <param name="toTypeBlock">변경 하는 블록</param>
    /// <param name="toTypeValue">블록값</param>
    protected void blockChange(BlockActor[][] blockActorArray, TYPE_BLOCK fromTypeBlock, TYPE_BLOCK toTypeBlock, TYPE_VALUE toTypeValue = TYPE_VALUE.VALUE_00)
    {
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {

                if (blockActorArray[y][x].typeBlock == fromTypeBlock)
                {

                    Block block = BlockManager.GetInstance.getTypeBlock(key, toTypeBlock, toTypeValue);

                    if (block != null)
                    {
                        blockActorArray[y][x].setBlock(block);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 야성해방 계산하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="combo"></param>
    /// <returns></returns>
    public abstract int calculateFeral(Block block, int combo);

    /// <summary>
    /// CPU 관련 블록 가져오기
    /// </summary>
    /// <returns></returns>
    public virtual Block getCPUBlock()
    {
        return null;
    }

    /// <summary>
    /// 자파리빵 결과값 가져오기
    /// </summary>
    /// <param name="typeJapariRate"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public int getJapariBread(TYPE_JAPARIRATE typeJapariRate, int value)
    {
        return (int)((float)value * m_japariRate[(int)typeJapariRate]);
    }

    /// <summary>
    /// 블록 무버 생성하기
    /// null - 없음
    /// </summary>
    /// <returns></returns>
    public virtual BlockMoverParent createBlockMover()
    {
        return null;
    }

    /// <summary>
    /// 화살표 패널 사용 여부
    /// Default - false
    /// true - 사용
    /// </summary>
    /// <returns></returns>
    public virtual UIButton.TYPE_BTN_PANEL useBtnPanel()
    {
        return UIButton.TYPE_BTN_PANEL.NONE;
    }

    public virtual void feralStart(Field field) { }
    public virtual void feralEnd(Field field) { }
}

