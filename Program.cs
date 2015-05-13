using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace HWP_EN_Searcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HWP_En_Searcher [ Z2est(namegpark) ]");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();

            Read_HwpList();
            Console.ReadLine();
        }
        static public void Read_HwpList()
        {
            StreamReader sr = new StreamReader(File.Open("HwpList.txt", FileMode.Open));
            while (!sr.EndOfStream)
            {
                string path = sr.ReadLine();
                Search(path);
            }
            sr.Close();
        }
        static public void Search(string path)
        {
            Console.WriteLine("===========================================");
            FileStream fs = File.Open(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs,Encoding.ASCII);
            StringBuilder sr = new StringBuilder();
            br.BaseStream.Position = 0; // File Header Section 
            byte[] hwp = br.ReadBytes((int)br.BaseStream.Length);
            Match m = Regex.Match(Encoding.ASCII.GetString(hwp), "HWP Document File");
            if(m.Success)
            {
                br.BaseStream.Position = m.Index;
                Console.WriteLine("Found! HWP File Document Section Index : {0}", m.Index);
                br.BaseStream.Position = m.Index + 36;
                byte[] en_flag = br.ReadBytes(1);
                string tmp = BitConverter.ToString(en_flag);

                int num16 = System.Convert.ToInt32(tmp + "00", 16);
                string enflagbit = System.Convert.ToString(num16, 2);
                Console.WriteLine("Encrypted Flag bit : " + enflagbit[1]);
                if (enflagbit[1].ToString().Contains("1") == true)
                {
                    Console.WriteLine("[ Encrypt : True ]");
                    Console.WriteLine("Path : " + path);
                }
                else
                {
                    Console.WriteLine("[ Encrypt : False ]");
                    Console.WriteLine("Path : " + path);
                }
                Console.WriteLine("===========================================");
                Console.WriteLine();
            }
            fs.Close();
            br.Close();
        }
    }
}
