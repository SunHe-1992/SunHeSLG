using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerHealthTests
{
    private GameObject playerObject;
    private PlayerHealth playerHealth;

    [SetUp]
    public void SetUp()
    {
        playerObject = new GameObject();
        playerHealth = playerObject.AddComponent<PlayerHealth>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(playerObject);
    }

    [Test]
    public void PlayerHealth_InitialHealth_IsMaxHealth()
    {
        Assert.AreEqual(playerHealth.maxHealth, playerHealth.currentHealth);
    }

    [Test]
    public void PlayerHealth_TakeDamage_DecreasesHealth()
    {
        int damage = 30;
        playerHealth.TakeDamage(damage);
        Assert.AreEqual(playerHealth.maxHealth - damage, playerHealth.currentHealth);
    }

    [Test]
    public void PlayerHealth_TakeDamage_HealthDoesNotGoBelowZero()
    {
        int damage = 150;
        playerHealth.TakeDamage(damage);
        Assert.AreEqual(0, playerHealth.currentHealth);
    }

    [Test]
    public void PlayerHealth_Heal_IncreasesHealth()
    {
        playerHealth.currentHealth = 50;
        int healAmount = 30;
        playerHealth.Heal(healAmount);
        Assert.AreEqual(80, playerHealth.currentHealth);
    }

    [Test]
    public void PlayerHealth_Heal_HealthDoesNotExceedMaxHealth()
    {
        playerHealth.currentHealth = 90;
        int healAmount = 20;
        playerHealth.Heal(healAmount);
        Assert.AreEqual(playerHealth.maxHealth, playerHealth.currentHealth);
    }

    [Test]
    public void PlayerHealth_SortBySpeed()
    {
        int[] speedArray = new int[20] { 45, 50, 55, 60, 65, 70, 75, 80, 85, 90,
         95, 100, 105, 110, 115, 120, 125, 130, 135, 140 };
        playerHealth.SortBySpeed(ref speedArray);
        Debug.Break();
        string print = "";
        foreach (int value in speedArray)
        {
            print += (value + " ");
        }
        Debug.Log(print);
    }
}
