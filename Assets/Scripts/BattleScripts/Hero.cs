using System.IO;

public class Hero: Unit
{
    private string[][] abilities;

    public void GetAbilities(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            int numberOfAbilities = lines.Length / 4;
            abilities = new string[numberOfAbilities][];

            for (int i = 0; i < numberOfAbilities; i++)
            {
                abilities[i] = new string[4];
                for (int j = 0; j < 4; j++)
                    abilities[i][j] = lines[i * 4 + j];
            }
        }
    }

}
