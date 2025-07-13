# 낮(Day) UI 상세 설계 (Day UI Detailed Design)

**목적:** 낮 페이즈의 모든 UI 컴포넌트와 상호작용을 상세히 정의하여 구현 시 참고할 수 있도록 합니다.

---

## 1. DayView (메인 화면)

### 1.1. 화면 구성 (새로운 공통 레이아웃 적용)
```
┌─────────────────────────────────────┐
│            Day Phase UI             │ ← 페이즈 패널 영역 (3/5)
│        CustomerListPanel            │
│                                    │
│  [고객1] [고객2] [고객3] [고객4]     │
│  Customer Customer Customer Customer │
│  Button   Button   Button   Button  │
│                                    │
│           [Hero도감]                │
│         HeroMenuButton              │
├─────────────────────────────────────┤
│ 골드: 1,500G | 12일차 | 낮 | 02:30   │ ← StatusBar (중간)
├─────────────────────────────────────┤
│        InventoryView (하단)         │ ← 인벤토리 영역 (2/5)
│      무기 탭  |  재료 탭            │
└─────────────────────────────────────┘
```

### 1.2. 컴포넌트 정의
- **customerListPanel: Transform** (CustomerButton들의 컨테이너)
- **heroMenuButton: Button** (Hero 관련 메뉴 진입 버튼)

---

## 2. 고객 시스템

### 2.1. CustomerListPanel (고객 목록 영역)

#### 화면 구성
```
┌─────────────────────────────────────┐
│     < 1/2 > (페이지네이션)           │
├─────────────────────────────────────┤
│ [고객1] [고객2] [고객3] [고객4]      │
│ Lv.15   Lv.8    Lv.22   Lv.11       │
│ [아이콘] [아이콘] [아이콘] [아이콘]    │
└─────────────────────────────────────┘
```

#### 컴포넌트 정의
- **customerButtons: CustomerButton[]** (오늘 방문한 고객들)
- **pageNavigation: UI Component** (< 1/2 > 형태, 상단)

### 2.2. CustomerButton (고객 아이템)

#### 표시 정보
- **customerIcon: Sprite** (고객 아이콘)
- **level: int** (고객 레벨 표시)

#### UI 레이아웃
```
┌─────────────────────┐
│  [고객 아이콘]       │
│                    │
│    Lv. XX          │
└─────────────────────┘
```

#### 클릭 이벤트
- **customerButton 클릭** → CustomerInfoPanel 호출

### 2.3. CustomerInfoPanel (고객 상세 정보)

#### 화면 구성 (7:3 비율)
```
┌─────────────────────────────────────┐
│           고객 정보 영역             │  ← 7/10 영역
│                                    │
│ 고객명: XXX        등급: XXX        │
│ 레벨: XXX          속성: XXX        │
│                                    │
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
│      XXXXXXXXXXXXXXXXXXXXX         │
├─────────────────────────────────────┤
│        던전/무기 선택 영역           │  ← 3/10 영역
│                                    │
│   [던전아이콘]    [무기아이콘]       │
│   DungeonIcon     WeaponIcon        │
└─────────────────────────────────────┘
```

#### 표시 정보
**상단 영역 (고객 정보):**
- **customerName: string** (고객 이름)
- **description: string** (고객 설명)
- **level: int** (고객 레벨)
- **grade: Grade** (고객 등급)
- **element: Element** (고객 속성)

**하단 영역 (선택 인터페이스):**
- **dungeonIcon: Sprite** (던전 아이콘, 클릭 가능)
- **weaponIcon: Sprite** (무기 아이콘, 초기엔 빈 아이콘, 클릭 가능)

#### 클릭 이벤트
- **dungeonIcon 클릭** → CustomerDungeonInfoPanel 호출
- **weaponIcon 클릭** → 인벤토리 패널 최상위 레이어로 이동, 무기 선택 모드

### 2.4. CustomerDungeonInfoPanel (고객의 던전 정보)

#### 표시 정보
- **dungeonName: string** (던전 이름)
- **description: string** (던전 설명)
- **level: int** (던전 레벨)
- **grade: Grade** (던전 등급)
- **element: Element** (던전 속성)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           던전 정보                  │
├─────────────────────────────────────┤
│ 던전명: XXX                        │
│ 등급: XXX                          │
│ 레벨: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

### 2.5. 무기 선택 인터페이스

#### 인벤토리 활성화
- **weaponIcon 클릭** 시 인벤토리 패널이 최상위 레이어로 이동
- **다른 UI 비활성화**: 배경 어두워짐, 다른 패널 클릭 불가
- **모든 무기 선택 가능**: 별도 하이라이트 없음

