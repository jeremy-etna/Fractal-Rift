using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public Transform myTeamGrid;
    public Button resetButton; 

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetMyTeamGrid);
        }
    }

    public void ResetMyTeamGrid()
    {
        foreach (Transform slot in myTeamGrid)
        {
            if (slot.childCount > 0)
            {
                foreach (Transform child in slot)
                {
                    Destroy(child.gameObject); 
                }
            }
        }
        Debug.Log("MyTeamGrid has been reset.");
    }
}