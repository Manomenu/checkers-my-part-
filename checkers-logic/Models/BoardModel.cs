namespace checkers_logic.Models;

public class BoardModel
{
    public FigureColor TurnColor { get; private set; } = FigureColor.None; // Make use of SetTurn instead of this inside state logics
    public CellState[,] BoardState { get; private set; }

    #region Check winner logic
    private int blackLeft = 12;
    private int whiteLeft = 12;
    public FigureColor GameWinner => blackLeft == 0 
        ? FigureColor.White 
        : (whiteLeft == 0 
            ? FigureColor.Black 
            : FigureColor.None);
    private void decreaseFigureCounter()
    {
        if (TurnColor == FigureColor.None) 
            throw new ApplicationException("You must be during a turn!");

        if (TurnColor == FigureColor.White)
        {
            --blackLeft;
            if (blackLeft < 0) throw new ApplicationException("Negative black figure counter!");
        }
        else
        {
            --whiteLeft;
            if (whiteLeft < 0) throw new ApplicationException("Negative white figure counter!");
        }
    }
    #endregion

    #region SelectedCell
    private (int row, int col, CellState state)? selectedCell = null;
    public (int row, int col, CellState state) SelectedCell
    {
        get
        {
            if (!IsCellSelected)
                throw new ApplicationException("Select a cell first!");
            return selectedCell!.Value;
        }
    }
    public void UnselectCell() => selectedCell = null;
    public bool IsCellSelected => selectedCell != null;
    public void SelectCell((int row, int col) cellIdx)
    {
        if (cellIdx.row < 0 || cellIdx.row >= 8 || cellIdx.col < 0 || cellIdx.col >= 8)
            selectedCell = (-1, -1, CellState.Invalid);
        selectedCell = (7 - cellIdx.row, cellIdx.col, BoardState[7 - cellIdx.row, cellIdx.col]);
    }
    #endregion

    #region Attacker
    private (int row, int col, CellState state)? attackerCell = null;
    public (int row, int col) AttackerCellPosition
    {
        get 
        {
            if (!IsAttackerSelected)
                throw new ApplicationException("Select an attacker first!");
            return (attackerCell!.Value.row, attackerCell!.Value.col);
        }
    }
    public (int row, int col, CellState state) AttackerCell
    {
        get
        {
            if (!IsAttackerSelected)
                throw new ApplicationException("Select an attacker first!");
            return attackerCell!.Value;
        }
    }
    public void SetSelectedCellAsAttacker() 
    {
        attackerCell = selectedCell;

        estabilishAllLegalTargets();
        estabilishAttackerLegalTargets();
    }
    public void UnselectAttacker()
    {
        clearAttackerLegalTargets();
        attackerCell = null;
    }
    public bool IsAttackerSelected => attackerCell != null;

    #endregion

    #region AttackerTargets

