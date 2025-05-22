[System.Serializable]
public class WeaponInstance
{
    public WeaponData data; // 무기 데이터
    public int quantity;

    public WeaponInstance(WeaponData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;
    }
}
