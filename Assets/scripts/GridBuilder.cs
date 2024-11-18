using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    public CustomGrid gridPrefab;
    private CustomGrid _gridInstance;

    public Tile gridTilePrefab;
    private Tile _tileInstance;
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    public CustomGrid CreateGrid()
    {
        _gridInstance = Instantiate(gridPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        InstantiateTiles(_gridInstance);
        PlaceTiles(_gridInstance);
        return _gridInstance;
    }
    
    private void InstantiateTiles(CustomGrid customGrid)
    {
        for (var i = 0; i < customGrid.tilesNumber; i++)
        {
            _tileInstance = Instantiate(gridTilePrefab, new Vector3(0, 0, 0), Quaternion.identity, customGrid.transform);
            int x = i % customGrid.sizeX;
            int z = i / customGrid.sizeZ;
            _tileInstance.name = $"Tile_{x}_{z}";

            customGrid.allTiles.Add(_tileInstance);
        }
    }
    
    private void PlaceTiles(CustomGrid grid)
    {
        for (int i = 0; i < grid.allTiles.Count; i++)
        {
            int x = i % grid.sizeX;
            int z = i / grid.sizeX;
            Tile tile = grid.allTiles[i];
            tile.transform.position = CalculatePosition(x, z, tile);
            tile.indexX = x;
            tile.indexZ = z;
        }
    }

    private Vector3 CalculatePosition(int x, int z, Tile tile)
    {
        float posX = x * (tile.sizeX);
        float posZ = z * (tile.sizeZ);
        return new Vector3(posX, 0, posZ);
    }
}