    #region AttackerAttackTargets helper
        private HashSet<(int row, int col)> attackerAttackTargets = new();
        public IReadOnlySet<(int row, int col)> AttackerAttackTargets => attackerAttackTargets;
        private bool AnyAttackerAttackTarget => attackerAttackTargets.Any();
        private void estabilishAttackerAttackTargets()
        {
            if (!IsAttackerSelected) throw new ApplicationException("Select an attacker first!");

            int forwardDirection = TurnColor == FigureColor.White ? -1 : 1;
            int leftDirection = TurnColor == FigureColor.White ? -1 : 1;
            clearAttackerAttackTargets();

            (int row, int col, CellState state) left1_up1_cell = getShiftedCell(AttackerCellPosition, (forwardDirection, leftDirection), 1);
            (int row, int col, CellState state) right1_up1_cell = getShiftedCell(AttackerCellPosition, (forwardDirection, -leftDirection), 1);
            (int row, int col, CellState state) left2_up2_cell = getShiftedCell(AttackerCellPosition, (forwardDirection, leftDirection), 2);
            (int row, int col, CellState state) right2_up2_cell = getShiftedCell(AttackerCellPosition, (forwardDirection, -leftDirection), 2);
            if (left2_up2_cell.state == CellState.Empty && isCellEnemyFigure((left1_up1_cell.row, left1_up1_cell.col))) 
                attackerAttackTargets.Add((left2_up2_cell.row, left2_up2_cell.col));
            if (right2_up2_cell.state == CellState.Empty && isCellEnemyFigure((right1_up1_cell.row, right1_up1_cell.col))) 
                attackerAttackTargets.Add((right2_up2_cell.row, right2_up2_cell.col));

            if (!DictionariesLogic.IsCellKing(AttackerCell.state)) return;

            (int row, int col, CellState state) left1_down1_cell = getShiftedCell(AttackerCellPosition, (-forwardDirection, leftDirection), 1);
            (int row, int col, CellState state) right1_down1_cell = getShiftedCell(AttackerCellPosition, (-forwardDirection, -leftDirection), 1);
            (int row, int col, CellState state) left2_down2_cell = getShiftedCell(AttackerCellPosition, (-forwardDirection, leftDirection), 2);
            (int row, int col, CellState state) right2_down2_cell = getShiftedCell(AttackerCellPosition, (-forwardDirection, -leftDirection), 2);
            if (left2_down2_cell.state == CellState.Empty && isCellEnemyFigure((left1_down1_cell.row, left1_down1_cell.col))) 
                attackerAttackTargets.Add((left2_down2_cell.row, left2_down2_cell.col));
            if (right2_down2_cell.state == CellState.Empty && isCellEnemyFigure((right1_down1_cell.row, right1_down1_cell.col))) 
                attackerAttackTargets.Add((right2_down2_cell.row, right2_down2_cell.col));
        }
        private void clearAttackerAttackTargets() => attackerAttackTargets.Clear();
        #endregion

        #region AttackerMoveTargets helper
        private HashSet<(int row, int col)> attackerMoveTargets = new();
        public IReadOnlySet<(int row, int col)> AttackerMoveTargets => attackerMoveTargets;
        private bool AnyAttackerMoveTarget => attackerMoveTargets.Any();
        private void estabilishAttackerMoveTargets()
        {
            if (!IsAttackerSelected) throw new ApplicationException("Select an attacker first!");

            int forwardDirection = TurnColor == FigureColor.White ? -1 : 1;
            int leftDirection = TurnColor == FigureColor.White ? -1 : 1;
            clearAttackerMoveTargets();

            (int row, int col, CellState state) left_up_cell = getShiftedCell(AttackerCellPosition, (forwardDirection, leftDirection));
            (int row, int col, CellState state) right_up_cell = getShiftedCell(AttackerCellPosition, (forwardDirection, -leftDirection));
            if (left_up_cell.state == CellState.Empty) attackerMoveTargets.Add((left_up_cell.row, left_up_cell.col));
            if (right_up_cell.state == CellState.Empty) attackerMoveTargets.Add((right_up_cell.row, right_up_cell.col));

            if (!DictionariesLogic.IsCellKing(AttackerCell.state)) return;

            (int row, int col, CellState state) left_down_cell = getShiftedCell(AttackerCellPosition, (-forwardDirection, leftDirection));
            (int row, int col, CellState state) right_down_cell = getShiftedCell(AttackerCellPosition, (-forwardDirection, -leftDirection));
            if (left_down_cell.state == CellState.Empty) attackerMoveTargets.Add((left_down_cell.row, left_down_cell.col));
            if (right_down_cell.state == CellState.Empty) attackerMoveTargets.Add((right_down_cell.row, right_down_cell.col));
        }
        private void clearAttackerMoveTargets() => attackerMoveTargets.Clear();
        #endregion 

