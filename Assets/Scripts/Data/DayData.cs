public enum DayPhase { Morning, Day, Night }

[System.Serializable]
public class DayResult
{
    public CustomerInstance customer;    // 고객 인스턴스
    public bool isSuccess;              // 모험 성공 여부
    public int reward;                  // 보상 금액
    public MaterialData expectedMaterial;         // 보상 재료 이름
    public int materialCount;           // 보상 재료 개수
}
