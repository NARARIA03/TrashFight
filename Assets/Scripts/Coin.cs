using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float minY = -6f;
    void Start()
    {
        Jump();
    }
    
    private void Jump()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        float verticalJumpForce = Random.Range(4f, 8f);
        float horizontalJumpForce = Random.Range(-2f, 2f);
        Vector2 jumpForce = new Vector2(horizontalJumpForce, verticalJumpForce);
        
        rigidBody.AddForce(jumpForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        // 코인이 일정 위치 이상 내려가면 코인 오브젝트 삭제
        if(transform.position.y <= minY) {
            Destroy(gameObject);
        }
    }
}
