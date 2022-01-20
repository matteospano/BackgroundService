using AE.Net.Mail;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            if (!File.Exists(ConfigFileName))
            {
                Console.WriteLine("Please create PECDownload.json configuration file");
                return;
            }

            Configuration Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigFileName));

            string DailyFolderPath = Path.Combine(Config.OutFolder, DateTime.Now.ToString("yyyyMMdd"));

            if (!Directory.Exists(DailyFolderPath))
            {
                Console.WriteLine($"Create Folder {DailyFolderPath}");
                Directory.CreateDirectory(DailyFolderPath);
            }

            using (var pop = new Pop3Client(Config.Host, Config.UserName, Config.Password, Config.Port, Config.UseSSL))
            {
                var MsgCount = pop.GetMessageCount();

                Console.WriteLine($"Detected {MsgCount} messages");

                for (var i = MsgCount - 1; i >= 0; i--)
                {
                    var msg = pop.GetMessage(i, false);
                    Console.WriteLine($"Processing Mail {msg.Subject} ....");

                    var MailFolderName = string.IsNullOrEmpty(msg.Subject) ? string.Concat("Mail-", DateTime.Now.ToString("HHmmss")) : msg.Subject.Substring(0, Math.Min(msg.Subject.Length, 255));
                    MailFolderName = Regex.Replace(MailFolderName, @"[^0-9a-zA-Z]+", " ");
                    var MailFolder = Path.Combine(DailyFolderPath, MailFolderName);

                    if (!Directory.Exists(MailFolder))
                    {
                        Console.WriteLine($"Create Folder {MailFolder}");
                        Directory.CreateDirectory(MailFolder);
                    }

                    foreach (var item in msg.Attachments)
                    {
                        if (string.Compare(PECMailFileName, item.Filename, true) != 0)
                            continue;

                        string FileName = Path.Combine(MailFolder, item.Filename);

                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                            Console.WriteLine($"Delete File {FileName}");
                        }

                        Console.WriteLine($"Create File {FileName}");
                        using (FileStream SW = new FileStream(FileName, FileMode.CreateNew))
                        {
                            var Buffer = item.GetData();
                            SW.Write(Buffer, 0, Buffer.Length);
                        }
                    }

                    string MailFile = Path.Combine(MailFolder, PECMailFileName);

                    if (File.Exists(MailFile))
                    {
                        using (StreamReader SR = new StreamReader(MailFile))
                        {
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

                                FName = Path.Combine(MailFolder, FName);
                                if (File.Exists(FName))
                                    File.Delete(FName);

                                using (FileStream FS = new FileStream(FName, FileMode.CreateNew, FileAccess.Write))
                                {
                                    var Buffer = Convert.FromBase64String(fileMessage.FContent);
                                    FS.Write(Buffer, 0, Buffer.Length);
                                }
                            }
                        }
                    }
                    else
                    {
                        Directory.Delete(MailFolder);
                    }

                    Console.WriteLine($"Processing Mail {msg.Subject} Done");
                    if (Config.DeleteMessages)
                    {
                        Console.WriteLine($"Deleting Message ${msg.Subject}");
                        pop.DeleteMessage(i);
                    }
                }
            }
        }
    }
}
