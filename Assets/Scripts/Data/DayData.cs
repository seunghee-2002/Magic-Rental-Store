public enum DayPhase { Morning, Day, Night }

[System.Serializable]
public class DayResult
{
    public Customer customer; // 고객 정보
    public WeaponData weapon; // 대여한 도구
    public bool isSuccess; // 모험 성공 여부
    public int reward; // 보상 금액
}
