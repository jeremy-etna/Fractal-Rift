using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private float reduceSpeed = 2;
    private float _target = 1;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth) {
        _target = currentHealth / maxHealth;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        healthBarSprite.fillAmount = Mathf.MoveTowards(healthBarSprite.fillAmount, _target, reduceSpeed * Time.deltaTime);
    }
}