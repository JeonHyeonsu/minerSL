using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick joy;
    public float speed;

    Rigidbody2D rigid;
    Animator anim;
    Vector2 moveVec;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 1. 조이스틱 입력값
        float x = joy.Horizontal;
        float y = joy.Vertical;

        // 2. 실제 이동
        moveVec = new Vector2(x, y) * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);

    }

    void LateUpdate()
    {
        anim.SetBool("isMove", true);
    }
}





    /*
    //##점프 상태##
    private void IsGround()  // 착지확인 함수 = isGround의 true false를 가려줌
    {

        isGround = Physics.Raycast(transform.position, Vector2.down, boxCol.bounds.extents.y + 0.1f);
        //레이캐스트 발사(발사하는 위치, 방향, 길이)//(현재 위치, 절대값 아래 방향으로, 콜라이더 영역 y값의 절반 사이즈)
        //현재 트랜스폼 값은 캡슐 콜라이더의 중심에 위치하므로 캡슐이 딱 땅과 닿았을 때 레이캐스트가 인식함 // 애매한 부분을 위해 0.1의 길이를 추가해줌


    }
    private void Jump() // 실제 점프
    {
        if (isCrouch) //점프 시 앉아있을 경우
            //Crouch(); //해당 함수를 사용해 앉은 상태를 해제함

        rigid.velocity = transform.up * jumpForce; //리지드 바디의 움직이는 속도를 변경하는 방법 (조금 인위적인 방법임)
                                                     //움직이는 속도를 transform.up(0,1,0) * jumpForce 만큼으로 변경


    }

    private void TryJump() // 점프 시도/ 레이 땅 확인 + 스태미너 조건 추가
    {
        //점프 조건추가 (버튼 && 상태)
            Jump();
    }
    */
