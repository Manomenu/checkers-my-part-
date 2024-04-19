namespace checkers_logic;

public enum CellState
{
    Invalid,
    Empty,
    BlackPawn,
    BlackKing,
    WhitePawn,
    WhiteKing
}

public enum FigureColor
{
    None,
    Black,
    White
}

public enum GameState
{
    Initial, // setup proper start checkers configuration and start/reset time count for game time, for real players
    WhiteTurn,
    BlackTurn,
    Finite, // end screen
    Finished // exit program 
}

public enum TurnState
{
    PickUpAttacker,
    SelectTarget,
    AttackSequence,
    Finished
}

