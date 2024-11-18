using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class GridBuilderTests
{
    private GridBuilder _gridBuilder;
    private CustomGrid _grid;

    [SetUp]
    public void Setup()
    {
        GameObject gridBuilderGameObject = new GameObject();
        _gridBuilder = gridBuilderGameObject.AddComponent<GridBuilder>();

        _gridBuilder.gridPrefab = new GameObject().AddComponent<CustomGrid>();
        _gridBuilder.gridPrefab.sizeX = 5;
        _gridBuilder.gridPrefab.sizeZ = 5;

        _gridBuilder.gridTilePrefab = new GameObject().AddComponent<Tile>();

        _grid = _gridBuilder.CreateGrid();
    }

    [Test]
    public void GridHasCorrectTileCount()
    {
        Assert.AreEqual(25, _grid.allTiles.Count);
    }

    [Test]
    public void GridTilesAreCorrectlyNamed()
    {
        for (int i = 0; i < _grid.allTiles.Count; i++)
        {
            int x = i % _grid.sizeX;
            int z = i / _grid.sizeX;
            Assert.AreEqual($"Tile_{x}_{z}", _grid.allTiles[i].name);
        }
    }

    [Test]
    public void GridTilesAreCorrectlyPositioned()
    {
        for (int i = 0; i < _grid.allTiles.Count; i++)
        {
            int x = i % _grid.sizeX;
            int z = i / _grid.sizeX;
            Vector3 expectedPosition = new Vector3(x, 0, z);
            Assert.AreEqual(expectedPosition, _grid.allTiles[i].transform.position);
        }
    }
}
