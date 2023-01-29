using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text _textHealth;

    private void OnEnable()
    {
        Player.onUpdateHealth += SetAmmountHealth;
    }

    private void OnDisable()
    {
        Player.onUpdateHealth -= SetAmmountHealth;
    }

    private void SetAmmountHealth(float amount)
    {
        _textHealth.text = amount.ToString();
    }
}
