// GradeLabelsAttribute.cs (간소화 버전)
using UnityEngine;

namespace MagicRentalShop.Utils
{
    /// <summary>
    /// 배열의 각 요소를 등급 이름으로 표시하는 어트리뷰트
    /// CustomLabels의 간편 버전 (Common, Uncommon, Rare, Epic, Legendary 고정)
    /// </summary>
    /// <example>
    /// [GradeLabels]
    /// public float[] gradeSpawnRates = {0.75f, 0.14f, 0.07f, 0.03f, 0.01f};
    /// </example>
    public class GradeLabelsAttribute : CustomLabelsAttribute
    {
        public GradeLabelsAttribute() : base("Common, Uncommon, Rare, Epic, Legendary")
        {
        }
    }
}