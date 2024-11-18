using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    private CustomGrid _customGrid;
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    public void SetGrid(CustomGrid gridInstance)
    {
        _customGrid = gridInstance;
    }
    
    public void MoveAlongPath(Unit unit, Tile startTile, Tile endTile)
    {
        List<Tile> path = _customGrid.FindPath(startTile, endTile);
        StartCoroutine(FollowPath(unit, path));
        startTile.isWalkable = true;
        startTile.state = TileState.Selectable;
        endTile.isWalkable = false;
        endTile.state = TileState.NotSelectable;
        unit.tile = endTile;
    }

    private IEnumerator FollowPath(Unit unit, List<Tile> pathTiles)
    {
        foreach (Tile tile in pathTiles)
            yield return StartCoroutine(MoveToTile(unit, tile));
    }

    private IEnumerator MoveToTile(Unit unit, Tile nextTile)
    {
        Vector3 startPosition = unit.transform.position;
        Vector3 endPosition = nextTile.transform.position;
        float timeToMove = 0.3f;
        float timeElapsed = 0;

        while (timeElapsed < timeToMove)
        {
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / timeToMove);
            currentPosition.y = startPosition.y;
    
            unit.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    public void CleanAccessibleTiles()
    {
        _customGrid.ResetAccessibleTiles();
        _customGrid.UpdateDisplay();
    }
}