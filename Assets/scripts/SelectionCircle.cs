using UnityEngine;

public class SelectionCircle : MonoBehaviour
{
    void Start()
    {
        SetVisible(false);
    }
    
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}