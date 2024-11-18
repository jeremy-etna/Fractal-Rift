using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerProfil : MonoBehaviour
{

    // public image form list à faire
    public List<Image> imageList;
    public Dictionary<string,Image> avatarList = new Dictionary<string, Image>();
    public TMP_Text usernameText;
    public TMP_Text currentCurrencyText;
    public TMP_Text obtainedCurrencyText;
    // public List<String> spellsText;
    public TMP_Text gamesPlayedText;
    public TMP_Text victoriesText;
    public TMP_Text monstersKilledText;
    public TMP_Text treasuresRecoveredText;
    public TMP_Text inputUsernameText;
    public PlayerData playerData;
    public Image userAvatar;
    
    
    
    // Set the player's profile information to the ui
    private void SetProfileInfo(string username, string avatarName, int currentCurrency, int obtainedCurrency, int gamesPlayed, int victories, int monstersKilled, int treasuresRecovered)
    {
        usernameText.text = username;
        currentCurrencyText.text = "Currency actuelle: " + currentCurrency.ToString();
        obtainedCurrencyText.text = "Currency obtenue: " + obtainedCurrency.ToString();
        // spellsText. = spells;
        gamesPlayedText.text = gamesPlayed.ToString();
        victoriesText.text = victories.ToString();
        monstersKilledText.text = monstersKilled.ToString();
        treasuresRecoveredText.text = treasuresRecovered.ToString();
        EditUserAvatar(avatarName);
    }

    public void EditUsername()
    {
        usernameText.text = inputUsernameText.text;
        playerData.userName = inputUsernameText.text;
        // Faire la validation et l'envoie au serveur ici !'
    }
    
    public void EditUserAvatar(string avatarName)
    {
        if (!avatarList.TryGetValue(avatarName, out Image avatarImage)) return;
        playerData.avatarImageName = avatarName;
        userAvatar.sprite = avatarImage.sprite;
    }

    private void SetupAvatarDictionary()
    {
        avatarList.Add("Brute", imageList[0]);
        avatarList.Add("Dog", imageList[1]);
        avatarList.Add("Rogue", imageList[2]);
        avatarList.Add("Paladin", imageList[3]);
        avatarList.Add("Shaman", imageList[4]);
    }
    
    private void Start()
    {
        SetupAvatarDictionary();
        SetProfileInfo(playerData.userName, playerData.avatarImageName, playerData.currency, playerData.currency, playerData.gamesPlayed, playerData.victories, playerData.monstersKilled, playerData.treasuresRecovered);
    }
}
