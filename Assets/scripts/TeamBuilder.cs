using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuilder : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    [SerializeField] private Unit unitPrefab;
    private Unit _unitInstance;
    
    [SerializeField] private Team teamPrefab;
    private Team _teamInstance;
    
    private CustomGrid _customGrid;
    private MovementManager _movementManager;
    private CapacityManager _capacityManager;

    [SerializeField] private List<UnitData> unitDataList = new List<UnitData>();
    public PlayerData playerData;
    public List<CapacityDefinition> capacityDefinitions;

    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void SetCustomGrid(CustomGrid customGrid)
    {
        _customGrid = customGrid;
    }
    
    public void SetMovementManager(MovementManager movementManager)
    {
        _movementManager = movementManager;
    }
    
    public void SetCapacityManager(CapacityManager capacityManager)
    {
        _capacityManager = capacityManager;
    }
    
    public Team BuildTeam(int teamIndex, List<int> positionsX, int row)
    {

        _teamInstance = Instantiate(teamPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _teamInstance.name = $"Team_{teamIndex + 1}";
        _teamInstance.teamIndex = teamIndex;
        
        for (int i = 0; i < playerData.teamUnits.Count; i++)
        {
            var spawnTile = _customGrid.GetTileAt(positionsX[i], row);
            var spawnPosition = spawnTile.transform.position;
            spawnPosition.y = 0;
            
            Quaternion rot = Quaternion.identity;

            if (teamIndex == 1)
                rot = Quaternion.Euler(0.0f, 180.0f, 00.0f);

            _unitInstance = Instantiate(unitPrefab, spawnPosition, rot, _teamInstance.transform);
            _unitInstance.name = $"team{teamIndex + 1}_unit_{i}";
            _unitInstance.tile = spawnTile;
            
            for (int j = 0; j < unitDataList.Count; j++)
            {
                if (unitDataList[j].unitName == playerData.teamUnits[i])
                {
                    _unitInstance.SetData(unitDataList[j]);
                    break;
                }
            }
            
            spawnTile.isWalkable = false;
            spawnTile.state = TileState.NotSelectable;
            _unitInstance.SetMovementManager(_movementManager);
            _unitInstance.SetCapacityManager(_capacityManager);
            _unitInstance.teamIndex = teamIndex;
            _teamInstance.AddUnit(_unitInstance);
        }
        return _teamInstance;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
}