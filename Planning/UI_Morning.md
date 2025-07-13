# 아침(Morning) UI 상세 설계 (Morning UI Detailed Design)

**목적:** 아침 페이즈의 모든 UI 컴포넌트와 상호작용을 상세히 정의하여 구현 시 참고할 수 있도록 합니다.

---

## 1. MorningView (메인 화면)

### 1.1. 화면 구성 (새로운 공통 레이아웃 적용)
```
┌─────────────────────────────────────┐
│                                     │
│          Morning Phase UI           │ ← 페이즈 패널 영역 (3/5)
│  [무기 상점]  [대장간]  [이벤트확인]  │
│ WeaponShop   Blacksmith  EventCheck │
│   Button      Button     Button    │
├─────────────────────────────────────┤
│ 골드: 1,500G | 12일차 | 아침 | 03:45 │ ← StatusBar (중간)
├─────────────────────────────────────┤
│        InventoryView (하단)         │ ← 인벤토리 영역 (2/5)
│      무기 탭  |  재료 탭            │
└─────────────────────────────────────┘
```

### 1.2. 컴포넌트 정의
- **weaponShopButton: Button** (무기 상점 진입 버튼)
- **blacksmithButton: Button** (대장간 진입 버튼)
- **eventCheckButton: Button** (일일 이벤트 확인 버튼, ? 아이콘)

### 1.3. 클릭 이벤트
- **weaponShopButton 클릭** → WeaponShopPanel 호출
- **blacksmithButton 클릭** → BlacksmithPanel 호출
- **eventCheckButton 클릭** → EventInfoPanel 호출

### 1.4. 월세 경고 시스템
- **아침 페이즈 시작 시**: 월세 납부일 하루 전 확인
- **월세 경고 조건**: 현재 일수가 (월세 납부일 - 1)인 경우
- **경고 표시**: AlertPopup으로 월세 납부 경고 메시지

#### 경고 메시지 예시
┌─────────────────────────────────────┐
│             월세 경고               │
├─────────────────────────────────────┤
│                                    │
│      내일 월세 1,400골드를           │
│      납부해야 합니다.               │
│                                    │
│      현재 보유 골드: 2,100골드       │
│                                    │
├─────────────────────────────────────┤
│              [확인]                 │
└─────────────────────────────────────┘

---

## 2. 무기 상점 시스템

### 2.1. WeaponShopPanel (무기 상점 메인)

#### 화면 구성
```
┌─────────────────────────────────────┐
│        무기 상점          [새로고침]  │
├─────────────────────────────────────┤
│     < 1/2 > (페이지네이션)           │
├─────────────────────────────────────┤
│ [무기1] [무기2] [무기3] [무기4]      │
│ Buy     Buy     Buy     Buy         │
│ Weapon  Weapon  Weapon  Weapon      │
│ Button  Button  Button  Button      │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

#### 컴포넌트 정의
- **weaponShopPanelContent: Transform** (buyWeaponButton들의 부모 객체)
- **refreshButton: Button** (상점 새로고침 버튼, 우측 상단)
- **refreshCost: int = 1000** (새로고침 비용)
- **pageNavigation: UI Component** (< 1/2 > 형태, 상단)
- **closeButton: Button**

#### 기능
- **새로고침**: 1000골드 소모하여 무기 목록 재생성, 구매 상태 초기화
- **페이지네이션**: GameConfig.uiPageSize에 따라 페이지 분할

### 2.2. BuyWeaponButton (무기 구매 아이템)

#### 표시 정보
- **weaponIcon: Sprite** (무기 아이콘)
- **cost: int** (구매 비용)
- **gradeSprite: Sprite** (등급에 따른 테두리 색상)

#### UI 레이아웃
```
┌─────────────────────┐
│  [무기 아이콘]       │ ← Grade에 따른 테두리 색상
│                    │
│    비용: XXX골드    │
└─────────────────────┘
```

#### 상태 변화
- **구매 전**: 정상 색상, 클릭 가능
- **구매 후**: 회색으로 비활성화
- **골드 부족**: 정상 색상 유지, 클릭 시 경고 메시지

#### 클릭 이벤트
- **buyWeaponButton 클릭** → BuyWeaponPanel 호출

### 2.3. BuyWeaponPanel (무기 구매 확인 팝업)

#### 표시 정보
- **weaponName: string** (무기 이름)
- **description: string** (무기 설명)
- **grade: Grade** (무기 등급)
- **element: Element** (무기 속성)
- **buyButton: Button** (구매 확인)
- **cancelButton: Button** (구매 취소)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           무기 구매                  │
├─────────────────────────────────────┤
│ 무기명: XXX                        │
│ 등급: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│        [구매]      [취소]           │
└─────────────────────────────────────┘
```

## 2.4. 무기 판매 시스템

### 2.4.1. 판매 조건
- **판매 가능**: 무기만 (재료는 판매 불가)
- **판매 가격**: 구매가의 50%
- **판매 방식**: 개별 확인 팝업, 일괄 판매 없음

