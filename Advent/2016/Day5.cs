using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day5
{
    public string Input;

    public Day5()
    {
        Input = "reyedfim";
        Input = "abc";
    }

    public void Solve()
    {
        var number = 0;
        var hasher = MD5.Create();
        var password = "";
        var password2 = new char[8] {'_', '_', '_', '_', '_', '_', '_', '_'};
        while (true)
        {
            string value = $"{Input}{number}";
            var hash = Hash(value);
            if(hash.Take(5).All(x => x == '0'))
            {
                var pass1Done = password.Length == 8;
                var pass2Done = !password2.Any(c => c == '_');

                if (!pass1Done)
                {
                    password += (char)hash[5];
                }

                if (!pass2Done)
                {
                    try
                    {
                        var index = int.Parse(hash[5].ToString());
                        if(password2[index] == '_')
                            password2[index] = hash[6];
                    }
                    catch { }
                }

                Console.WriteLine($"{number} | {password} | {new string(password2)}");
                pass1Done = password.Length == 8;
                pass2Done = !password2.Any(c => c == '_');

                if (pass1Done && pass2Done) break;
            }
            number++;
        }
        Console.WriteLine(password);
        Console.WriteLine(new string(password2));
    }

    public static string Hash(string input)
    {
        // Use input string to calculate MD5 hash
        using MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to hexadecimal string
        StringBuilder sb = new();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
