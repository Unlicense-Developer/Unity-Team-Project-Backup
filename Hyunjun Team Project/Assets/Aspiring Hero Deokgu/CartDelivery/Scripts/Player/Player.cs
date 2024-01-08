using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CartDelivery
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        Transform horseCartTransform; //레이 위치(말과 마차사이)
        public Transform cameraTransform; //카메라 위치
        float speed, inputH, inputV;
        public float moveSpeed = 5f;//말 이동속도
        public float runSpeed = 1.5f;//말 달리는속도
        public float jumpForce = 15f;//말 점프높이
        public bool isJumping;
        public LayerMask floor;
        public float groundHeight = 1f;
        [SerializeField]
        Rigidbody horseRigd; //말 리지드바디

        [SerializeField]
        Animator horseAni;//말 애니
        //애니
        void Start()
        {
            AniDefult();//말 기본애니(Idle)
            speed = moveSpeed;
            isJumping = false;
        }
        void Move()
        {
            //이동값
            float inputH = Input.GetAxisRaw("Horizontal"); //A,D키 누르면 (왼,오)
            float inputV = Input.GetAxisRaw("Vertical"); //W,S키 누르면 (앞,뒤)

            Vector3 velocity = new Vector3(inputH, 0, inputV);

            velocity = cameraTransform.TransformDirection(velocity);

            velocity *= moveSpeed;


            #region 땅에서 떨어지는 속도
            //떨어지는 속도
            float fallSpeed = horseRigd.velocity.y;

            velocity.y = fallSpeed; //떨어지는속도 초기화
            horseRigd.velocity = velocity;
            #endregion

            #region A,S,D,F 입력에 애니상태
            if (inputV >= 1) //1(앞)
            {
                AniFrontWalk();
            }
            else if (inputV < 0)//-1(뒤)
            {
                AniBackWalk();
            }
            else if (inputH < 0) //왼(-1)
            {
                LeftRotation();
                AniLeftWalk();
            }
            else if (inputH >= 1) //오(1)
            {
                RightRotation();
                AniRightWalk();
            }
            else if (inputV == 0 || inputH == 0)
            {
                AniDefult(); //기본(0)
                             //SoundManager.instance.StopSoundHorseWalk();

            }
            #endregion
        }
        void RightRotation()//오른쪽 회전
        {
            inputH = inputH * speed * Time.deltaTime;
            this.transform.Rotate(Vector3.up * inputH);
        }
        void LeftRotation()//왼쪽 회전
        {
            inputH = inputH * speed * Time.deltaTime;
            this.transform.Rotate(Vector3.up * -inputH);
        }
        void SpeedUp()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))//Shift키+W키(앞으로) 누르면
            {
                Vector3 pos = new Vector3(0, 0, 1);
                pos = cameraTransform.TransformDirection(pos);
                transform.position = transform.position + pos * (runSpeed * Time.deltaTime);//이동속도 높이기
                AniRun();
            }
        }


        #region 말 애니 상태 종류
        void AniBackWalk()
        {
            horseAni.SetInteger("Motion", -1);//BackwardWalk // 뒤_걷기


        }
        void AniDefult()
        {
            horseAni.SetInteger("Motion", 0);//Idle // 기본동작


        }
        void AniFrontWalk()
        {
            horseAni.SetInteger("Motion", 1);//ForwardWalk // 앞_걷기
        }
        void AniRun()
        {
            horseAni.SetInteger("Motion", 2);//Run // 뛰기

        }
        void AniLeftWalk()
        {
            horseAni.SetInteger("Motion", 3);//LeftWalk // 왼_걷기


        }
        void AniRightWalk()
        {
            horseAni.SetInteger("Motion", 4);//RightWalk //오_걷기


        }
        void AniJump()
        {
            horseAni.SetInteger("Motion", 5);//Jump // 점프

        }
        #endregion
        void Update()
        {
            Move();
            SpeedUp();
        }

        void OnWalkSound()
        {
            SoundManager.instance.PlaySoundHorseWalk();

        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("River"))//강에 닿았을때 게임오버
                UIManager.instance.OnGameOver();

            else if (other.gameObject.CompareTag("GameClearZone"))//게임클리어존에 갔을때 게임클리어
                UIManager.instance.OnGameClear();

        }



    }











}