### 2.4.2. 인벤토리 Management 모드
- **활성화**: 아침 페이즈에서 인벤토리가 Management 모드로 전환
- **UI 변화**: 각 무기에 "판매" 버튼 추가 표시
- **재료 제외**: 재료에는 판매 버튼 미표시

---

## 3. 대장간 시스템

### 3.1. BlacksmithPanel (대장간 메인)

#### 화면 구성 (공통 레이아웃 활용)
```
┌─────────────────────────────────────┐
│        현재 페이즈 패널 영역          │  ← 3/5 비율
│     < 1/2 > (레시피 페이지네이션)     │
│                                    │
│ [레시피1] [레시피2] [레시피3]        │
│ Recipe   Recipe   Recipe           │
│ Button   Button   Button           │
├─────────────────────────────────────┤
│ 골드: 1,500G | 12일차 | 아침 | 03:45 │ ← StatusBar (중간)
├─────────────────────────────────────┤
│   BlacksmithDisplay (인벤토리 영역)   │  ← 2/5 비율 
│   < 1/2 > (제작중 무기 페이지네이션)  │  (인벤토리 대신 제작 현황)
│                                    │
│ [제작중1] [제작중2] [완료1]          │
│ Crafting  Crafting  Complete       │
│ Weapon    Weapon    Weapon         │
└─────────────────────────────────────┘
```

#### 컴포넌트 정의
- **recipePanelContent: Transform** (RecipeButton들의 부모, 상단 페이즈 영역)
- **blacksmithDisplayContent: Transform** (제작중 무기들의 부모, 하단 인벤토리 영역 대체)
- **recipePageNavigation: UI Component** (레시피용 페이지네이션)
- **craftingPageNavigation: UI Component** (제작중 무기용 페이지네이션)
- **closeButton: Button**

### 3.2. RecipeButton (레시피 아이템)

#### 표시 정보
- **recipeIcon: Sprite** (레시피 아이콘)

#### UI 레이아웃
```
┌─────────────────────┐
│  [레시피 아이콘]     │
│                    │
└─────────────────────┘
```

#### 클릭 이벤트
- **recipeButton 클릭** → RecipeInfoPanel 호출

### 3.3. RecipeInfoPanel (레시피 상세 정보 팝업)

#### 표시 정보
- **recipeName: string** (레시피 이름)
- **description: string** (레시피 설명)
- **cost: int** (제작 비용)
- **resultWeaponButton: Button** (결과 무기 정보 버튼)
- **materialButtons: Button[]** (필요 재료 버튼들)
- **craftButton: Button** (제작 시작 버튼)
- **cancelButton: Button**

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           레시피 정보                │
├─────────────────────────────────────┤
│ 레시피명: XXX                      │
│ 제작비용: XXX골드                   │
├─────────────────────────────────────┤
│ 결과 무기: [resultWeaponButton]     │
├─────────────────────────────────────┤
│ 필요 재료:                         │
│ [재료1] [재료2] [재료3]             │
│ Mat1    Mat2    Mat3               │
│ Button  Button  Button             │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│        [제작]      [취소]           │
└─────────────────────────────────────┘
```

#### 클릭 이벤트
- **resultWeaponButton 클릭** → ResultWeaponInfoPanel 호출
- **materialButton 클릭** → MaterialInfoPanel 호출
- **craftButton 클릭** → 제작 시작, BlacksmithDisplay에 추가

### 3.4. ResultWeaponInfoPanel (결과 무기 정보 팝업)

#### 표시 정보
- **resultWeaponIcon: Sprite** (결과 무기 아이콘)
- **weaponName: string** (무기 이름)
- **description: string** (무기 설명)
- **grade: Grade** (무기 등급)
- **element: Element** (무기 속성)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           결과 무기 정보             │
├─────────────────────────────────────┤
│ [무기 아이콘]                      │
│                                    │
│ 무기명: XXX                        │
│ 등급: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

### 3.5. MaterialInfoPanel (재료 정보 팝업)

#### 표시 정보
- **materialName: string** (재료 이름)
- **description: string** (재료 설명)
- **grade: Grade** (재료 등급)
- **availableDungeonNames: string[]** (구할 수 있는 던전 목록)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           재료 정보                  │
├─────────────────────────────────────┤
│ 재료명: XXX                        │
│ 등급: XXX                          │
├─────────────────────────────────────┤
│ 구할 수 있는 던전:                  │
│ - 고블린 동굴                      │
│ - 어둠의 숲                        │
│ - XXX 던전                         │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

### 3.6. BlacksmithDisplay (제작중 무기 표시 영역)

#### CraftingWeaponButton (제작중/완료 무기 아이템)

##### 표시 정보
- **resultWeaponIcon: Sprite** (제작될 무기 아이콘)
- **completionStatus: bool** (완료 여부)
- **checkIcon: Sprite** (완료 시 체크 표시)

##### UI 레이아웃
```
제작중:
┌─────────────────────┐
│  [무기 아이콘]       │
│                    │
│   제작중...         │
└─────────────────────┘

