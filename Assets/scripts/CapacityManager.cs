using System.Collections.Generic;
using UnityEngine;

public class CapacityManager : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    private CustomGrid _customGrid;
    [SerializeField] private GameObject capacityAnimationPrefab;
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void SetGrid(CustomGrid gridInstance)
    {
        _customGrid = gridInstance;
    }
    
    public List<Unit> GetAccessibleUnits(Tile originTile, CapacityDefinition capacity, Team Team)
    {
        var accessibleUnits = new List<Unit>();
        var aoeTiles = _customGrid.GetNeighborsWithinRadius(originTile, capacity.aoeRange);
        
        foreach (var unit in Team.units)
        {
            bool isInAttackRange = aoeTiles.Contains(unit.tile);
            if (isInAttackRange && unit.targetableState == UnitTargetableState.NotTargetable)
                accessibleUnits.Add(unit);
        }
        return accessibleUnits;
    }
    
    public void ApplyCapacity(CapacityDefinition capacity, List<Unit> targetUnits)
    {
        foreach (var unit in targetUnits)
        {
            if (capacity.type == CapacityType.Attack)
                unit.TakeDamage(capacity.value);
        }
    }
    public void DisplayCapacityEffect(CapacityDefinition capacity, Tile targetedTile)
    {
        if (capacity.isAoe)
        {
            var effectInstance = Instantiate(capacityAnimationPrefab, targetedTile.transform.position, Quaternion.identity);
            Destroy(effectInstance, 5f);
        }
    }
}
