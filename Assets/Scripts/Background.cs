using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // 다른 클래스에서 접근하지 못하도록 private 처리
    private float moveSpeed = 3f;
    /* 
    Background.cs 핵심 논리
    두 개의 background 오브젝트에 대해 적용되는 스크립트임
    background 오브젝트는 게임이 시작되면 일정 속도로 아래로 내려감 (그래야 캐릭터가 가만히 있어도 앞으로 가는것처럼 보임)
    background 오브젝트의 맨 위 부분이 화면 맨 아래까지 내려가면 (여기서는 -10보다 더 y값이 작아지면) 
    맨 아래 부분이 화면 맨 위까지 올라가도록 위치를 재조정해줌

    구현을 위해 사용한 것
    1. Vector3 구조체로 오브젝트의 x, y, z 값에 접근해 값 수정, 이 때 Vector3를 통해서 접근해야 함에 유의
    2. Time.deltaTime을 곱해 144fps가 나오는 컴퓨터이던, 30fps가 나오는 컴퓨터이던 오브젝트가 이동하는 속도가 다르지 않도록 보정해줌
    */
    void Update()
    {
        // background.cs 스크립트가 적용된 게임 오브젝트들에 대해 transform 컴포넌트의 y값 조절
        // pc 성능에 상관없이 똑같은 위치만큼 이동하도록 Time.deltaTime을 곱해줘야 함!
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        
        // 이미지가 바닥까지 내려갔다면 -> 다시 위로 올려서 무한반복되도록 구현
        if(transform.position.y < -10) {
            // transform.position = new Vector3(0, 10, 0); <- 이렇게 하면 코드 수정이 불편하고 뭔가 변수가 static해짐
            transform.position += new Vector3(0, 10f * 2, 0); // 이렇게 하면 한 칸 더 올리고 싶으면 곱하기 3으로만 바꿔주면 되므로 좀 더 동적임
        }
    }
}
