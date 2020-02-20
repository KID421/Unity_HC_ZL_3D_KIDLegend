using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 是否過關，過關時就前往玩家的位置
    /// </summary>
    [HideInInspector]   // 在屬性面板上隱藏
    public bool pass;

    [Header("道具音效")]
    public AudioClip sound;

    private Transform player;   // 玩家變形元件
    private AudioSource aud;

    private void Start()
    {
        Physics.IgnoreLayerCollision(10, 10, false);    // 金幣要互相碰撞
        aud = GetComponent<AudioSource>();
        player = GameObject.Find("鼠王").transform;

        HandleCollision();
    }

    private void Update()
    {
        GoToPlayer();
    }

    /// <summary>
    /// 控制忽略碰撞
    /// </summary>
    private void HandleCollision()
    {
        Physics.IgnoreLayerCollision(10, 8);    // 忽略碰撞 金幣 玩家
        Physics.IgnoreLayerCollision(10, 9);    // 忽略碰撞 金幣 敵人
    }

    /// <summary>
    /// 前往玩家位置
    /// </summary>
    private void GoToPlayer()
    {
        if (pass)
        {
            Physics.IgnoreLayerCollision(10, 10);
            transform.position = Vector3.Lerp(transform.position, player.position, 0.8f * Time.deltaTime * 20);

            if (Vector3.Distance(transform.position, player.position) < 1.5f && !aud.isPlaying)
            {
                aud.PlayOneShot(sound, 0.3f);
                Destroy(gameObject, 0.3f);
            }
        }
    }
}
