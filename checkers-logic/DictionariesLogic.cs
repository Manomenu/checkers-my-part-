namespace checkers_logic;

public static class DictionariesLogic
{
    public static bool IsCellStateWhite(CellState state) => state == CellState.WhitePawn || state == CellState.WhiteKing;
    public static bool IsCellStateBlack(CellState state) => state == CellState.BlackPawn || state == CellState.BlackKing;
    public static bool IsCellKing(CellState state) => state == CellState.WhiteKing || state == CellState.BlackKing;

    public static FigureColor CellStateToColor(CellState state)
    {
        if (IsCellStateBlack(state)) 
            return FigureColor.Black;
        if (IsCellStateWhite(state)) 
            return FigureColor.White;
        return FigureColor.None;
    }
}


