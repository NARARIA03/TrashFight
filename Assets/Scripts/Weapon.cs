using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, 1);    
    }
    
    void Update()
    {
        // 일정 속도만큼 object가 위로 올라가게 됨
        // Vector3.up = Vector3(0, 1, 0)을 의미
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
