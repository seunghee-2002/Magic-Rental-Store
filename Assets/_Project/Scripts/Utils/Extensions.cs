using System;

namespace MagicRentalShop.Extensions
{
    public static class KoreanExtensions
    {
        public static string ToKoreanString(this Grade grade)
        { // 한글 등급 변환
            return grade switch
            {
                Grade.Common => "일반",
                Grade.Uncommon => "고급",
                Grade.Rare => "희귀",
                Grade.Epic => "영웅",
                Grade.Legendary => "전설",
                _ => "알 수 없음"
            };
        }

        public static string ToKoreanString(this Element element)
        { // 한글 속성 변환
            return element switch
            {
                Element.Fire => "불",
                Element.Water => "물",
                Element.Thunder => "번개",
                Element.Earth => "땅",
                Element.Air => "바람",
                Element.Ice => "얼음",
                Element.Light => "빛",
                Element.Dark => "어둠",
                _ => "알 수 없음"
            };
        }

        public static string ToKoreanString(this GamePhase phase)
        { // 한글 게임 시간대 변환
            return phase switch
            {
                GamePhase.Morning => "아침",
                GamePhase.Day => "낮", 
                GamePhase.Night => "밤",
                _ => "알 수 없음"
            };
        }
    }
}