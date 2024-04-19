using checkers_logic;

namespace game_logic.Heuristics;

public static class PiotrHeuristic
{
    // positive -> points to white
    private static double A_White_Sum, B_White_Sum, C_White_Sum, D_White_Sum;
    private static double A_Black_Sum, B_Black_Sum, C_Black_Sum, D_Black_Sum;
    private static CellState[,] boardState = new CellState[8,8];


    public static double BoardStateRating(CellState[,] boardState)
    {
        PiotrHeuristic.boardState = boardState;

        return H();
    }
    private static double H()
    {
        ComputeSums();

        return H_White() - H_Black();
    }
    private static double H_White() 
    {
        return Math.Pow(A_White_Sum, 1.1) 
            + Math.Pow(B_White_Sum, 1.0 / 1.12) 
            + C_White_Sum 
            + Math.Pow(D_White_Sum, 1.0 / 2.0);
    }
    private static double H_Black() 
    {
        return Math.Pow(A_Black_Sum, 1.1)
            + Math.Pow(B_Black_Sum, 1.0 / 1.12)
            + C_Black_Sum
            + Math.Pow(D_Black_Sum, 1.0 / 2.0);
    }
    private static void ComputeSums()
    {
        A_White_Sum = 0.0;
        B_White_Sum = 0.0;
        C_White_Sum = 0.0;
        D_White_Sum = 0.0;

        A_Black_Sum = 0.0;
        B_Black_Sum = 0.0;
        C_Black_Sum = 0.0;
        D_Black_Sum = 0.0;

        for (int row = 0; row < 8; ++row)
        {
            for (int col = 0; col < 8; ++col)
            {
                if (DictionariesLogic.IsCellStateWhite(boardState[row, col]))
                {
                    A_White_Sum += X_Value_Sum((row, col), boardState);
                    B_White_Sum += B_Value((row, col));
                    C_White_Sum += C_Value(getCellState((row, col)));
                    D_White_Sum += D_Value((row, col));
                }
                else if (DictionariesLogic.IsCellStateBlack(boardState[row, col]))
                {
                    A_Black_Sum += X_Value_Sum((row, col), boardState);
                    B_Black_Sum += B_Value((row, col));
                    C_Black_Sum += C_Value(getCellState((row, col)));
                    D_Black_Sum += D_Value((row, col));
                }
            }
        }
    }
    private static double D_Value((int row, int col) cellIdx)
    {
        bool isKing = DictionariesLogic.IsCellKing(getCellState(cellIdx));
        bool isWhite = DictionariesLogic.IsCellStateWhite(getCellState(cellIdx));
        bool isBlack = DictionariesLogic.IsCellStateBlack(getCellState(cellIdx));

        switch (cellIdx.col)
        {
            case 0:
            case 7: return isKing ? 4 : 3;
            case 1:
            case 6: return isKing ? 2 : 1;
            default: break;
        }
        
        switch (cellIdx.row)
        {
            case 0:
            case 7: return isKing ? 4 : 3;
            default: break;
        }

        if (isWhite && cellIdx.row == 1) return isKing ? 2 : 1;
        if (isBlack && cellIdx.row == 6) return isKing ? 2 : 1;

        return 0;
    }
    private static double B_Value((int row, int col) cellIdx)
    {
        var state = getCellState(cellIdx);

        if (DictionariesLogic.IsCellKing(state)) return 0;

        if (DictionariesLogic.CellStateToColor(state) == FigureColor.White)
        {
            switch(cellIdx.row)
            {
                case 1: return 4;
                case 2: return 2;
                case 3: return 1;
                default: return 0;
            }
        }
        if (DictionariesLogic.CellStateToColor(state) == FigureColor.Black)
        {
            switch (cellIdx.row)
            {
                case 6: return 4;
                case 5: return 2;
                case 4: return 1;
                default: return 0;
            }
        }
        return 0;
    }
    private static double C_Value(CellState state)
    {
        if (DictionariesLogic.IsCellKing(state)) return 13;
        if (state == CellState.Empty) return 0;
        return 5;
    }
    private static double X_Value_Sum((int row, int col) cellIdx, CellState[,] states)
    {
        FigureColor figureColor = DictionariesLogic.CellStateToColor(getCellState(cellIdx));
        double x = 0;

        int forwardDirection = figureColor == FigureColor.White ? -1 : 1;
        int leftDirection = figureColor == FigureColor.White ? -1 : 1;

        (int row, int col, CellState state) left1_up1_cell = getShiftedCell(cellIdx, (forwardDirection, leftDirection), 1);
        (int row, int col, CellState state) right1_up1_cell = getShiftedCell(cellIdx, (forwardDirection, -leftDirection), 1);
        (int row, int col, CellState state) left2_up2_cell = getShiftedCell(cellIdx, (forwardDirection, leftDirection), 2);
        (int row, int col, CellState state) right2_up2_cell = getShiftedCell(cellIdx, (forwardDirection, -leftDirection), 2);
        if (left2_up2_cell.state == CellState.Empty && isCellEnemyFigure(DictionariesLogic.CellStateToColor(getCellState(cellIdx)),  (left1_up1_cell.row, left1_up1_cell.col)))
        {
            x += CapturedFigureValue(left1_up1_cell.state);
            CellState[,] innerState = (CellState[,])states.Clone();
            innerState[left2_up2_cell.row, left2_up2_cell.col] = getCellState(cellIdx);
            innerState[left1_up1_cell.row, left1_up1_cell.col] = CellState.Empty;
            innerState[cellIdx.row, cellIdx.col] = CellState.Empty;
            x += X_Value_Sum((left2_up2_cell.row, left2_up2_cell.col), innerState);
        }
        if (right2_up2_cell.state == CellState.Empty && isCellEnemyFigure(DictionariesLogic.CellStateToColor(getCellState(cellIdx)),  (right1_up1_cell.row, right1_up1_cell.col)))
        {
            x += CapturedFigureValue(right1_up1_cell.state);
            CellState[,] innerState = (CellState[,])states.Clone();
            innerState[right2_up2_cell.row, right2_up2_cell.col] = getCellState(cellIdx);
            innerState[right1_up1_cell.row, right1_up1_cell.col] = CellState.Empty;
            innerState[cellIdx.row, cellIdx.col] = CellState.Empty;
            
            x += X_Value_Sum((right2_up2_cell.row, right2_up2_cell.col), innerState);
        }

        if (!DictionariesLogic.IsCellKing(getCellState(cellIdx))) return x;

        (int row, int col, CellState state) left1_down1_cell = getShiftedCell(cellIdx, (-forwardDirection, leftDirection), 1);
        (int row, int col, CellState state) right1_down1_cell = getShiftedCell(cellIdx, (-forwardDirection, -leftDirection), 1);
        (int row, int col, CellState state) left2_down2_cell = getShiftedCell(cellIdx, (-forwardDirection, leftDirection), 2);
        (int row, int col, CellState state) right2_down2_cell = getShiftedCell(cellIdx, (-forwardDirection, -leftDirection), 2);
        if (left2_down2_cell.state == CellState.Empty && isCellEnemyFigure(DictionariesLogic.CellStateToColor(getCellState(cellIdx)),  (left1_down1_cell.row, left1_down1_cell.col)))
        {
            x += CapturedFigureValue(left1_down1_cell.state);
            CellState[,] innerState = (CellState[,])states.Clone();
            innerState[left2_down2_cell.row, left2_down2_cell.col] = getCellState(cellIdx);
            innerState[left1_down1_cell.row, left1_down1_cell.col] = CellState.Empty;
            innerState[cellIdx.row, cellIdx.col] = CellState.Empty;
            x += X_Value_Sum((left2_down2_cell.row, left2_down2_cell.col), innerState);
        }
        if (right2_down2_cell.state == CellState.Empty && isCellEnemyFigure(DictionariesLogic.CellStateToColor(getCellState(cellIdx)), (right1_down1_cell.row, right1_down1_cell.col)))
        {
            x += CapturedFigureValue(right1_up1_cell.state);
            CellState[,] innerState = (CellState[,])states.Clone();
            innerState[right2_down2_cell.row, right2_down2_cell.col] = getCellState(cellIdx);
            innerState[right1_down1_cell.row, right1_down1_cell.col] = CellState.Empty;
            innerState[cellIdx.row, cellIdx.col] = CellState.Empty;

            x += X_Value_Sum((right2_down2_cell.row, right2_down2_cell.col), innerState);
        }

        return x;
    }
    private static double CapturedFigureValue(CellState figure)
    {
        if (DictionariesLogic.IsCellKing(figure)) 
            return 15.0;
        if (figure == CellState.Empty) 
            return 0.0;
        return 7.0;
    }

    #region Helpers
    private static CellState getCellState((int row, int col) cellIdx)
    {
        return boardState[cellIdx.row, cellIdx.col];
    }
    private static (int row, int col, CellState state) getShiftedCell(
        (int row, int col) cellIdx,
        (int row, int col) shift,
        int distance = 1)
    {
        (int row, int col) newCellIdx = (cellIdx.row + distance * shift.row, cellIdx.col + distance * shift.col);

        return (newCellIdx.row, newCellIdx.col, getCellState(newCellIdx));
    }
    private static bool isCellEnemyFigure(FigureColor attackerColor, (int row, int col) cellIdx)
    {
        if (attackerColor == FigureColor.White)
        {
            return DictionariesLogic.CellStateToColor(getCellState(cellIdx)) == FigureColor.Black;
        }
        else
        {
            return DictionariesLogic.CellStateToColor(getCellState(cellIdx)) == FigureColor.White;
        }
        throw new ApplicationException("Turn not specified!");
    }
    #endregion
}
