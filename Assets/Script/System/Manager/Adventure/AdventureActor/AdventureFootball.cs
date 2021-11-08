using System.Collections.Generic;
using UnityEngine;

//1.4
public class AdventureFootball : AdventureMover, IAdventure
{

    const float c_coordinateOffset = 0.5f;
    const float c_acceleration = 2.5f;
    const float c_addAcceleration = 2f;
    const int c_createBlock = 5;
    bool m_isShoot;
    const int m_count = 1;
    int m_cnt = 0;

    public AdventureFootball(string key)
        : base(key)
    {
        m_japariRate = new float[] { 1.5f, 10f, 2f, 1.5f };
        m_cnt = 0;
    }
    public override BlockMoverParent createBlockMover()
    {
        return new BlockMoverBall();
    }

    
    public override BlockMover createMover(BlockMoverDelegate blockMoverDel)
    {
        return blockMoverDel(resourceBlockMover, BlockMover.TYPE_USER.CPU, 3, 3, 1.5f);
    }

    //1.4
    public override void batchBlock(BlockActor[][] blockActorArray, float rate, AdventureActor.BlockActorDelegate blockActorDel)
    {
        m_isShoot = false;

        int cnt = 0;

        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            if (y % 2 == 1)
            {
                for (int x = 0; x < PrepClass.xCnt; x++)
                {
                    blockActorArray[y][x].setBlock(BlockManager.GetInstance.getEmptyBlock());
                }
            }
            else
            {

                for (int x = 0; x < PrepClass.xCnt; x++)
                {
                    //홀수이면 없어야 함
                    //
                    //y1~5이면 비어있어야 함

                        if (x % 2 == 0)
                        {
                            if ((x == 0 || x == PrepClass.yCnt - 1) || !(y >= 1 && y <= PrepClass.yCnt - 2))
                            {
                                blockActorArray[y][x].setBlock(BlockManager.GetInstance.getTypeBlock(key, TYPE_BLOCK.FRIEND, (TYPE_VALUE)cnt));
                                cnt++;
                            }
                            else
                            {
                                blockActorArray[y][x].setBlock(BlockManager.GetInstance.getEmptyBlock());
                            }
                        }
                        else
                            blockActorArray[y][x].setBlock(BlockManager.GetInstance.getEmptyBlock());
                }
            }
        }


        int count = c_createBlock;
        int maxLimit = 50;

