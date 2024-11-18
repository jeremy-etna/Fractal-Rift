using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    public int sizeX;
    public int sizeZ;

    public List<Tile> allTiles = new List<Tile>();
    public List<Tile> walkableTiles = new List<Tile>();
    public List<Tile> selectableTiles = new List<Tile>();

    private int _tilesNumber;
    public int tilesNumber => sizeX * sizeZ;
    
    // -----------------------------------------------------------------------------------------------------------------
    // S I G N A L S   H A N D L I N G
    // -----------------------------------------------------------------------------------------------------------------
    
    // Signal handling
    private void OnEnable()
    {
        Tile.OnTileSelected += DeselectPreviousTile;
    }

    private void OnDisable()
    {
        Tile.OnTileSelected -= DeselectPreviousTile;
    }
    
    public void DeselectPreviousTile(Tile selectedTile)
    {
        foreach (Tile tile in walkableTiles)
        {
            if (tile != selectedTile)
            {
                tile.state = TileState.Selectable;
                tile.UpdateTileShader();
            }

        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    // TESTED
    public Tile GetSelectedTile()
    {
        foreach (Tile tile in allTiles)
        {
            if (tile.state == TileState.Selected)
                return tile;
        }
        return null;
    }
    
    // TESTED
    public Tile GetTileAt(int x, int z)
    {
        foreach (Tile tile in allTiles)
        {
            if (tile.indexX == x && tile.indexZ == z)
                return tile;
        }
        return null;
    }
    
    public List<Tile> GetAccessibleTiles(Tile originTile, int maxDistance)
    {
        List<Tile> accessibleTiles = GetNeighbourTilesInRange(originTile, maxDistance);
        List<Tile> filteredTiles = FilterTilesWithinManhattanDistance(originTile, accessibleTiles, maxDistance);
        filteredTiles.Remove(originTile);
        return filteredTiles;
    }
    
    /// <summary>
    ///  Cette methode permet de récupérer les tiles voisines d'une tile d'origine.
    ///  Le radius permet d'augmenter le nombre de 'rangs' de tiles voisines.
    /// </summary>
    public List<Tile> GetNeighbourTilesInRange(Tile originTile, int radius)
    {
        List<Tile> neighbourTiles = new List<Tile>();

        for (int xOffset = -radius; xOffset <= radius; xOffset++)
        {
            for (int zOffset = -radius; zOffset <= radius; zOffset++)
            {
                int checkX = originTile.indexX + xOffset;
                int checkZ = originTile.indexZ + zOffset;

                Tile neighbour = GetTileAt(checkX, checkZ);
                if (neighbour != null)
                    neighbourTiles.Add(neighbour);
            }
        }
        return neighbourTiles;
    }

    /// <summary>
    ///  Cette methode permet de récupérer filtrer une selection de tiles récupérer avec
    /// la méthode GetNeighbourTilesInRange() et d'appliquer la distance de Manhatthan afin
    ///  d'arrondir la sélection de tiles.
    /// </summary>
    public List<Tile> FilterTilesWithinManhattanDistance(Tile originTile, List<Tile> tiles, int radius)
    {
        List<Tile> filteredTiles = new List<Tile>();

        foreach (Tile tile in tiles)
        {
            int distanceX = Mathf.Abs(tile.indexX - originTile.indexX);
            int distanceZ = Mathf.Abs(tile.indexZ - originTile.indexZ);

            if (distanceX + distanceZ <= radius)
                filteredTiles.Add(tile);
        }
        return filteredTiles;
    }
    
    /// <summary>
    ///  Cette methode permet de récupérer filtrer une selection de tiles et de supprimer
    /// de la liste les tiles qui sont non walkables.
    /// </summary>
    public List<Tile> RemoveNonWalkableTiles(List<Tile> tiles)
    {
        tiles.RemoveAll(tile => tile.isWalkable == false);
        return tiles;
    }
    
    public void MarkTilesAsAccessible(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
            tile.isAccessible = true;
    }
    
    public void ResetAccessibleTiles()
    {
        foreach (Tile tile in allTiles)
            tile.isAccessible = false;
    }
    
    public void InitMapTiles(bool[,] walkableMap)
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                if (walkableMap[x, z])
                {
                    Tile tile = GetTileAt(x, z);
                    tile.isWalkable = true;
                    walkableTiles.Add(tile);
                    tile.state = TileState.Selectable;
                    selectableTiles.Add(tile);
                }
            }
        }
    }
    
    public void UpdateDisplay()
    {
        foreach (Tile tile in allTiles)
            tile.UpdateTileShader();
    }

    ///////////////////////////////////////////////////////////////////////
    ///  methods to remove or move in managers
    ///////////////////////////////////////////////////////////////////////
    
    public List<Tile> GetTilesInAOE(Tile tile, int range)
    {
        List<Tile> tilesInAOE = new List<Tile>();

        for (int xOffset = -range; xOffset <= range; xOffset++)
        {
            for (int zOffset = -range; zOffset <= range; zOffset++)
            {
                int checkX = tile.indexX + xOffset;
                int checkZ = tile.indexZ + zOffset;
                Tile neighbour = GetTileAt(checkX, checkZ);
                if (neighbour != null)
                    tilesInAOE.Add(neighbour);
            }
        }
        return tilesInAOE;
    }
    

    public List<Tile> GetNeighborsWithinRadius(Tile originTile, int radius)
    {
        List<Tile> neighborsWithinRadius = new List<Tile>();

        foreach (Tile tile in allTiles)
        {
            int distanceX = Mathf.Abs(tile.indexX - originTile.indexX);
            int distanceZ = Mathf.Abs(tile.indexZ - originTile.indexZ);

            if (distanceX + distanceZ <= radius)
                neighborsWithinRadius.Add(tile);
        }
        return neighborsWithinRadius;
    }
    
    public List<Tile> GetNeighboursTiles(Tile tile)
    {
        List<Tile> neighbourTiles = new List<Tile>();

        int[] xOffsets = {-1, -1, -1, 0, 1, 1, 1, 0};
        int[] zOffsets = {-1, 0, 1, 1, 1, 0, -1, -1};

        for (int i = 0; i < 8; i++)
        {
            int checkX = tile.indexX + xOffsets[i];
            int checkZ = tile.indexZ + zOffsets[i];
            Tile neighbour = GetTileAt(checkX, checkZ);
            
            if (neighbour != null)
                neighbourTiles.Add(neighbour);
        }
        return neighbourTiles;
    }

    private List<(Tile, int)> GetWalkableNeighbours(Tile tile)
    {
        List<(Tile, int)> neighbours = new List<(Tile, int)>();

        int[] xOffsets = {0, -1, 1, 0};
        int[] zOffsets = {-1, 0, 0, 1};
        for (int i = 0; i < 4; i++)
        {
            Tile neighbour = GetTileAt(tile.indexX + xOffsets[i], tile.indexZ + zOffsets[i]);
            if (neighbour != null && neighbour.isWalkable)
                neighbours.Add((neighbour, 1));
        }

        xOffsets = new int[] {-1, -1, 1, 1};
        zOffsets = new int[] {-1, 1, -1, 1};
        for (int i = 0; i < 4; i++)
        {
            Tile neighbour = GetTileAt(tile.indexX + xOffsets[i], tile.indexZ + zOffsets[i]);
            if (neighbour != null && neighbour.isWalkable)
                neighbours.Add((neighbour, 3));
        }
        return neighbours;
    }
    
    public void MarkNeighboursTiles(Tile originTile)
    {
        var neighbourTiles = GetNeighboursTiles(originTile);
        foreach (Tile tile in neighbourTiles)
            tile.isAccessible = true;
    }
    
    public void MarkAccessibleTiles(Tile originTile, int maxDistance)
    {
        var accessibleTiles = GetAccessibleTiles(originTile, maxDistance);
        foreach (Tile tile in accessibleTiles)
            tile.isAccessible = true;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // S T A T E   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void MarkAllTilesAsSelectable()
    {
        foreach (Tile tile in allTiles)
        {
            tile.state = TileState.Selectable;
            tile.UpdateTileShader();
        }
        UpdateDisplay();
    }
    
    public void MarkAllTilesAsNotSelectable()
    {
        foreach (Tile tile in allTiles)
        {
            tile.state = TileState.NotSelectable;
            tile.UpdateTileShader();
        }
        UpdateDisplay();
    }
    
    public void MarkAllTilesAsSelected()
    {
        foreach (Tile tile in allTiles)
        {
            tile.state = TileState.Selected;
            tile.UpdateTileShader();
        }
        UpdateDisplay();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    
    
    public void MarkTileAsSelectable(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.state = TileState.Selectable;
            tile.UpdateTileShader();
        }
        UpdateDisplay();
    }
    
    public void MarkTileAsNotSelectable(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.state = TileState.NotSelectable;
            tile.UpdateTileShader();
        }
        UpdateDisplay();
    }
    
    public void MarkTileAsSelected(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.state = TileState.Selected;
            tile.UpdateTileShader();
        }
        UpdateDisplay();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    
    public void MarkTilesAsSelectableExcept(List<Tile> tiles, Tile selectedTile)
    {
        foreach (Tile tile in tiles)
        {
            if (tile != selectedTile)
            {
                tile.state = TileState.Selectable;
                tile.UpdateTileShader();
            }
        }
        UpdateDisplay();
    }
    
    public void MarkTilesAsNotSelectableExcept(List<Tile> tiles, Tile selectedTile)
    {
        foreach (Tile tile in tiles)
        {
            if (tile != selectedTile)
            {
                tile.state = TileState.NotSelectable;
                tile.UpdateTileShader();
            }
        }
        UpdateDisplay();
    }
    
    public void MarkTilesAsSelectedExcept(List<Tile> tiles, Tile selectedTile)
    {
        foreach (Tile tile in tiles)
        {
            if (tile != selectedTile)
            {
                tile.state = TileState.Selected;
                tile.UpdateTileShader();
            }
        }
        UpdateDisplay();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // P A T H   F I N D I N G
    // -----------------------------------------------------------------------------------------------------------------
    
        public List<Tile> FindPath(Tile startTile, Tile endTile)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = openSet[0];
            
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                    currentTile = openSet[i];
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == endTile)
                return RetracePath(startTile, endTile);

            foreach (var (neighbour, moveCost) in GetWalkableNeighbours(currentTile))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    continue;

                int newCostToNeighbour = currentTile.gCost + moveCost;
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endTile);
                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

        }
        return new List<Tile>();
    }

    private List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();
        return path;
    }

    private int GetDistance(Tile tileA, Tile tileB)
    {
        int distX = Mathf.Abs(tileA.indexX - tileB.indexX);
        int distZ = Mathf.Abs(tileA.indexZ - tileB.indexZ);

        return distX + distZ; // Distance de Manhattan
    }
}
