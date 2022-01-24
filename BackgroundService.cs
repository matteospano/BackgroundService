using System;

namespace App.WindowsService
{
    public sealed class MainClass
    {               
        static void Main()
        {
            PECMail.Program _mailReader= new PECMail.Program();
            SFTP.Class1 _class1 = new SFTP.Class1();

            Console.WriteLine("BackgroundService chiama Mail:");
            byte[] Buffer = _mailReader.Main();
            Console.WriteLine("MainBackgroundService chiama SFTP:");
            _class1.Main(Buffer);
            Console.WriteLine("---------------------------------Process end-----------------------------------");
            Console.ReadKey();
        }
    }
}