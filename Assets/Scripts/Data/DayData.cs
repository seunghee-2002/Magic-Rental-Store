public enum DayPhase { Morning, Day, Night }

[System.Serializable]
public class DayResult
{
    public CustomerInstance customer; // 고객 정보
    public bool isSuccess; // 모험 성공 여부
    public int reward; // 보상 금액
}
