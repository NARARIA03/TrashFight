using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Variable
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private float moveSpeed = 10f;
    public float minY = -6f;
    
    [SerializeField]
    private float hp = 1f;

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

    // 적과 총알이 충돌하는 경우 작동하는 메소드
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon") // 충돌한 대상이 Weapon오브젝트일 경우
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>(); // 충돌한 Weapon 오브젝트의 Component를 받아옴
            hp -= weapon.damage; // 적의 hp에서 무기의 damage를 빼줌
            if (hp <= 0) // 적의 hp가 0 이하라면
            {
                Instantiate(coin, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject); // 적 오브젝트 제거
            }
            Destroy(other.gameObject); // 적과 Weapon이 충돌했다면, 충돌한 Weapon 오브젝트도 제거
        }
    }
}
