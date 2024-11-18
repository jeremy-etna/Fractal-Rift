using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int playerId;
    public string userName;
    public string email;
    public string password;
    public string authToken;
    public string avatarImageName;
    public int currency;
    // public string currentCurrency;
    // public string obtainedCurrency;
    public int gamesPlayed;
    public int victories;
    public int monstersKilled;
    public int treasuresRecovered;
    public int score;
    public List<string> teamUnits;
}
