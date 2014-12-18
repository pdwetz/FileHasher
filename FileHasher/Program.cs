/*
    FileHasher - Simple file hashing console app
    Copyright (C) 2014 Peter Wetzel

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FileHasher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FileHasher   Copyright (C) 2014 Peter Wetzel");
            Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY; for details see license.txt.");
            Console.WriteLine();
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("File path required");
                return;
            }

            if (args[0] == "/?" || args[0] == "/h" || args[0] == "-?" || args[0] == "-h")
            {
                Console.WriteLine("filehasher[.exe] [filepath]");
                Console.WriteLine(@"    filepath - Can be relative or full path (e.g. c:\my files\image.iso)");
                return;
            }

            string filePath = args[0];
            if (args.Length > 1)
            {
                filePath = string.Join(" ", args);
            }
            Process(filePath);
        }

        public static async void Process(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist: {0}", filePath);
                return;
            }
            Console.WriteLine("Calculating...");
            string hash = await CalculateMD5Hash(filePath);
            Console.WriteLine(hash);
            Console.WriteLine(hash.Replace("-", "").ToLower());
        }

        public static async Task<string> CalculateMD5Hash(string sFilePath)
        {
            using (MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider())
            {
                byte[] hashvalue;
                using (var stream = new BufferedStream(File.OpenRead(sFilePath), 1200000))
                {
                    await stream.FlushAsync();
                    hashvalue = hasher.ComputeHash(stream);
                }
                return System.BitConverter.ToString(hashvalue);
            }
        }
    }
}
