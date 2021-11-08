using System;
using System.Collections.Generic;
using UnityEngine;

public class AdventureDrive : AdventureMover, IAdventure
{

    enum TYPE_DRIVE_PATTERN {NORMAL, RANDOM, STAIR, WALL }


    TYPE_DRIVE_PATTERN m_pattern;
    int m_patternCount = 5;
    int[] m_focusPoints = { 0, 0 };

    //public AdventureDrive(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey) :
    //    base(key, images, name, contents, goal, cost, eventTime, bgmKey)
    //{
    //    m_japariRate = new float[] { 0.5f, 10f, 0.5f, 0.5f };
    //}

    public AdventureDrive(string key) : base(key) {
        m_japariRate = new float[] { 0.5f, 10f, 0.5f, 0.5f };
    }

    public override void batchBlock(BlockActor[][] blockActorArray, float rate, BlockActorDelegate blockActorDel)
    {
        
        m_patternCount = 5;
        m_pattern = TYPE_DRIVE_PATTERN.NORMAL;

        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                createBlock(blockActorArray[y][x], rate);

                Block block = BlockManager.GetInstance.getWallBlock(key, TYPE_VALUE.VALUE_00);

                if (block != null)
                {
                    blockActorArray[y][x].setBlock(block);
                    blockActorArray[y][x].setInnerBlock(blockActorDel(x, y));
                    createBlock(blockActorArray[y][x], rate);
                }
            }
        }
    }

    /// <summary>
    /// 세룰리안에 닿으면 패배
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <returns></returns>
    public override bool isDefeat(BlockActor blockActor, bool isFeral)
    {
        if (!isFeral)
        {
            if (blockActor.typeBlock == TYPE_BLOCK.CERULEAN)
                return true;
        }
        return false;
    }

    protected override void checkSandStar(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral, int depth)
    {
        int indexX = blockActor.indexX;
        int indexY = blockActor.indexY;

        switch (blockActor.typeValue)
        {
            //크게먹기
            case TYPE_VALUE.VALUE_00:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 2, 2);
                break;
            //야성해방 스코어 증가
            //case TYPE_VALUE.VALUE_01:
            //    indexAction(blockActorField, indexX, indexY, 1, 1, isFeral);
            //    break;
            //가로공격
            case TYPE_VALUE.VALUE_02:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 7, 0);
                break;
            //세로공격
            case TYPE_VALUE.VALUE_03:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 0, 7);
                break;
            //가로세로 공격
            case TYPE_VALUE.VALUE_04:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 0, 7);
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 7, 0);
                break;
            //자파리빵으로 채우기
            case TYPE_VALUE.VALUE_05:
                fieldFillBread(blockActorField, TYPE_BLOCK.BLOCK);
                break;
            //세룰리안을 자파리빵으로
            case TYPE_VALUE.VALUE_06:
                blockChange(blockActorField, TYPE_BLOCK.CERULEAN, TYPE_BLOCK.BLOCK);
                break;
            default:
                Debug.LogError(string.Format("{0} 해당하는 샌드스타 데이터가 없습니다.", blockActor.typeValue));
                break;

        }
    }

    public override List<BlockActor> indexAction(
       BlockActor[][] blockActorField,
       BlockActor blockActor,
       bool isFeral,
       int depth = 0
       )
    {

        blockActor = checkBlock(blockActor, depth, isFeral);
        if (blockActor != null)
        {
            blockList.Add(blockActor);

            if (depth == 1 && isFeral)
                indexAction(blockActorField, blockActor.indexX, blockActor.indexY, depth, isFeral, 2, 2);

            if (blockActor.typeBlock == TYPE_BLOCK.SANDSTAR)
                checkSandStar(blockActorField, blockActor, isFeral, depth);
        }
        return blockList;
    }

    //public override List<BlockActor> indexAction(BlockActor[][] blockActorField, int nowX, int nowY, int rangeX = 1, int rangeY = 1, bool isFeral = false)
    //{
    //    int indexY = nowY;
    //    int indexX = nowX;

    //    for (int y = -rangeY; y <= rangeY; y++)
    //    {
    //        indexY = nowY + y;

    //        if (indexY < 0)
    //            continue;
    //        else if (indexY >= PrepClass.yCnt)
    //            break;

    //        for (int x = -rangeX; x <= rangeX; x++)
    //        {
    //            indexX = nowX + x;

    //            if (indexX < 0)
    //                continue;
    //            else if (indexX >= PrepClass.xCnt)
    //                break;

    //            BlockActor blockActor = checkBlock(blockActorField[indexY][indexX], isFeral);

    //            if (blockActor != null)
    //            {
    //                blockList.Add(blockActor);
    //                if (blockActor.block.typeBlock == TYPE_BLOCK.SANDSTAR)
    //                    checkSandStar(blockActorField, blockActor, isFeral);
    //            }
    //        }
    //    }

    //    return blockList;
    //}

    public override BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        if (!blockList.Contains(blockActor))
        {
            switch (blockActor.typeBlock)
            {
                case TYPE_BLOCK.NONE:
                    if (!isFeral)
                    {
                        return null;
                    }
                    break;
                case TYPE_BLOCK.CERULEAN:
                    if(!isFeral)
                        return null;
                    break;
            }
            return blockActor;
        }
        return null;
    }

    public override float adventureEvent(Field field, float eventTime)
    {

        for (int y = PrepClass.yCnt - 1; y >= 0; y--)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if(y == 0)
                    field.blockActorField[y][x].setBlock(BlockManager.GetInstance.getEmptyBlock());
                else
                    field.blockActorField[y][x].copyBlock(field.blockActorField[y - 1][x]);
            }
        }

        //야생해방 시간 필요
        //1.5
        //isFeral - 야생해방
        //feralRate - 야생해방 시간 삽입
        firstLineBatchBlock(field.blockActorField[0], field.friend.realLuck, field.isFeral, field.feralRate);

        if (field.isFeral)
            return eventTime * 0.1f;
        return eventTime;
    }

    //1.5 
    //isFeral - 야생해방 삽입
    //feralRate - 야성해방 시간 삽입
    void firstLineBatchBlock(BlockActor[] blockActorArray, float rate, bool isFeral, float feralRate)
    {

        //패턴에 따라서 벽 출력하기
        //5초 또는 8초마다 변경 - 아니면 해당 패턴의 카운트마다 변경
        //야생해방이 거의 끝나갈 때 - 1초 남았을 때 일반 패턴만 나옴
        if (m_patternCount <= 0)
            m_pattern = selectPattern();

        //야성해방이고 얼마 남지 않았을 경우 - 일반 패턴만 나오기
        if (isFeral && feralRate < 0.05f)
        {
            m_patternCount = 2;
            m_pattern = TYPE_DRIVE_PATTERN.NORMAL;
        }

        runPattern(blockActorArray, rate);
//        randomPattern(blockActorArray, rate);
    }

    /// <summary>
    /// 패턴 선택하기
    /// </summary>
    /// <returns></returns>
    TYPE_DRIVE_PATTERN selectPattern()
    {



        if (m_pattern == TYPE_DRIVE_PATTERN.NORMAL)
        {
            m_patternCount = UnityEngine.Random.Range(5, 25);
            m_focusPoints = setFocusPoint();
            return (TYPE_DRIVE_PATTERN)UnityEngine.Random.Range(1, Enum.GetValues(typeof(TYPE_DRIVE_PATTERN)).Length);
        }
        else
        {
            m_patternCount = 5;
            return TYPE_DRIVE_PATTERN.NORMAL;
        }
    }

    int[] setFocusPoint()
    {
        int[] focusPoints = new int[m_focusPoints.Length];
        for (int i = 0; i < focusPoints.Length; i++)
        {
            focusPoints[i] = UnityEngine.Random.Range(0, PrepClass.xCnt);
        }
        return focusPoints;
    }


    void runPattern(BlockActor[] blockActorArray, float rate)
    {
        m_patternCount--;

        switch (m_pattern)
        {
            case TYPE_DRIVE_PATTERN.NORMAL:
                normalPattern(blockActorArray, rate);
                break;
            case TYPE_DRIVE_PATTERN.RANDOM:
                randomPattern(blockActorArray, rate);
                break;
            case TYPE_DRIVE_PATTERN.STAIR:
                stairPattern(blockActorArray, rate);
                break;
            case TYPE_DRIVE_PATTERN.WALL:
                wallPattern(blockActorArray, rate);
                break;
            //case TYPE_DRIVE_PATTERN.HIGHWAY:
            //    highwayPattern(blockActorArray, rate);
            //    break;
        }
    }

    /// <summary>
    /// 세룰리안이 안나오는 패턴
    /// </summary>
    /// <param name="blockActorArray"></param>
    /// <param name="rate"></param>
    void normalPattern(BlockActor[] blockActorArray, float rate)
    {
        for (int x = 0; x < PrepClass.xCnt; x++)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < 50)
                blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));
        }
    }

    /// <summary>
    /// 랜덤으로 블록이 생성되는 패턴
    /// </summary>
    /// <param name="blockActorArray"></param>
    /// <param name="rate"></param>
    void randomPattern(BlockActor[] blockActorArray, float rate)
    {
        for (int x = 0; x < PrepClass.xCnt; x++)
        {

            if (m_focusPoints[0] == x)
            {
                blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));
                continue;
            }

            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < 50)
                createBlock(blockActorArray[x], rate, false);
            else
                createBlock(blockActorArray[x], rate);

        }

        m_focusPoints = focusCalculator();

    }

    /// <summary>
    /// 계단형으로 생성되는 패턴
    /// </summary>
    /// <param name="blockActorArray"></param>
    /// <param name="rate"></param>
    void stairPattern(BlockActor[] blockActorArray, float rate)
    {
        //기준점을 기준으로 계단을 제작해야 함
        //
        
        for (int x = 0; x < PrepClass.xCnt; x++)
        {
            for (int i = 0; i < m_focusPoints.Length; i++)
            {
                if (m_focusPoints[i] - 1 <= x && m_focusPoints[i] + 1 >= x)
                    blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));
                else
                    blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.CERULEAN, rate));
            }
        }

        m_focusPoints = focusCalculator();


    }

    void wallPattern(BlockActor[] blockActorArray, float rate)
    {
        for (int x = 0; x < PrepClass.xCnt; x++)
        {
            for (int i = 0; i < m_focusPoints.Length; i++)
            {
                if (m_patternCount % 2 == 0)
                {
                    if (m_focusPoints[i] - 1 <= x && m_focusPoints[i] >= x) 
                        blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));
                    else
                        blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.CERULEAN, rate));
                }
                else
                {
                    blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));
                }
            }
        }

        m_focusPoints = focusCalculator();
    }

    //void highwayPattern(BlockActor[] blockActorArray, float rate)
    //{


    //    for (int x = 0; x < PrepClass.xCnt - 1; x++)
    //    {
    //        for (int i = 0; i < m_focusPoints.Length; i++)
    //        {
    //            if(x == 0 || x == PrepClass.xCnt - 1)
    //                blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));

    //            if (m_focusPoints[i] - 1 <= x && m_focusPoints[i] + 1 >= x)
    //                blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.CERULEAN, rate));
    //            else
    //                blockActorArray[x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.BLOCK, rate));
    //        }
    //    }

    //    m_focusPoints = focusCalculator(true);
    //}


    public override int calculateFeral(Block block, int combo)
    {
        if (block.typeBlock == TYPE_BLOCK.SANDSTAR)
            return 10;
        return 3;
    }

    int[] focusCalculator(bool isEdge = false)
    {
        for (int i = 0; i < m_focusPoints.Length; i++)
        {
            if (UnityEngine.Random.Range(0, 100) > 50)
                m_focusPoints[i]++;
            else
                m_focusPoints[i]--;

            //가장자리 여부
            if (isEdge)
            {
                //가장자리 전까지
                if (m_focusPoints[i] < 1)
                    m_focusPoints[i] = 1;
                else if (m_focusPoints[i] > PrepClass.xCnt - 1)
                    m_focusPoints[i] = PrepClass.xCnt - 1;
            }
            else
            {
                if (m_focusPoints[i] < 0)
                    m_focusPoints[i] = 0;
                else if (m_focusPoints[i] > PrepClass.xCnt)
                    m_focusPoints[i] = PrepClass.xCnt;
            }

        }
        return m_focusPoints;

    }

    public override BlockMover createMover(BlockMoverDelegate blockMoverDel)
    {
        return blockMoverDel(resourceBlockMover, BlockMover.TYPE_USER.PLAYER, 3, 5);
    }

    public override BlockMoverParent createBlockMover()
    {
        return new BlockMoverDrive();
    }

    public override UIButton.TYPE_BTN_PANEL useBtnPanel()
    {
        return UIButton.TYPE_BTN_PANEL.ARROW;
    }

    //public override void feralStart(Field field)
    //{
    //    //속도 업
    //}

    //public override void feralEnd(Field field)
    //{
    //    //속도 다운
    //}
}

