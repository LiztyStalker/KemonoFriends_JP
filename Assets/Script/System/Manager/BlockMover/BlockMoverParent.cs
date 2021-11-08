using System;
using UnityEngine;

public abstract class BlockMoverParent : IBlockMover
{

    readonly Vector3 m_defaultSize = new Vector3(0.75f, 0.75f, 0.75f);
    float m_size = 1f;

    public Vector3 defaultSize { get { return m_defaultSize * m_size; } }

    /// <summary>
    /// 무버 사이즈 등록
    /// </summary>
    /// <param name="size"></param>
    public void setSize(float size) { m_size = size; }

    /// <summary>
    /// 이동에 대한 행동하기
    /// </summary>
    /// <param name="blockMover"></param>
    public virtual void moveAction(BlockMover blockMover)
    {
        //무버 이동속도 정하기
        if (blockMover.typeUser == BlockMover.TYPE_USER.PLAYER)
        {
            float speed = (blockMover.isFeral) ? 0.1f * blockMover.friend.realDex * 1.5f : 0.1f * blockMover.friend.realDex;
            blockMover.transform.position = Vector3.Lerp(blockMover.transform.position, blockMover.nextBlockActor.transform.position, speed);

        }
        else
            blockMover.transform.position = Vector3.Lerp(blockMover.transform.position, blockMover.nextBlockActor.transform.position, 0.05f);



        //점프 상태이면
        if (blockMover.jumpParticle.gameObject.activeSelf)
        {

            float nowDistance = Vector2.Distance(blockMover.nowBlockActor.transform.position, blockMover.transform.position);
            float nextDistance = Vector2.Distance(blockMover.nextBlockActor.transform.position, blockMover.transform.position);
            float totalDistance = Vector2.Distance(blockMover.nextBlockActor.transform.position, blockMover.nowBlockActor.transform.position);
            float rate;

            if (nextDistance > nowDistance)
            {
                //확대
                rate = (nowDistance / totalDistance) * 1.5f + 1f;

            }
            else
            {
                //축소
                rate = (nextDistance / totalDistance) * 1.5f + 1f;
            }

            blockMover.transform.localScale = new Vector3(m_defaultSize.x * rate, m_defaultSize.y * rate, 1f);
        }


        //목표에 도착했으면
        if (Vector2.Distance(blockMover.transform.position, blockMover.nextBlockActor.transform.position) < 0.075f)
        {
            blockMover.transform.position = blockMover.nextBlockActor.transform.position;
            blockMover.GetComponent<Collider2D>().enabled = true;
            blockMover.nowBlockActor = blockMover.nextBlockActor;
            blockMover.nextBlockActor = null;

            if (blockMover.jumpParticle.gameObject.activeSelf)
                setJump(blockMover, false);

        }
    }

    /// <summary>
    /// 다음으로 이동할 블록 가져오기
    /// </summary>
    /// <param name="blockMover"></param>
    /// <param name="isAuto"></param>
    /// <returns></returns>
    public virtual BlockActor getNextBlockActor(BlockMover blockMover, bool isAuto)
    {
        //자동 이동
        if (!isAuto)
            return blockMover.nextBlockActor;

        //멈춤
        if (blockMover.isStun)
            return blockMover.nextBlockActor;
        
        //다음 블록이 없으면
        if (blockMover.nextBlockActor == null)
        {
            //현재 블록에서 해당 방향으로 벽이 없으면
            if (isCheckMove(blockMover, blockMover.nowBlockActor, blockMover.typeArrow))
            {

                //현재 위치 가져오기
                int indexX = blockMover.nowBlockActor.indexX;
                int indexY = blockMover.nowBlockActor.indexY;

                //해당 화살표에 따른 연산
                switch (blockMover.typeArrow)
                {
                    case TYPE_ARROW.UP:
                        //도약
                        if ((int)blockMover.nowBlockActor.typeTopValue == 13)
                        {
                            setJump(blockMover, true);
                            blockMover.GetComponent<Collider2D>().enabled = false;
                            blockMover.nextBlockActor = blockMover.blockActorField[6][blockMover.nowBlockActor.indexX];
                            return blockMover.nextBlockActor;
                        }

                        indexY--;
                        break;
                    case TYPE_ARROW.DOWN:
                        //도약
                        if ((int)blockMover.nowBlockActor.typeTopValue == 14)
                        {
                            setJump(blockMover, true);
                            blockMover.GetComponent<Collider2D>().enabled = false;
                            blockMover.nextBlockActor = blockMover.blockActorField[0][blockMover.nowBlockActor.indexX];
                            return blockMover.nextBlockActor;
                        }

                        indexY++;
                        break;
                    case TYPE_ARROW.LEFT:
                        indexX--;
                        break;
                    case TYPE_ARROW.RIGHT:
                        indexX++;
                        break;
                }


                //인덱스가 블록 밖으로 넘어가지 않으면
                if (indexX >= 0 && indexX < PrepClass.xCnt && indexY >= 0 && indexY < PrepClass.yCnt)
                {

                    //블록 가져오기
                    BlockActor tmpBlockActor = blockMover.blockActorField[indexY][indexX];


                    //다음 방향의 벽에도 해당하는 방향 반대쪽에 벽이 없으면
                    if (isCheckMove(blockMover, tmpBlockActor, getReverseArrow(blockMover.typeArrow)))
                    {
                        //다음으로 향할 벽 등록
                        blockMover.nextBlockActor = tmpBlockActor;
                        return blockMover.nextBlockActor;
                    }
                }
            }
        }
        else
        {
            return blockMover.nextBlockActor;
        }

        //다음 블록 없애기
        blockMover.nextBlockActor = null;
        return blockMover.nextBlockActor;
    }

