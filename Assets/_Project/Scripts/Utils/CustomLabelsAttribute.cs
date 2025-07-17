// CustomLabelsAttribute.cs
using UnityEngine;

namespace MagicRentalShop.Utils
{
    /// <summary>
    /// 배열의 각 요소를 커스텀 라벨로 표시하는 어트리뷰트
    /// 쉼표로 구분된 문자열을 받아서 배열 요소 이름으로 사용
    /// </summary>
    /// <example>
    /// [CustomLabels("+1, +2, +3, +4, +5")]
    /// public float[] levelUpRates = {0.40f, 0.30f, 0.20f, 0.08f, 0.02f};
    /// 
    /// [CustomLabels("체력, 마나, 공격력, 방어력")]
    /// public int[] statValues = {100, 50, 25, 15};
    /// </example>
    public class CustomLabelsAttribute : PropertyAttribute
    {
        public readonly string[] labels;
        
        public CustomLabelsAttribute(string labelsString)
        {
            // 쉼표로 구분하고 앞뒤 공백 제거
            labels = labelsString.Split(',');
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = labels[i].Trim();
            }
        }
    }
}