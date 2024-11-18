using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    public PlayerData playerData;
    
    public int playerId;
    public string userName;
    public string email;
    public string password;
    public string authToken;
    public int score;
    public int currency;
    public List<string> teamUnits;
    
    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D  M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------   
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    public void SetData()
    {
        this.playerId = playerData.playerId;
        this.userName = playerData.userName;
        this.email = playerData.email;
        this.password = playerData.password;
        this.authToken = playerData.authToken;
        this.score = playerData.score;
        this.currency = playerData.currency;
        this.teamUnits = playerData.teamUnits;
    }

    public void DisplayData()
    {
        Debug.Log($"PlayerId: {playerData.playerId}");
        Debug.Log($"UserName: {playerData.userName}");
        Debug.Log($"Email: {playerData.email}");
        Debug.Log($"Password: {playerData.password}");
        Debug.Log($"Score: {playerData.score}");
        Debug.Log($"Currency: {playerData.currency}");
        foreach (var unit in playerData.teamUnits)
            Debug.Log($"Unit: {unit}");
    }
    
    public void UpdateData()
    {
        playerData.playerId = this.playerId;
        playerData.userName = this.userName;
        playerData.email = this.email;
        playerData.password = this.password;
        playerData.authToken = this.authToken;
        playerData.score = this.score;
        playerData.currency = this.currency;
        playerData.teamUnits = this.teamUnits;
    }

}
