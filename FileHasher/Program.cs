/*
    FileHasher - Simple file hashing console app
    Copyright (C) 2018 Peter Wetzel

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
using CommandLine;
using System;
using System.IO;
using System.Security.Cryptography;

namespace FileHasher
{
    class Program
    {
        private const int BufferSize = 1200000;

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<FileHasherOptions>(args)
                .MapResult(options => Process(options), _ => 1);
        }

        public static int Process(FileHasherOptions options)
        {
            if (!File.Exists(options.FilePath))
            {
                Console.WriteLine($"File does not exist: {options.FilePath}");
                return 1;
            }
            Console.WriteLine("Calculating...");
            string hash = CalculateMD5Hash(options.FilePath);
            Console.WriteLine(hash);
            Console.WriteLine(hash.Replace("-", "").ToLower());
            return 0;
        }

        public static string CalculateMD5Hash(string filePath)
        {
            using (var hasher = new MD5CryptoServiceProvider())
            {
                byte[] hashvalue;
                using (var stream = new BufferedStream(File.OpenRead(filePath), BufferSize))
                {
                    stream.Flush();
                    hashvalue = hasher.ComputeHash(stream);
                }
                return BitConverter.ToString(hashvalue);
            }
        }
    }
}