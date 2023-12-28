using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies; // 적 prefabs 7개의 GameObject를 Unity에서 가져옴

    private float[] aryPosX = { -2.2f, -1.1f, 0, 1.1f, 2.2f }; // 적이 생성될 x좌표 배열

    [SerializeField]
    private float spawnInterval = 1.5f; // 적이 생성된 후, 다음 적이 생성되기까지 시간

    void Start()
    {
        StartEnemyRoutine();
    }

    private void StartEnemyRoutine()
    {
        StartCoroutine("EnemyRoutine");
    }

    private IEnumerator EnemyRoutine()
    {
        yield return new WaitForSeconds(3f); // 3초동안 아래 무한 반복문을 실행하기 전에 기다린다
        // 처음에는 쪼랩 위주로 나오게 하다가 시간이 지날수록 고랩들이 많이 나오게 하려면 selectEnemy를 정하는 random을 조절해야 한다
        // 스폰 회차를 체크하면서 얼만큼 스폰된 뒤에는 다음 레벨 몬스터가 출력되도록
        int spawnCount = 0;
        int enemyIdx = 0;
        int goNextLevel = 10;
        float moveSpeed = 5f;

        while (true)
        {
            // foreach 문을 아래처럼 사용하면 aryPosX배열의 값을 하나씩 posX로 가져오며, aryPosX의 모든 값을 읽을 때까지 반복한다
            foreach (float posX in aryPosX)
            {
                // int selectEnemy = Random.Range(0, enemies.Length);
                SpawnEnemy(posX, moveSpeed, enemyIdx);
            }
            spawnCount += 1;
            if (spawnCount % goNextLevel == 0) // spawnCount가 goNextLevel과 같아지면 다음 레벨로 넘어간다
            {
                enemyIdx += 1;
                moveSpeed += 1;
            }

            yield return new WaitForSeconds(spawnInterval); // spawnInterval 값 동안 아래 무한 반복문을 실행하기 전에 기다린다
        }
    }

    private void SpawnEnemy(float posX, float moveSpeed, int idx) // Enemy 생성하는 함수
    {
        // 적 생성 x좌표는 -2.2 -1.1 0 1.1 2.2 이렇게 5개로 잡아볼 예정, 이거는 Unity에서 배치해보면서 찾은 x좌표
        Vector3 spawnPos = new Vector3(posX, transform.position.y, transform.position.z);

        if (Random.Range(0, 5) == 0) // 20%의 확률을 구현
        {
            idx += 1; // 20%의 확률로 1 단계 더 강한 몬스터가 출력되도록
        }
        
        if (idx >= enemies.Length) // index값이 넘어가도 에러를 방지하기 위해
        {
            idx = enemies.Length - 1;
        }

        GameObject enemyObj = Instantiate(enemies[idx], spawnPos, Quaternion.identity); // 첫 인수는 게임오브젝트, 두번째 인수는 생성위치, 세번째 위치는 회전값
        Enemy enemy = enemyObj.GetComponent<Enemy>(); // 새로 만든 enemyObj에서 Enemy라는 Component를 가져옴
        enemy.SetMoveSpeed(moveSpeed); // Enemy 클래스의 SetMoveSpeed메소드를 가져와 사용
    }
}
