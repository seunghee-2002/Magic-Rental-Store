// ArrayElementTitle.cs
using System;
using UnityEngine;

namespace MagicRentalShop.Utils
{
    /// <summary>
    /// 배열의 각 요소 이름을 지정된 프로퍼티 값으로 표시
    /// </summary>
    /// <example>
    /// [ArrayElementTitle("name")]
    /// public MonsterData[] monsters;
    /// </example>
    public class ArrayElementTitle : PropertyAttribute
    {
        public string varName;
        
        public ArrayElementTitle(string VarName)
        {
            varName = VarName;
        }
    }
}