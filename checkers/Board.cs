using checkers_logic;
using System.Drawing.Drawing2D;

namespace checkers
{
    internal class Board
    {
        private Bitmap bitmap;
        private PictureBox pictureBox;

        public Board(PictureBox BoardPictureBox)
        {
            pictureBox = BoardPictureBox;
            bitmap = new Bitmap(pictureBox.Width, this.pictureBox.Height);
            pictureBox.Image = bitmap;
        }

        public void Draw((CellState[,] boardState, IReadOnlySet<(int row, int col)> attackCells, IReadOnlySet<(int row, int col)> moveCells) drawData)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        // Draw square of the board
                        g.FillRectangle(
                            (row + col) % 2 == 0 ? Constants.BrightSquaresBrush : Constants.DarkSquaresBrush,
                            col * Constants.CellSize,
                            row * Constants.CellSize,
                            Constants.CellSize,
                            Constants.CellSize);

                        // Draw piece if exists
                        if (drawData.boardState[row, col] != CellState.Empty)
                        {
                            g.FillEllipse(
                                drawData.boardState[row, col] == CellState.WhitePawn || drawData.boardState[row, col] == CellState.WhiteKing ?
                                    Constants.WhitesPiecesBrush : Constants.BlacksPiecesBrush,
                                col * Constants.CellSize + (Constants.CellSize - Constants.PieceSize) / 2,
                                row * Constants.CellSize + (Constants.CellSize - Constants.PieceSize) / 2,
                                Constants.PieceSize,
                                Constants.PieceSize);

                            // Add marker if it is the king
                            if (drawData.boardState[row, col] == CellState.WhiteKing || drawData.boardState[row, col] == CellState.BlackKing)
                            {
                                g.DrawEllipse(
                                    new Pen(Color.Gray, Constants.KingSymbolSize / 4),
                                    col * Constants.CellSize + (Constants.CellSize - Constants.KingSymbolSize) / 2,
                                    row * Constants.CellSize + (Constants.CellSize - Constants.KingSymbolSize) / 2,
                                    Constants.KingSymbolSize,
                                    Constants.KingSymbolSize);
                            }
                        }
                    }
                }

                foreach (var attackCell in drawData.attackCells)
                    g.FillEllipse(
                                Constants.AttackSquareBrush,
                                attackCell.col * Constants.CellSize + (Constants.CellSize - Constants.ActionSymbolSize) / 2,
                                attackCell.row * Constants.CellSize + (Constants.CellSize - Constants.ActionSymbolSize) / 2,
                                Constants.ActionSymbolSize,
                                Constants.ActionSymbolSize);

                foreach (var moveCell in drawData.moveCells)
                    g.FillEllipse(
                                Constants.MoveSquareBrush,
                                moveCell.col * Constants.CellSize + (Constants.CellSize - Constants.ActionSymbolSize) / 2,
                                moveCell.row * Constants.CellSize + (Constants.CellSize - Constants.ActionSymbolSize) / 2,
                                Constants.ActionSymbolSize,
                                Constants.ActionSymbolSize);

            }
            pictureBox.Refresh();
        }

        public void DrawEndScreen(FigureColor gameWinner)
        {
            var rectf = new RectangleF(0, 0, Constants.CellSize * 8, Constants.CellSize * 8);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(
                    gameWinner == FigureColor.White 
                        ? Constants.WhitesPiecesBrush 
                        : Constants.BlacksPiecesBrush, 
                    rectf);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString($"{gameWinner}\nwins!", new Font("Tahoma", 32, FontStyle.Bold), 
                    gameWinner == FigureColor.Black
                        ? Constants.WhitesPiecesBrush
                        : Constants.BlacksPiecesBrush, 
                    rectf, sf);
            }
            pictureBox.Refresh();
        }
    }
}