#### 무기 선택 → RentWeaponPanel

##### 표시 정보
- **weaponName: string** (무기 이름)
- **description: string** (무기 설명)
- **grade: Grade** (무기 등급)
- **element: Element** (무기 속성)
- **confirmButton: Button** (대여 확인)
- **cancelButton: Button** (대여 취소)

##### UI 레이아웃
```
┌─────────────────────────────────────┐
│           무기 대여                  │
├─────────────────────────────────────┤
│ 무기명: XXX                        │
│ 등급: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│        [확인]      [취소]           │ ← 각 패널에서 직접 처리 (AlertPopup 대신)
└─────────────────────────────────────┘
```

##### 확인 후 처리
- **CustomerInfoPanel의 weaponIcon**에 선택한 무기 아이콘 표시
- **모험 예약 상태**: 밤 페이즈 전환 시 모험 시작
- **무기 교체 가능**: weaponIcon 재클릭으로 다른 무기 선택 가능

---

## 3. Hero 시스템

### 3.1. HeroMenuButton 클릭 → HeroMenuPanel

#### 화면 구성
```
┌─────────────────────────────────────┐
│           Hero 메뉴                 │
├─────────────────────────────────────┤
│                                    │
│         [Hero 도감]                 │
│       HeroCollection                │
│          Button                     │
│                                    │
│         [Hero 모험]                 │
│       HeroAdventure                 │
│          Button                     │
│                                    │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

#### 컴포넌트 정의
- **heroCollectionButton: Button** (Hero 도감 버튼)
- **heroAdventureButton: Button** (Hero 모험 보내기 버튼)
- **closeButton: Button**

### 3.2. HeroCollectionPanel (Hero 도감)

#### 화면 구성
```
┌─────────────────────────────────────┐
│           Hero 도감                 │
├─────────────────────────────────────┤
│     < 1/3 > (페이지네이션)           │
├─────────────────────────────────────┤
│ [그림자] [Hero1] [그림자] [Hero2]    │
│ 미획득   Lv.15   미획득   Lv.22     │
│ [?아이콘] [실제]  [?아이콘] [실제]    │
└─────────────────────────────────────┘
```

#### 표시 방식
- **미획득 Hero**: 그림자 아이콘, 클릭 불가
- **획득 Hero**: 실제 아이콘, 레벨 표시, 클릭 가능

#### 클릭 이벤트
- **획득한 Hero 클릭** → HeroCollectionInfoPanel 호출

### 3.3. HeroCollectionInfoPanel (Hero 상세 정보)

#### 표시 정보
- **customerName: string** (Hero 이름)
- **description: string** (Hero 설명)
- **grade: Grade** (Hero 등급)
- **element: Element** (Hero 속성)
- **level : int** (Hero 레벨)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           Hero 정보                 │
├─────────────────────────────────────┤
│ [Hero 아이콘]                      │
│                                    │
│ Hero명: XXX                        │
│ 현재 레벨: XX (획득시: XX)          │
│ 등급: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│ 특수능력: (TODO - 추후 구현)        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

### 3.4. HeroListPanel (Hero 모험 보내기)

#### 구조
**※ CustomerListPanel과 완전히 동일한 구조**
- **HeroButton** (CustomerButton과 동일)
- **HeroInfoPanel** (CustomerInfoPanel과 동일)
- **HeroDungeonInfoPanel** (CustomerDungeonInfoPanel과 동일)
- **무기 선택 시스템** (동일)

#### 차이점
- **보유 Hero만 표시**: 획득한 Hero들만 목록에 표시
- **부상 상태 표시**: 실패 후 10일간 비활성화된 Hero는 회색 처리, 클릭 불가
- **부상 Hero 표시**: "부상 회복까지 X일 남음" 텍스트 표시

#### 부상 시스템
```
부상 상태 Hero 표시:
┌─────────────────────┐
│  [Hero 아이콘] [X]   │ ← 회색 처리 + X 표시
│   치료비: 1,250골드   │ ← 치료비 표시 추가
│                    │
└─────────────────────┘

클릭 시 → HeroTreatmentPanel 호출
```

### 3.5. HeroTreatmentPanel (Hero 치료 확인 팝업)

#### 표시 정보
- **heroName: string** (Hero 이름)
- **injuryInfo: string** (부상 정보)
- **treatmentCost: int** (치료비)
- **treatmentDays: int** (예상 치료 기간)
- **treatButton: Button** (치료 확인)
- **cancelButton: Button** (치료 취소)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│             Hero 치료               │
├─────────────────────────────────────┤
│ Hero: 용감한 기사                   │
│ 부상 상태: 중상                     │
│                                    │
│ 치료비: 1,250골드                   │
│ 예상 치료 기간: 8일                 │
│                                    │
│ 치료하시겠습니까?                   │
├─────────────────────────────────────┤
│        [치료]      [취소]           │
└─────────────────────────────────────┘
```