        #region AttackerLegalTargets
        private HashSet<(int row, int col)> attackerLegalTargets = new();
        public bool AnyAttackerLegalTarget => attackerLegalTargets.Any();
        public bool AnyAttackerLegalAttackTarget => attackerLegalTargets.Any(t => attackerAttackTargets.Contains(t));
        private void clearAttackerLegalTargets()
        {
            clearAttackerAttackTargets();
            clearAttackerMoveTargets();
            attackerLegalTargets.Clear();
        }
        private void estabilishAttackerLegalTargets()
        {
            clearAttackerLegalTargets();

            estabilishAttackerAttackTargets();
            if (AnyAttackerAttackTarget)
            {
                foreach (var at in attackerAttackTargets) 
                    attackerLegalTargets.Add(at);
                return;
            }

            estabilishAttackerMoveTargets();
            foreach (var mt in attackerMoveTargets)
                attackerLegalTargets.Add(mt);
        }
        #endregion

    #endregion

    #region AllTargets

        #region AllAttackTargets helper
        private HashSet<(int row, int col)> allAttackTargets = new();
            private void clearAllAttackTargets() => allAttackTargets.Clear();
            private bool AnyAttackTarget => allAttackTargets.Any();
            private void estabilishAllAttackTargets()
            {
                int forwardDirection = TurnColor == FigureColor.White ? -1 : 1;
                int leftDirection = TurnColor == FigureColor.White ? -1 : 1;
                clearAllAttackTargets();

                for (int row = 0; row < 8; ++row)
                {
                    for (int col = 0; col < 8; ++col)
                    {
                        (int row, int col) nextCellPosition = (row, col);
                        if (isCellEnemyFigure(nextCellPosition) || getCellState(nextCellPosition) == CellState.Empty) continue;

                        (int row, int col, CellState state) left1_up1_cell = getShiftedCell(nextCellPosition, (forwardDirection, leftDirection), 1);
                        (int row, int col, CellState state) right1_up1_cell = getShiftedCell(nextCellPosition, (forwardDirection, -leftDirection), 1);
                        (int row, int col, CellState state) left2_up2_cell = getShiftedCell(nextCellPosition, (forwardDirection, leftDirection), 2);
                        (int row, int col, CellState state) right2_up2_cell = getShiftedCell(nextCellPosition, (forwardDirection, -leftDirection), 2);
                        if (left2_up2_cell.state == CellState.Empty && isCellEnemyFigure((left1_up1_cell.row, left1_up1_cell.col)))
                            allAttackTargets.Add((left2_up2_cell.row, left2_up2_cell.col));
                        if (right2_up2_cell.state == CellState.Empty && isCellEnemyFigure((right1_up1_cell.row, right1_up1_cell.col)))
                            allAttackTargets.Add((right2_up2_cell.row, right2_up2_cell.col));

                        if (!DictionariesLogic.IsCellKing(getCellState(nextCellPosition))) continue;

                        (int row, int col, CellState state) left1_down1_cell = getShiftedCell(nextCellPosition, (-forwardDirection, leftDirection), 1);
                        (int row, int col, CellState state) right1_down1_cell = getShiftedCell(nextCellPosition, (-forwardDirection, -leftDirection), 1);
                        (int row, int col, CellState state) left2_down2_cell = getShiftedCell(nextCellPosition, (-forwardDirection, leftDirection), 2);
                        (int row, int col, CellState state) right2_down2_cell = getShiftedCell(nextCellPosition, (-forwardDirection, -leftDirection), 2);
                        if (left2_down2_cell.state == CellState.Empty && isCellEnemyFigure((left1_down1_cell.row, left1_down1_cell.col)))
                            allAttackTargets.Add((left2_down2_cell.row, left2_down2_cell.col));
                        if (right2_down2_cell.state == CellState.Empty && isCellEnemyFigure((right1_down1_cell.row, right1_down1_cell.col)))
                            allAttackTargets.Add((right2_down2_cell.row, right2_down2_cell.col));
                    }
                }
            }
            #endregion

