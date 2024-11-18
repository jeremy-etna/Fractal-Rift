using UnityEngine;
using UnityEngine.UI;

public class TeamBar : MonoBehaviour
{
    [SerializeField] private Image teamBarSprite;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
    
    public void SetColor(Color color)
    {
        teamBarSprite.color = color;
    }
}
