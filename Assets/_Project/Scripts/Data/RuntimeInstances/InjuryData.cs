namespace MagicRentalShop.Data
{
    /// <summary>
    /// 부상입은 히어로를 저장하는 클래스
    /// </summary>
    [System.Serializable]
    public class InjuryData
{
    public string heroInstanceID;    // 부상당한 Hero ID
    public InjuryType injuryType;    // 부상 종류
    public int injuryStartDay;       // 부상 시작 날짜
    public int returnDay;            // 복귀 예정 날짜
}

    /// <summary>
    /// 부상 유형
    /// </summary>
    [System.Serializable]
    public enum InjuryType
    {
        None,      // 부상 없음
        Minor,     // 경상 (3일)
        Moderate,  // 중상 (7일)
        Severe     // 중증 (14일)
    }
}