    /// <summary>
    /// 점프 설정하기
    /// </summary>
    /// <param name="blockMover"></param>
    /// <param name="isJump"></param>
    public virtual void setJump(BlockMover blockMover, bool isJump)
    {
        if (blockMover.jumpParticle != null)
        {
            //점프 파티클 보임 여부
            blockMover.jumpParticle.gameObject.SetActive(isJump);

            if (isJump)
                blockMover.soundPlay("EffectJump", TYPE_SOUND.EFFECT);
            else
                blockMover.transform.localScale = defaultSize;

        }
    }

    /// <summary>
    /// 지나갈 수 있는지 여부
    /// </summary>
    /// <param name="blockActor"></param>
    /// <returns></returns>
    bool isCheckMove(BlockMover blockMover, BlockActor blockActor, TYPE_ARROW typeArrow)
    {

        //사방이 막힌 블록이면 - CPU는 이동 가능
        if (blockMover.typeUser == BlockMover.TYPE_USER.CPU && (int)blockMover.nowBlockActor.typeTopValue == 15)
        {
            return true;
        }

        //통로이면
        if ((int)blockActor.typeTopValue == 13 || (int)blockActor.typeTopValue == 14)
        {
            return true;
        }

        //해당하는 방향에 벽이 있으면
        if (((int)blockActor.typeTopValue & (int)typeArrow) == (int)typeArrow)
        {
            //이동 못함

            return false;
        }
        //없으면 이동 가능
        return true;

    }

    /// <summary>
    /// 자동 움직임 체크
    /// </summary>
    /// <returns></returns>
    public abstract bool autoCheck();

    /// <summary>
    /// 방향 변경하기
    /// </summary>
    /// <param name="blockMover"></param>
    /// <param name="selector"></param>
    /// <param name="typeArrow"></param>
    public abstract void setArrowChange(BlockMover blockMover, int selector, TYPE_ARROW typeArrow);

    /// <summary>
    /// 각도 방향 가져오기
    /// </summary>
    /// <param name="typeArrow"></param>
    /// <returns></returns>
    public virtual float angleView(TYPE_ARROW typeArrow)
    {
        return 0f;
    }

    /// <summary>
    /// 야생해방 설정하기
    /// </summary>
    /// <param name="blockMover"></param>
    /// <param name="isFeral"></param>
    public virtual void setFeral(BlockMover blockMover, bool isFeral)
    {
        if (blockMover.feralParticle != null)
        {
            blockMover.feralParticle.gameObject.SetActive(isFeral);
        }

    }

    /// <summary>
    /// 방향 변경하기
    /// </summary>
    /// <param name="blockMover"></param>
    /// <param name="typeArrow"></param>
    protected void setArrowChange(BlockMover blockMover, TYPE_ARROW typeArrow)
    {
        if (blockMover.typeUser == BlockMover.TYPE_USER.PLAYER)
        {
            blockMover.typeArrow = typeArrow;

            angleView(typeArrow);

        }

        if (!blockMover.isAuto)
        {
            getNextBlockActor(blockMover, true);
        }

    }

    /// <summary>
    /// 방향 반대로 전환하기
    /// </summary>
    /// <param name="typeArrow"></param>
    /// <returns></returns>
    TYPE_ARROW getReverseArrow(TYPE_ARROW typeArrow)
    {
        int revArrow = (int)typeArrow * 4;
        if (revArrow > (int)TYPE_ARROW.UP)
            revArrow /= 16;
        return (TYPE_ARROW)revArrow;
    }

    /// <summary>
    /// 충돌 이벤트
    /// </summary>
    /// <param name="field"></param>
    /// <param name="blockMover"></param>
    /// <param name="empactBlockMover"></param>
    /// <returns></returns>
    public abstract bool crashAction(Field field, BlockMover blockMover, BlockMover crashBlockMover);

    /// <summary>
    /// 이동 제어
    /// </summary>
    /// <param name="field"></param>
    /// <param name="blockMover"></param>
    /// <returns></returns>
    public virtual bool moverController(Field field, BlockMover blockMover)
    {
        return false;
    }

    /// <summary>
    /// 방향 랜덤 설정하기
    /// </summary>
    /// <returns></returns>
    protected TYPE_ARROW randomArrow()
    {
        int cnt = UnityEngine.Random.Range(0, 4);
        return (TYPE_ARROW)pow(cnt);
    }

    /// <summary>
    /// 제곱 계산
    /// </summary>
    /// <param name="cnt"></param>
    /// <returns></returns>
    int pow(int cnt)
    {
        int pw = 1;
        while (cnt > 0)
        {
            pw *= 2;
            cnt--;
        }
        return pw;
    }

    /// <summary>
    /// 블록 가져오기
    /// </summary>
    /// <param name="field"></param>
    /// <param name="blockMover"></param>
    /// <param name="nowBlockActor"></param>
    /// <param name="nextBlockActor"></param>
    protected void catchBlock(Field field, BlockMover blockMover, BlockActor nowBlockActor, BlockActor nextBlockActor)
    {
        if (blockMover.typeUser == BlockMover.TYPE_USER.PLAYER)
        {
            if (nextBlockActor == null)
            {
                field.indexAction(nowBlockActor, 1);
            }
            else
            {
                if (Vector2.Distance(blockMover.transform.position, nextBlockActor.transform.position) <
                     Vector2.Distance(blockMover.transform.position, nowBlockActor.transform.position))
                {
                    field.indexAction(nextBlockActor, 1);
                }
                else
                    field.indexAction(nowBlockActor, 1);
            }
        }
    }
}

