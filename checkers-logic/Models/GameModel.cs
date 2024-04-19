using checkers_logic.States;

namespace checkers_logic.Models
{
    public class GameModel
    {
        private BoardModel boardModel;
        private GameState currentState;
        private Dictionary<GameState, IGameState> states;

        public GameState CurrentState => currentState;

        public GameModel()
        {
            boardModel = new BoardModel();
            currentState = GameState.Initial;
            states = new()
            {
                { GameState.Initial, new InitialState(boardModel) },
                { GameState.WhiteTurn, new WhiteTurnState(boardModel) },
                { GameState.BlackTurn, new BlackTurnState(boardModel) },
                { GameState.Finite, new FiniteState(boardModel) }
            };
        }

        public void UpdateState((int row, int col)? cellIdx = null)
        {
            // modify so it will not stop updating until it's real player move, or game is finished

            GameState newState = states[currentState].Update(cellIdx);

            if (newState == currentState) return;

            states[currentState].Exit();
            currentState = newState;
            states[currentState].Enter();
        }

        public (CellState[,] boardState, IReadOnlySet<(int row, int col)> attackCells, IReadOnlySet<(int row, int col)> moveCells) DrawData => boardModel.DrawData;
        public FigureColor GameWinner => boardModel.GameWinner;
    }
}
