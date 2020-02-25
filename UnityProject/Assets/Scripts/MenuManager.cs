using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("玩家資料")]
    public PlayerData dataPlayer;

    public void BuyHp_500()
    {
        dataPlayer.hpMax += 500;            // 最大值遞增
        dataPlayer.hp = dataPlayer.hpMax;
    }

    public void BuyAtk_50()
    {
        dataPlayer.attack += 50;            // 攻擊力遞增
    }

    public void LoadLevel()
    {
        dataPlayer.hp = dataPlayer.hpMax;
        SceneManager.LoadScene("關卡 1");
    }
}
