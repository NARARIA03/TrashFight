# TrashFight
## Unity, C# 공부, 드래곤플라이트 모작 따라 만들기
나도코딩님의 "유니티 무료 강의 (Crash Course) - 5시간 만에 게임 만드는 법 배우기" 강의를 보며 제작했습니다.
[강의 링크](https://www.youtube.com/watch?v=wBsSUBEUYV4)

- 20231219
  - 설치, 기본적인 기능 공부 후 배경 화면이 끊기지 않고 이동하는것까지 구현
- 20231221
  - 게임 캐릭터와 무기 발사 구현
    - 게임 캐릭터 구현
      - 4x4 게임 캐릭터 이미지를 인스펙터 뷰에서 Sprite Mode = Multiple로 변경하고 Sprite Editor에서 적절하게 나눠줌 (이렇게 하면 하나의 오브젝트를 나누어서 여러개로 사용 가능)
      - 위만 바라볼것이므로 위를 바라보는 4개의 이미지를 복수 선택해서 Scene뷰로 드래그
      - 새 .anim 파일을 생성하라는 창이 뜨는데 run이라는 이름으로 Sprites 폴더에 생성
      - 생성한 anim 파일을 클릭하고 인스펙터 뷰에서 Open을 클릭해서 캐릭터가 걸어가는듯한 움직임을 적절한 속도로 조절해줌
      - 캐릭터는 배경 위에 있어야 하므로 Hierarchy 뷰에서 Player를 선택하고, 인스펙터 뷰에서 Order in Layer 값을 2 이상으로 설정 (배경은 0, 후에 나올 무기는 1)
      - Rigidbody 2d (Gravity Scale = 0), Circle Collider 2d를 사용해 벽을 뚫고 나가지 못하도록 구현할 수도 있으나 최종적으로 마우스를 사용해 움직이도록 할 것이므로 이 부분은 Pass
      - Player.cs를 Scripts 폴더에 생성한 후에 Player 오브젝트와 연결
      - 마우스의 xy값과 게임 내에서의 xy값은 다르다, 따라서 마우스 좌표 -> 게임 좌표 변환이 필요 Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      - 마우스가 게임 화면 밖을 벗어나더라도, 캐릭터는 화면을 벗어나면 안 된다. 즉 최대 최소 x값 고정이 필요하다 -> float toX = Mathf.Clamp(mousePos.x, -2.4f, 2.4f);
      - 이렇게 구한 마우스의 x좌표를 캐릭터의 위치에 반영해준다 -> transform.position = new Vector3(toX, transform.position.y, transform.position.z); **y,z값은 변동X**
    - 무기 발사 구현
      - 무기는 한 발만 발사하지 않고, 일정 간격을 두고 두두두두 발사한다. 이런식으로 오브젝트를 복제하며 사용하는 경우 Prefabs 폴더에 오브젝트를 지정하고 사용하게 된다
      - 먼저 weapon 오브젝트를 하나 만들고, weapon.cs 스크립트를 연결한 뒤에 transform.position += Vector3.up * moveSpeed * Time.deltaTime;과 같이 위로 쭉 올라가도록 weapon 오브젝트의 움직임을 구현한다
      - 그런 뒤 Hierarchy 뷰에서 **weapon오브젝트를 Prefabs 폴더에 드래그해 집어넣고**, Hierarchy 뷰에서는 삭제한다 (Scene에서도 삭제한다)
      - Player의 머리 위에서 발사하고, 이 위치는 Player를 따라다니므로 Player 오브젝트 안에 Empty Object인 ShootTransform을 생성한다. 위치는 무기 발사 시작지점으로 잡는다
      - 무기가 발사된 이후의 움직임은 이미 Object로서 구현된 상태고, 발사만 해주면 된다 -> 즉 Player.cs에서 마저 코딩해야한다
      - Prefabs 폴더에 들어가있는 weapon 오브젝트와, 발사 위치를 가지고 있는 ShootTransform을 Player.cs에서 불러온다 -> [SerializeField] private GameObject weapon; [SerializeField] private Transform shootTransform;
      - 다음에 Unity의 Player 오브젝트를 선택하고, Script 컴포넌트에서 Weapon에는 Prefabs폴더 내의 Weapon 오브젝트를 드래그&드랍, ShootTransform에는 Player아래에 있는 ShootTransform 오브젝트를 드래그&드랍한다 (이러면 오브젝트를 하나의 변수처럼 관리 가능)
      - 마지막 무기 발사 시간 lastShootTime, 무기 발사 간격 shootInterval, 변수를 함께 도입해서 Shoot() 메소드를 구현한다. 이 때 Instantiate()메소드를 사용하는데 이 메소드는 오브젝트를 화면에 새로 생성해주는 메소드라고 이해하면 된다
      - Shoot 메소드의 구성은 **만약 현재시간 - 마지막 발사시간 > 발사 간격이라면 무기를 발사하고, 마지막 발사시간을 현재 시간으로 업데이트하는 구성**이다
        ```cs
        void Shoot() {
        // 마지막 미사일 발사 이후로 shootInterval 만큼 시간이 지난 상황이라면 새 미사일을 발사하고, 마지막 미사일 발사 시간을 현재로 수정
        // Time.time은 현재 시간 값이다.
          if (Time.time - lastShootTime > shootInterval) {
              Instantiate(weapon, shootTransform.position, Quaternion.identity);
              lastShootTime = Time.time;
          }
        }
        ```
      - 마지막으로 발사된 모든 무기들이 계속 리소스를 차지하기 때문에, 무기 생성 후 1초 뒤에 자동으로 삭제되도록 해야 한다 -> weapon.cs의 Start메소드에서 Destroy(gameObject, 1);
