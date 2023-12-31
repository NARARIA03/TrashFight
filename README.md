# TrashFight
## Unity & C# 공부, 드래곤플라이트 모작
### 나도코딩님의 `유니티 무료 강의 (Crash Course) - 5시간 만에 게임 만드는 법 배우기` 강의를 보며 진행했습니다.
- [강의 링크](https://www.youtube.com/watch?v=wBsSUBEUYV4)
---
# Version
- Unity 2021.3.33f1 Apple Silicon LTS
- VScode 1.85.1
	- Extension
		- C# 1.24.4
		- Unity Tools
		- Unity Code Snippets
## 2023-12-19
- Unity 설치 후 기본적인 UI, 기능들에 대해 공부한 뒤 배경이 끊기지 않고 계속 이동하도록 구현
---
## 2023-12-21
게임 캐릭터, 무기 발사 기능 구현 <br />    
**게임 캐릭터 구현** <br />
1. 4x4 게임 캐릭터 이미지를 인스펙터 뷰에서 `Sprite Mode = Multiple`로 변경하고 `Sprite Editor`에서 적절하게 나눠줌 -> (이렇게 하면 하나의 오브젝트를 나누어서 여러개로 사용 가능)
2. 드래곤 플라이트를 모작하는 중이므로, 캐릭터는 위만 바라보면 되고 이동은 좌우로만 하면 된다
3. 위를 바라보는 4개의 이미지를 복수선택해 Scene뷰로 드래그&드랍
4. 새 .anim 파일을 생성하라는 창이 뜨는데, 여기선 Run이라는 이름으로 Sprites 폴더에 생성
5. Run.anim 파일을 클릭하고 인스펙터 뷰에서 Open -> 캐릭터 애니메이션의 속도를 적당히 조절 가능
6. 캐릭터는 배경 위에서 움직여야 하므로 Player의 인스펙터 뷰에서 Order in Layer 값을 2 이상으로 줌 (Order in Layer 값은 클 수록 위에 배치됨)
7. Player.cs 스크립트를 생성해 Player 오브젝트의 컴포넌트에 추가
8. 마우스의 XY 좌표와, 실제 게임 내 오브젝트들이 사용하는 XY 좌표는 다르다 -> 마우스의 XY 좌표를 변환할 필요가 있음 <br />`Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);`
9. 마우스가 게임 화면 밖으로 나가더라도 캐릭터는 화면을 벗어나면 안 된다 -> 최대, 최소 좌표값을 고정해줘야 한다 <br />`float toX = Mathf.Clamp(mousePos.x, -2.4f, 2.4f);`
10. 이렇게 구한 마우스의 X좌표를 캐릭터의 위치에 반영해주면 이동까지 구현이 끝난다
<br />`transform.position = new Vector3(toX, transform.position.y, transform.position.z);` <br />

**무기 발사 구현** <br />
1. 무기는 한 발만 발사하는게 아니라, 여러 발 타다다당 쏴야 한다. 이렇게 컴포넌트, 프로퍼티 값, 또는 게임 오브젝트를 재사용해야 하는 경우 Prefabs를 사용하게 된다 
2. 먼저 weapon오브젝트를 하나 만들고, weapon.cs 스크립트도 만든 뒤 weapon 오브젝트의 컴포넌트에 추가한다
3. weapon.cs에서 `transform.position += Vector3.up * moveSpeed * Time.deltaTime;`를 입력해 총알이 생성 위치에서 위로 쭉 올라가도록 오브젝트의 움직임을 구현한다. moveSpeed는 weapon.cs에서 만든 지역변수로 말 그대로 총알의 속도를 나타내는 변수이고, Time.deltaTime은 컴퓨터의 성능에 의해 fps가 달라져 게임에 영향을 주지 않도록 보정해주는 수치라고 생각하면 된다
4. Hierarchy 뷰에서 weapon.cs를 컴포넌트로 가지는 weapon 오브젝트를 Prefabs폴더에 드래그해 집어넣고, Hierarchy 뷰에서는 삭제한다. 이 과정을 거치면 Prefabs에 저장된 weapon 오브젝트는 **재사용이 가능**해진 상태가 된다
5. 무기의 생성 위치는 Player의 머리 위로 고정된다. 즉 Player의 좌표를 활용해 구현해도 되나, 여기서는 sub Object를 생성해서 구현했다. Player 오브젝트를 우클릭한 뒤 Empty Object인 ShootTransform을 생성한다. ShootTransform은 Player의 위치가 변하면 함께 변하는 오브젝트다.
6. 무기 구현은 끝났으므로, 무기 발사만 구현하면 된다 -> Player.cs 스크립트로 이동해서 아래 코드를 입력하고 Unity에서 오브젝트를 연결해 Prefabs 폴더의 **weapon** 오브젝트와, Player의 sub Object인 **ShootTransform** 오브젝트를 가져온다. <br />
`[SerializeField]`<br />
`private GameObject weapon;` <br />
`[SerializeField]`<br />
`private Transform shootTransform;` <br />
7. Shoot() 메소드를 구현해 Update()메소드에 추가해준다. 먼저 무기를 발사하는 간격을 저장할 shootInterval 변수와, 마지막으로 무기를 발사한 시간을 저장할 lastShootTime 변수를 선언한다. if문을 사용해 현재 시간과 마지막으로 무기를 발사한 시간의 차이가 shootInterval 값보다 커졌을 때 weapon 오브젝트를 새로 Instantiate해준 뒤 lastShootTime을 현재 시간으로 업데이트해준다. 이를 코드로 보면 다음과 같다 <br />
`void Shoot() {` <br />
`if (Time.time - lastShootTime > shootInterval) {` <br />
`Instantiate(weapon, shootTransform.position, Quaternion.identity);` <br />
`lastShootTime = Time.time;` <br />
`}` <br />
`}` <br />
8. 마지막으로 Instantiate된 모든 weapon들이 사라지지 않고 계속 리소스를 차지하기 때문에 메모리 손실이 발생한다. 때문에 일정 시간 이후에는 생성되었던 weapon 오브젝트를 삭제할 필요가 있다. -> weapon.cs의 Start메소드에 Destroy메소드를 추가해준다 <br />
`Destroy(gameObject, 1);` <- 1초 뒤에 해당 오브젝트를 삭제하는 메소드
---
## 2023-12-28
>랜덤한 적을 일정 시간 간격을 두고 무한으로 생성 <br />
코루틴 사용법 공부 <br />
순차적으로 더 높은 레벨의 적이 나오게 구현 <br />
일정 확률로 더 높은 레벨의 적이 함께 나오게 구현 <br /> 
레벨이 높아질수록 적이 내려오는 속도가 빨라지도록 구현

## 7개의 적 prefabs 만들기

1. 적 이미지를 Multiple로 해서 이미지를 잘라줌
2. 하나를 가져와서 게임 화면에 가져다둠(Enemy1) 크기 조절은 Pixels Per Unit 값을 통해 조절, Order in Layer는 2 (배경, 무기보다 위쪽)
3. 스크립트 폴더에 Enemy.cs 생성하고, 아까 화면에 가져온 Enemy1에 연결
4. 적 오브젝트는 화면 위쪽 바깥부터 아래로 쭉 내려가면 된다, 그리고 화면 특정 위치에 도달하면 적 오브젝트를 삭제한다
    
    ```csharp
    private float moveSpeed = 10f;
    private float minY = -7f;
    
    void Update() 
    {
    	transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    	if(transform.position.y < minY)
    	{
    		Destroy(gameObject);
    	}
    }
    ```
    
5. enemy1 오브젝트의 충돌 영역을 지정해줌 (Box Colider 2D), 물리엔진을 추가해줌 (RigidBody 2D) 이 때 RigidBody 2D의 Gravity Scale을 0으로 바꿔 중력을 무시하고, 캐릭터랑 부딧쳤을 때 캐릭터가 밀리지 않도록 Box Colider 2D의 Is Trigger를 체크해준다, 그리고 Enemy 태그를 생성해서 이 태그를 달아준다
6. 이렇게 만든 enemy1을 Prefabs 폴더로 드래그해 Prefab 오브젝트로 만들고 복붙해 Component는 동일하고 이미지만 다른 7개의 enemy를 생성한 뒤 각각의 충돌영역 조절 (Enemy1 ~ Enemy7)
    
    > 이미지만 바꾸고 싶으면, 인스펙터 뷰에서 Sprite Renderer의 Sprite에 이미지를 드래그&드랍하면 됨
    > 

## 랜덤으로 적 생성

1. 7개의 Prefab을 랜덤으로 뽑아 위에서 생성시키기 위해 빈 오브젝트 EnemySpawner를 생성, 위치를 상단 중앙으로 배치한다 (x = 0, y = 6)
2. 빈 오브젝트 EnemySpawner를 통해 enemy1 ~ 7중에 골라 5개를 화면에 출력하기 위해 EnemySpawner.cs 스크립트를 생성해 EnemySpawner 오브젝트와 연결
3. 적이 생성될 y, z좌표는 EnemySpawner를 따라가고, x좌표만 계산해 배열로 따로 저장한다 { -2.2f, -1.1f, 0f, 1.1f, 2.2f }
4. GameObject 배열을 SerializeField 적용한 뒤 Unity에서 Prefabs에 있는 enemy1 ~ 7을 넣어준다
    
    ```csharp
    [SerializeField]
    private GameObject[] enemies;
    private float[] aryPosX = { -2.2f, -1.1f, 0, 1.1f, 2.2f };
    ```
    
5. 적 생성 x위치와 enemies배열의 index를 매개변수로 받는 메소드 SpawnEnemy 메소드를 만들고, 적이 생성될 Vector3 위치 변수 생성, Instantiate 함수로 적을 생성해준다. 이렇게 만든 메소드를 Start메소드에서 테스트해보면 랜덤의 적이 -2.2f 위치에서 한 번 생성된다
    
    ```csharp
    void Start()
    {
    	int idx = Random.Range(0, enemies.Lenght);
    	SpawnEnemy(aryPosX[0], idx);
    }
    
    private void SpawnEnemy(float posX, int idx) 
    {
    	Vector3 spawnPos = new Vector3(posX, transform.position.y, transform.position.z);
    	Instantiate(enemies[idx], spawnPos, Quaternion.identity);
    }
    ```
    
6. 5개의 적을 모두 나오게 수정하기 위해 반복문을 사용한다, foreach를 활용해 aryPosX의 값을 하나씩 가져와서 posX 매개변수에 집어넣어주면 편하다. 이렇게 하면 랜덤한 적 5개가 줄을 맞춰 내려오게 된다
    
    ```csharp
    void Start()
    {
    	foreach(float posX in aryPosX)
    	{
    			int idx = Random.Range(0, enemies.Length);
    			SpawnEnemy(posX, idx);
    	}
    }
    ```
    

## 적을 무한으로 생성 (여기 좀 어려우니 주의)

> 코루틴을 사용해 메소드 내에서 시간을 정해줄 수 있다. “몇초 뒤에 수행해줘” 라는 특별한 기능이다. 일반적으로 몇 초를 기다리는 동작을 프로그래밍 하게 되면 나머지 동작들도 모두 기다리게 되는데, 코루틴을 사용하면 나머지 동작들은 모두 작동하고 코루틴만 기다리게 된다
> 

> 코루틴의 구조는 Routine메소드, StartRoutine메소드가 필요하고, Start메소드 안에 StartRoutine메소드가 들어가는 형태다
> 
1. IEnumerator 반환형을 가지는 EnemyRoutine 메소드를 생성해서 Start메소드의 foreach 블록을 옮겨준 뒤 foreach문 위에 `yield return new WaitForSeconds(3f);` 를 추가해주면 3초 뒤에 아래 코드가 실행되도록 지연시킬 수 있다
    
    ```csharp
    private IEnumerator EnemyRoutine()
    {
    	yield return new WaitForSeconds(3f);
    	
    	foreach(float posX in aryPosX)
    	{
    		int idx = Random.Range(0, enemies.Length);
    		SpawnEnemy(posX, idx);
    	}
    }
    ```
    
2. StartEnemyRoutine 메소드를 생성한 후 안에 `StartCoroutine(”EnemyRoutine”);`을 넣어준다. StartCoroutine 메소드의 매개변수는 문자열이고, 코루틴을 시작할 메소드명을 넣어주면 된다
3. 이제 Start메소드에서 StartEnemyRoutine 메소드를 호출해주면 된다
4. 이제 EnemyRoutine 메소드를 수정해서 계속해서 적을 생성하도록 코드를 수정한다. foreach문을 무한반복문으로 감싸주고, 한 번 foreach가 실행된 뒤에 WaitForSecond를 사용해 다음 적 생성까지 잠시 대기한다. 최종 흐름 : 3초 대기 → 적 5개 생성 → 1.5초 대기 → 적 5개 생성 → 1.5초 대기 → …
    
    ```csharp
    private float spawnInterval = 1.5f;
    
    private IEnumerator EnemyRoutine()
    {
    	yield return new WaitForSeconds(3f);
    	
    	while (true)
    	{
    		foreach(float posX in aryPosX)
    		{
    			int idx = Random.Range(0, enemies.Length);
    			SpawnEnemy(posX, idx);
    		}
    		yield return new WaitForSeconds(spawnInterval);
    	}
    }
    ```
    

## 게임 난이도를 조절하는 방법을 공부

### 초반에는 낮은 레벨 적이 나오고, 순차적으로 높은 레벨 적이 나오게 수정

1. SpawnEnemy의 두번째 매개변수인 idx는 적 prefab을 골라주는 변수인데, 이 값을 초반에는 0~1 위주로 나오고, 시간이 지날수록 2, 3, … 점점 커지게 구현하면 될 것 같다
2. EnemyRoutine 메소드에서 몬스터 Random 부분 코드를 삭제한 뒤, 적이 생성된 횟수를 기록할 변수 spawnCount와 적 prefab을 골라주는 변수 enemyIdx를 새로 선언하고 무한반복문이 한 바퀴 돌 때마다 spawnCount를 1씩 증가시키도록 한다
    
    ```csharp
    private IEnumerator EnemyRoutine()
    {
    	int spawnCount = 0;
    	int enemyIdx = 0;
    	
    	yield return new WaitForSeconds(3f);
    	
    	while (true)
    	{
    		foreach(float posX in aryPosX)
    		{
    			SpawnEnemy(posX, enemyIdx);
    		}
    		spawnCount++;
    
    		yield return new WaitForSeconds(spawnInterval);
    	}
    }
    ```
    
3. 10번 적이 생성되면 다음 레벨로 넘어간다고 가정하면, Modulus 연산을 활용해서 enemyIdx를 증가시킴으로서 다음 레벨 적이 생성되도록 구현할 수 있다. 이렇게 하면 1-10회차는 enemy1만, 11-20회차는 enemy2만 출력되게 된다. 
    
    ```csharp
    while (true)
    	{
    		foreach(float posX in aryPosX)
    		{
    			SpawnEnemy(posX, enemyIdx);
    		}
    		spawnCount++;
    		if (spawnCount % 10 == 0)
    		{
    			enemyIdx++;
    		}
    
    		yield return new WaitForSeconds(spawnInterval);
    	}
    ```
    
4. enemyIdx가 enemies 배열의 Size를 초과하게 되면 에러가 발생할 것이다 (70회차, 즉 마지막 적이 10번 출력된 이후엔 enemyIdx가 7이 될 텐데, 이 값이 spawnEnemy의 idx매개변수로 들어가 enemies 배열의 Size 밖의 값에 접근하려고 시도하며 ArrayIndexOutOfBound Exception이 발생) 따라서 if문으로 enemyIdx가 enemies배열의 Size 이상이 되면 enemyIdx를 더 커지지 않도록 예외처리를 해 줄 필요가 있다
    
    ```csharp
    private void SpawnEnemy(float posX, int idx) 
    {
    	Vector3 spawnPos = new Vector3(posX, transform.position.y, transform.position.z);
    
    	// 적 생성 전에 idx 값이 enemies의 index 범위를 벗어나는지 확인하고 조절
    	if (idx >= enemies.Length)
    	{
    		idx = enemies.Length - 1;
    	}
    
    	Instantiate(enemies[idx], spawnPos, Quaternion.identity);
    }
    ```
    

### 게임의 난이도를 더 높히기 위해 중간에 현재 레벨보다 더 높은 적이 함께 나오게 수정

1. 1-10회차에서 enemy2가 가끔 나오고, 11-20회차에서 enemy3가 가끔 나올 수 있도록 SpawnEnemy 메소드 내에서 Random메소드와 if문을 이용해 확률을 구현해 일정 확률로 `idx++;` 을 수행한다
    
    ```csharp
    private void SpawnEnemy(float posX, int idx) 
    {
    	Vector3 spawnPos = new Vector3(posX, transform.position.y, transform.position.z);
    	
    	if (Random.Range(0, 5) == 0) // 0, 1, 2, 3, 4 중 0이 나올 확률 : 20%
    	{
    		idx++; // 20% 확률로 해당 회차보다 1 높은 레벨의 적이 나옴
    	}
    
    	// 적 생성 전에 idx 값이 enemies의 index 범위를 벗어나는지 확인하고 조절
    	if (idx >= enemies.Length)
    	{
    		idx = enemies.Length - 1;
    	}
    
    	Instantiate(enemies[idx], spawnPos, Quaternion.identity);
    }
    ```
    

### 회차가 증가해 레벨이 높아질수록 적이 내려오는 속도가 빨라지게 수정

1. spawnCount가 10이 증가할 때마다 (enemyIdx가 1 증가할때마다) moveSpeed를 조금씩 키우는 식으로 구현하기 위해 moveSpeed 변수를 EnemyRoutine 메소드에 새로 선언해주고, SpawnEnemy 메소드의 매개변수에 moveSpeed를 새로 추가해준다. 그리고 enemyIdx가 1 증가할 때 moveSpeed도 함께 2 증가하도록 코드를 수정해준다
    
    ```csharp
    private IEnumerator EnemyRoutine()
    {
    	int spawnCount = 0;
    	int enemyIdx = 0;
    	float moveSpeed = 5f; // 이부분
    	
    	yield return new WaitForSeconds(3f);
    	
    	while (true)
    	{
    		foreach(float posX in aryPosX)
    		{
    			SpawnEnemy(posX, enemyIdx);
    		}
    		spawnCount++;
    		if (spawnCount % 10 == 0)
    		{
    			enemyIdx++;
    			moveSpeed += 2; // 이부분
    		}
    
    		yield return new WaitForSeconds(spawnInterval);
    	}
    }
    
    ..
    
    private void SpawnEnemy(float posX, float moveSpeed, int idx) // 이부분
    {
    	..
    }
    ```
    
2. 적의 이동속도를 조절하는 코드는 Enemy.cs에 있었기 때문에 EnemySpawner.cs의 moveSpeed변수를 Enemy.cs로 전달해 사용해야 한다. 때문에 Enemy.cs에 public 형태의 setter를 만들어 moveSpeed 변수를 업데이트 할 수 있도록 열어둬야 한다
    
    ```csharp
    /* 
    EnemySpawner.cs 클래스에서 접근해 Enemy.cs 클래스의 moveSpeed를 조절하기 위해
    Enemy.cs에서 Setter 메소드를 선언하고, EnemySpawner.cs에서 접근
    */
    
    // Enemy.cs
    public void SetMoveSpeed(float moveSpeed)
    {
    	// this.moveSpeed = 해당 클래스의 moveSpeed
    	// moveSpeed = 전달인자의 moveSpeed
    	this.moveSpeed = moveSpeed;
    }
    ```
    
3. EnemySpawner.cs의 SpawnEnemy 메소드에서 Instantiate를 통해 만든 GameObject를 반환형을 통해 받아온 뒤 Enemy클래스(Enemy.cs)를 하나 생성해 아까 만든 GameObject로부터 Component를 얻어온다. 이후에는 Enemy클래스의 Public메소드에 대해 접근할 수 있다
    
    ```csharp
    // EnemySpawner.cs
    private void SpawnEnemy(float posX, float moveSpeed, int idx)
    {
    	..
    	// Instantiate의 return을 통해 GameObject형의 오브젝트를 받을 수 있음
    	GameObject enemyObj = Instantiate(enemies[idx], spawnPos, Quaternion.identity);
    	// 새로 만든 enemyObj에서 Enemy라는 Component를 가져옴
    	Enemy enemy = enemyObj.GetComponent<Enemy>();
    	// Enemy 클래스의 setter메소드에 접근
    	enemy.SetMoveSpeed(moveSpeed);
    }
    ```
---
## 2023-12-30
>무기 - 적 충돌 처리 구현 <br />
적 - 플레이어 충돌 처리 구현 <br />
적이 처치된 위치에서 코인이 생성되어 랜덤하게 떨어지도록 구현 <br />
떨어지는 코인과 플레이어가 충돌하면 코인을 먹어 점수가 오르도록 구현<br /> 
## 충돌 처리 (무기 - 적, 적 - 플레이어)

### 무기가 적에게 닿았을 때 적이 사라지도록 구현

1. Weapon 오브젝트에 Circle Colider 2D를 적용하고, Is Trigger를 체크해줌
2. Enemy.cs에 hp 변수를 private로 선언하고 SerializeField로 만들어줌, 그리고 Enemy1 - Enemy7의 hp를 Unity에서 각각 지정해줌
3. Weapon.cs에 damage 변수를 **public**으로 선언 ← GetComponent 메소드를 통해 damage 변수에 접근 가능하도록 열어둠
4. Enemy.cs에 `OnTriggerEnter2D` 메소드를 선언하고 구현 (Is Trigger를 체크했을 때, 충돌감지만 일어났을 때 구현)
    - 참고로 Is Trigger를 체크하지 않은 경우는 `OnCollisionEnter2D` 메소드로 구현
    
    ```csharp
    // Enemy.cs
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if(other.gameObject.tag = "Weapon")
    	{
    		Weapon weapon = other.gameObject.GetComponent<Weapon>();
    		hp -= weapon.damage;
    		if(hp <= 0)
    		{
    			Destroy(gameObject);
    		}
    		Destroy(other.gameObject);
    	}
    }
    ```
    
    - Unity에서 오브젝트에 부여한 tag로 오브젝트를 구분 가능하다. 이걸로 적과 충돌한 오브젝트게 Weapon인지 확인한다
    - 무기와 적이 충돌했다면 적의 hp에서 무기의 데미지를 빼서 업데이트
    - 적의 hp가 0 이하라면 적 오브젝트를 제거
    - 적과 무기가 충돌했다면 모든 일이 수행된 뒤에 무기 오브젝트를 제거

---

### 플레이어가 적에게 닿았을 때 게임이 종료되도록 구현

> 무기 - 적 충돌과 구현하는 방법은 비슷하다. 만약 캐릭터의 체력도 구현한다면 완전히 동일할 것으로 보인다
> 
1. MouseForPlayer.cs에 OnTriggerEnter2D 메소드를 선언하고 구현 (Enemy태그 오브젝트와 충돌했다면 Destroy 메소드로 플레이어 오브젝트를 제거)

	```csharp
	// MouseForPlayer.cs
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			Destroy(gameObject);
		}
	}
	```

---

# 코인 기능 구현

> 적을 처치하면 코인이 생성되어 랜덤한 위치로 떨어지도록 구현
플레이어가 이 코인을 먹으면 점수도 오르고 무기를 업그레이드 할 수 있도록 구현할 예정
> 
1. 먼저 코인 오브젝트부터 구현해 보자
    1. 코인 이미지의 Pixels Per Unit 값을 50정도로 줄여서 크기를 키워준다
    2. Sprite Mode를 Multiple로 설정하고, Sprite Editror에서 Slice 해준다
    3. Animation을 적용해 빙글빙글 회전하는 것 처럼 보이게 만든다. Animation 속도를 조절하고 싶다면 .anim 파일을 누른 뒤 인스펙터 뷰에서 Samples값을 더 키워주면 된다
    4. 마지막으로 Order in Layer 값을 1로 줘서 배경에 묻히지 않도록 해준다.
2. 코인과 주인공이 충돌했을 때 충돌기능을 구현해야 하므로, 코인에도 태그를 새로 만들어 붙여주고, Circle Colider 2D 컴포넌트도 추가해준다. 역시 Is Trigger 체크를 해줘야 한다 (안 해주면 코인에 밀려서 떨어져버림)
3. 코인의 움직임을 구현하기 위해 우선 Rigidbody 2D 컴포넌트를 추가해준 뒤, Coin.cs 스크립트를 생성해 연결해준다
4. 코인이 생성되면 랜덤으로 좌상단 또는 우상단으로 튀어올라갔다가 떨어지도록 구현하기 위해 Rigidbody2D 컴포넌트를 GetComponent 메소드로 가져온 뒤 AddForce 메소드를 활용해서 힘의 방향을 조절해준다. AddForce 메소드에는 Vector2값과 ForceMode값이 들어가는데, ForceMode값은 Impulse로 설정한다
5. 먼저 좌상단, 우상단 랜덤으로 튀어오르도록 하기 위해 Random.Range메소드를 사용해 실수형 변수 verticalJumpForce, horizontalJumpForce 두 개를 선언해 Vector2로 묶어주고 이를 AddForce 메소드에 적용해준다
6. 코인은 시간이 지나면 사라지게 만들어야 하므로 minY 변수를 생성해주고, Update 메소드에서 Destroy를 구현해준다
    
    ```csharp
    // Coin.cs
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
    
    	rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
    }
    
    void Update()
    {
    	if (transform.position.y <= minY)
    	{
    		Destroy(gameObject);
    	}
    }
    ```
    
7. 코인은 적을 처치하면 나와야 하고, 재활용되는 오브젝트이므로 Prefabs폴더로 집어넣는다
8. 코인은 적이 처치되면 등장하므로, Enemy.cs의 충돌 처리 이후 삭제하는 부분에서 Coin이 생성되도록 구현하면 된다. 먼저 Enemy.cs에서 GameObject 변수를 SerializeFIeld로 선언한 뒤 Unity에서 Coin Prefab 오브젝트를 모든 Enemy 오브젝트에 연결해준다
9. Enemy.cs의 충돌 처리 부분 코드에서 Instantiate 메소드를 사용해 Coin오브젝트를 생성해준다. 위치는 적이 삭제되는 위치로 잡아준다
    
    ```csharp
    // Enemy.cs
    [SerializeField]
    private GameObject coin;
    
    .. // 중간 코드 생략
    
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if(other.gameObject.tag == "Weapon")
    	{
    		Weapon weapon = other.gameObject.GetComponent<Weapon>();
    		hp -= weapon.damage;
    		if(hp <= 0)
    		{
    			// 여기 수정!
    			Instantiate(coin, gameObject.transform.position, Quaternion.identity);
    			Destroy(gameObject);
    		}
    		Destroy(other.gameObject);
    	}
    }
    ```
    
10. 플레이어가 코인과 충돌하면 코인 오브젝트를 없애주고, 어떤 내부 변수를 1씩 키워줘서 먹은 코인 수를 기록할 수 있도록 하자. 그러려면 플레이어와 적이 충돌하는 부분의 코드에 코인 관련 내용도 추가해주면 된다

	```csharp
	// MouseForPlayer.cs
	..

	public int coinScore = 0;

	..

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
		}
	}
	```
---
## 2023-12-31
>점수 출력 기능 구현<br />
무기 업그레이드 기능 구현<br />
## 점수 출력 기능 구현
### 1. 점수 출력 UI 구현

> 점수 출력이나, 게임에 필요한 UI들은 Hierarchy뷰의 UI 를 통해 구현할 수 있다. UI를 추가하게 되면 Hierarchy 뷰에 Canvas와 EventSystem이라는 요소가 자동으로 추가되게 된다
> 

> Hierarchy 뷰에서 Canvas를 더블클릭해보면 엄청 큰 사각형이 생긴 것을 알 수 있는데, 이게 캔버스 영역이다. 이 영역은 게임 영역과 별도로 작동하나, 캔버스 영역에 UI들을 배치한 뒤 게임을 실행하면 게임 화면에 비율이 맞춰져서 나타나게 된다
> 
1. Hierarchy뷰에서 우클릭 → UI → Panel을 클릭해 새 패널을 추가한다. 새로 추가된 패널 이름은 ScorePannel로 설정한다
    - 패널은 게임 내의 버튼이나 텍스트, 이미지 등을 한꺼번에 관리하기 위한 도구라고 생각하면 된다
2. Canvas를 클릭하고 UI Scale Mode를 Scale With Screen Size로 설정하고, 해상도를 게임 해상도와 동일하게 잡아준다
    - 게임 실행한 환경 해상도에 따라 UI의 크기도 따라서 변하게 하는 설정이다
3. ScorePannel을 클릭하고 패널의 크기와 위치를 조절해준다
    - 크기와 모양은 Scene 뷰에서 Rect Tool로 크기를 줄여 작은 직사각형으로 만들어주었다
    - 위치는 Rect Transform의 왼쪽에 있는 버튼을 클릭한 뒤 opt키를 누른 채로 좌상단 아이콘을 눌러 화면 좌상단에 붙여주었다
    - 이후 Scene 뷰에서 Rect Tool을 사용해 살짝 여백을 줬다
4. ScorePannel을 우클릭하고 UI → Image를 클릭한 뒤 위치를 잡아주고, Source Image에 코인 이미지를 넣어주고 Set Native Size를 클릭해 원본 비율로 바꿔준 뒤 크기와 위치를 조절해준다
5. ScorePannel을 우클릭하고 UI → TextMeshPro를 클릭한 뒤 위치를 잡아주고, Font Size와 Alignment 값을 조절해준다. 텍스트 초기값은 우선 0으로 설정했다. 그리고 Wrapping을 Disabled로 바꿔준다
    - Wrapping은 사각형 영역 밖으로 텍스트가 나가면 다음 줄로 옮겨서 작성하도록 하는 기능이다
6. ScorePannel의 색상을 투명하게 바꿔준다. Color에서 A(Alpha) 값을 0으로 수정해주면 된다

---

### 2. 점수 출력 UI에 실제로 먹은 코인 개수가 뜨도록 구현

> Empty Object인 GameManager 오브젝트를 생성하고, GameManager.cs 스크립트를 연결해준 뒤 스크립트 상에서 구현한다. 게임의 전반적인 내용을 관리하는 스크립트이므로 일반적으로 **싱글톤 패턴**으로 구현하게 된다
> 

> 싱글톤 패턴의 정의는 “객체의 인스턴스가 오직 1개만 생성되는 패턴” 이다. 
싱글톤 패턴을 사용하면 메모리를 효율적으로 사용할 수 있고, 클래스 간 데이터 공유가 쉽다
> 
1. Hierarchy 뷰에서 Create Empty를 통해 GameManager라는 이름으로 빈 게임 오브젝트를 만든다
2. Scripts 폴더에 GameManager.cs 스크립트를 생성한 뒤 GameManager 오브젝트에 연결한다
3. GameManager.cs 스크립트를 열어준 뒤 싱글톤 패턴을 적용하기 위해 아래와 같이 수정해준다
    
    ```csharp
    // GameManager.cs
    
    public class GameManager : MonoBehaviour
    {
    	public static GameManager instance = null;
    
    	private void Awake() 
    	{
    		if(instance == null)
    		{
    			instance = this;
    		}
    	}
    }
    ```
    
4. 코인 개수를 저장할 변수가 필요하니까 코인 변수를 새로 선언하고, 코인을 증가시키는 public 메소드를 선언해준다
    
    ```csharp
    // GameManager.cs
    
    public class GameManager : MonoBehaviour
    {
    	public static GameManager instance = null;
    	
    	private int coin = 0; // 코인 변수 선언
    
    	private void Awake() 
    	{
    		if(instance == null)
    		{
    			instance = this;
    		}
    	}
    
    	public void IncreaseCoin() // 코인 개수 1 증가시키는 public 메소드
    	{
    		coin++;
    	}
    }
    ```
    
5. MouseForPlayer.cs의 충돌처리 부분에서 동전과 플레이어가 충돌했을 때 IncreaseCoin메소드를 실행시켜준다
    
    > GameManager.cs 스크립트가 싱글톤 패턴으로 구현되어 있으므로`GameManager.instance`를 통해 GameManager 클래스의 메소드에 쉽게 접근할 수 있다
    > 
    
    ```csharp
    // MouseForPlayer.cs
    
	// 생략
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if(other.gameObject.tag == "Enemy")
    	{
    		Destroy(gameObject);
    	}
    	else if(other.gameObject.tag == "Coin")
    	{
    		Destroy(other.gameObject);
    		GameManager.instance.IncreaseCoin(); // 쉽게 GameManager의 메소드에 접근
    	}
    }
    ```
    
6. 이제 게임 화면에 coin변수값이 뜨도록 하기 위해 GameManager.cs에서 TextMeshProUGUI형의 변수를 [SerializeField]와 함께 선언해준 뒤, Unity에서 아까 만들어둔 TextMeshPro UI 오브젝트를 이 변수에다 연결해준다. 빈 오브젝트인 GameManager에 SerializeField 변수가 뜨므로 여기다 연결하면 된다
    
    ```csharp
    // GameManager.cs
    
	[SerializeField]
    	private TextMeshProUGUI text;
    ```
    
7. GameManager.cs의 IncreaseCoin메소드에서 TextMeshPro 변수에 접근해 SetText메소드를 활용해서 텍스트 내용을 변경해주는 코드를 추가해준다. SetText 메소드는 문자열 데이터만 받을 수 있으므로 ToString 메소드를 사용해 coin 변수를 문자열로 수정해서 집어넣는다
    
    ```csharp
    // GameManager.cs
    
    public void IncreaseCoin()
    {
    	coin++;
    	text.SetText(coin.ToString());
    }
    ```


## 무기 업그레이드 기능 구현
> 플레이어가 일정 이상의 코인을 획득하면 다음 단계의 무기를 사용할 수 있도록 구현
> 
1. Prefabs 폴더에서 무기를 복제해준 뒤 이름을 각각 WeaponBlue, WeaponRed로 바꿔주고
무기 이미지와 Move Speed, Damage를 Unity상에서 수정해준다
2. MouseForPlayer.cs에서 weapon변수를 weapons 배열로 바꿔준 뒤 Unity에서 weapons 배열에 3개의 무기를 추가해준다
3. MouseForPlayer.cs에서 weaponIdx 변수를 새로 선언해주고, 기존 weapon변수가 사용되던 곳에서 weaponIdx 변수를 활용해 수정해준다
    
    ```csharp
    // MouseForPlayer.cs
    
    public class MouseForPlayer : MonoBehaviour
    {
    	[SerializeField]
    	private GameObject[] weapons;
    	
    	private int weaponIdx = 0;
    
    	// 생략
    	
    	void Shoot()
    	{
    		if(Time.time - lastShootTime > shootInterval)
    		{
    			Instantiate(weapons[weaponIdx], shootTransform.position, Quaternion.identity);
    			lastShootTime = Time.time;
    		}
    	}
    	// 생략
    }
    ```
    
4. MouseForPlayer 클래스 밖에서 weaponIdx 값을 조절할 수 있도록 하기 위해서MouseForPlayer.cs에 WeaponUpgrade 메소드를 public으로 선언해주고, weapons 배열의 index 범위 내에서 weaponIdx 변수를 1씩 키우도록 구현한다
    
    ```csharp
    // MouseForPlayer.cs
    
    public class MouseForPlayer : MonoBehaviour
    {
    	// 생략
    	public void WeaponUpgrade()
    	{
    		if(weaponIdx < weapons.Length - 1)
    		{
    			weaponIdx++;
    		}
    	}
    }
    ```
    
5. GameManager.cs에서 coin의 개수가 15의 배수가 되면 MouseForPlayer.cs의 WeaponUpgrade 메소드에 접근해 다음 무기로 업그레이드하도록 구현한다. 
    > GameManager.cs에서 MouseForPlayer.cs의 메소드에 접근하기 위해 `FindObjectOfType<>();` 메소드를 사용한다
    
    ```csharp
    // GameManager.cs
    
    public class GameManager : MonoBehaviour
    {
    	// 생략
    	public void IncreaseCoin()
      {
    	  coin++;
        text.SetText(coin.ToString());
    
        if (coin % 15 == 0)
        {
          MouseForPlayer player = FindObjectOfType<MouseForPlayer>();
          if (player != null)
          {
    	      player.WeaponUpgrade();
          }
        }
      }
    }
    ```
    
    
> 최종적으로 게임을 테스트해보면 코인을 15개 먹었을 때 파란 무기로 바뀌고, 30개 먹었을 때 빨간 무기로 바뀌고, 45개부터는 변화가 없음을 알 수 있다
---
