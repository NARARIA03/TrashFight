# TrashFight
## Unity & C# 공부, 드래곤플라이트 모작
### 나도코딩님의 `유니티 무료 강의 (Crash Course) - 5시간 만에 게임 만드는 법 배우기` 강의를 보며 진행했습니다.
- [강의 링크](https://www.youtube.com/watch?v=wBsSUBEUYV4)
---
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
