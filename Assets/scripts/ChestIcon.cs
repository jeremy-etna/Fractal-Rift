using UnityEngine;
using UnityEngine.UI;


public class ChestIcon : MonoBehaviour
{
    [SerializeField] private Image chestIconSprite;
    private bool _isVisible;
    private Camera _cam;

    
    void Start()
    {
        _cam = Camera.main;
        _isVisible = false;
    }
    
    public void SetVisible(bool visible)
    {
        _isVisible = visible;
        
        if (_isVisible)
            chestIconSprite.enabled = true;
        else
            chestIconSprite.enabled = false;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
