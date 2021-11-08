using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


interface IAdventure
{

    BlockMover createMover(AdventureActor.BlockMoverDelegate blockMoverDel);
    void batchBlock(BlockActor[][] blockActorArray, float rate, AdventureActor.BlockActorDelegate blockActorDel);

    List<BlockActor> indexAction(
        BlockActor[][] blockActorField,
        BlockActor blockActor,
        bool isFeral,
        int depth = 0
        );

    void indexAction(BlockMover blockMover, BlockActor blockActor);

    BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false);
    int calculateFeral(Block block, int combo);
    int getJapariBread(AdventureActor.TYPE_JAPARIRATE typeJapariRate, int value);
    float adventureEvent(Field field, float eventTime);
    int calculateDamage(int damage, bool isFeral);
    void clearBlockList();
    Block createBlock(BlockActor blockActor, float rate, bool isEmpty = true);
    bool isDefeat(BlockActor[][] blockActorField);
    bool isDefeat(BlockActor blockActor, bool isFeral);
    bool isDefeat(int loseCount);

    BlockMoverParent createBlockMover();
    Block getCPUBlock();
    UIButton.TYPE_BTN_PANEL useBtnPanel();

//    bool moverController(BlockMover blockMover);

    void feralStart(Field field);
    void feralEnd(Field field);
}
