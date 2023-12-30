using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardForPlayer : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    
    void Update()
    {
        // // 키보드 입력받아 상하좌우 모두 이동하게 하는 코드
        // float horizontalInput = Input.GetAxisRaw("Horizontal"); // 좌우 방향키 값 받아옴
        // float verticalInput = Input.GetAxisRaw("Vertical"); // 상하 방향키 값 받아옴
        
        // Vector3 moveTo = new Vector3(horizontalInput, verticalInput, 0f); // Vector3로 xyz값 저장
    
        // // transform 컴포넌트 값에 더해준다, 이 때 이동속도와 deltaTime을 곱해서 이속 확보 + fps와 상관없는 이동속도를 해준다
        // transform.position += moveTo * moveSpeed * Time.deltaTime; 

        // 다른 방법으로 키보드 움직임 받아보자
        Vector3 moveTo = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        
        if(Input.GetKey(KeyCode.LeftArrow)) {
            // 왼쪽 방향키 눌렀다면
            transform.position -= moveTo; // 빼준다
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            // 오른쪽 방향키 눌렀다면
            transform.position += moveTo; // 더해준다
        }
    }
}
