using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Authentication : MonoBehaviour
{
    public PlayerData playerData;

    private void Start()
    {
        // Assume the game starts and we need to authenticate the user
        StartCoroutine(AuthenticateUser());
    }

    private IEnumerator AuthenticateUser()
    {
        // Use the correct endpoint
        string url = "http://localhost:3000/users/1";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Authentication failed: " + www.error);
        }
        else
        {
            // Assuming the server returns a JSON with id, userName, and authToken
            UserData userData = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
            playerData.playerId = userData.id;
            playerData.userName = userData.userName;
            playerData.authToken = userData.authToken;
            playerData.teamUnits = userData.teamUnits;

            Debug.Log("User authenticated successfully");
        }
    }

    [System.Serializable]
    private class UserData
    {
        public int id;
        public string userName;
        public string authToken;
        public List<string> teamUnits;
    }
}