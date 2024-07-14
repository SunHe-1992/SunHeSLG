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
}
