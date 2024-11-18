using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    Selectable,
    NotSelectable,
    Selected
}

public enum UnitTargetableState
{
    Targetable,
    NotTargetable,
    Targeted
}

public class Unit : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------

    [Header("UNIT STATS")]
    public int teamIndex;
    public Tile tile;
    public int maxHealthPoints;
    public int currentHealthPoints;
    public int maxActionPoints;
    public int currentActionPoints;
    public int movementPoints;
    public List<CapacityDefinition> capacities;
    
    public bool hasChest;
    private Chest _chest;
    
    [Header("UNIT PREFABS")]
    [SerializeField] private TeamBar teamBarPrefab;
    private TeamBar _teamBarInstance;
    
    [SerializeField] private ActionBar actionBarPrefab;
    private ActionBar _actionBarInstance;
    
    [SerializeField] private HealthBar healthBarPrefab;
    private HealthBar _healthBarInstance;
    
    [SerializeField] private ChestIcon chestIconPrefab;
    private ChestIcon _chestIconInstance;
    
    [SerializeField] private  SelectionCircle selectionCirclePrefab;
    private SelectionCircle _selectionCircleInstance;
    
    [SerializeField] private  TargetionCircle targetionCirclePrefab;
    private TargetionCircle _targetionCircleInstance;
    
    public UnitState state = UnitState.NotSelectable;
    public UnitTargetableState targetableState = UnitTargetableState.NotTargetable;

    private MovementManager _movementManager;
    private CapacityManager _capacityManager;

    [SerializeField] private GameObject modelPrefab;
    private GameObject _modelInstance;
    
    public static event Action<Unit> OnUnitSelected;


    
    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D  M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------    

    private void Start()
    {
        _teamBarInstance = Instantiate(teamBarPrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity, transform);
        if (teamIndex == 1)
            _teamBarInstance.SetColor(Color.white);
        
        _healthBarInstance = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity, transform);
        _healthBarInstance.UpdateHealthBar(maxHealthPoints, currentHealthPoints);
        
        _actionBarInstance = Instantiate(actionBarPrefab, transform.position + new Vector3(0, 2.25f, 0), Quaternion.identity, transform);
        // _actionBarInstance.UpdateActionBar(maxActionPoints, currentActionPoints);
        
        _chestIconInstance = Instantiate(chestIconPrefab, transform.position + new Vector3(0, 4.0f, 0), Quaternion.identity, transform);
        _chestIconInstance.SetVisible(false);
        
        _selectionCircleInstance = Instantiate(selectionCirclePrefab, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity, transform);
        _targetionCircleInstance = Instantiate(targetionCirclePrefab, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity, transform);
    }

    void OnMouseDown()
    {
        if (state == UnitState.Selectable)
        {
            state = UnitState.Selected;
            _selectionCircleInstance.SetVisible(true);
            OnUnitSelected?.Invoke(this);
        }

        else if (state == UnitState.Selected)
            state = UnitState.Selected;

        if (targetableState == UnitTargetableState.Targetable)
            targetableState = UnitTargetableState.Targeted;
        
        else if (targetableState == UnitTargetableState.Targeted)
            targetableState = UnitTargetableState.Targetable;
        
        UpdateCirclesDisplay();
    }

    private void Update()
    {
        UpdateCirclesDisplay();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void SetData(UnitData unitData)
    {
        name = unitData.unitName;
        // tile = unitData.tile;
        maxHealthPoints = unitData.maxHealthPoints;
        currentHealthPoints = unitData.maxHealthPoints;
        maxActionPoints = unitData.maxActionPoints;
        currentActionPoints = unitData.maxActionPoints;
        movementPoints = unitData.movementPoints;
        capacities = unitData.capacities;
        
        var spawn = transform.position;
        var rot = transform.rotation;
        _modelInstance = Instantiate(unitData.modelPrefab, spawn, rot, transform);
    }
    
    public void SetMovementManager(MovementManager movementManager)
    {
        _movementManager = movementManager;
    }
    
    public void SetCapacityManager(CapacityManager capacityManager)
    {
        _capacityManager = capacityManager;
    }

    public void UpdateCirclesDisplay()
    {
        if (state == UnitState.Selected)
            _selectionCircleInstance.SetVisible(true);
        else
            _selectionCircleInstance.SetVisible(false);
        
        if (targetableState == UnitTargetableState.Targeted)
            _targetionCircleInstance.SetVisible(true);
        else
            _targetionCircleInstance.SetVisible(false);
    }
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // A T T A C K   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    public void UseCapacity(CapacityDefinition capacity, List<Unit> targetUnits)
    {
        if (currentActionPoints > 0)
        {
            DecreaseActionPoints();
            _capacityManager.ApplyCapacity(capacity, targetUnits);
        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    
    public void TakeDamage(int damage)
    {
        currentHealthPoints -= damage;
        _healthBarInstance.UpdateHealthBar(maxHealthPoints, currentHealthPoints);
        if (currentHealthPoints <= 0)
            Die();
    }

    private void Die()
    {
        tile.isWalkable = true;
        tile.state = TileState.Selectable;
        if (hasChest)
            _chest.Drop(tile);
        Destroy(gameObject);
    }
    
    private void DecreaseActionPoints()
    {
        currentActionPoints--;
        UpdateActionBarDisplay();
        if (currentActionPoints <= 0)
            state = UnitState.NotSelectable;
    }
    
    public void SetActionPoints(int actionPoints)
    {
        currentActionPoints = actionPoints;
        UpdateActionBarDisplay();
    }
    
    public void MaxActionPoints()
    {
        currentActionPoints = maxActionPoints;
        UpdateActionBarDisplay();
    }
    
    public void ZeroActionPoints()
    {
        currentActionPoints = 0;
        UpdateActionBarDisplay();
    }

    private void UpdateActionBarDisplay()
    {
        _actionBarInstance.UpdateActionBar(maxActionPoints, currentActionPoints);
    }
    
    public void Move(Tile targetTile)
    {
        if (currentActionPoints > 0)
        {
            DecreaseActionPoints();
            _movementManager.MoveAlongPath(this, tile, targetTile);
        }
    }
    
    public void CollectChest(Chest chest)
    {
        DecreaseActionPoints();
        hasChest = true;
        _chest = chest;
        _chest.Collect();
        _chestIconInstance.SetVisible(true);
    }
    
    public void DropChest()
    {
        DecreaseActionPoints();
        hasChest = false;
        _chest.Drop(tile);
        _chest = null;
        _chestIconInstance.SetVisible(false);
    }
}