using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含：
/// 1.生成随机横坐标并生成敌人
/// 2.加速生成
/// 3.生成Boss
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    //敌人预制体
    public GameObject Enemy;
    //敌人出生冷却
    public float spawnCold { get; private set; }
    //敌人出生冷却计时器
    public float spawnColdCount { get; private set; }

    //敌人最快出生速度
    public float maxSpawnCold { get; private set; }
    //难度增加时出生速度增量
    public float SpawnColdRise { get; private set; }

    //存储地图最小X值
    public float mapMinX { get; private set; }
    //存储地图最大X值
    public float mapMaxX { get; private set; }

    //Boss预制体
    public GameObject Boss;

    //随机横向坐标
    private Vector3 randomX;

    private void Awake()
    {
        //敌人出生冷却时间
        spawnCold = 2.0f;
        spawnColdCount = 2.0f;
        maxSpawnCold = 1.0f;
        SpawnColdRise = 0.2f;
        //拿到地图横向的最大最小值
        mapMinX = FindObjectOfType<Utils>().getMapMinX();
        mapMaxX = FindObjectOfType<Utils>().getMapMaxX();
    }

    private void FixedUpdate()
    {
        EnemySpawnCold();
    }

    //出生冷却器
    private void EnemySpawnCold()
    {
        if (spawnColdCount >= spawnCold)
        {
            Instantiate(Enemy, new Vector3(GetRandomX().x, transform.position.y, transform.position.z),transform.rotation);
            spawnColdCount = 0;
        }
        else
        {
            spawnColdCount += Time.deltaTime;
        }
    }

    //获得一个随机的横向坐标
    private Vector3 GetRandomX()
    {
        randomX.x = Random.Range(mapMinX + 0.3f, mapMaxX - 0.3f);
        return randomX;
    }

    //出生速度加快，最快为maxSpawnCold
    public void SpawnFaster()
    {
        if (spawnCold > maxSpawnCold)
        {
            spawnCold -= SpawnColdRise;
            FindObjectOfType<PlayingManager>().DifficutLevelRise();
        }
    }

    //Boss出生
    public void BossSpawn()
    {
        Instantiate(Boss, transform.position, transform.rotation);
    }
}
