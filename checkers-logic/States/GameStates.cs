using checkers_logic.Models;

namespace checkers_logic.States;

public interface IGameState
{
    public GameState Update((int row, int col)? cellIdx = null);
    public void Enter();
    public void Exit();
}

/// <summary>
/// Base class for game state, substates can be created.
/// </summary>
public abstract class BaseGameState : IGameState
{
    protected BoardModel boardModel;

    public abstract GameState Update((int row, int col)? cellIdx = null);
    public abstract void Enter();
    public abstract void Exit();

    public BaseGameState(BoardModel BoardModel)
    {
        boardModel = BoardModel;
    }
}

internal class InitialState : BaseGameState
{
    public InitialState(BoardModel BoardModel) : base(BoardModel)
    {
       
    }

    public override void Enter()
    {
        boardModel.SetTurn(FigureColor.None);
    }

    public override void Exit()
    {

    }

    public override GameState Update((int row, int col)? cellIdx = null)
    {
        // todo logic

        return GameState.WhiteTurn;
    }
}

public class WhiteTurnState : BaseGameState
{
    private Dictionary<TurnState, ITurnState> states;
    private TurnState currentState = TurnState.PickUpAttacker;

    public WhiteTurnState(BoardModel BoardModel) : base(BoardModel)
    {
        states = new()
        {
            { TurnState.PickUpAttacker, new PickUpAttackerState(BoardModel) },
            { TurnState.SelectTarget, new SelectTargetState(BoardModel) },
            { TurnState.AttackSequence, new AttackSequenceState(BoardModel) }
        };
    }

    public override void Enter()
    {
        boardModel.SetTurn(FigureColor.White);
        currentState = TurnState.PickUpAttacker;
    }

    public override void Exit()
    {
    }

    public override GameState Update((int row, int col)? cellIdx)
    {
        boardModel.SelectCell(cellIdx!.Value);

        TurnState state = states[currentState].Update();

        if (state == currentState) 
            return GameState.WhiteTurn;

        states[currentState].Exit();
        currentState = state;

        if (currentState == TurnState.Finished)
            return boardModel.GameWinner != FigureColor.None
                ? GameState.Finite : GameState.BlackTurn;

        states[currentState].Enter();

        return GameState.WhiteTurn;
    }
}

    public class BlackTurnState : BaseGameState
    {
        private Dictionary<TurnState, ITurnState> states;
        private TurnState currentState = TurnState.PickUpAttacker;

        public BlackTurnState(BoardModel BoardModel) : base(BoardModel)
        {
            states = new()
            {
                { TurnState.PickUpAttacker, new PickUpAttackerState(BoardModel) },
                { TurnState.SelectTarget, new SelectTargetState(BoardModel) },
                { TurnState.AttackSequence, new AttackSequenceState(BoardModel) }
            };
        }

        public override void Enter()
        {
            boardModel.SetTurn(FigureColor.Black);
            currentState = TurnState.PickUpAttacker;
        }

        public override void Exit()
        {

        }

        public override GameState Update((int row, int col)? cellIdx)
        {
            boardModel.SelectCell(cellIdx!.Value);

            TurnState state = states[currentState].Update();

            if (state == currentState)
                return GameState.BlackTurn;

            states[currentState].Exit();
            currentState = state;

            if (currentState == TurnState.Finished)
                return boardModel.GameWinner != FigureColor.None
                ? GameState.Finite : GameState.WhiteTurn;

            states[currentState].Enter();

            return GameState.BlackTurn;
        }
    }

    public class FiniteState : BaseGameState
    {
        public FiniteState(BoardModel BoardModel) : base(BoardModel)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override GameState Update((int row, int col)? cellIdx = null)
        {
            return GameState.Finite;
        }
    }
