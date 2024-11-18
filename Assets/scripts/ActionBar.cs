using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private Image actionBarSprite;
    [SerializeField] private float reduceSpeed = 2;
    private float _target = 1;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateActionBar(float maxAction, float currentAction)
    {
        _target = currentAction / maxAction;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        actionBarSprite.fillAmount = Mathf.MoveTowards(actionBarSprite.fillAmount, _target, reduceSpeed * Time.deltaTime);
    }
}