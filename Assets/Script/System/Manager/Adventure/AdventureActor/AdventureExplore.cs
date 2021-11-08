using System.Collections.Generic;
using UnityEngine;


public class AdventureExplore : AdventureMover, IAdventure
{
    int[,] testMapGrid = new int[7, 7]{
        {12, 10, 10, 13, 10, 10, 9},
        {6, 8, 10, 2, 10, 8, 3},
        {12, 0, 8, 10, 8, 0, 9},
        {5, 5, 5, 15, 5, 5, 5},
        {6, 0, 2, 10, 2, 0, 3},
        {12, 2, 10, 8, 10, 2, 9},
        {6, 10, 10, 14, 10, 10, 3}
    };

    int m_count;
    const int m_maxCount = 15;
    const int m_createBlock = 1;


    public AdventureExplore(string key) : base(key) {
        m_japariRate = new float[] { 1f, 10f, 10f, 2f };
    }

    //public AdventureExplore(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey) :
    //    base(key, images, name, contents, goal, cost, eventTime, bgmKey)
    //{
    //    m_japariRate = new float[] { 1f, 10f, 10f, 2f };
    //}

    public override void batchBlock(BlockActor[][] blockActorArray, float rate, BlockActorDelegate blockActorDel)
    {
        m_count = 12;

        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                createBlock(blockActorArray[y][x], rate);

                Block block = BlockManager.GetInstance.getWallBlock(key, (TYPE_VALUE)testMapGrid[y, x]);

                if (block != null)
                {
                    blockActorArray[y][x].setBlock(block);
                    blockActorArray[y][x].setInnerBlock(blockActorDel(x, y));

                    //if (x == 0 && y == 0 || x == 3 && y == 3)
                    //    createBlock(blockActorArray[y][x], rate);
                    //else
                        createBlock(blockActorArray[y][x], rate);
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            randomBlockCreate(blockActorArray, rate);
        }

    }

    public override BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        if (!blockList.Contains(blockActor))
        {
            switch (blockActor.typeBlock)
            {
                case TYPE_BLOCK.NONE:
                    return null;
            }
            return blockActor;
        }
        return null;
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
            //자파리빵으로채우기
            case TYPE_VALUE.VALUE_05:
                fieldFillBread(blockActorField, TYPE_BLOCK.BLOCK);
                break;
            default:
                Debug.LogError(string.Format("{0} 해당하는 샌드스타 데이터가 없습니다.", blockActor.typeValue));
                break;

        }
    }

    //public override bool isLose(BlockActor blockActorField)
    //{
    //    //세룰리안과 닿으면 패배
    //    return false;
    //}

    public override float adventureEvent(Field field, float eventTime)
    {
        m_count++;
        if (m_count % m_maxCount == 0)
        {
//            m_blockMoverDel(blockMover, BlockMover.TYPE_USER.CPU, 3, 3);
            field.createBlockMover(resourceBlockMover, BlockMover.TYPE_USER.CPU, 3, 3);
        }

        if (m_count % m_createBlock == 0)
        {
            randomBlockCreate(field.blockActorField, field.friend.realLuck);
        }

        return eventTime;

    }

    void randomBlockCreate(BlockActor[][] blockActorField, float rate)
    {
        bool isCheck = false;
        int maxLoop = 100;
        while (!isCheck)
        {

            
            int indexX = Random.Range(0, PrepClass.xCnt);
            int indexY = Random.Range(0, PrepClass.yCnt);

            if (indexX == 3 && indexY == 3)
            {
                maxLoop--;
                if (maxLoop <= 0)
                    break;
                continue;
            }

            if (blockActorField[indexY][indexX].typeBlock == TYPE_BLOCK.NONE)
            {
                createBlock(blockActorField[indexY][indexX], rate, false);
                isCheck = true;
            }
        }
    }

    public override int calculateFeral(Block block, int combo)
    {
        if (block.typeBlock == TYPE_BLOCK.SANDSTAR)
            return 10;
        return 5 + combo;
    }


    public override BlockMover createMover(BlockMoverDelegate blockMoverDel)
    {
        return blockMoverDel(resourceBlockMover, BlockMover.TYPE_USER.PLAYER, 0, 0);
    }

    public override BlockMoverParent createBlockMover()
    {
        return new BlockMoverExplore();
    }


    public override UIButton.TYPE_BTN_PANEL useBtnPanel()
    {
        return UIButton.TYPE_BTN_PANEL.ARROW;
    }

    public override Block getCPUBlock()
    {
        return BlockManager.GetInstance.getCeruleanBlock(key, TYPE_VALUE.VALUE_00);
    }
}

