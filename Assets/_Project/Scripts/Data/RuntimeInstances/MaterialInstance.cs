using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 재료의 런타임 인스턴스 클래스
    /// StaticData(MaterialData)와 동적 정보(수량)를 결합
    /// </summary>
    [System.Serializable]
    public class MaterialInstance
    {
        [Header("정적 데이터 참조")]
        [Tooltip("원본 재료 데이터 참조")]
        public MaterialData data;
        
        [Header("동적 속성")]
        [Tooltip("보유 수량")]
        public int quantity = 0;

        /// <summary>
        /// 생성자 - MaterialData로부터 인스턴스 생성
        /// </summary>
        public MaterialInstance(MaterialData materialData, int initialQuantity = 1)
        {
            data = materialData;
            quantity = initialQuantity;
        }
        
        /// <summary>
        /// 기본 생성자 (JSON 역직렬화용)
        /// </summary>
        public MaterialInstance()
        {
            quantity = 0;
        }

        /// <summary>
        /// 재료 이름 반환
        /// </summary>
        public string GetDisplayName()
        {
            return data?.materialName ?? "Unknown Material";
        }
        
        /// <summary>
        /// 재료의 기본 가치 반환
        /// </summary>
        public int GetValue()
        {
            return data?.baseValue ?? 0;
        }
        
        /// <summary>
        /// 총 가치 계산 (가치 × 수량)
        /// </summary>
        public int GetTotalValue()
        {
            return GetValue() * quantity;
        }
        
        /// <summary>
        /// 재료 추가
        /// </summary>
        public void AddQuantity(int amount)
        {
            if (amount > 0)
            {
                quantity += amount;
            }
        }
        
        /// <summary>
        /// 재료 소모
        /// </summary>
        public bool ConsumeQuantity(int amount)
        {
            if (amount > 0 && quantity >= amount)
            {
                quantity -= amount;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 지정된 수량이 있는지 확인
        /// </summary>
        public bool HasQuantity(int requiredAmount)
        {
            return quantity >= requiredAmount;
        }
        
        /// <summary>
        /// 재료가 비어있는지 확인
        /// </summary>
        public bool IsEmpty()
        {
            return quantity <= 0;
        }
        
        /// <summary>
        /// 재료가 유효한지 확인 (data가 null이 아닌지)
        /// </summary>
        public bool IsValid()
        {
            return data != null;
        }
        
        /// <summary>
        /// 디버그용 문자열 표현
        /// </summary>
        public override string ToString()
        {
            if (data == null) return $"MaterialInstance [Invalid] Qty: {quantity}";
            
            return $"MaterialInstance [{GetDisplayName()}] " +
                   $"Grade: {data.grade} | Qty: {quantity} | " +
                   $"Value: {GetValue()}G | Total: {GetTotalValue()}G";
        }
    }
    
    /// <summary>
    /// 저장/로드용 MaterialInstance 데이터
    /// Unity가 직렬화할 수 없는 데이터를 ID로 변환하여 저장
    /// </summary>
    [System.Serializable]
    public class MaterialInstanceSaveData
    {
        public string staticDataID;      // MaterialData의 ID
        public int quantity;             // 보유 수량
        
        /// <summary>
        /// MaterialInstance에서 저장 데이터 생성
        /// </summary>
        public MaterialInstanceSaveData(MaterialInstance material)
        {
            staticDataID = material.data?.id ?? "";
            quantity = material.quantity;
        }
        
        /// <summary>
        /// JSON 역직렬화용 기본 생성자
        /// </summary>
        public MaterialInstanceSaveData() { }
        
        /// <summary>
        /// 저장 데이터에서 MaterialInstance 복원
        /// DataManager를 통해 MaterialData 참조 복원
        /// </summary>
        public MaterialInstance ToMaterialInstance(MaterialData materialData)
        {
            var material = new MaterialInstance
            {
                data = materialData,
                quantity = quantity
            };
            
            return material;
        }
    }
}