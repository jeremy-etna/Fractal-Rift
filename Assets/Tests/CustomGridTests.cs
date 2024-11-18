using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CustomGridTests
{
    private CustomGrid _grid;

    [SetUp]
    public void Setup()
    {
        GameObject gridGameObject = new GameObject();
        _grid = gridGameObject.AddComponent<CustomGrid>();

        _grid.sizeX = 10;
        _grid.sizeZ = 11;

        for (int x=0; x < _grid.sizeX; x++)
        {
            for (int z = 0; z < _grid.sizeZ; z++)
            {
                GameObject tileGameObject = new GameObject($"Tile_{x}_{z}");
                Tile tile = tileGameObject.AddComponent<Tile>();
                tile.indexX = x;
                tile.indexZ = z;
                tile.isWalkable = true;
                _grid.allTiles.Add(tile);
            }
        }
    }
    
    private Tile CreateTile(int x, int z)
    {
        GameObject tileGameObject = new GameObject($"Tile_{x}_{z}");
        Tile tile = tileGameObject.AddComponent<Tile>();
        tile.indexX = x;
        tile.indexZ = z;
        return tile;
    }
    
    [Test]
    public void GetTileAt_SHOULD_ReturnsNull_WHEN_InputXYIndicesAreWrong()
    {
        // Arrange
        Tile tile = _grid.GetTileAt(20, 5);
        
        //Assert
        Assert.IsNull(tile);
    }

    [Test]
    public void GetTileAt_SHOULD_ReturnsCorrectTile_WHEN_InputXYIndicesAreRight()
    {
        Tile tile = _grid.GetTileAt(5, 5);

        Assert.NotNull(tile);
        Assert.AreEqual(5, tile.indexX);
        Assert.AreEqual(5, tile.indexZ);
    }
    
    [Test]
    public void GetSelectedTile_SHOULD_ReturnsNull_WHEN_NoTileSelected()
    {
        // Arrange
        Tile tile = _grid.GetSelectedTile();
        
        // Assert
        Assert.IsNull(tile);
    }
    
    [Test]
    public void GetSelectedTile_SHOULD_ReturnsTheSeletedTile_WHEN_TileIsSelected()
    {
        // Arrange
        Tile tile = _grid.GetTileAt(5, 5);
        tile.state = TileState.Selected;

        // Act
        Tile selectedTile = _grid.GetSelectedTile();

        // Assert
        Assert.AreEqual(TileState.Selected, selectedTile.state);
    }
    
    [Test]
    public void GetAccessibleTiles_SHOULD_ReturnsTheCorrespondingListOfAccessibleTiles_WHEN_AllTilesAreWalkable()
    {
        // Arrange
        Tile origin = _grid.GetTileAt(2, 2);
        int maxDistance = 1;

        _grid.GetTileAt(1, 2).isWalkable = true;  // tile à gauche
        _grid.GetTileAt(3, 2).isWalkable = true;  // tile à droite
        _grid.GetTileAt(2, 1).isWalkable = true;  // tile en bas
        _grid.GetTileAt(2, 3).isWalkable = true;  // tile en haut
        
        List<Tile> expectedTiles = new List<Tile>
        {
            _grid.GetTileAt(1, 2),  // tile à gauche
            _grid.GetTileAt(3, 2),  // tile à droite
            _grid.GetTileAt(2, 1),  // tile en bas
            _grid.GetTileAt(2, 3)   // tile en haut
        };
        
        // Act
        List<Tile> accessibleTiles = _grid.GetAccessibleTiles(origin, maxDistance);
        Debug.Log("RESULT : Accessible Tiles");
        foreach (Tile tile in accessibleTiles)
            Debug.Log($"Tile  X: {tile.indexX}, Z: {tile.indexZ}, Walkable: {tile.isWalkable}");
        
        // Assert
        CollectionAssert.AreEquivalent(expectedTiles, accessibleTiles);
    }

    [Test]
    public void GetAccessibleTiles_SHOULD_ReturnEmptyList_WHEN_NoTilesAreWalkable()
    {
        // Arrange
        Tile origin = _grid.GetTileAt(2, 2);
        int maxDistance = 1;

        _grid.GetTileAt(1, 2).isWalkable = false;
        _grid.GetTileAt(3, 2).isWalkable = false;
        _grid.GetTileAt(2, 1).isWalkable = false;
        _grid.GetTileAt(2, 3).isWalkable = false;

        
        // Act
        List<Tile> accessibleTiles = _grid.GetAccessibleTiles(origin, maxDistance);

        // Assert
        Assert.IsEmpty(accessibleTiles, "No tiles should be accessible when all are non-walkable.");
    }
    
    [Test]
    public void GetAccessibleTiles_SHOULD_ReturnOnlyWalkableTiles_WHEN_MixedWalkability()
    {
        // Arrange
        Tile origin = _grid.GetTileAt(2, 2);
        origin.isWalkable = true;

        _grid.GetTileAt(1, 2).isWalkable = true;  // tile à gauche
        _grid.GetTileAt(3, 2).isWalkable = false; // tile à droite
        _grid.GetTileAt(2, 1).isWalkable = true;  // tile en bas
        _grid.GetTileAt(2, 3).isWalkable = false; // tile en haut

        int maxDistance = 1;

        List<Tile> expectedTiles = new List<Tile>
        {
            _grid.GetTileAt(1, 2),  // tile à gauche
            _grid.GetTileAt(2, 1),  // tile en bas
        };
        
        // Act
        List<Tile> accessibleTiles = _grid.GetAccessibleTiles(origin, maxDistance);
        Debug.Log("RESULT : Accessible Tiles");
        foreach (Tile tile in accessibleTiles)
            Debug.Log($"Tile  X: {tile.indexX}, Z: {tile.indexZ}, Walkable: {tile.isWalkable}");

        // Assert
        CollectionAssert.AreEquivalent(expectedTiles, accessibleTiles);
    }
    
    [Test]
    public void MarkAccessibleTiles_SHOULD_SetsTilesAsAccessible_WHEN_OriginAndDistanceIsGiven()
    {
        // Arrange
        Tile originTile = _grid.GetTileAt(2, 2);
        int maxDistance = 1;
        List<Tile> expectedTiles = new List<Tile>
        {
            _grid.GetTileAt(1, 2),  // tile à gauche
            _grid.GetTileAt(3, 2),  // tile à droite
            _grid.GetTileAt(2, 1),  // tile en bas
            _grid.GetTileAt(2, 3)   // tile en haut
        };
        
        // Act
        _grid.MarkAccessibleTiles(originTile, maxDistance);
        
        // Assert
        foreach (Tile tile in expectedTiles)
            Assert.IsTrue(tile.isAccessible);
    }

    [Test]
    public void GetNeighboursTiles_SHOULD_ReturnsCorrectNeighbourTilesList_WHEN_OriginTileIsGiven()
    {
        // Arrange
        Tile originTile = _grid.GetTileAt(2, 2);

        List<Tile> expectedTiles = new List<Tile>
        {
            CreateTile(1, 1),  // tile haut gauche
            CreateTile(1, 2),  // tile mid gauche
            CreateTile(1, 3),  // tile en bas à gauche
            CreateTile(2, 3),  // tile bas mid
            CreateTile(3, 3),  // tile bas droite
            CreateTile(3, 2),  // tile en mid droite
            CreateTile(3, 1),  // tile haut droite
            CreateTile(2, 1)   // tile haut mid
        };

        // Act
        List<Tile> actualNeighbours = _grid.GetNeighboursTiles(originTile);

        // Assert
        Assert.AreEqual(expectedTiles.Count, actualNeighbours.Count, "The number of neighbours does not match.");
    
        for (int i = 0; i < expectedTiles.Count; i++)
        {
            Assert.AreEqual(expectedTiles[i].indexX, actualNeighbours[i].indexX, $"Neighbour at index {i} has incorrect X coordinate.");
            Assert.AreEqual(expectedTiles[i].indexZ, actualNeighbours[i].indexZ, $"Neighbour at index {i} has incorrect Z coordinate.");
        }
    }
    
    [Test]
    public void MarkNeighboursTiles_SHOULD_SetsNeighboursAsAccessible_WHEN_OriginAndDistanceIsGiven()
    {
        // Arrange
        Tile originTile = _grid.GetTileAt(2, 2);
        _grid.ResetAccessibleTiles();
        
        // Act
        List<Tile> neighbourTiles = _grid.GetNeighboursTiles(originTile);
        _grid.MarkNeighboursTiles(originTile);
        // foreach (Tile tile in neighbourTiles)
        //     Debug.Log($"Tile  X: {tile.indexX}, Z: {tile.indexZ}, isAccessible: {tile.isAccessible}");
        
        // Assert
        foreach (Tile neighbour in neighbourTiles)
            Assert.IsTrue(neighbour.isAccessible);
    }

    [Test]
    public void ResetAccessibleTiles_SHOULD_ClearsAllAccessibleTiles()
    {
        Tile originTile = _grid.GetTileAt(2, 2);
        _grid.MarkAccessibleTiles(originTile, 2);

        _grid.ResetAccessibleTiles();

        foreach (Tile tile in _grid.allTiles)
            Assert.IsFalse(tile.isAccessible);
    }

    [Test]
    public void GetTilesInAOE_ReturnsTilesWithinRange()
    {
        Tile originTile = _grid.GetTileAt(2, 2);
        List<Tile> tilesInAOE = _grid.GetTilesInAOE(originTile, 1);

        Assert.AreEqual(9, tilesInAOE.Count);
    }
}
