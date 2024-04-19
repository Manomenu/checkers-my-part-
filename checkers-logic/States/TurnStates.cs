using checkers_logic.Models;

namespace checkers_logic.States;

public interface ITurnState
{
    public TurnState Update();
    public void Enter();
    public void Exit();
}

public abstract class BaseTurnState : ITurnState
{
    protected BoardModel boardModel;

    public BaseTurnState(BoardModel BoardModel)
    {
        boardModel = BoardModel;
    }


    public abstract void Enter();

    public abstract void Exit();

    public abstract TurnState Update();
}

public class PickUpAttackerState : BaseTurnState
{
    public PickUpAttackerState(BoardModel BoardModel) : base(BoardModel)
    {
    }

    public override void Enter()
    {
        boardModel.UnselectAttacker();
    }

    public override void Exit()
    {
    }

    public override TurnState Update()
    {
        boardModel.SetSelectedCellAsAttacker();

        if (DictionariesLogic.CellStateToColor(boardModel.AttackerCell.state) != boardModel.TurnColor
            || !boardModel.AnyAttackerLegalTarget || (boardModel.AnyLegalAttackTarget && !boardModel.AnyAttackerLegalAttackTarget))
        {
            boardModel.UnselectAttacker();
            return TurnState.PickUpAttacker;
        }
        
        return TurnState.SelectTarget;
    }
}

public class SelectTargetState : BaseTurnState
{
    public SelectTargetState(BoardModel BoardModel) : base(BoardModel)
    {
    }

    public override void Enter()
    {
        boardModel.UnselectTarget();
    }

    public override void Exit()
    {
        boardModel.UnselectTarget();
    }

    public override TurnState Update()
    {
        boardModel.SetSelectedCellAsTarget();
        if (!boardModel.IsSelectedTargetLegal)
        {
            return TurnState.PickUpAttacker;
        }
        if (boardModel.IsTargetLegalAttack) // move
        {
            boardModel.PerformAttackerToTargetAction();
            if (boardModel.AnyAttackerLegalAttackTarget)
                return TurnState.AttackSequence;
            else
            {
                boardModel.UnselectAttacker();
                return TurnState.Finished;
            }
        }
        else
        {
            boardModel.PerformAttackerToTargetAction();
            boardModel.UnselectAttacker();
            return TurnState.Finished;
        }
    }
}

public class AttackSequenceState : BaseTurnState
{
    public AttackSequenceState(BoardModel BoardModel) : base(BoardModel)
    {
    }

    public override void Enter()
    {
        boardModel.UnselectTarget();
    }

    public override void Exit()
    {
        boardModel.UnselectTarget();
        boardModel.UnselectCell();
        boardModel.UnselectAttacker();
    }

    public override TurnState Update()
    {
        boardModel.SetSelectedCellAsTarget();

        if (!boardModel.IsTargetLegalAttack)
        {
            boardModel.UnselectTarget();
            return TurnState.AttackSequence;
        }
        else
        {
            boardModel.PerformAttackerToTargetAction();
            boardModel.UnselectTarget();
            if (boardModel.AnyAttackerLegalAttackTarget)
                return TurnState.AttackSequence;
            else 
                return TurnState.Finished;
        }
    }
}
