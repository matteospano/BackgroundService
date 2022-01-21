using AE.Net.Mail;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TechEdge.Mail.Receiver
{
    class Program
    {
        public const string FileFormatToken = "=?utf-8?Q?";
        public const string PECMailFileName = "postacert.eml";
        public const string ConfigFileName = "PECDownload.json";

        static void Main()
        {
            if (!File.Exists(ConfigFileName))
            {
                Console.WriteLine("Please create PECDownload.json configuration file");
                return;
            }

            byte[] Buffer = new byte[0];
            Configuration Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigFileName));
            //using (var pop = new ImapClient(Config.Host, Config.UserName, Config.Password, 0, Config.Port, Config.UseSSL))
            using (Pop3Client pop = new Pop3Client(Config.Host, Config.UserName, Config.Password, Config.Port, Config.UseSSL))
            {
                var MsgCount = pop.GetMessageCount();
                for (var i = MsgCount - 1; i >= 0; i--)
                {
                    var msg = pop.GetMessage(i, false);
                    foreach (var item in msg.Attachments)
                    {
                        if (string.Compare(PECMailFileName, item.Filename, true) != 0)
                            continue;

                        Buffer = Buffer.Concat(item.GetData()).ToArray();
                        Console.WriteLine($"LOG: {msg.Date},{msg.Subject},{PECMailFileName}"); //data,titolo,nomeFile
                    }
                }
                //for (var i = MsgCount - 1; i >= 0; i--)
                //{
                //    MemoryStream stream = new MemoryStream(Buffer);
                //    StreamReader SR = new StreamReader(stream);
                //    MailMessage M = MailMessage.FromStream(SR);
                //    foreach (var fileMessage in M.Attachments)
                //    {
                //        var FName = fileMessage.FName;
                //        if (FName.StartsWith(FileFormatToken))
                //        {
                //            FName = FName.Replace(FileFormatToken, string.Empty);
                //            if (FName.EndsWith("="))
                //                FName = FName.Substring(0, FName.Length - 1);

                //            FName = FName.
                //                    Replace("=", "%").
                //                    Replace("?", string.Empty);

                //            FName = Uri.UnescapeDataString(FName);                            
                //        }
                //        Console.WriteLine($"{FName} read succesfully");
                //    }
                //}
            }
            Console.WriteLine("---------------------------------check buffer:---------------------------------");

            MemoryStream stream = new MemoryStream(Buffer);
            StreamReader SR = new StreamReader(stream);
            MailMessage M = MailMessage.FromStream(SR);
            foreach (var fileMessage in M.Attachments)
            {
                var FName = fileMessage.FName;
                if (FName.StartsWith(FileFormatToken))
                {
                    FName = FName.Replace(FileFormatToken, string.Empty);
                    if (FName.EndsWith("="))
                        FName = FName.Substring(0, FName.Length - 1);

                    FName = FName.
                            Replace("=", "%").
                            Replace("?", string.Empty);

                    FName = Uri.UnescapeDataString(FName);
                }
                Console.WriteLine($"{FName} read succesfully");
            }
            Console.WriteLine("---------------------------------Process end---------------------------------");
            Console.ReadKey();
        }
    }
}
