using UnityEngine;

public enum CapacityType
{
    Attack,
    Heal,
    Buff,
    Debuff
}

[CreateAssetMenu(fileName = "CapacityDefinition", menuName = "ScriptableObjects/CapacityDefinition", order = 1)]
public class CapacityDefinition : ScriptableObject
{
    public CapacityType type;
    public int value;
    public int distanceRange;
    public bool isAoe;
    public int aoeRange;
    public GameObject animationPrefab;
}