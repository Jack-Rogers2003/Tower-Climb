using System.Xml;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;


/*To Save:
 * Who's current turn is it
 * What is the enemy unit's stats
 * What is the player unit's stats
 * What abilities does the player currently have
 * What state is either of them in
 * How many battles has the current player won
 */

public static class SaveGame
{
    private static readonly string saveFilePath = "Assets/Resources/Save/SaveFile.txt";

    public static void Save(Hero hero, Enemy enemy, Unit currentTurn)
    {
        string content = "";
        if (File.Exists(saveFilePath))
        {
            try
            {
                File.Delete(saveFilePath);
                Debug.Log("Existing file deleted.");
            }
            catch (IOException ioEx)
            {
                Debug.LogError("Error deleting the file: " + ioEx.Message);
                return;
            }
        }

        content += EncryptString(PlayerPrefs.GetString("BattleCount", "0")) + "\n";

        if (currentTurn is Hero)
        {
            content += EncryptString("Hero") + "\n";
        } else
        {
            content += EncryptString("Enemy") + "\n";
        }

        foreach(AbilityData ability in hero.GetAbilites())
        {
            content += EncryptString(ability.ToString()) +"\n";
        }

        content += EncryptString(hero.GetMaxHP().ToString()) + "\n" + EncryptString(hero.GetCurrentHP().ToString()) + "\n" + EncryptString(hero.GetCurrentState().ToString()) + "\n";
        content += EncryptString(enemy.GetMaxHP().ToString()) + "\n" + EncryptString(enemy.GetCurrentHP().ToString()) + "\n" + EncryptString(enemy.GetCurrentState().ToString()) + "\n";
        File.WriteAllText(saveFilePath, content);
    }

    public static List<string> Load()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("SaveFile not found in Resources!");
        }

        List<string> savedContents = File.ReadAllText(saveFilePath).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        for (int i = 0; i < savedContents.Count; i++)
        {
            savedContents[i] = DecryptString(savedContents[i]);
        }
        return savedContents;
    }

    private static string EncryptString(string input)
    {
        string key = DatabaseManager.GetCurrentUserID();

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
        aesAlg.IV = new byte[16];

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using MemoryStream msEncrypt = new();
        using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using StreamWriter swEncrypt = new(csEncrypt);
            swEncrypt.Write(input);
        }
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    private static string DecryptString(string text)
    {
        string key = DatabaseManager.GetCurrentUserID();
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
        aesAlg.IV = new byte[16];

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        byte[] buffer = Convert.FromBase64String(text);

        using MemoryStream msDecrypt = new(buffer);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        return srDecrypt.ReadToEnd();
    }

}