#### 클릭 이벤트
- **treatButton 클릭** → 골드 지불, 치료 시작
- **cancelButton 클릭** → 팝업 닫기, 부상 상태 유지

---

## 4. Hero 전환 시스템

### 4.1. Hero 잠금해제 시스템
**잠금해제 조건**:
- Common Hero: 10일차부터 사용 가능
- Uncommon Hero: 20일차부터 사용 가능  
- Rare Hero: 30일차부터 사용 가능
- Epic Hero: 40일차부터 사용 가능
- Legendary Hero: 50일차부터 사용 가능

**잠금된 Hero 표시**:
잠금 상태 Hero 표시:
┌─────────────────────┐
│  [Hero 아이콘]       │ ← 회색 처리
│    (잠금됨)         │
│ X일차에 해금됩니다    │
└─────────────────────┘

### 4.2. Hero 전환 시 처리
- **Customer 풀에서 제거**: 더 이상 일반 고객으로 등장하지 않음
- **Hero 풀에 추가**: HeroListPanel에서 사용 가능
- **도감 업데이트**: HeroCollectionPanel에서 그림자 → 실제 아이콘으로 변경

### 4.3. Hero 전용 능력

#### 성공률 계산 시 등급보정 (Hero 전용)
```
Hero 고객등급보정 (고객등급 - 던전등급):
+4등급차: ×3.0    |  -4등급차: ×0.4
+3등급차: ×2.0    |  -3등급차: ×0.7  
+2등급차: ×1.4    |  -2등급차: ×0.9
+1등급차: ×1.2    |  -1등급차: ×1.0
 0등급차: ×1.0
```

#### 무기 손실 확률 (Hero 전용)
```
Hero 무기 회수 확률 (등급별):
Common: 40% 회수 (+10%)    |  60% 손실 (-10%)
Uncommon: 35% 회수 (+10%)  |  65% 손실 (-10%)  
Rare: 30% 회수 (+10%)      |  70% 손실 (-10%)
Epic: 25% 회수 (+10%)      |  75% 손실 (-10%)
Legendary: 20% 회수 (+10%) |  80% 손실 (-10%)
```

#### 실패 패널티
- **일반 고객**: 실패 시 사라짐 (사망)
- **Hero**: 실패 시 10일간 모험 불가 (부상 상태)

---

## 5. UI 상호작용 플로우

### 5.1. 고객 모험 예약 플로우
```
DayView 
    → customerButton 클릭 
        → CustomerInfoPanel 
            → dungeonIcon 클릭 → CustomerDungeonInfoPanel (정보 확인)
            → weaponIcon 클릭 → 인벤토리 활성화 
                → 무기 선택 → RentWeaponPanel 
                    → 확인 → 모험 예약 완료
```

### 5.2. Hero 도감 확인 플로우
```
DayView 
    → heroMenuButton 클릭 
        → HeroMenuPanel 
            → heroCollectionButton 클릭 
                → HeroCollectionPanel 
                    → 획득한 Hero 클릭 → HeroCollectionInfoPanel
```

### 5.3. Hero 모험 보내기 플로우
```
DayView 
    → heroMenuButton 클릭 
        → HeroMenuPanel 
            → heroAdventureButton 클릭 
                → HeroListPanel (CustomerListPanel과 동일한 플로우)
```

---

## 6. 구현 시 고려사항

### 6.1. 상태 관리
- **모험 예약 상태**: 무기 장착된 고객들을 별도 리스트로 관리
- **Hero 상태**: 활성/부상 상태, 부상 회복 날짜 관리
- **도감 상태**: 획득한 Hero 목록 저장

### 6.2. 데이터 동기화
- **고객 → Hero 전환**: 모험 성공 시 확률 계산 및 전환 처리
- **Hero 풀 관리**: Customer 풀에서 제거, Hero 풀에 추가

### 6.3. UI 레이어 관리
- **인벤토리 최상위**: 무기 선택 시 다른 UI 비활성화
- **팝업 계층**: 여러 정보 패널들의 계층 관리 (UIManager를 통한 AlertPopup, LoadingPopup 관리)
- **확인/선택 처리**: 각 패널 내에서 직접 확인/취소 버튼으로 처리

### 6.4. 페이지네이션
- **고객/Hero 목록**: GameConfig.uiPageSize에 따라 페이지 분할
- **도감**: 전체 Customer 풀 크기에 따른 페이지 관리