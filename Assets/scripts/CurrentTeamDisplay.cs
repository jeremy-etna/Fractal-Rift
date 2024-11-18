using UnityEngine;
using UnityEngine.UI;

public class CurrentTeamDisplay : MonoBehaviour
{
    [SerializeField] private Text teamNameText;
    
    void Start()
    {
        teamNameText.text = "--------";
    }
    
    public void UpdateTeamNameDisplay(string teamName)
    {
        teamNameText.text = teamName + " is playing";
    }
}
