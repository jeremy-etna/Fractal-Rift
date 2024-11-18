using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDescription : MonoBehaviour
{
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void Start()
    {
        avatarImage.gameObject.SetActive(false);
    }

    public void DisplayCharacterInfo(Sprite characterSprite, string name, string description)
    {
        Debug.Log(characterSprite);
        if (characterSprite == null)
        {
            Debug.Log("Videe");
            avatarImage.gameObject.SetActive(false);
        }
        else
        {
            avatarImage.gameObject.SetActive(true);
            avatarImage.sprite = characterSprite;
        }

        nameText.text = name;
        descriptionText.text = description;
    }
}
