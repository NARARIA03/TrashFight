using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    private TextMeshProUGUI text;

    private int coin = 0;

    private void Awake() // Start메소드보다 더 먼저 실행되는 메소드
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void IncreaseCoin()
    {
        coin++;
        text.SetText(coin.ToString());

        if (coin % 15 == 0) // 15, 30, 45 ...
        {
            MouseForPlayer player = FindObjectOfType<MouseForPlayer>();
            if (player != null)
            {
                player.WeaponUpgrade();
            }
        }
    }
}
