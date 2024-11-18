using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour

{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    public List<Unit> units = new List<Unit>();
    public int teamIndex;
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    // Signal handling
    private void OnEnable()
    {
        Unit.OnUnitSelected += DeselectPreviousUnit;
    }

    private void OnDisable()
    {
        Unit.OnUnitSelected -= DeselectPreviousUnit;
    }
    
    private void DeselectPreviousUnit(Unit selectedUnit)
    {
        foreach (Unit unit in units)
        {
            if (unit != selectedUnit)
            {
                unit.state = UnitState.Selectable;
                // unit.UpdateSelectionCircleDisplay();
            }
        }
    }
    // End of Signal handling

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }
    
    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }
    

    public void MaxActionPointsForAllUnits()
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.MaxActionPoints();
        }
    }
    
    public void ZeroActionPointsForAllUnits()
    {
        foreach (var unit in units)
        {
            Debug.Log(unit.maxActionPoints + " " + unit.currentActionPoints);
            if (unit != null)
                unit.ZeroActionPoints();
            Debug.Log(unit.maxActionPoints + " " + unit.currentActionPoints);

        }
    }
    
    public int GetTeamSize()
    {
        return units.Count;
    }
    
    public bool IsTeamDefeated()
    {
        return units.Count == 0;
    }

    public void CleanUpDestroyedUnits()
    {
        foreach (Unit unit in units)
        {
            if (unit == null)
                units.Remove(unit);
        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // S E L E C T I O N   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void MarkAllUnitsSelectable()
    {
        foreach (var unit in units)
            if (unit != null)
            {
                if (unit.currentActionPoints > 0)
                    unit.state = UnitState.Selectable;
            }
    }
    
    public void MarkAllUnitsNotSelectable()
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.state = UnitState.NotSelectable;
        }
    }
    
    public void MarkAllUnitsAsSelected()
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.state = UnitState.Selected;
        }
    }
    
    public void MarkOtherUnitsNotSelectable(Unit selectedUnit)
    {
        foreach (Unit unit in units)
        {
            if (unit != selectedUnit)
                unit.state = UnitState.NotSelectable;
        }
    }
    
    public void MarkUnitsSelectable(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.state = UnitState.Selectable;
    }

    
    public void MarkUnitsNotSelectable(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.state = UnitState.NotSelectable;
    }
    
    public void MarkUnitsAsSelected(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.state = UnitState.Selected;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    
    public List<Unit> GetSelectableUnits(List<Unit> units)
    {
        List<Unit> selectableUnit = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.state == UnitState.Selectable)
                selectableUnit.Add(unit);
        }
        return selectableUnit;
    }
    
    public List<Unit> GetNotSelectableUnits(List<Unit> units)
    {
        List<Unit> notSelectableUnit = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.state == UnitState.NotSelectable)
                notSelectableUnit.Add(unit);
        }
        return notSelectableUnit;
    }
    
    public List<Unit> GetSelectedUnit(List<Unit> units)
    {
        List<Unit> selectedUnit = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.state == UnitState.Selected)
                selectedUnit.Add(unit);
        }
        return selectedUnit;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // T A R G E T I O N   M E T H O D S
    //------------------------------------------------------------------------------------------------------------------
    
    public void MarkAllUnitsAsTargetable()
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.targetableState = UnitTargetableState.Targetable;
        }
    }
    
    public void MarkAllUnitsAsNotTargetable()
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.targetableState = UnitTargetableState.NotTargetable;
        }
    }
    
    public void MarkAllUnitsAsTargeted()
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.targetableState = UnitTargetableState.Targeted;
        }
    }
    
    
    public void MarkOtherUnitsAsTargetable(Unit selectedUnit)
    {
        foreach (Unit unit in units)
        {
            if (unit != selectedUnit)
                unit.targetableState = UnitTargetableState.Targetable;
        }
    }
    
    public void MarkOtherUnitsAsNotTargetable(Unit selectedUnit)
    {
        foreach (Unit unit in units)
        {
            if (unit != selectedUnit)
                unit.targetableState = UnitTargetableState.NotTargetable;
        }
    }
    
    public void MarkOtherUnitsAsTargeted(Unit selectedUnit)
    {
        foreach (Unit unit in units)
        {
            if (unit != selectedUnit)
                unit.targetableState = UnitTargetableState.Targeted;
        }
    }
    
    
    public void MarkUnitsAsTargetable(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.targetableState = UnitTargetableState.Targetable;
    }
    
    public void MarkUnitsAsNotTargetable(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.targetableState = UnitTargetableState.NotTargetable;
    }
    
    public void MarkUnitsAsTargeted(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.targetableState = UnitTargetableState.Targeted;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    
    public List<Unit> GetTargetableUnits(List<Unit> units)
    {
        List<Unit> targetableUnit = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.targetableState == UnitTargetableState.Targetable)
                targetableUnit.Add(unit);
        }
        return targetableUnit;
    }
    
    public List<Unit> GetNotTargetableUnits(List<Unit> units)
    {
        List<Unit> notTargetableUnit = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.targetableState == UnitTargetableState.NotTargetable)
                notTargetableUnit.Add(unit);
        }
        return notTargetableUnit;
    }
    
    public List<Unit> GetTargetedUnits(List<Unit> units)
    {
        List<Unit> targetedUnit = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.targetableState == UnitTargetableState.Targeted)
                targetedUnit.Add(unit);
        }
        return targetedUnit;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
}