        #region AllMoveTargets helper
        private HashSet<(int row, int col)> allMoveTargets = new();
        private void clearAllMoveTargets() => allMoveTargets.Clear();
        private bool AnyMoveTarget => allMoveTargets.Any();
        private void estabilishAllMoveTargets()
        {
            int forwardDirection = TurnColor == FigureColor.White ? -1 : 1;
            int leftDirection = TurnColor == FigureColor.White ? -1 : 1;
            clearAllMoveTargets();

            for (int row = 0; row < 8; ++row)
            {
                for (int col = 0; col < 8; ++col)
                {
                    (int row, int col) nextCellPosition = (row, col);
                    if (isCellEnemyFigure(nextCellPosition) || getCellState(nextCellPosition) == CellState.Empty) continue;

                    (int row, int col, CellState state) left_up_cell = getShiftedCell(nextCellPosition, (forwardDirection, leftDirection));
                    (int row, int col, CellState state) right_up_cell = getShiftedCell(nextCellPosition, (forwardDirection, -leftDirection));
                    if (left_up_cell.state == CellState.Empty) allMoveTargets.Add((left_up_cell.row, left_up_cell.col));
                    if (right_up_cell.state == CellState.Empty) allMoveTargets.Add((right_up_cell.row, right_up_cell.col));

                    if (!DictionariesLogic.IsCellKing(getCellState(nextCellPosition))) continue;

                    (int row, int col, CellState state) left_down_cell = getShiftedCell(nextCellPosition, (-forwardDirection, leftDirection));
                    (int row, int col, CellState state) right_down_cell = getShiftedCell(nextCellPosition, (-forwardDirection, -leftDirection));
                    if (left_down_cell.state == CellState.Empty) allMoveTargets.Add((left_down_cell.row, left_down_cell.col));
                    if (right_down_cell.state == CellState.Empty) allMoveTargets.Add((right_down_cell.row, right_down_cell.col));
                }
            }
        
        }
        #endregion

        #region AllLegalTargets
        private HashSet<(int row, int col)> allLegalTargets = new();
        public bool AnyLegalTarget => allLegalTargets.Any();
        public bool AnyLegalAttackTarget => allLegalTargets.Any(t => allAttackTargets.Contains(t));
        private void clearAllLegalTargets()
        {
            clearAllAttackTargets();
            clearAllMoveTargets();
            allLegalTargets.Clear();
        }
        private void estabilishAllLegalTargets()
        {
            clearAllLegalTargets();

            estabilishAllAttackTargets();
            if (AnyAttackTarget)
            {
                foreach (var at in allAttackTargets)
                    allLegalTargets.Add(at);
                return;
            }

            estabilishAllMoveTargets();
            foreach (var mt in allMoveTargets)
                allLegalTargets.Add(mt);
        }
        #endregion

    #endregion

    #region Target
    private (int row, int col, CellState state)? targetCell = null;
    public (int row, int col, CellState state) TargetCell
    {
        get
        {
            if (!IsTargetSelected)
                throw new ApplicationException("Select a target first!");
            return targetCell!.Value;
        }
    }
    public void SetSelectedCellAsTarget() => targetCell = selectedCell;
    public void UnselectTarget() => targetCell = null;
    public bool IsTargetSelected => targetCell != null;
    public bool IsTargetLegalAttack
    {
        get
        {
            return attackerAttackTargets.Contains((TargetCell.row, TargetCell.col)); 
        }
    }
    public bool IsSelectedTargetLegal
    {
        get
        {
            if (!IsTargetSelected)
                throw new ApplicationException("Select a target first!");
            return attackerLegalTargets.Contains((TargetCell.row, TargetCell.col));
        }
    }
    public bool IsTargetEnemyFigure
    {
        get
        {
            return attackerAttackTargets.Contains((TargetCell.row, TargetCell.col));
        }
        //get
        //{
        //    if (TurnColor == FigureColor.White) return DictionariesLogic.IsCellStateBlack(TargetCell.state);
        //    if (TurnColor == FigureColor.Black) return DictionariesLogic.IsCellStateWhite(TargetCell.state);
        //    throw new ApplicationException("No turn selected!");
        //}
    }
    #endregion

