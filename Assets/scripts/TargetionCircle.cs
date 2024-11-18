using UnityEngine;

public class TargetionCircle : MonoBehaviour
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
