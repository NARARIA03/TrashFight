using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
// Variable
    [SerializeField]
    private float moveSpeed = 10f;
    private float minY = -6f;

// Getter, Setter
    public void SetMoveSpeed(float moveSpeed)
    {
        // this.moveSpeed = 해당 클래스의 moveSpeed
        // moveSpeed = 전달인자의 moveSpeed
        this.moveSpeed = moveSpeed;
    }

// Main method
    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // 특정 y좌표보다 enemy가 내려가면 없애주자
        if (transform.position.y < minY)
        {
            Destroy(gameObject);
        }
    }
}
