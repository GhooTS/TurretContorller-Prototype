using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image bar;
    [SerializeField]
    private Health health;


    private void OnEnable()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        bar.fillAmount = health.GetCurrentHP() / health.MaxHP.Value;
    }
}
