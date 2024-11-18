using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Waiting,
    Lost,
    Win
}

public class BattleManager : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------

    [SerializeField] private GridBuilder gridBuilderPrefab;
    private GridBuilder _gridBuilderInstance;

    [SerializeField] private MovementManager movementManagerPrefab;
    private MovementManager _movementManager;

    [SerializeField] private CapacityManager capacityManagerPrefab;
    private CapacityManager _capacityManager;

    [SerializeField] private TeamBuilder teamBuilderPrefab;
    private TeamBuilder _teamBuilderInstance;

    [SerializeField] private Portal portalPrefab;
    private Portal _portalInstance;

    [SerializeField] private Chest chestPrefab;
    private Chest _chestInstance;

    private CustomGrid _customGrid;

    private List<Team> _teams = new List<Team>();
    private int _currentTeamIndex;

    public GameState state;

    private int _keyRPressState = 0;
    private int _keyYPressState = 0;
    
    private CurrentTeamDisplay _currentTeamDisplay;
    
    private Timer _timer;

    // -----------------------------------------------------------------------------------------------------------------
    // S I G N A L S
    // -----------------------------------------------------------------------------------------------------------------
    
    // Signal handling
    private void OnEnable()
    {
        Timer.OnTimerEnd += EndGame;
    }

    private void OnDisable()
    {
        Timer.OnTimerEnd -= EndGame;
    }
    
    private void EndGame(Timer timer)
    {
        state = GameState.Lost;
    }
    // End of Signal handling

    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    void Start()
    {
        _currentTeamIndex = 0;
        _timer = FindFirstObjectByType<Timer>();
        
        state = GameState.Playing;

        InitializeGrid();
        InitializeMovementManager();
        InitializeCapacityManager();
        InitializeTeams();
        InitializeTeamDisplay();
        InitializePortal();
        InitializeChest();
        InitializeFirstRound();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartNextUnitTurn();

        ProcessSelectedUnitActions();
        CleanUpDestroyedUnits();
        CheckWinner();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // I N I T I A L I Z A T I O N   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    private void InitializeGrid()
    {
        _gridBuilderInstance =
            Instantiate(gridBuilderPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        _customGrid = _gridBuilderInstance.CreateGrid();
        _customGrid.InitMapTiles(WalkableMap.Map);
    }

    private void InitializeMovementManager()
    {
        _movementManager =
            Instantiate(movementManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        _movementManager.SetGrid(_customGrid);
    }

    private void InitializeCapacityManager()
    {
        _capacityManager =
            Instantiate(capacityManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        _capacityManager.SetGrid(_customGrid);
    }

    private void InitializeTeams()
    {
        var posX = new List<int> { 6, 7, 8, 9 };

        _teamBuilderInstance =
            Instantiate(teamBuilderPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        _teamBuilderInstance.SetCustomGrid(_customGrid);
        _teamBuilderInstance.SetMovementManager(_movementManager);
        _teamBuilderInstance.SetCapacityManager(_capacityManager);

        _teams.Add(_teamBuilderInstance.BuildTeam(0, posX, 0));
        _teams.Add(_teamBuilderInstance.BuildTeam(1, posX, 10));
    }

    private void InitializePortal()
    {
        _portalInstance = Instantiate(portalPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _portalInstance.tile = _customGrid.GetTileAt(8, 5);
        var rot = Quaternion.Euler(0.0f, 90.0f, 00.0f);
        _portalInstance.transform.rotation = rot;
        _portalInstance.transform.position = _portalInstance.tile.transform.position;
        var vector3 = _portalInstance.transform.position;
        vector3.y = 1;
        _portalInstance.transform.position = vector3;
        _portalInstance.tile.isWalkable = true;
        _portalInstance.tile.state = TileState.Selectable;
    }

    private void InitializeChest()
    {
        _chestInstance = Instantiate(chestPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _chestInstance.tile = _customGrid.GetTileAt(0, 5);
        _chestInstance.transform.position = _chestInstance.tile.transform.position;
        _portalInstance.tile.isWalkable = true;
        _portalInstance.tile.state = TileState.Selectable;
    }

    private void InitializeFirstRound()
    {
        // ne pas refactoriser avec la methode InitializeNewRound()
        var currentTeam = _teams[_currentTeamIndex];
        var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];
        _currentTeamDisplay.UpdateTeamNameDisplay(currentTeam.name);

        currentTeam.MarkAllUnitsSelectable();
        opponentTeam.MarkAllUnitsNotSelectable();
    }
    
    private void InitializeNewRound()
    {
        // ne pas refactoriser avec la methode InitializeFirstRound()
        var currentTeam = _teams[_currentTeamIndex];
        var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];
        _currentTeamDisplay.UpdateTeamNameDisplay(currentTeam.name);

        currentTeam.MaxActionPointsForAllUnits();
        currentTeam.MarkAllUnitsSelectable();

        opponentTeam.MaxActionPointsForAllUnits();
        opponentTeam.MarkAllUnitsNotSelectable();
    }
    
    private void InitializeTeamDisplay()
    {
        _currentTeamDisplay = FindFirstObjectByType<CurrentTeamDisplay>();
    }
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // B A T T L E   M A N A G E M E N T   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    private void StartNextUnitTurn()
    {
        foreach (var team in _teams)
        {
            foreach (var unit in team.units)
            {
                if (unit != null)
                {
                    if (unit.currentActionPoints > 0)
                    {
                        HandleEndUnitTurn();
                        HandleNextUnitTurn();
                        return;
                    }
                }
            }
        }
        InitializeNewRound();
        _customGrid.UpdateDisplay();
    }

    private void HandleEndUnitTurn()
    {
        var currentTeam = _teams[_currentTeamIndex];

        foreach (var unit in currentTeam.units)
        {
            if (unit.state == UnitState.Selected)
                unit.SetActionPoints(0);
        }
        _customGrid.UpdateDisplay();
    }

    private void HandleNextUnitTurn()
    {
        _currentTeamIndex = (_currentTeamIndex + 1) % _teams.Count;
        var currentTeam = _teams[_currentTeamIndex];
        var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];

        currentTeam.MarkAllUnitsSelectable();
        _currentTeamDisplay.UpdateTeamNameDisplay(currentTeam.name);
        opponentTeam.MarkAllUnitsNotSelectable();
        _customGrid.UpdateDisplay();
    }
    
    private void ProcessSelectedUnitActions()
    {
        var currentTeam = _teams[_currentTeamIndex];
        var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];

        Unit selectedUnit = null;
        foreach (var unit in currentTeam.units)
        {
            if (unit.state == UnitState.Selected)
            {
                selectedUnit = unit;
                break;
            }
        }

        if (selectedUnit == null) return;
        currentTeam.MarkOtherUnitsNotSelectable(selectedUnit);
        opponentTeam.MarkAllUnitsNotSelectable();

        HandleUnitCapacity(selectedUnit);
        HandleUnitMovement(selectedUnit);
        HandleChestPickup(selectedUnit);
    }

    private void HandleUnitCapacity(Unit selectedUnit)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];
            opponentTeam.MarkAllUnitsNotSelectable();
            var capacity = selectedUnit.capacities[1];

            switch (_keyRPressState)
            {
                case 0:
                    _customGrid.ResetAccessibleTiles();
                    _customGrid.MarkAccessibleTiles(selectedUnit.tile, capacity.distanceRange);
                    _customGrid.UpdateDisplay();
                    
                    _keyRPressState = 1;
                    _keyYPressState = 0;
                    break;
                case 1:
                    var accessibleTiles = _customGrid.GetAccessibleTiles(selectedUnit.tile, capacity.distanceRange);
                    Tile targetedTile = null;
                    
                    foreach (var tile in accessibleTiles)
                    {
                        if (tile.state == TileState.Selected)
                        {
                            targetedTile = tile;
                            break;
                        }
                    }
                    
                    if (targetedTile == null) return;
                    
                    var accessibleUnits =
                        _capacityManager.GetAccessibleUnits(targetedTile, capacity, opponentTeam);
                    
                    selectedUnit.UseCapacity(capacity, accessibleUnits);
                    _capacityManager.DisplayCapacityEffect(capacity, targetedTile);
                    
                    _customGrid.MarkAllTilesAsSelectable();
                    _customGrid.ResetAccessibleTiles();
                    _customGrid.UpdateDisplay();
                    _keyRPressState = 0;
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];
            opponentTeam.MarkAllUnitsNotSelectable();
            var capacity = selectedUnit.capacities[0];
            
            switch (_keyRPressState)
            {
                case 0:
                    _customGrid.ResetAccessibleTiles();
                    _customGrid.MarkNeighboursTiles(selectedUnit.tile);
                    _customGrid.UpdateDisplay();
                    var accessibleUnits = _capacityManager.GetAccessibleUnits(selectedUnit.tile, capacity, opponentTeam);
                    opponentTeam.MarkUnitsAsTargetable(accessibleUnits);
                    
                    _keyRPressState = 1;
                    _keyYPressState = 0;
                    break;
                case 1:
                    List<Unit> targetedUnit = new List<Unit>();
                    foreach (var unit in opponentTeam.units)
                    {
                        if (unit.targetableState == UnitTargetableState.Targeted)
                        {
                            targetedUnit.Add(unit);
                            break;
                        }
                    }
                    if (targetedUnit.Count == 0) return;
                    selectedUnit.UseCapacity(capacity, targetedUnit);
                    opponentTeam.MarkAllUnitsAsNotTargetable();
                    _customGrid.MarkAllTilesAsSelectable();
                    _customGrid.ResetAccessibleTiles();
                    _customGrid.UpdateDisplay();
                    
                    _keyRPressState = 0;
                    break;
            }
        }
    }

    private void HandleUnitMovement(Unit selectedUnit)
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            var currentTeam = _teams[_currentTeamIndex];
            currentTeam.MarkOtherUnitsNotSelectable(selectedUnit);

            switch (_keyYPressState)
            {
                case 0:
                    _customGrid.ResetAccessibleTiles();
                    _customGrid.MarkAccessibleTiles(selectedUnit.tile, selectedUnit.movementPoints);
                    _customGrid.UpdateDisplay();
                    _keyYPressState = 1;
                    _keyRPressState = 0;
                    break;
                case 1:
                    _keyYPressState = 0;
                    Tile targetTile = _customGrid.GetSelectedTile();
                    if (targetTile != null)
                    {
                        var accessibleTiles =
                            _customGrid.GetNeighborsWithinRadius(selectedUnit.tile, selectedUnit.movementPoints);
                        if (accessibleTiles.Contains(targetTile))
                        {
                            selectedUnit.Move(targetTile);
                            _customGrid.ResetAccessibleTiles();
                            _customGrid.UpdateDisplay();
                        }
                    }

                    if (targetTile == _portalInstance.tile)
                    {
                        if (_portalInstance.TeleportUnit(selectedUnit))
                            state = GameState.Win;
                    }
                    break;
            }
        }
    }
    
    private void HandleChestPickup(Unit selectedUnit)
    {
        if (state == GameState.Waiting) return;
    
        if (Input.GetKeyDown(KeyCode.U))
        {
            var currentTeam = _teams[_currentTeamIndex];
            currentTeam.MarkOtherUnitsNotSelectable(selectedUnit);
            
            if (selectedUnit.hasChest == false)
            {
                if (selectedUnit.tile == _chestInstance.tile)
                    selectedUnit.CollectChest(_chestInstance);
            }
            else
                selectedUnit.DropChest();
        }
    }

    private void CleanUpDestroyedUnits()
    {
        foreach (var team in _teams)
        {
            for (int i = team.units.Count - 1; i >= 0; i--)
            {
                if (team.units[i] == null)
                    team.units.RemoveAt(i);
            }
        }
    }
    
    private void CheckWinner()
    {
        var opponentTeam = _teams[(_currentTeamIndex + 1) % _teams.Count];
        
        if (opponentTeam.IsTeamDefeated())
            state = GameState.Win;
    }
}
