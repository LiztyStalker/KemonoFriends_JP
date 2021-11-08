using System.Collections.Generic;
using UnityEngine;

public class AdventureRepulse : AdventureBlock, IAdventure
{

    public AdventureRepulse(string key) : base(key) {
        m_japariRate = new float[] { 0.5f, 10f, 1f, 0.5f };
    }


    //public AdventureRepulse(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey) :
    //    base(key, images, name, contents, goal, cost, eventTime, bgmKey)
    //{
    //    m_japariRate = new float[] { 0.5f, 10f, 1f, 0.5f };
    //}

    public override void batchBlock(BlockActor[][] blockActorArray, float rate, BlockActorDelegate blockActorDel)
    {
        //랜덤으로 블록 25개 생성
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                createBlock(blockActorArray[y][x], rate);
            }
        }


        int count = 25;
        while (count >= 0)
        {
            int indexX = Random.Range(0, PrepClass.xCnt);
            int indexY = Random.Range(0, PrepClass.yCnt);



            if (blockActorArray[indexY][indexX].typeBlock == TYPE_BLOCK.NONE)
            {
                createBlock(blockActorArray[indexY][indexX], 1f, false);
                count--;
            }
        }
        


//        firstLineBatchBlock(blockActorArray[0], rate);

        //for (int y = 1; y < PrepClass.yCnt; y++)
        //{
        //    for (int x = 0; x < PrepClass.xCnt; x++)
        //    {
        //        blockActorArray[y][x].setBlock(BlockManager.GetInstance.getEmptyBlock());
        //    }
        //}
    }


    //void firstLineBatchBlock(BlockActor[] blockActorArray, float rate)
    //{
    //    for (int x = 0; x < PrepClass.xCnt; x++)
    //    {
    //        createBlock(blockActorArray[x], rate, false);
    //    }
    //}

    public override BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        if (!blockList.Contains(blockActor))
        {
            switch (blockActor.typeBlock)
            {
                case TYPE_BLOCK.NONE:
                    if(depth != 0)
                        return null;
                    break;
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


    //            indexAction(blockActorField, blockActorField[indexY][indexX], isFeral);


    //            //BlockActor blockActor = checkBlock(blockActorField[indexY][indexX], isFeral);

    //            //if (blockActor != null)
    //            //{
    //            //    blockList.Add(blockActor);
    //            //    if (blockActor.typeBlock == TYPE_BLOCK.SANDSTAR)
    //            //        checkSandStar(blockActorField, blockActor, isFeral);
    //            //}
    //        }
    //    }

    //    return blockList;
    //}

    //public override List<BlockActor> indexAction(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral)
    //{

    //}


    protected override void checkSandStar(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral, int depth)
    {
        int indexX = blockActor.indexX;
        int indexY = blockActor.indexY;

        switch (blockActor.typeValue)
        {
            //크게공격
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
            default:
                Debug.LogError(string.Format("{0} 해당하는 샌드스타 데이터가 없습니다.", blockActor.typeValue));
                break;

        }
    }

    /// <summary>
    /// 블록이 가득 차면 패배
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <returns></returns>
    public override bool isDefeat(BlockActor[][] blockActorField)
    {
        int count = 0;
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if (blockActorField[y][x].typeBlock != TYPE_BLOCK.NONE)
                {
                    count++;
                }
            }
        }


        //맨 밑줄에 세룰리안이 있으면 패배
        //for(int x = 0; x < PrepClass.xCnt; x++){
        //    if (blockActorField[PrepClass.yCnt - 1][x].typeBlock == TYPE_BLOCK.CERULEAN)
        //        return true;
        //}


        if (count == PrepClass.yCnt * PrepClass.xCnt)
            return true;

        return false;
    }

    public override float adventureEvent(Field field, float eventTime)
    {

        int count = 5 + (int)(field.gameTimeUp * 0.1f);
        int max = 100;
        while (count >= 0)
        {
            int indexX = Random.Range(0, PrepClass.xCnt);
            int indexY = Random.Range(0, PrepClass.yCnt);

            if (field.blockActorField[indexY][indexX].typeBlock == TYPE_BLOCK.NONE)
            {
                createBlock(field.blockActorField[indexY][indexX], field.friend.realLuck, false);
                count--;
                max = 100;
            }
            else
                max--;

            if (max < 0)
                break;
        }

        //for (int y = PrepClass.yCnt - 1; y >= 1; y--)
        //{
        //    for (int x = 0; x < PrepClass.xCnt; x++)
        //    {
        //        field.blockActorField[y][x].copyBlock(field.blockActorField[y - 1][x]);
        //    }
        //}

//        firstLineBatchBlock(field.blockActorField[0], field.friend.realLuck);
        
        if (field.isFeral)
            return eventTime * 0.75f;
        return eventTime;
    }

    public override int calculateDamage(int damage, bool isFeral)
    {
        return (isFeral) ? damage * 2 : damage;
    }

    public override int calculateFeral(Block block, int combo)
    {
        //세룰리안 등급에 따라서 달라짐
        if (block.typeBlock == TYPE_BLOCK.SANDSTAR)
            return 10;
        return 3;
    }
    

}

