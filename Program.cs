using System;
using System.Diagnostics;
//using System.Drawing;
using System.IO;
//using System.Collections.Generic;
//using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DSDCModInstaller
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            RegistryKey installPath;

            if (Environment.Is64BitOperatingSystem)
                installPath = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            else
                installPath = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            string _value = installPath.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Death Stranding Directors Cut_is1")?.GetValue("InstallLocation")?.ToString();

            if (_value != null)
            {
                string _path = '\u0022' + _value.Remove(_value.Length - 1) + '\u0022'; // обрезка последнего бекслеша и заключение в кавычки
                string _toolkit = Path.GetFullPath(@"Decima\decima.bat"); // Путь к Decima Workshop
                string _sourcesTMP = Path.GetFullPath(@"Sources"); // Путь к папке с ресурсами для перепаковки
                string _sources = '\u0022' + _sourcesTMP + '\u0022'; // заключаем путь к ресурсам в кавычки

                //Console.WriteLine(_value); // тестовая, подлежит удалению.
                //Console.WriteLine(_path); // тестовая, подлежит удалению.
               // Console.WriteLine(_toolkit); // тестовая, подлежит удалению.
               //Console.WriteLine(_sources); // тестовая, подлежит удалению.
                string _target = @"data\59b95a781c9170b0d13773766e27ad90.bin";
                string _targetFile = '\u0022' + _value + _target + '\u0022'; // заключаем путь до целевого файла в кавычки
               // Console.WriteLine(_targetFile); // тестовая, подлежит удалению.

                // Отправка готовой команды для DW CLI
                Process process = new Process();
                {
                    process.StartInfo.FileName = _toolkit; //путь к приложению, которое будем запускать
                    process.StartInfo.WorkingDirectory = Path.GetFullPath(@"Decima\"); //путь к рабочей директории приложения
                    process.StartInfo.Arguments = "repack " + "--backup " + "--project " + _path + " " + "--rebuild-prefetch " + "--changed-files-only " + _targetFile + " " + _sources +">log.txt"; //аргументы командной строки (параметры)
                    process.Start();
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey(true);
                };

            }
            else
            {
                Console.WriteLine("No registry key found; Please pick ds.exe manualy"); // Начало пикера.
                string _toolkit = Path.GetFullPath(@"Decima\decima.bat"); // Путь к Decima Workshop
                string _sourcesTMP = Path.GetFullPath(@"Sources"); // Путь к папке с ресурсами для перепаковки
                string _sources = '\u0022' + _sourcesTMP + '\u0022'; // заключаем путь к ресурсам в кавычки
//                Console.WriteLine(_toolkit); // тестовая, подлежит удалению.
//                Console.WriteLine(_sources); // тестовая, подлежит удалению.
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "Game executable exe|ds.exe"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string _path = '\u0022' + Path.GetDirectoryName(ofd.FileName) + '\u0022'; // получаем путь к папке игры из выбора и заключаем в кавычки
                    string _path2 = Path.GetDirectoryName(ofd.FileName); // получаем путь к папке игры из выбора
                    string _target = @"\data\59b95a781c9170b0d13773766e27ad90.bin";
                    string _targetFile = '\u0022' + _path2 + _target + '\u0022'; // заключаем путь до целевого файла в кавычки
                    Process process = new Process();
                    {
                        process.StartInfo.FileName = _toolkit; //путь к приложению, которое будем запускать
                        process.StartInfo.WorkingDirectory = Path.GetFullPath(@"Decima\"); //путь к рабочей директории приложения
                        process.StartInfo.Arguments = "repack " + "--backup " + "--project " + _path + " " + "--rebuild-prefetch " + "--changed-files-only " + _targetFile + " " + _sources + ">log.txt"; //аргументы командной строки (параметры)
                        process.Start();
                        
                    };
                    //Console.WriteLine(_toolkit + " " + "repack " + "--backup " + "--project " + _path + " " + "--rebuild-prefetch " + "--changed-files-only " + _targetFile + " " + _sources); // тестовая, подлежит удалению.
                }
                Console.WriteLine("Press any key to exit");
                Console.ReadKey(true);
              
            }
        }   
    }
}