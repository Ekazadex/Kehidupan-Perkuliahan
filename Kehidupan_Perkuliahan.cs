using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}

class Game
{
    private Player player;
    private List<Enemy> enemies;
    private List<Item> items;
    private List<Quest> availableQuests;
    private Location currentLocation;
    private bool isGameRunning = true;

    public Game()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
    // Initialize player
    Console.WriteLine("=== Selamat Datang di Kehidupan Perkuliahan RPG ===");
    Console.Write("Masukkan nama karaktermu: ");
    string playerName = Console.ReadLine();
    Console.WriteLine("\nPilih fokus karakter:");
        Console.WriteLine("1. Fokus Serangan");
        Console.WriteLine("2. Fokus Pertahanan");
        Console.WriteLine("3. Fokus Kecepatan");
        Console.Write("Pilihan: ");
        string characterFocus = Console.ReadLine();

        // Initialize player with dynamic stats based on focus
        int health = 100, attack = 10, defense = 5, speed = 5;
        switch (characterFocus)
        {
            case "1": 
                attack += 3; 
                defense -= 1; 
                break;
            case "2": 
                defense += 3; 
                speed -= 1; 
                break;
            case "3": 
                speed += 3; 
                attack -= 1; 
                break;
        }
    // Buat objek Player
    player = new Player(playerName, health, attack, defense, speed);

    // Initialize enemies with location-specific enemies
    enemies = new List<Enemy>
    {
    new Enemy("Preman Kampus", 50, 8, 3, 4, 
        "Geng preman yang menganggu kegiatan mahasiswa di area kampus"),
    new Enemy("Pelanggan Menyebalkan", 40, 6, 2, 6, 
        "Pelanggan yang suka membuat masalah di tempat kerja part-time"),
    new Enemy("Mahasiswa Jahat", 60, 7, 4, 5, 
        "Mahasiswa yang suka mencuri buku dan mengganggu ketenangan perpustakaan")
    };

    // Initialize items
    items = new List<Item>
    {
           new Item("Buku Referensi", "Meningkatkan kemampuan akademik", "skill", 5, Location.Perpustakaan, 30),
            new Item("Kopi Energi", "Memulihkan stamina dan kesehatan", "health", 25, Location.Kafe, 20),
            new Item("Jaket Kampus", "Meningkatkan pertahanan", "defense", 3, Location.Kampus, 50),
            new Item("Tas Kuliah", "Menyimpan item tambahan", "inventory", 2, Location.Kampus, 40)
    };

    // Initialize quests
        availableQuests = new List<Quest>
        {
            new Quest("Tugas Kelompok", "Selesaikan tugas kelompok dengan mahasiswa lain", 50, 30),
            new Quest("Kerja Part-Time", "Kumpulkan uang tambahan di kafe", 40, 25),
            new Quest("Pertahankan Buku", "Lindungi buku penting dari pencuri di perpustakaan", 60, 35)
        };

