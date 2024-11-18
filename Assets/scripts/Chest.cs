using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    public Tile tile;
    public int value;
    public bool isCollected;
    
    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D  M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------
    
    public void Collect()
    {
        isCollected = true;
        tile = null;
        Renderer component = GetComponent<Renderer>();
        component.enabled = false;
    }

    public void Drop(Tile unitTile)
    {
        isCollected = false;
        tile = unitTile;
        this.transform.position = new Vector3(unitTile.transform.position.x, 0, unitTile.transform.position.z);
        Renderer component = GetComponent<Renderer>();
        component.enabled = true;
    }
}
