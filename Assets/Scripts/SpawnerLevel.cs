using Meta.WitAi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLevel : MonoBehaviour
{
    public GameObject Player;
    public GameObject Zombie;
    public Transform SpawnArea;
    public List<Level> listOfLevels;
    public int currentLevel;

    Level level;
    float lastSpawnedWave;
    List<GameObject> listOfZombies = new List<GameObject>();
    public Stats stats = new Stats();
    bool alive = true;

    public static SpawnerLevel Instance { get; private set; }
    void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;
            // Optionally, persist this instance between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If instance already exists and it's not this, then destroy this to enforce the singleton pattern
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = -1;
        NextLevel();
    }
    void NextLevel()
    {
        foreach (GameObject zombie in listOfZombies)
        {
            zombie.DestroySafely();
        }

        listOfZombies.Clear();
        stats.CurrentWaveZombiesKilled = 0;
        currentLevel++;
        level = listOfLevels[currentLevel];
        lastSpawnedWave = level.SpawnRate - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (level != null)
        {
            if (level.ZombiesToClear() > 0 && lastSpawnedWave > level.SpawnRate)
            {
                if (CanSpawnWave())
                    SpawnZombieWave();
            }

            else if (level.ZombiesToClear() <= 0)
            {
                NextLevel();
            }

            lastSpawnedWave += Time.deltaTime;
        }

        else
        {
        }

        if (alive)
        {
            stats.TimeSurvived += Time.deltaTime;
        }
    }

    bool CanSpawnWave()
    {
        int zombiesAlive = listOfZombies.Count - stats.CurrentWaveZombiesKilled;

        //Debug.Log(listOfZombies.Count);
        //Debug.Log(stats.CurrentWaveZombiesKilled);

        if (zombiesAlive + level.SpawnNumber <= level.MaximumZombiesSpawned)
        {
            return true;
        }

        return false;
    }

    void SpawnZombieWave()
    {
        for (int i = 0; i < level.SpawnNumber; i++)
        {
            if (level.NumberToClear > listOfZombies.Count)
            {
                // Calculate a random position within the spawn area
                Vector3 spawnPosition = new Vector3(
                    UnityEngine.Random.Range(SpawnArea.position.x - SpawnArea.localScale.x / 2, SpawnArea.position.x + SpawnArea.localScale.x / 2),
                    Zombie.transform.position.y,
                    UnityEngine.Random.Range(SpawnArea.position.z - SpawnArea.localScale.z / 2, SpawnArea.position.z + SpawnArea.localScale.z / 2)
                );

                // Instantiate the zombie at the calculated position
                GameObject zombie = Instantiate(Zombie, spawnPosition, Quaternion.identity);
                zombie.transform.LookAt(new Vector3(Player.transform.position.x, zombie.transform.position.y, Player.transform.position.z));
                AssignNewZombieValues(zombie);
            }
        }

        lastSpawnedWave = 0;
    }

    void AssignNewZombieValues(GameObject zombie)
    {
        zombie.SetActive(true);
        Zombie zombieScript = zombie.GetComponent<Zombie>();
        zombieScript.Armor = level.Armor;
        zombieScript.AttackDamage = level.AttackDamage;
        zombieScript.AttackSpeed = level.AttackSpeed;
        zombieScript.Speed = level.MovementSpeed;

        listOfZombies.Add(zombie);
    }

    public void KillZombie()
    {
        level.KillZombie();
        stats.KilledZombies++;
        stats.CurrentWaveZombiesKilled++;
    }

    public class Stats
    {
        public float KilledZombies;
        public float SurvivedWaves;
        public float PancakesMade;
        public float PancakesDropped;
        public float TimeSurvived;

        public int CurrentWaveZombiesKilled;
    }

    [Serializable]
    public class Level
    {
        public float MovementSpeed;
        public float AttackDamage;
        public float AttackSpeed;
        public float Armor;
        public int NumberToClear;
        public float SpawnNumber;
        public int SpawnRate;
        public int MaximumZombiesSpawned;
        int killedZombies;

        public Level(float attackDamage, float attackSpeed, float armor, int numberToClear, float spawnNumber, int spawnRate, float movementSpeed, int maximumZombiesSpawned)
        {
            this.AttackDamage = attackDamage;
            this.Armor = armor;
            this.AttackSpeed = attackSpeed;
            this.NumberToClear = numberToClear;
            this.SpawnNumber = spawnNumber;
            this.SpawnRate = spawnRate;
            this.MovementSpeed = movementSpeed;
            this.MaximumZombiesSpawned = maximumZombiesSpawned;

        }

        public bool KillZombie()
        {
            if (NumberToClear <= killedZombies)
                return false;

            killedZombies++;
            return true;
        }
        
        public int ZombiesToClear()
        {
            return NumberToClear - killedZombies;
        }
    }
}