완료:
┌─────────────────────┐
│  [무기 아이콘] [✓]   │ ← 체크 아이콘 오버레이
│                    │
│   제작 완료!        │
└─────────────────────┘
```

##### 클릭 이벤트
- **제작중인 경우** → CraftingInfoPanel 호출
- **완료된 경우** → 무기 획득, 인벤토리로 이동, Display에서 제거

### 3.7. CraftingInfoPanel (제작 진행 상황 팝업)

#### 표시 정보
- **resultWeaponIcon: Sprite** (제작중인 무기 아이콘)
- **weaponName: string** (무기 이름)
- **remainingDays: int** (제작 완료까지 남은 일수)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           제작 진행 상황             │
├─────────────────────────────────────┤
│ [무기 아이콘]                      │
│                                    │
│ 제작중인 무기: XXX                  │
│ 완료까지: X일 남음                  │
├─────────────────────────────────────┤
│              [확인]                 │
└─────────────────────────────────────┘
```

---

## 4. 일일 이벤트 시스템

### 4.1. EventInfoPanel (일일 이벤트 정보 팝업)

#### 표시 정보
- **eventIcon: Sprite** (이벤트 아이콘)
- **eventName: string** (이벤트 이름)
- **eventEffect: string** (이벤트 효과 설명)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           오늘의 이벤트             │
├─────────────────────────────────────┤
│ [이벤트 아이콘]                    │
│                                    │
│ 이벤트명: XXX                      │
├─────────────────────────────────────┤
│ 효과: XXXXXXXXXXXXXXXXXXXXX        │
│                                    │
│ 예) 무기상점 진열 무기 +2개         │
│     모든 무기 20% 할인              │
├─────────────────────────────────────┤
│              [확인]                 │
└─────────────────────────────────────┘
```

---

## 5. UI 상호작용 플로우

### 5.1. 월세 경고 플로우 (신규)
```
MorningView 시작 시 
    → MorningController.CheckAndShowRentWarning()
        → RentCalculator.IsRentWarningDay() 확인
        → 경고일인 경우: UIManager.ShowRentWarning()
```

### 5.2. 무기 상점 플로우
```
MorningView 
    → weaponShopButton 클릭 
        → WeaponShopPanel 
            → buyWeaponButton 클릭 
                → BuyWeaponPanel 
                    → buyButton 클릭 → 구매 완료, buyWeaponButton 비활성화
            → refreshButton 클릭 → 1000골드 소모, 목록 재생성
```

### 5.3. 대장간 플로우
```
MorningView 
    → blacksmithButton 클릭 
        → BlacksmithPanel 
            → recipeButton 클릭 
                → RecipeInfoPanel 
                    → resultWeaponButton 클릭 → ResultWeaponInfoPanel
                    → materialButton 클릭 → MaterialInfoPanel
                    → craftButton 클릭 → 제작 시작
            → craftingWeaponButton 클릭 
                → (제작중) CraftingInfoPanel
                → (완료) 무기 획득
```

### 5.4. 이벤트 확인 플로우
```
MorningView 
    → eventCheckButton 클릭 
        → EventInfoPanel → 오늘의 이벤트 정보 표시
```

### 5.5. 무기 판매 플로우
InventoryView (Management 모드)
→ 무기 판매 버튼 클릭
→ UIManager.ShowSellWeaponPanel()
→ [판매] 버튼 클릭 → InventoryController.SellWeapon()
→ [취소] 버튼 클릭 → 팝업 닫기

---

## 6. 데이터 구조 확장

### 6.1. MaterialData 확장
```csharp
[System.Serializable]
public class MaterialData : ScriptableObject 
{
    public string id;
    public string materialName;
    public string description;
    public Grade grade;
    public Sprite icon;
    public List<string> availableDungeonIDs; // 새로 추가
}
```

### 6.2. GameConfig 추가 설정
```csharp
public class GameConfig : ScriptableObject
{
    // 기존 설정...
    public int weaponShopRefreshCost = 1000; // 상점 새로고침 비용
    public int uiPageSize = 8; // 한 페이지당 표시 아이템 수
    public int rentPaymentDay = 7; // 월세 납부 주기
    public int rentMultiplier = 100; // 일수당 월세 배율
}
```

---

## 7. 구현 시 고려사항

### 7.1. 상태 관리
- 구매한 무기, 제작중인 무기, 완료된 무기 상태를 PlayerData에 저장
- 새로고침 시 구매 상태 초기화 처리
- 대장간 패널에서 인벤토리 영역을 제작 현황으로 대체 사용

### 7.2. 월세 경고 시스템
- **정확한 타이밍**: 납부일 하루 전 아침 페이즈 시작 시에만 표시
- **조건 확인**: 현재 일수가 (7의 배수 - 1)인 경우
- **경고 메시지**: 명확하고 이해하기 쉬운 내용

### 7.3. 페이지네이션
- GameConfig.uiPageSize에 따라 동적으로 페이지 분할
- 페이지가 1개일 때 화살표 비활성화 처리

### 7.4. 비용 확인
- 구매/제작/새로고침 시 골드 부족 체크
- 재료 부족 시에도 제작 불가 안내