        while (count-- > 0)
        {
            int indexX = Random.Range(1, PrepClass.xCnt - 1);
            int indexY = Random.Range(1, PrepClass.yCnt - 1);

            if (blockActorArray[indexY][indexX].typeBlock == TYPE_BLOCK.NONE)
            {
                createBlock(blockActorArray[indexY][indexX], rate, false);
                maxLimit = 50;
            }
            else
            {
                count++;
                maxLimit--;
            }

            if (maxLimit <= 0)
                break;
        }
    }

    public override BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        
        //
        //세룰리안이면 세룰리안 없애고 튕겨나가야 함
        //
        if (!blockList.Contains(blockActor)){

            switch (blockActor.typeBlock)
            {
                case TYPE_BLOCK.CERULEAN:

                    if (!blockActor.field.isFeral)
                    {
                        foreach (BlockMover mover in blockActor.field.cpuList)
                        {
                            indexAction(mover, blockActor);
                        }
                    }

                    break;
            }

            return blockActor;
        }
        return null;
    }

    public override float adventureEvent(Field field, float eventTime)
    {
        //1.4
        //1회성으로 쏘기
        if (!m_isShoot)
        {
            foreach (BlockMover mover in field.cpuList)
            {

                float range = UnityEngine.Random.Range(-180f, 180f);
                float vX = Mathf.Cos(range * Mathf.Deg2Rad) * c_acceleration;
                float vY = Mathf.Sin(range * Mathf.Deg2Rad) * c_acceleration;

                Debug.Log(range + " " + vX + " " + vY);

                mover.GetComponent<Rigidbody2D>().velocity = new Vector2(vX, vY);
            }
            m_isShoot = true;
        }


        if (m_cnt >= m_count)
        {
            m_cnt = 0;
            int maxLimit = 50;
            while (maxLimit-- > 0)
            {
                int x = UnityEngine.Random.Range(1, PrepClass.xCnt - 1);
                int y = UnityEngine.Random.Range(1, PrepClass.yCnt - 1);

                if (field.blockActorField[y][x].typeBlock == TYPE_BLOCK.NONE)
                {
                    createBlock(field.blockActorField[y][x], field.friend.realLuck, false);
                    break;
                }

            }

        }
        else
        {
            m_cnt++;
        }

        if (field.isFeral)
            return eventTime * 0.2f;
        return eventTime;
    }

    public override int calculateFeral(Block block, int combo)
    {
        if(block.typeBlock == TYPE_BLOCK.FRIEND)
            return combo + 1;
        return 3;
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
//                indexAction(blockActorField, indexX, indexY, depth, isFeral, 7, 0);
                break;
            //세로공격
            case TYPE_VALUE.VALUE_03:
//                indexAction(blockActorField, indexX, indexY, depth, isFeral, 0, 7);
                break;
            //가로세로 공격
            case TYPE_VALUE.VALUE_04:
//                indexAction(blockActorField, indexX, indexY, depth, isFeral, 0, 7);
//                indexAction(blockActorField, indexX, indexY, depth, isFeral, 7, 0);
                break;
            //자파리빵으로 채우기
            case TYPE_VALUE.VALUE_05:
                fieldFillBread(blockActorField, TYPE_BLOCK.BLOCK);
                break;
            //세룰리안을 자파리빵으로
            case TYPE_VALUE.VALUE_06:
//                blockChange(blockActorField, TYPE_BLOCK.CERULEAN, TYPE_BLOCK.BLOCK);
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
            blockActor.depth = depth;
            blockList.Add(blockActor);

            if (depth == 0)
                indexAction(blockActorField, blockActor.indexX, blockActor.indexY, depth, isFeral, 0, 0);

            if (blockActor.typeBlock != TYPE_BLOCK.FRIEND)
            {
                if (blockActor.typeBlock == TYPE_BLOCK.SANDSTAR)
                    checkSandStar(blockActorField, blockActor, isFeral, depth);
            }
        }
        return blockList;
    }

    //1.4
    //충돌
    public override void indexAction(BlockMover blockMover, BlockActor blockActor)
    {
        //현재 블록의 위치 가져오기
        Vector3 nowPos = blockActor.transform.position;

        //블록 좌표 offset 조정
        //북쪽 블록
        if (blockActor.indexX == 0)
            nowPos.x -= c_coordinateOffset;
        //남쪽 블록
        else if (blockActor.indexX == PrepClass.xCnt - 1)
            nowPos.x += c_coordinateOffset;

        //서쪽 블록
        if(blockActor.indexY == 0)
            nowPos.y += c_coordinateOffset;
        //동쪽 블록
        else if(blockActor.indexY == PrepClass.yCnt - 1)
            nowPos.y -= c_coordinateOffset;


        //블록과 무버의 거리 가져오기
        Vector2 distance = blockMover.transform.position - nowPos;


        //각도 계산
        float rad = Mathf.Atan2(distance.y, distance.x);
        //float vX = Mathf.Cos(rad) * m_accelator;
        //float vY = Mathf.Sin(rad) * m_accelator;

        //가속도 구하기
        Vector2 velocity = new Vector2(Mathf.Cos(rad) * c_acceleration, Mathf.Sin(rad) * c_acceleration);


        //첫속도
        blockMover.GetComponent<Rigidbody2D>().velocity = velocity * ((blockActor.field.isFeral) ? c_addAcceleration : 1f);
        blockMover.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-720f, 720f);
        blockMover.createParticle();
        blockMover.soundPlay("EffectHit", TYPE_SOUND.EFFECT);
    }

    // 1.4
    public override Block getCPUBlock()
    {
        return BlockManager.GetInstance.getBallBlock(key);
    }

    public override UIButton.TYPE_BTN_PANEL useBtnPanel()
    {
        return UIButton.TYPE_BTN_PANEL.NONE;
    }

    public override void feralStart(Field field)
    {
        foreach(BlockMover mover in field.cpuList)
            mover.GetComponent<Rigidbody2D>().velocity *= c_addAcceleration * field.friend.realLuck;
    }

    public override void feralEnd(Field field)
    {
        foreach (BlockMover mover in field.cpuList)
            mover.GetComponent<Rigidbody2D>().velocity /= (c_addAcceleration * field.friend.realLuck);
    }
}

