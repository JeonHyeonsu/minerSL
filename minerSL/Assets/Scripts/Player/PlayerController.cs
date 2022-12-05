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
        // 1. ���̽�ƽ �Է°�
        float x = joy.Horizontal;
        float y = joy.Vertical;

        // 2. ���� �̵�
        moveVec = new Vector2(x, y) * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);

    }

    void LateUpdate()
    {
        anim.SetBool("isMove", true);
    }
}





    /*
    //##���� ����##
    private void IsGround()  // ����Ȯ�� �Լ� = isGround�� true false�� ������
    {

        isGround = Physics.Raycast(transform.position, Vector2.down, boxCol.bounds.extents.y + 0.1f);
        //����ĳ��Ʈ �߻�(�߻��ϴ� ��ġ, ����, ����)//(���� ��ġ, ���밪 �Ʒ� ��������, �ݶ��̴� ���� y���� ���� ������)
        //���� Ʈ������ ���� ĸ�� �ݶ��̴��� �߽ɿ� ��ġ�ϹǷ� ĸ���� �� ���� ����� �� ����ĳ��Ʈ�� �ν��� // �ָ��� �κ��� ���� 0.1�� ���̸� �߰�����


    }
    private void Jump() // ���� ����
    {
        if (isCrouch) //���� �� �ɾ����� ���
            //Crouch(); //�ش� �Լ��� ����� ���� ���¸� ������

        rigid.velocity = transform.up * jumpForce; //������ �ٵ��� �����̴� �ӵ��� �����ϴ� ��� (���� �������� �����)
                                                     //�����̴� �ӵ��� transform.up(0,1,0) * jumpForce ��ŭ���� ����


    }

    private void TryJump() // ���� �õ�/ ���� �� Ȯ�� + ���¹̳� ���� �߰�
    {
        //���� �����߰� (��ư && ����)
            Jump();
    }
    */
