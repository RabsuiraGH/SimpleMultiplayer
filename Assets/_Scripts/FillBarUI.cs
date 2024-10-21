using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class FillBarUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        public void UpdateValue(float currentAmount, float maxAmount)
        {
            _fillImage.fillAmount = currentAmount / maxAmount;
        }
    }
}