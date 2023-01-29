using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;

    private float _health;

    public static Action<float> onUpdateHealth;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _health = _maxHealth;
        onUpdateHealth?.Invoke(_health);
    }

    public void Damage(float damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        onUpdateHealth?.Invoke(_health);
    }
}
