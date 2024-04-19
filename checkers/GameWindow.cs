using checkers_logic;
using checkers_logic.Models;

namespace checkers
{
    public partial class GameWindow : Form
    {
        private Board board;
        private GameModel game;

        public GameWindow()
        {
            InitializeComponent();
            SetConstants();

            board = new Board(BoardPictureBox);
            game = new GameModel();


            board.Draw(game.DrawData);
        }

        private void SetConstants()
        {
            Constants.CellSize = BoardPictureBox.Height / 8;
        }

        private void BoardPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (game.CurrentState == GameState.Initial)
            {
                return;
            }
            if (game.CurrentState == GameState.Finite)
            {
                board.DrawEndScreen(game.GameWinner);
                return;
            }


            // maybe modify mousepos to consider 
            game.UpdateState(MousePositionToCellIdx((e.Location.X, e.Location.Y)));
            if (game.GameWinner != FigureColor.None)
                board.DrawEndScreen(game.GameWinner);
            else
                board.Draw(game.DrawData);
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            if (game.CurrentState != GameState.Initial) return;

            // before initial state game might have no pawns set on the board, and in initial uptate we can set pawns in proper startup configuration.
            // initial state might not be left, if no args are given for example, so game might not be started from the begining
            game.UpdateState(); // not necessary, but it can do some setup for example run time (so u can delete it and it will be executed first time on click :D)
            board.Draw(game.DrawData);
        }

        private (int row, int col) MousePositionToCellIdx((int x, int y) mousePosition)
        {
            int row = 7 - mousePosition.y / Constants.CellSize;
            int col = mousePosition.x / Constants.CellSize;

            return (row, col);
        }

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
