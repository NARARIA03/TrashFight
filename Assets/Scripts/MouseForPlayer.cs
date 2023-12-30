using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseForPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject weapon;

    [SerializeField]
    private Transform shootTransform;

    [SerializeField]
    private float shootInterval = 0.05f;

    private float lastShootTime = 0f;

    public int coinScore = 0;

    void Update()
    {
        // Debug.Log(Input.mousePosition);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 받아온 마우스 xyz좌표중 x좌표만 활용해야 한다.
        // 따라서 transform.position에 값을 집어넣기 전에 새로운 Vector3에 x값만 적용해주고
        // yz는 예전 값 그대로 사용한다
        // 그리고 좌우 화면 밖으로 나가지 못하도록 좌우 끝 좌표랑 if문으로 못 나가게 막아준다
        // if (mousePos.x > -2.4f && mousePos.x < 2.4f) {
        //     transform.position = new Vector3(mousePos.x, transform.position.y, transform.position.z);
        // }
        //----------------------------//
        // 근데 Mathf.Clamp를 활용하면 어떤 변수의 값의 최소 최대값을 지정해두고 
        // 최소값보다 작으면 최소값, 최대값보다 크면 최대값, 그 사이라면 사이값 이렇게 자동으로 최대최소 Lock을 걸어주는 함수가 존재한다
        // 이를 사용하면 아래처럼 된다
        float toX = Mathf.Clamp(mousePos.x, -2.4f, 2.4f);
        transform.position = new Vector3(toX, transform.position.y, transform.position.z);
        Shoot();
    }

    // 미사일 발사하는 메소드
    void Shoot()
    {
        // 마지막 미사일 발사 이후로 shootInterval 만큼 시간이 지난 상황이라면 새 미사일을 발사하고, 마지막 미사일 발사 시간을 현재로 수정
        // Time.time은 현재 시간 값이다.
        if (Time.time - lastShootTime > shootInterval)
        {
            Instantiate(weapon, shootTransform.position, Quaternion.identity);
            lastShootTime = Time.time;
        }
    }

    // 플레이어가 적 또는 코인과 충돌했을때의 기능을 구현하기 위한 메소드
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinScore++;
            Debug.Log(coinScore);
        }
    }
}
