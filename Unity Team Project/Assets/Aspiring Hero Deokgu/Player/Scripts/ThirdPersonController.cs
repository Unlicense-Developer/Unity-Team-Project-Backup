using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* 참고: 애니메이션은 캐릭터와 캡슐 양쪽에 대해 컨트롤러를 통해 호출되며, 애니메이터 널 체크를 사용합니다.
 */

namespace PlayerController
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("플레이어")]
        [Tooltip("캐릭터의 이동 속도 (m/s)")]
        public float MoveSpeed = 2.0f;

        [Tooltip("캐릭터의 스프린트 속도 (m/s)")]
        public float SprintSpeed = 5.335f;

        [Tooltip("움직임 방향을 향해 회전하는 캐릭터의 회전 속도")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("가속도 및 감속도")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("플레이어가 점프할 수 있는 높이")]
        public float JumpHeight = 1.2f;

        [Tooltip("캐릭터는 자체 중력 값을 사용합니다. 엔진 기본값은 -9.81f입니다.")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("다시 점프할 수 있는 시간 간격. 즉시 다시 점프하려면 0f로 설정합니다.")]
        public float JumpTimeout = 0.50f;

        [Tooltip("낙하 상태로 진입하기 전에 지나야 할 시간 간격. 계단을 내려갈 때 유용합니다.")]
        public float FallTimeout = 0.15f;

        [Header("플레이어 땅 체크")]
        [Tooltip("캐릭터가 땅에 있는지 여부입니다. CharacterController 내장 땅 체크의 일부가 아님")]
        public bool Grounded = true;

        [Tooltip("울퉁불퉁한 지형에 유용합니다.")]
        public float GroundedOffset = -0.14f;

        [Tooltip("땅 체크의 반지름. CharacterController의 반지름과 일치해야 합니다.")]
        public float GroundedRadius = 0.28f;

        [Tooltip("캐릭터가 땅으로 사용하는 레이어")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("카메라가 따라갈 Cinemachine 가상 카메라에서 설정한 추적 대상")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("카메라를 위로 움직일 수 있는 최대 각도 (도 단위)")]
        public float TopClamp = 70.0f;

        [Tooltip("카메라를 아래로 움직일 수 있는 최대 각도 (도 단위)")]
        public float BottomClamp = -30.0f;

        [Tooltip("카메라 위치를 묶을 때 추가 각도 조정. 위치가 잠겼을 때 카메라 위치를 미세 조정하는 데 유용합니다.")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("카메라 위치를 모든 축에 대해 잠길 때")]
        public bool LockCameraPosition = false;

        // Cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // 플레이어
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // 타임아웃 델타타임
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // 애니메이션 ID
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;


        //낚시미니게임 연결을 위해 선언됨
        FishingZone_ ZoneManager;
        HitQTE Qte;
        MiniGame Minigame;
        PlayerFishingController FishingController;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private PlayerControllerInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        public bool _isMoving;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private void Awake()
        {
            // 주요 카메라에 대한 참조 가져오기
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerControllerInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
    Debug.LogError("Starter Assets 패키지에 필요한 종속성이 누락되었습니다. 도구/Starter Assets/종속성 다시 설치를 사용하여 해결하십시오.");
#endif

            AssignAnimationIDs();

            // 시작 시 타임아웃 초기화
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            //낚시를 위한 변수들
            FishingController = GetComponent<PlayerFishingController>();
            ZoneManager = GetComponent<FishingZone_>();
            Qte = GetComponent<HitQTE>();
            Minigame = GetComponent<MiniGame>();
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();

        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            
        }

        private void GroundedCheck()
        {
            // 구체 위치 설정, 오프셋 사용
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // 사용 중인 캐릭터 애니메이터 업데이트
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // 입력이 있고 카메라 위치가 고정되지 않은 경우
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                // 마우스 입력을 Time.deltaTime으로 곱하지 마십시오.
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // 회전 값을 360도로 제한
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine이 이 대상을 따라갈 것입니다.
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
         
            // 이동 속도를 이동 속도, 스프린트 속도 및 스프린트가 눌렸는지 여부에 따라 설정합니다.
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // 쉽게 제거, 대체 또는 반복할 수 있도록 설계된 간단한 가속 및 감속

            // 주의: Vector2의 == 연산자는 근사치를 사용하므로 부동 소수점 오류가 없으며 크기보다 저렴합니다.
            // 입력이 없는 경우 대상 속도를 0으로 설정합니다.
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // 플레이어의 현재 수평 속도에 대한 참조
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 대상 속도로 가속 또는 감속
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 더 유기적인 속도 변경을 제공하기 위해 곡선 모양의 결과를 생성합니다.
                // Lerp의 T는 클램프되므로 속도를 클램프할 필요가 없습니다.
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // 속도를 소수점 세 자리까지 반올림합니다.
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // 입력 방향을 정규화합니다.
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // 주의: Vector2의 != 연산자는 근사치를 사용하므로 부동 소수점 오류가 없으며 크기보다 저렴합니다.
            // 움직임 입력이 있으면 플레이어를 회전시킵니다.
            if (_input.move != Vector2.zero)
            {
                _isMoving = true;   //움직임 판별을 위한 bool변수
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // 카메라 위치에 상대적인 입력 방향을 향해 회전합니다.
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            else
            {
                _isMoving = false;  //움직임 판별을 위한 bool변수
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 플레이어 이동
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // 사용 중인 캐릭터 애니메이터 업데이트
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // 낙하 타임아웃 타이머 재설정
                _fallTimeoutDelta = FallTimeout;

                // 사용 중인 캐릭터 애니메이터 업데이트
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // 땅에 있는 동안 무한하게 속도가 떨어지지 않도록 멈춥니다.
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // 점프
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // H * -2 * G의 제곱근 = 원하는 높이에 도달하기 위해 필요한 속도
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // 사용 중인 캐릭터 애니메이터 업데이트
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // 점프 타임아웃
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // 점프 타임아웃 타이머 재설정
                _jumpTimeoutDelta = JumpTimeout;

                // 낙하 타임아웃
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // 사용 중인 캐릭터 애니메이터 업데이트
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // 땅에 서지 않으면 점프하지 않습니다.
                _input.jump = false;
            }

            // 터미널 아래에 있으면 시간에 따라 중력을 적용합니다 (두 번 deltaTime을 곱해 선형적으로 시간을 단축합니다).
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // 선택되었을 때, 땅 콜라이더의 위치와 일치하는 위치에 지점을 그리십시오.
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

}