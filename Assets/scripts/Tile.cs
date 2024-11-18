using System;
using UnityEngine;

public enum TileState
{
    Selectable,
    Selected,
    NotSelectable
}

public class Tile : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------

    public int indexX;
    public int indexZ;
    public float sizeZ = 2f;
    public float sizeX = 2f;

    public bool isWalkable;
    public bool isAccessible;
    
    public TileState state = TileState.NotSelectable;
    
    public int gCost, hCost;
    public Tile parent;
    public int fCost => gCost + hCost;
    public static event Action<Tile> OnTileSelected;
    
    [SerializeField] Material[] materials;
    private Renderer _renderer;


    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.sharedMaterial = materials[0];
        UpdateTileShader();
    }
    
    private void OnMouseEnter()
    {
        if (!isWalkable)
            _renderer.sharedMaterial = materials[3];
        else if (isAccessible)
        {
            _renderer.sharedMaterial = materials[1];
            if (state == TileState.Selected)
                _renderer.sharedMaterial = materials[4];
        }
        else
            _renderer.sharedMaterial = materials[0];
    }

    private void OnMouseExit()
    {
        UpdateTileShader();
    }

    private void OnMouseDown()
    {
        switch (state)
        {
            case TileState.NotSelectable:
                return;
            case TileState.Selectable:
                OnTileSelected?.Invoke(this);
                state = TileState.Selected;
                break;
            case TileState.Selected:
                state = TileState.Selectable;
                break;
        }
        UpdateTileShader();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void UpdateTileShader()
    {
        switch (state)
        {
            case TileState.Selected:
                _renderer.sharedMaterial = materials[2];
                break;
            case TileState.Selectable:
                if (isWalkable)
                    _renderer.sharedMaterial = isAccessible ? materials[1] : materials[0];
                else
                    _renderer.sharedMaterial = materials[3];
                break;
            case TileState.NotSelectable:
                _renderer.sharedMaterial = materials[3];
                break;
        }
    }
}