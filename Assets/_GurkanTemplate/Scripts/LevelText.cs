using TMPro;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public class LevelText : MonoBehaviour
    {
        private void Awake()
        {
            if (TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
            {
                textMeshProUGUI.text = $"LEVEL {LevelHelper.LevelNo}";
            }
        }
    }
}