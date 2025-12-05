# 🎮 Top-Down Stealth FPS Prototype

> Unity 2022.3.62f2 기반 2D 탑다운 시점 PvE 슈팅 게임  
> 실제 게임 재미 요소 검증을 위한 프로토타입(Prototype)

---

## 📌 프로젝트 소개

플레이어는 시야가 닿는 곳만 밝아지는 어두운 환경에서  
다양한 적을 상대하며 진행하는 **스텔스 기반 시점 FPS 슈팅 게임**입니다.

---

## 🕹️ 핵심 게임 특징

| 기능 | 설명 |
|------|------|
| 🔦 플레이어 시야 라이트 기반 시야 제한 | 실시간 Occlusion 적용 |
| 👀 Enemy 인식 시스템 | 시야각 + 거리 + 장애물 체크 |
| 🔫 다양한 무기 제공 | 권총 / 라이플 / 샷건 |
| 🧠 Enemy AI FSM | Patrol → 탐색 → 공격 |
| 🚪 상호작용 오브젝트 | 문 열기 / 커버 밀기 / 탄약 상자 |
| 🎯 Sniper 특수공격 | 조준(레이저) 후 발사 |

---

## ⚔️ Enemy 시스템 구조

**적 유형은 ScriptableObject 기반으로 확장 가능**

| Enemy Type | 행동 방식 | 공격 타입 |
|------------|----------|----------|
| Infantry | 순찰 및 추격 | 즉시 사격 |
| Assault | 고속 접근 | 근거리 우선 공격 |
| Sniper | 조준 → 레이저 → 발사 | 고데미지 단발 |

✔ EnemyData ScriptableObject 예시

```
public EnemyType enemyType;
public float maxHP;
public float moveSpeed;
public float attackDamage;
public float viewAngle;
public float detectRange;
public LayerMask obstacleMask;

// Sniper 전용
public bool isSniper;
public float aimTime;
public Color laserColor;
```

## 🔫 무기 시스템
| 무기 | 탄창 | 예비탄 | 특성 |
|------|------|------|------|
| Pistol | 무제한 |	X |	기본 시작 무기 |
| Rifle | 30 | 90 |	중거리 지속 사격 |
| Shotgun | 6 | 24 | 근거리 고화력 |

탄약 시스템 완료

Reload 애니메이션 + 사운드 적용

Raycast 기반 피격 판정


## 🧱 오브젝트 상호작용
| 오브젝트 | 기능 설명 |
|------|------|
| Door | E키로 열고 닫기 | 빛/시야 차단 |
| Pushable Cover | 플레이어가 밀 수 있는 엄폐물 |
| Supply Box | 탄약 보급(재사용 가능 옵션 포함) |
| Gun Pickup | 무기 해금 |

## 🌒 조명 및 시야 시스템
| 요소 | 적용 설명 |
|------|------|
| Light2D Spot | 플레이어 중심 시야 |
| ShadowCaster2D | 벽/문이 시야 차단 |
| Raycast | Enemy 인식 판정 |
| Occlusion | 적이 보이는 순간 적 조명 활성화 |

## 🔧 기술 스택
| 항목 | 내용 |
|------|------|
| Engine | Unity 2022.3.62f2 (URP) |
| Physics |	Rigidbody2D, Collider2D |
| UI | HP Bar, Ammo UI, 시작 UI |
| Audio | 발사/재장전/피격 효과음 |
| Code Style | ScriptableObject + Manager 구조 |

## 👥 팀 구성
| 이름 | 역할 |
|------|------|
| 김경찬 (팀장) | Enemy |
| 오민근 | Player, 사운드 |
| 김문경 | 오브젝트 & 총기 |
| 백성현 | 로비 UI & 메인 UI |
| 장준혁 | 기획 & 맵 |

## 📌 현재 개발 상태
🚧 Prototype – 기능 검증 단계
추후 콘텐츠 확장/연출 강화 예정