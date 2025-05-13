public enum DayPhase { Morning, Day, Night }

[System.Serializable]
public class DayResult
{
    public Customer customer; // 고객 정보
    public ItemData item; // 대여한 아이템
    public bool isSuccess; // 모험 성공 여부
    public int reward; // 보상 금액
}