    #region Cell information helper
    private CellState getCellState((int row, int col) cellIdx)
    {
        if (cellIdx.row < 0 || cellIdx.row >= 8 || cellIdx.col < 0 || cellIdx.col >= 8)
            return CellState.Invalid;
        return BoardState[cellIdx.row, cellIdx.col];
    }
    private (int row, int col, CellState state) getShiftedCell(
        (int row, int col) cellIdx, 
        (int row, int col) shift, 
        int distance = 1)
    {
        (int row, int col) newCellIdx = (cellIdx.row + distance * shift.row, cellIdx.col + distance * shift.col);
        
        return (newCellIdx.row, newCellIdx.col, getCellState(newCellIdx));
    }
    private bool isCellEnemyFigure((int row, int col) cellIdx)
    {
        if (TurnColor == FigureColor.White)
        {
            return DictionariesLogic.CellStateToColor(getCellState(cellIdx)) == FigureColor.Black;
        }
        else
        {
            return DictionariesLogic.CellStateToColor(getCellState(cellIdx)) == FigureColor.White;
        }
        throw new ApplicationException("Turn not specified!");
    }
    private bool shouldAttackerBecomeKing
    {
        get
        {
            if (DictionariesLogic.IsCellStateWhite(AttackerCell.state) && AttackerCell.row == 0) return true;
            if (DictionariesLogic.IsCellStateBlack(AttackerCell.state) && AttackerCell.row == 7) return true;
            return false;
        }
    }
    #endregion

    public void SetTurn(FigureColor turnColor)
    {
        TurnColor = turnColor;
        if (turnColor != FigureColor.None)
            estabilishAllLegalTargets();
    }

    public void PerformAttackerToTargetAction()
    {
        CellState originalAttackerState = AttackerCell.state;

        if (!IsAttackerSelected || !IsTargetSelected || !IsSelectedTargetLegal) throw new ApplicationException("Cannot perform an attack!");

        BoardState[TargetCell.row, TargetCell.col] = BoardState[AttackerCell.row, AttackerCell.col];
        BoardState[AttackerCell.row, AttackerCell.col] = CellState.Empty;
        if (IsTargetLegalAttack)
        {
            decreaseFigureCounter();
            BoardState[(AttackerCell.row + TargetCell.row) / 2, (AttackerCell.col + TargetCell.col) / 2] = CellState.Empty;
        }
        attackerCell = (TargetCell.row, TargetCell.col, originalAttackerState);
        if (shouldAttackerBecomeKing)
        {
            attackerCell = (AttackerCell.row, AttackerCell.col, DictionariesLogic.IsCellStateWhite(originalAttackerState) ? CellState.WhiteKing : CellState.BlackKing);
            BoardState[AttackerCell.row, AttackerCell.col] = AttackerCell.state;
        }

        estabilishAttackerLegalTargets();
        estabilishAllLegalTargets();
        UnselectTarget();
    }

    public (CellState[,] boardState, IReadOnlySet<(int row, int col)> attackCells, IReadOnlySet<(int row, int col)> moveCells) DrawData =>
        (BoardState, AttackerAttackTargets, AttackerMoveTargets);

    public BoardModel()
    {
        CellState e = CellState.Empty;
        CellState w = CellState.WhitePawn;
        CellState b = CellState.BlackPawn;

        BoardState = new CellState[8, 8]
        {
            { e, b, e, b, e, b, e, b },
            { b, e, b, e, b, e, b, e },
            { e, b, e, b, e, b, e, b },
            { e, e, e, e, e, e, e, e },
            { e, e, e, e, e, e, e, e },
            { w, e, w, e, w, e, w, e },
            { e, w, e, w, e, w, e, w },
            { w, e, w, e, w, e, w, e }
        };
    }
}
