using UnityEngine;
using UnityEngine.AI;       // 引用 人工智慧 API

public class Enemy : MonoBehaviour
{
    [Header("怪物資料")]
    public EnemyData data;                      // 腳本化物件：所有實體物件共用

    private float hp;                           // 欄位：所有實體物件獨立使用
    private Animator ani;                       // 動畫控制器
    private NavMeshAgent nav;                   // 導覽網格代理器
    private Transform target;                   // 目標變形
    private float timer;                        // 計時器
    private HpValueManager hpValueManager;      // 血條數值管理器

    private void Start()
    {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = data.speed;                         // 調整 代理器.速度
        nav.stoppingDistance = data.stopDistance;
        hp = data.hp;

        target = GameObject.Find("鼠王").transform;   // 目標 = 尋找
        hpValueManager = GetComponentInChildren<HpValueManager>();      // 取得子物件元件
    }

    private void Update()
    {
        Move();                     // 呼叫移動方法
    }

    // 摺疊 Ctrl + M O
    // 展開 Ctrl + M L

    /// <summary>
    /// 等待
    /// </summary>
    private void Wait()
    {
        ani.SetBool("跑步開關", false);      // 等待動畫
        timer += Time.deltaTime;            // 計時器累加

        if (timer >= data.cd)               // 如果 計時器 >= 資料.冷卻
        {
            Attack();                       // 攻擊
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (ani.GetBool("死亡開關")) return;

        Vector3 posTarget = target.position;    // 區域變數三維向量 目標位置  = 目標.座標
        posTarget.y = transform.position.y;     // 目標位置.Y = 本身.Y
        transform.LookAt(posTarget);            // 變形.看著(目標位置)
        nav.SetDestination(target.position);    // 代理器.設定目的地(目標.座標)

        // 如果 代理器.剩餘距離 < 資料.停止距離
        if (nav.remainingDistance < data.stopDistance)
        {
            Wait();                                 // 等待
        }
        else
        {
            ani.SetBool("跑步開關", true);           // 走路
        }
    }

    // protected 保護：允許子類別存取，禁止外部類別存取
    // virtual 虛擬：允許子類別複寫
    /// <summary>
    /// 攻擊
    /// </summary>
    protected virtual void Attack()
    {
        ani.SetTrigger("攻擊觸發");          // 攻擊動畫
        timer = 0;                          // 歸零
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">接收玩家給予的傷害值</param>
    public void Hit(float damage)
    {
        if (ani.GetBool("死亡開關")) return;                                // 如果 死亡開關 是勾選 跳出
        hp -= damage;
        hpValueManager.SetHp(hp, data.hpMax);                          // 更新血量(目前，最大)
        StartCoroutine(hpValueManager.ShowValue(damage, "-", Color.white)); // 啟動協程
        if (hp <= 0) Dead();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        ani.SetBool("死亡開關", true);       // 死亡動畫
        nav.isStopped = true;               // 代理器 停止
        Destroy(this);                      // Destroy(GetComponent<元件>()); 刪除元件
        Destroy(gameObject, 0.3f);
        CreateCoin();
    }

    [Header("金幣")]
    public GameObject coin;

    private void CreateCoin()
    {
        // (int) 強制轉型 - 強制將浮點數轉為整數，去小數點
        int r = (int)Random.Range(data.coinRange.x, data.coinRange.y);

        for (int i = 0; i < r; i++)
        {
            Instantiate(coin, transform.position + transform.up * 2, transform.rotation);
        }
    }
}
