using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum IsAttackable
{
    Ready,
    Reloading
}


public class PlayerController : MonoBehaviour
{
    private const float ROTATION_HANDS_OFFSET = 0.53f;
    private const int HANDED_PISTOL = 0;
    private const int HANDED_RIFLE = 1;
    private const int HANDED_SHOTGUN = 2;

    [Header("Player Settings")]
    private SpriteRenderer spriteRenderer; // 플레이어 스프라이트 렌더러
    private bool isLeftHanded = false; // 플레이어가 무기를 어느 손에 들고 있는지 여부
    private Transform firePosition;
    private PlayerWeaponController weapon;
    public GameObject[] playerSkins;

    [Header("Movement Settings")]
    public float moveSpeed; // 이동 속도
    private Rigidbody2D rb; // 플레이어 리지드바디
    private Vector2 curMovementInput; // 현재 이동 입력 벡터 (W,A,S,D 입력)

    [Header("Look Settings")]
    public Vector3 mouseDelta; // 현재 마우스 커서의 위치값

    [Header("Combat Settings")]
    private Aiming aiming;
    public IsAttackable isAttackable;

    [Header("SystemControll")]
    private AnimationHandler animationHandler; // 애니메이션 핸들러
    private ParticleSystemHandler particleSystemHandler; // 파티클 시스템 핸들러

    [Header("PlayerPosition")]
    public Vector2 playerPosition; // 플레이어의 실시간 위치

    private Camera cam; // 마우스 커서의 좌표를 얻기 위한 카메라 참조

    private PlayerAmmo playerAmmo;
    private PlayerWeaponController weaponController;

    private void Awake()
    {
        playerPosition = Vector2.zero;
        animationHandler = GetComponent<AnimationHandler>();
        weapon = GetComponent<PlayerWeaponController>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        particleSystemHandler = GetComponent<ParticleSystemHandler>();

        playerAmmo = GetComponent<PlayerAmmo>();
        weaponController = GetComponent<PlayerWeaponController>();
    }

    private void Start()
    {
        isAttackable = IsAttackable.Ready;
        firePosition = this.transform.Find("PlayerObj/FirePosition");
        aiming = firePosition.GetComponent<Aiming>();
        // 자식 오브젝트의 스프라이트 렌더러 컴포넌트 가져오기
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.flipY = isLeftHanded;
        Cursor.visible = false;
    }
    private void Update()
    {
        IsChangeHand();
        // 매 프레임마다 플레이어의 위치 업데이트
        playerPosition = transform.GetChild(0).position;
        GetInput();
        PlayerRotate();
        HandleCombatInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    private void Move()
    {
        Vector2 move = transform.right * curMovementInput.x + transform.up * curMovementInput.y;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

        animationHandler.Move(curMovementInput * moveSpeed);
    }

    /// <summary>
    /// 공격 처리
    /// </summary>
    private void HandleCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
    {
            if (isAttackable == IsAttackable.Reloading) return;

            //탄약 시스템 연결 구간
            WeaponType currentWeapon = weaponController.currentWeapon;

            // 권총은 무한탄 
            if (currentWeapon != WeaponType.Pistol)
            {
                // 라이플/샷건은 탄약 필요
                if (!playerAmmo.ConsumeFromMag(currentWeapon.ToString()))
                {
                    Debug.Log("R 눌러서 재장전");
                    return;
                }
            }

            int damage = weapon.GetCurrentWeaponDamage();

            // ---- 기존 발사 처리 ----
            particleSystemHandler.FireEffectsOn();
            animationHandler.Shoot();
            aiming.Attack(damage);
        }

    /// <summary>
    /// 플레이어 회전
    /// </summary>
    private void PlayerRotate()
    {
        float spriteBaseOffset = -90f;
        Vector2 lookDir = ((Vector2)mouseDelta - playerPosition);

        if (lookDir.magnitude < 0.8f)
        {
            return;
        }

        float angle = Mathf.Atan2(lookDir.y , lookDir.x ) * Mathf.Rad2Deg;

        rb.rotation = angle + spriteBaseOffset;
    }

    /// <summary>
    /// 입력 처리
    /// </summary>
    private void GetInput()
    {
        curMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(cam.transform.position.z);

        mouseDelta = cam.ScreenToWorldPoint(mouseScreen);
    }

    private void IsChangeHand()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.flipY = isLeftHanded;
        firePosition.localPosition = new Vector3(firePosition.localPosition.x, isLeftHanded ? ROTATION_HANDS_OFFSET : - ROTATION_HANDS_OFFSET, firePosition.localPosition.z);
    }

    public void OnReloadKeyPressed()
    {
        if(isAttackable == IsAttackable.Reloading) return; // 이미 재장전 중이면 무시

        isAttackable = IsAttackable.Reloading; // 재장전 상태로 변경
        animationHandler.Reload(); // 재장전 애니메이션 설정
    }

    /// <summary>
    /// 손 바꾸기 커맨드 입력시 호출
    /// </summary>
    public void HandChangeButtonOnPress()
    {
        isLeftHanded = !isLeftHanded;
    }

    public void ReloadComplete()
    {

        WeaponType currentWeapon = weaponController.currentWeapon;

        if (currentWeapon != WeaponType.Pistol)
        {
            playerAmmo.Reload(currentWeapon.ToString());
        }

        Debug.Log("재장전 완료");
        isAttackable = IsAttackable.Ready;
    }

    public void SendNewSkin(WeaponType type)
    {
        switch(type)
        {
            case WeaponType.Pistol:
                ReplaceSkin(playerSkins[HANDED_PISTOL]);
                break;
            case WeaponType.Rifle:
                ReplaceSkin(playerSkins[HANDED_RIFLE]);
                break;
            case WeaponType.Shotgun:
                ReplaceSkin(playerSkins[HANDED_SHOTGUN]);
                break;
        }
    }

    private void ReplaceSkin(GameObject newSkinPrefab)
    {
        GameObject newSkin = Instantiate(newSkinPrefab, transform);
        newSkin.name = "PlayerObj";

        // 2) 참조를 새 PlayerObj 기준으로 먼저 갱신!
        RefreshPlayerChildReferences(newSkin.transform);

        // 3) 마지막에 기존 스킨 삭제
        Transform oldSkin = transform.Find("PlayerObj (old)"); // 이렇게 하면 안전
                                                               // 또는 Destroy 전에 이름을 바꿔두기

        // 만약 이름 변경이 번거로우면:
        foreach (Transform child in transform)
        {
            if (child != newSkin.transform)
                Destroy(child.gameObject);
        }
    }

    private void RefreshPlayerChildReferences(Transform newObj)
    {
        // Animator 확보
        Animator animator = newObj.GetComponent<Animator>();

        firePosition = newObj.Find("FirePosition");
        aiming = firePosition.GetComponent<Aiming>();
        aiming.SetFirePoint(firePosition);

        spriteRenderer = newObj.GetComponentInChildren<SpriteRenderer>();

        animationHandler.RefreshAnimator(animator);
        particleSystemHandler.EffectReset(firePosition);
    }

    public void OnPistolHanded()
    {
        weapon.HandleWeaponSelectInput(WeaponType.Pistol);
    }
    public void OnRifleHanded()
    {
        weapon.HandleWeaponSelectInput(WeaponType.Rifle);
    }
    public void OnShotgunHanded()
    {
        weapon.HandleWeaponSelectInput(WeaponType.Shotgun);
    }
}
