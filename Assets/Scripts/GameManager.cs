using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    private int coin = 0;

    [HideInInspector]
    public bool isGameOver = false;

    [SerializeField]
    private GameObject gameOverPannel;
    
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

    public void SetGameOver(bool isWin)
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.StopEnemyRoutine();
            isGameOver = true;
        }
        gameOverText.SetText(coin.ToString());
        Invoke("ShowGameOverPannel", 1f);
    }

    private void ShowGameOverPannel()
    {
        gameOverPannel.SetActive(true);
    }

    public void PlayAgain()
    {
        // 게임 씬을 리 로딩 해주면 처음부터 다시 작동되게 된다
        SceneManager.LoadScene("SampleScene");
    }
}