    currentLocation = Location.Kampus;
    }

    public void Start()
    {
        while (isGameRunning)
        {
            ShowMainMenu();
            string choice = Console.ReadLine();
            ProcessMainMenuChoice(choice);
        }
    }

    private void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine($"\nLokasi: {currentLocation}");
        Console.WriteLine($"Status: {player.Name} | HP: {player.Health} | Level: {player.Level} | Exp: {player.Experience} | Money: {player.Money}");
        Console.WriteLine("\n=== Menu Utama ===");
        Console.WriteLine("1. Jelajahi Area");
        Console.WriteLine("2. Lihat Status");
        Console.WriteLine("3. Buka Inventori");
        Console.WriteLine("4. Ganti Lokasi");
        Console.WriteLine("5. Beli Item");
        Console.WriteLine("6. Keluar Game");
        Console.Write("\nPilihan: ");
    }

    private void ProcessMainMenuChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                ExploreArea();
                break;
            case "2":
                ShowPlayerStatus();
                break;
            case "3":
                ShowInventory();
                break;
            case "4":
                ChangeLocation();
                break;
            case "5":
                BuyItem();
                break;
            case "6":
                isGameRunning = false;
                Console.WriteLine("Terima kasih telah bermain!");
                break;
            default:
                Console.WriteLine("Pilihan tidak valid!");
                Thread.Sleep(1000);
                break;
        }
    }

    private void ExploreArea()
    {
        if (currentLocation == Location.Kafe)
        {
            Console.WriteLine("1. Jelajahi Area");
            Console.WriteLine("2. Kerja");
            Console.Write("\nPilihan: ");

            string subChoice = Console.ReadLine();
            if (subChoice == "2")
            {
                WorkAtCafe();
                return;
            }
        }

        Console.Clear();
        Console.WriteLine($"Menjelajahi {currentLocation}...");
        Thread.Sleep(1000);

        Random rnd = new Random();
        int eventChance = rnd.Next(1, 101);

        if (eventChance <= 40) // 40% chance for combat
        {
            InitiateCombat();
        }
        else if (eventChance <= 50) // 50% chance for finding item
        {
            FindItem();
        }
        else // 30% chance for nothing
        {
            Console.WriteLine("Tidak ada yang menarik di sekitar sini...");
            Thread.Sleep(1500);
        }
    }

    private void WorkAtCafe()
    {
        Console.Clear();
        Console.WriteLine("Sedang bekerja...");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }

        Console.WriteLine("\nKerja selesai! Kamu mendapatkan 40 uang.");
        player.Money += 40;
        Thread.Sleep(1500);
    }

    private void InitiateCombat()
    {
        Random rnd = new Random();
        Enemy enemy = null;
         // Select enemy based on current location
        switch (currentLocation)
        {
        case Location.Kampus:
            enemy = enemies.First(e => e.Name == "Preman Kampus");
            break;
        case Location.Kafe:
            enemy = enemies.First(e => e.Name == "Pelanggan Menyebalkan");
            break;
        case Location.Perpustakaan:
            enemy = enemies.First(e => e.Name == "Mahasiswa Jahat");
            break;
    }

    Enemy currentEnemy = new Enemy(enemy.Name, enemy.Health, enemy.Attack, enemy.Defense, enemy.Speed, enemy.Description);

    Console.WriteLine($"\nKamu bertemu dengan {currentEnemy.Name}!");
    Console.WriteLine($"Deskripsi: {currentEnemy.Description}");
    Thread.Sleep(1000);


        while (currentEnemy.Health > 0 && player.Health > 0)
        {
            Console.Clear();
            Console.WriteLine($"\n{currentEnemy.Name} | HP: {currentEnemy.Health}");
            Console.WriteLine($"{player.Name} | HP: {player.Health}");
            Console.WriteLine("\n1. Serang");
            Console.WriteLine("2. Gunakan Item");
            Console.WriteLine("3. Coba Kabur");
            Console.Write("\nPilihan: ");

            string combatChoice = Console.ReadLine();
            switch (combatChoice)
            {
                case "1":
                    // Player attacks
                    int damage = Math.Max(1, player.Attack - currentEnemy.Defense);
                    currentEnemy.Health -= damage;
                    Console.WriteLine($"\nKamu menyerang {currentEnemy.Name} dan memberikan {damage} damage!");
                    Thread.Sleep(1000);

                    // Enemy attacks if still alive
                    if (currentEnemy.Health > 0)
                    {
                        int enemyDamage = Math.Max(1, currentEnemy.Attack - player.Defense);
                        player.Health -= enemyDamage;
                        Console.WriteLine($"{currentEnemy.Name} menyerang dan memberikan {enemyDamage} damage!");
                        Thread.Sleep(1000);
                    }
                    break;

                case "2":
                    ShowInventory();
                    break;

                case "3":
                    if (rnd.Next(1, 101) <= 40) // 40% chance to escape
                    {
                        Console.WriteLine("Berhasil kabur!");
                        Thread.Sleep(1000);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Gagal kabur!");
                        int escapeDamage = Math.Max(1, currentEnemy.Attack - player.Defense);
                        player.Health -= escapeDamage;
                        Console.WriteLine($"{currentEnemy.Name} menyerang dan memberikan {escapeDamage} damage!");
                        Thread.Sleep(1000);
                    }
                    break;
            }
        }

        if (currentEnemy.Health <= 0)
        {
            Console.WriteLine($"\nKamu mengalahkan {currentEnemy.Name}!");
            int expGained = rnd.Next(10, 21);
            player.GainExperience(expGained);
            Console.WriteLine($"Mendapatkan {expGained} exp!");
            Thread.Sleep(1500);
        }
        else if (player.Health <= 0)
        {
            Console.WriteLine("\nKamu kalah...");
            player.Health = 100; // Reset health
            Thread.Sleep(1500);
        }
    }

    private void FindItem()
    {
        Random rnd = new Random();
        Item foundItem = items[rnd.Next(items.Count)];
        Console.WriteLine($"\nKamu menemukan {foundItem.Name}!");
        player.AddItem(foundItem);
        Thread.Sleep(1500);
    }

    private void ShowPlayerStatus()
    {
        Console.Clear();
        Console.WriteLine("\n=== Status Karakter ===");
        Console.WriteLine($"Nama: {player.Name}");
        Console.WriteLine($"Level: {player.Level}");
        Console.WriteLine($"Experience: {player.Experience}/100");
        Console.WriteLine($"Health: {player.Health}");
        Console.WriteLine($"Attack: {player.Attack}");
        Console.WriteLine($"Defense: {player.Defense}");
        Console.WriteLine($"Speed: {player.Speed}");
        Console.WriteLine($"Money: {player.Money}");
        Console.WriteLine("\nTekan Enter untuk kembali...");
        Console.ReadLine();
    }

    private void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("\n=== Inventori ===");
        
        if (player.Inventory.Count == 0)
        {
            Console.WriteLine("Inventori kosong!");
        }
        else
        {
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name} - {player.Inventory[i].Description}");
            }
            Console.WriteLine("\nPilih nomor item untuk menggunakannya (0 untuk kembali):");
            
            if (int.TryParse(Console.ReadLine(), out int itemChoice) && itemChoice > 0 && itemChoice <= player.Inventory.Count)
            {
                Item selectedItem = player.Inventory[itemChoice - 1];
                UseItem(selectedItem);
                player.Inventory.RemoveAt(itemChoice - 1);
            }
        }
        Thread.Sleep(1500);
    }

    private void ShowQuests()
    {
        Console.Clear();
        Console.WriteLine("\n=== Daftar Quest ===");
        for (int i = 0; i < availableQuests.Count; i++)
        {
            Quest quest = availableQuests[i];
            Console.WriteLine($"{i + 1}. {quest.Name} - Reward: {quest.ExpReward} EXP, {quest.MoneyReward} Uang");
            Console.WriteLine($"   {quest.Description}");
        }
        Console.WriteLine("\nTekan Enter untuk kembali...");
        Console.ReadLine();
    }

    private void UseItem(Item item)
    {
        switch (item.Type)
        {
            case "health":
                player.Health = Math.Min(100, player.Health + item.Value);
                Console.WriteLine($"Menggunakan {item.Name}. Health bertambah {item.Value}!");
                break;
            case "attack":
                player.Attack += item.Value;
                Console.WriteLine($"Menggunakan {item.Name}. Attack bertambah {item.Value}!");
                break;
            case "defense":
                player.Defense += item.Value;
                Console.WriteLine($"Menggunakan {item.Name}. Defense bertambah {item.Value}!");
                break;
        }
        Thread.Sleep(1500);
    }

    private void BuyItem()
    {
    Console.Clear();
    Console.WriteLine("\n=== Daftar Item yang Dijual ===");
    for (int i = 0; i < items.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {items[i].Name} - {items[i].Description} | Harga: {items[i].Price}");
    }
    Console.WriteLine("0. Kembali");
    Console.Write("\nPilih item untuk dibeli: ");

    if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= items.Count)
    {
        Item selectedItem = items[choice - 1];
        if (player.Money >= selectedItem.Price)
        {
            player.Money -= selectedItem.Price;
            player.AddItem(selectedItem);
            Console.WriteLine($"Kamu membeli {selectedItem.Name}!");
        }
        else
        {
            Console.WriteLine("Uangmu tidak cukup!");
        }
    }
    else if (choice != 0)
    {
        Console.WriteLine("Pilihan tidak valid.");
    }
    Thread.Sleep(1500);
    }

    private void ChangeLocation()
    {
        Console.Clear();
        Console.WriteLine("\n=== Pilih Lokasi ===");
        Console.WriteLine("1. Kampus");
        Console.WriteLine("2. Kafe");
        Console.WriteLine("3. Perpustakaan");
        Console.Write("\nPilihan: ");

        string locationChoice = Console.ReadLine();
        switch (locationChoice)
        {
            case "1":
                currentLocation = Location.Kampus;
                break;
            case "2":
                currentLocation = Location.Kafe;
                break;
            case "3":
                currentLocation = Location.Perpustakaan;
                break;
            default:
                Console.WriteLine("Lokasi tidak valid!");
                Thread.Sleep(1000);
                break;
        }
    }
}

