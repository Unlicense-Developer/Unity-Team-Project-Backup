using System.Collections;
using UnityEngine;


namespace Alchemy
{
    public class Gem : MonoBehaviour
    {
        public GameManager manager; // 게임 매니저 참조
        public ParticleSystem effect; // 이펙트 시스템 참조
        public int level; // gem의 현재 레벨
        public bool isDrag; // 드래그 중인지 여부
        public bool isMerge; // 합쳐지는 중인지 여부
        public bool isAttach; // 붙어있는 상태인지 여부
        public bool isFirstTimeOnTurntable; // 원판에 최초로 진입했는지 여부
        public bool isOnTurntable; // 원판 위에 있는지 여부를 나타내는 변수

        public string gravityCenterName;
        public Transform gravityCenter; // 중력의 중심점
        public float gravityIntensity = 9.8f; // 중력 강도
        public float creationTime; // 보석 생성 시간을 기록하는 변수

        public float rotationSpeed = -30.0f; // 회전 속도 설정
        public float baseMass = 1.0f; // 기본 질량 값
        public float massIncreasePerLevel = 2.0f; // 레벨 당 질량 증가량
        public float baseAngularDrag = 0.05f; // 기본 각속도 저항 값
        public float angularDragIncreasePerLevel = 0.05f; // 레벨 당 각속도 저항 증가량

        public Rigidbody2D rigid; // 물리 엔진 컴포넌트
        CircleCollider2D circleCollider; // 원형 콜라이더 컴포넌트
        Animator anim; // 애니메이터 컴포넌트
        SpriteRenderer spriteRenderer; // 스프라이트 렌더러 컴포넌트
        // ParticleSystem particleSystem; // 파티클 시스템 컴포넌트

        float deadTime; // 게임 오버 판정 시간

        public void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            // particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            anim.SetInteger("Level", level); // gem 활성화 시 레벨 설정
            UpdateRigidbodyProperties();
            RemoveCurrentParticleEffect(); // 기존 파티클 이펙트 제거
            EffectPlay(); // 이펙트 재생
            ActivateParticleEffect(level); // 새 레벨에 맞는 파티클 이펙트 생성
            creationTime = Time.time; // 보석이 생성된 현재 시간을 기록
        }

        private void OnDisable()
        {   // gem 비활성화 시 속성 초기화
            level = 0;
            isDrag = false;
            isMerge = false;
            isAttach = false;
            isFirstTimeOnTurntable = false;
            isOnTurntable = false;
            creationTime = 0f;
            // gem 트랜스폼 초기화
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;
            // gem 물리 초기화
            rigid.simulated = false;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = 0;
            rigid.gravityScale = 1.0f;
            circleCollider.enabled = true;
        }

        public void Drag()
        {
            isDrag = true; // 드래그 시작
        }

        public void Drop()
        {
            isDrag = false; // 드래그 종료
            rigid.simulated = true; // 물리 효과 활성화
            rigid.gravityScale = 1.0f;
        }

