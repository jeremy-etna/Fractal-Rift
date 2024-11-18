using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]

public class UnitData : ScriptableObject
{
    public string unitName;
    public Tile tile;
    public int maxHealthPoints;
    public int maxActionPoints;
    public int movementPoints;
    public List<CapacityDefinition> capacities;
    public GameObject modelPrefab;
}