// New Quest class to support side quests
class Quest
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int ExpReward { get; private set; }
    public int MoneyReward { get; private set; }

    public Quest(string name, string description, int expReward, int moneyReward)
    {
        Name = name;
        Description = description;
        ExpReward = expReward;
        MoneyReward = moneyReward;
    }
}

class Player
{
    public string Name { get; private set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public int Money { get; set; }
    public List<Item> Inventory { get; private set; }

    public Player(string name, int health, int attack, int defense, int speed)
    {
        Name = name;
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Level = 1;
        Experience = 0;
        Money = 0;
        Inventory = new List<Item>();
    }

    public void GainExperience(int exp)
    {
        Experience += exp;
        if (Experience >= 100)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience -= 100;
        Attack += 2;
        Defense += 1;
        Speed += 1;
        Health = 100;
        Money += 30;
        Console.WriteLine($"\nLevel Up! Sekarang level {Level}!");
        Console.WriteLine("Status meningkat!");
        Thread.Sleep(1500);
    }

    public void AddItem(Item item)
    {
        Inventory.Add(item);
    }
}

class Enemy
{
    public string Name { get; private set; }
    public int Health { get; set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Speed { get; private set; }
    public string Description { get; private set; }

    public Enemy(string name, int health, int attack, int defense, int speed, string description)
    {
        Name = name;
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Description = description;
    }
}


class Item
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Type { get; private set; }
    public int Value { get; private set; }
    public Location ItemLocation { get; private set; }
    public int Price { get; private set; }

    public Item(string name, string description, string type, int value, Location itemLocation, int price)
    {
        Name = name;
        Description = description;
        Type = type;
        Value = value;
        ItemLocation = itemLocation;
        Price = price;
    }
}


enum Location
{
    Kampus,
    Kafe,
    Perpustakaan
}