        // 매 프레임마다 호출되는 업데이트 함수
        void Update()
        {
            if (isDrag)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float leftBorder = -5.0f + transform.localScale.x / 2f;
                float rightBorder = 5.0f - transform.localScale.x / 2f;

                // 화면 밖으로 나가지 않도록 제한
                if (mousePos.x < leftBorder)
                {
                    mousePos.x = leftBorder;
                }
                else if (mousePos.x > rightBorder)
                {
                    mousePos.x = rightBorder;
                }

                mousePos.y = 8;
                mousePos.z = 0;
                transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f); // 마우스 위치로 이동
            }
            else
            {
                // 원판 내에서 보석 회전 로직
                RotateAroundCenter();
                GameOverCheck();
            }
        }

        void RotateAroundCenter()
        {
            if (isFirstTimeOnTurntable && isOnTurntable)
            {
                Vector3 centerPoint = new Vector3(0, -1.8f, 0); // 원판의 중심점 설정
                transform.RotateAround(centerPoint, Vector3.forward, rotationSpeed * Time.deltaTime);

                Vector2 directionToCenter = (gravityCenter.position - transform.position).normalized; // 중심점으로 향하는 방향 벡터
                float distanceToCenter = Vector2.Distance(gravityCenter.position, transform.position); // 중심점까지의 거리

                // 거리에 따른 중력 강도 조절 (거리가 멀어질수록 강도 증가)
                float gravityForce = gravityIntensity + distanceToCenter; // 예시: 거리에 따라 중력 강도를 증가시킴

                // 중력 힘 적용
                rigid.AddForce(directionToCenter * gravityForce);

                // 속도 감소 로직 조정 (감소 효과를 줄여서 보석이 더 오래 동안 빠르게 움직이도록 함)
                float speedReduction = distanceToCenter * 0.05f; // 속도 감소 비율 감소
                rigid.velocity -= rigid.velocity * speedReduction * Time.fixedDeltaTime; // 현재 속도에서 일정 비율 감소
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Turntable") // 원판 레이어의 태그를 확인
            {
                isOnTurntable = true; // 원판 위에 있다고 표시
                deadTime = 0f;
                spriteRenderer.color = Color.white; // 원래 색상으로 복귀

                if (!isFirstTimeOnTurntable)
                {
                    isFirstTimeOnTurntable = true; // 최초 진입 여부 설정
                    rigid.gravityScale = 0.0f;
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Turntable")
            {
                isOnTurntable = false; // 원판에서 벗어났다고 표시
            }
        }

        void GameOverCheck()
        {
            // 보석이 생성된 후 0.3초가 지났는지 확인
            if (Time.time - creationTime < 0.3f) return;

            // 원판의 중심과 반지름
            Vector2 turntableCenter = manager.turntable.GetComponent<CircleCollider2D>().bounds.center;
            float turntableRadius = manager.turntable.GetComponent<CircleCollider2D>().bounds.extents.x; // 반지름은 bounds의 extents의 x 값

            // 보석의 중심과 반지름
            Vector2 gemCenter = GetComponent<CircleCollider2D>().bounds.center;
            float gemRadius = GetComponent<CircleCollider2D>().bounds.extents.x;

            // 원판 중심점과 보석 중심점 사이의 거리
            float distanceToCenter = Vector2.Distance(turntableCenter, gemCenter);

            // 원판에 들어온 보석만 체크
            if (isFirstTimeOnTurntable)
                // 보석이 원판 밖으로 나갔는지 확인
                if (distanceToCenter > turntableRadius + gemRadius)
                {
                    // 보석이 원판 밖으로 나감
                    deadTime += Time.deltaTime;
                }
                else
                {
                    // 보석이 원판 안에 있음
                    deadTime = 0f;
                    spriteRenderer.color = Color.white;
                }

            // 사망 시간이 초과되었는지 확인
            if (deadTime > 0.5f)
            {
                spriteRenderer.color = Color.red;
            }

            if (deadTime > 2.0f)
            {
                manager.GameOver();
                return;
            }
        }

        public void Hide(Vector3 targetPos)
        {
            isMerge = true;

            rigid.simulated = false;
            circleCollider.enabled = false;

            if (targetPos == Vector3.up * 100)
            {
                EffectPlay(); // 이펙트 재생
            }

            StartCoroutine(HideRoutine(targetPos)); // 숨김 처리 코루틴
        }

        IEnumerator HideRoutine(Vector3 targetPos)
        {
            int frameCount = 0;

            while (frameCount < 20)
            {
                frameCount++;
                if (targetPos != Vector3.up * 100)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
                }
                else if (targetPos == Vector3.up * 100)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f); // 점점 사라지는 효과
                }

                yield return null;
            }

            manager.score += (int)Mathf.Pow(2, level); // 점수 증가

            isMerge = false;
            gameObject.SetActive(false); // 보석 비활성화
        }

        void LevelUp()
        {
            isMerge = true;

            RemoveCurrentParticleEffect();

            rigid.velocity = Vector2.zero;
            rigid.angularVelocity = 0.0f;

            StartCoroutine(LevelUpRoutine()); // 레벨업 처리 코루틴
        }

        void RemoveCurrentParticleEffect()
        {
            // 현재 활성화된 파티클 이펙트 찾기
            ParticleSystem currentEffect = GetComponentInChildren<ParticleSystem>();

            // 파티클 이펙트가 있다면 제거
            if (currentEffect != null)
            {
                Destroy(currentEffect.gameObject);
            }
        }

        IEnumerator LevelUpRoutine()
        {
            yield return new WaitForSeconds(0.1f);

            if (anim != null)
            {
                anim.SetInteger("Level", level + 1); // 애니메이션 레벨 업데이트
                EffectPlay(); // 이펙트 재생
                manager.SfxPlay(GameManager.Sfx.LevelUp); // 레벨업 사운드 효과 재생
                ActivateParticleEffect(level + 1); // 새 레벨에 맞는 파티클 이펙트 생성
            }

            yield return new WaitForSeconds(0.1f);
            level++; // 레벨 증가
            UpdateRigidbodyProperties(); // Rigidbody 속성 업데이트

            if (manager.maxLevel <= 3)
            {
                manager.maxLevel = Mathf.Max(level, manager.maxLevel); // 최대 레벨 업데이트
            }

            isMerge = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Gem")
            {   // 다른 보석과 충돌 시 합치기 로직
                Gem other = collision.gameObject.GetComponent<Gem>();

                // 동일 레벨의 보석과만 합치기
                if (level == other.level && !isMerge && !other.isMerge && level < 7)
                {   // 나와 상대 위치 비교
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;
                    // 내가 아래에 있거나 같은 높이에서 오른쪽에 있을 때
                    if (meY < otherY || (meY == otherY && meX > otherX))
                    {   // 상대는 숨기고, 나는 레벨업
                        other.Hide(transform.position);
                        LevelUp();
                    }
                }
                else if (level == other.level && !isMerge && !other.isMerge && level == 7)
                {
                    Win();
                }

                // 레벨 차이를 기반으로 반발력 계산
                int levelDifference = Mathf.Abs(level - other.level);
                float bounceForce = CalculateBounceForce(levelDifference);

                // 충돌 방향 계산
                Vector2 collisionDirection = collision.transform.position - transform.position;
                collisionDirection.Normalize();

                // 반발력 적용
                rigid.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);
                other.rigid.AddForce(collisionDirection * bounceForce, ForceMode2D.Impulse);
            }

            StartCoroutine(AttachRoutine()); // 부착 처리 코루틴
        }

        float CalculateBounceForce(int levelDifference)
        {
            // 레벨 차이에 따른 기본 반발력
            float baseBounce = 4.0f;

            // 레벨 차이가 클수록 반발력 증가
            return baseBounce + levelDifference * 4.0f;
        }

        void UpdateRigidbodyProperties()
        {
            // 레벨에 따른 mass와 angularDrag 값 계산
            rigid.mass = baseMass + level * massIncreasePerLevel; // baseMass는 기본 mass 값, massIncreasePerLevel은 레벨 당 증가하는 mass 값
            rigid.angularDrag = baseAngularDrag + level * angularDragIncreasePerLevel; // baseAngularDrag는 기본 angularDrag 값, angularDragIncreasePerLevel은 레벨 당 증가하는 angularDrag 값
        }

        IEnumerator AttachRoutine()
        {
            if (isAttach)
            {
                yield break;
            }

            isAttach = true;
            // manager.SfxPlay(GameManager.Sfx.Attach); // 부착 사운드 효과 재생

            yield return new WaitForSeconds(1.0f);

            isAttach = false;
        }

        void EffectPlay()
        {
            effect.transform.position = transform.position; // 이펙트 위치 설정
            // effect.transform.localScale = transform.localScale; // 이펙트 크기 설정
            effect.Play(); // 이펙트 재생
        }

        void ActivateParticleEffect(int newLevel)
        {
            // 기존 파티클 이펙트 제거
            RemoveCurrentParticleEffect();

            // 새 레벨에 맞는 파티클 이펙트 생성
            GameObject particleEffect = Instantiate(manager.particlePrefabs[newLevel], transform.position, Quaternion.identity);
            particleEffect.transform.SetParent(transform);

            // 보석의 스케일 애니메이션이 완료된 후 파티클 이펙트의 스케일을 조정
            StartCoroutine(SetParticleEffectScale(particleEffect));
        }

        IEnumerator SetParticleEffectScale(GameObject particleEffect)
        {
            // 애니메이션 'Level'의 지속 시간 동안 대기 (0.3초 + 추가 지연)
            yield return new WaitForSeconds(0.3f + 0.05f);

            // 파티클 이펙트가 여전히 존재하는지 확인
            if (particleEffect != null)
            {
                // 4레벨 이하의 경우 기본 스케일, 5레벨 이상의 경우 스케일을 줄임
                float scaleMultiplier = level <= 4 ? 1.0f : 0.75f; // 5레벨 이상의 경우 스케일을 0.75배로 조정
                particleEffect.transform.localScale = transform.localScale * scaleMultiplier;
            }
        }

        void Win()
        {
            Debug.Log("승리!");
        }

        void FixedUpdate()
        {
        }
    }
}

