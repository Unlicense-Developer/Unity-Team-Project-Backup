using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartDelivery
{
    public class WheelController : MonoBehaviour
    {
        //바퀴 콜라이더
        [SerializeField] WheelCollider frontLeftColl; //앞_왼쪽 바퀴
        [SerializeField] WheelCollider frontRightColl; //앞_오른쪽 바퀴
        [SerializeField] WheelCollider backLeftColl; //뒤_왼쪽 바퀴
        [SerializeField] WheelCollider backRightColl; //뒤_오른쪽 바퀴


        [SerializeField] Transform frontLeftTransform; //앞_왼쪽 바퀴 위치값
        [SerializeField] Transform frontRightTransform; //앞_오른쪽 바퀴 위치값
        [SerializeField] Transform backLeftTransform; //뒤_왼쪽 바퀴 위치값
        [SerializeField] Transform backRightTransform; //뒤_오른쪽 바퀴 위치값


        public float acceleration = 500f; //가속도
        public float breakingForce = 300f; //브레이크강도
        public float maxTurnAngle = 30f; //회전각도
        float prevSteerAngle;

        float nowAcceleration = 0f; //현재 가속도
        float nowBreakForce = 0f; //현재 브레이크강도
        float nowTurnAngle = 0f; //현재 회전각도 

        void Start()
        {

        }

        private void FixedUpdate()
        {
            //ForwardMove();//이동 
            Turn();//회전
            //Break();//브레이크
            UpdateWheel(frontLeftColl, frontLeftTransform);
            UpdateWheel(frontRightColl, frontRightTransform);
            UpdateWheel(backLeftColl, backLeftTransform);
            UpdateWheel(backRightColl, backRightTransform);
        }

        // void ForwardMove()//앞뒤 이동
        // {
        //     nowAcceleration = acceleration * Input.GetAxis("Vertical");
        //     frontLeftColl.motorTorque = nowAcceleration;
        //     frontRightColl.motorTorque = nowAcceleration;
        // }

        void Turn()//좌우 방향
        {
            nowTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
            frontLeftColl.steerAngle = nowTurnAngle;
            frontRightColl.steerAngle = nowTurnAngle;
        }
        // void Break()//멈춤
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //         nowBreakForce = breakingForce; //브레이크 강도 전달
        //     else
        //         nowBreakForce = 0f;
        //     frontLeftColl.brakeTorque = nowBreakForce;
        //     frontRightColl.brakeTorque = nowBreakForce;
        //     backLeftColl.brakeTorque = nowBreakForce;
        //     backRightColl.brakeTorque = nowBreakForce;
        // }

        void UpdateWheel(WheelCollider col, Transform trans)
        {
            //바퀴 콜라이더 상태값 가져오기
            Vector3 position;
            Quaternion rotation;
            col.GetWorldPose(out position, out rotation);

            //바퀴 위치값 내보내기
            trans.position = position;
            trans.rotation = rotation;

        }
        void TurnOn()
        {
            // leftTransform.Rotate(Vector3.up, leftColl.steerAngle - prevSteerAngle, Space.World);
            // rightTransform.Rotate(Vector3.up, rightColl.steerAngle - prevSteerAngle, Space.World);
            // prevSteerAngle = rightColl.steerAngle;

        }

        void Update()
        {
            //TurnOn();
        }





    }
}