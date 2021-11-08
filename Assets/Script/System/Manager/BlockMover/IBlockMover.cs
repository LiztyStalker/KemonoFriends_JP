using UnityEngine;


interface IBlockMover
{
    Vector3 defaultSize { get; }

    void setSize(float size);
    void setFeral(BlockMover blockMover, bool isFeral);
    float angleView(TYPE_ARROW typeArrow);
    bool autoCheck();
    void setArrowChange(BlockMover blockMover, int selector, TYPE_ARROW typeArrow);
    void moveAction(BlockMover blockMover);
    void setJump(BlockMover blockMover, bool isJump);
    BlockActor getNextBlockActor(BlockMover blockMover, bool isAuto);
    bool crashAction(Field field, BlockMover blockMover, BlockMover crashBlockMover);
    bool moverController(Field field, BlockMover blockMover);
}

