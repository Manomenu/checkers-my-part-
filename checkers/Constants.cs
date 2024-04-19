namespace checkers
{
    internal static class Constants
    {
        public static int CellSize = 0;
        public static int PieceSize => (int)(0.8 * CellSize);
        public static int KingSymbolSize => (int)(0.5 * CellSize);
        public static int ActionSymbolSize => (int)(0.4 * CellSize);

        public static readonly Brush BrightSquaresBrush = new SolidBrush(Color.PapayaWhip);
        public static readonly Brush DarkSquaresBrush = new SolidBrush(Color.Peru);
        public static readonly Brush WhitesPiecesBrush = new SolidBrush(Color.White);
        public static readonly Brush BlacksPiecesBrush = new SolidBrush(Color.Black);
        public static readonly Brush MoveSquareBrush = new SolidBrush(Color.Yellow);
        public static readonly Brush AttackSquareBrush = new SolidBrush(Color.Orange);
    }
}
