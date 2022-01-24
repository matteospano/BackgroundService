using System;

namespace App.WindowsService
{
    public sealed class MainClass
    {               
        static void Main()
        {
            PECMail.Program _mailReader= new PECMail.Program();
            SFTP.Class1 _class1 = new SFTP.Class1();
            Console.WriteLine("MainBackgroundService chiama Mail:");
            _mailReader.Main();//restituirà il buffer
            Console.WriteLine("MainBackgroundService chiama SFTP:");
            _class1.Main();
            Console.WriteLine("---------------------------------Mail end-----------------------------------");
            Console.ReadKey();
        }
    }
}