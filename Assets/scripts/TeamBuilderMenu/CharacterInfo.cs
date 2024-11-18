using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    public Sprite characterSprite;
    public string characterName;
    public string characterDescription;

    public CharacterDescription descriptionPanel;

    void Start()
    {
        if (descriptionPanel == null)
        {
            Debug.LogError("DescriptionPanel is not assigned in the inspector for " + gameObject.name);
            return;
        }
        else
        {
            Debug.Log("DescriptionPanel is assigned correctly for " + gameObject.name);
        }

        GetComponent<Button>().onClick.AddListener(OnCharacterClicked);
    }

    void OnCharacterClicked()
    {
        Debug.Log("Button clicked for " + characterName);
        if (descriptionPanel != null)
        {
            descriptionPanel.DisplayCharacterInfo(characterSprite, characterName, characterDescription);
            Debug.Log("Character info displayed for " + characterName);
        }
        else
        {
            Debug.LogError("DescriptionPanel is null when trying to display character info for " + gameObject.name);
        }
    }
}