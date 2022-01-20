using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TechEdge.Mail
{
    public class MailMessage
    {
        const string FindAttachment = "Content-Disposition: attachment;";
        const string FindBoundary = "Content-Type: multipart/mixed; boundary";
        const string FileNameToken = "filename=";

        const string SubjectToken = "Subject: ";
        const string FromToken = "From: ";
        const string ToToken = "To: ";
        const string CCToken = "CC: ";


        private List<MailAttachment> _Attachments;

        public MailMessage()
        {
            _Attachments = new List<MailAttachment>();
        }


        public string FileName { get; private set; }


        public string Subject { get; private set; }

        public string From { get; set; }


        public string To { get; set; }

        public string CC { get; set; }


        public static MailMessage FromStream(StreamReader emlStream, string fileName = "")
        {
            var Rtn = new MailMessage() { FileName = fileName };


            var Boundery = string.Empty;
            var ContentBuilder = new StringBuilder();

            while (!emlStream.EndOfStream)
            {
                var Line = emlStream.ReadLine();

                if (Line.IndexOf(FromToken) >= 0)
                    Rtn.From = Line.Substring(Line.IndexOf(FromToken) + FromToken.Length);

                if (Line.IndexOf(ToToken) >= 0)
                {
                    Rtn.To = Line.Substring(Line.IndexOf(ToToken) + ToToken.Length);
                    continue;
                }

                if (Line.IndexOf(CCToken) >= 0)
                {
                    Rtn.CC = Line.Substring(Line.IndexOf(CCToken) + CCToken.Length);
                    continue;
                }

                if (Line.IndexOf(SubjectToken) >= 0)
                {
                    Rtn.Subject = Line.Substring(Line.IndexOf(SubjectToken) + SubjectToken.Length);
                    continue;
                }

                if (Line.IndexOf(FindBoundary) >= 0)
                {
                    Boundery = Line.
                        Substring(Line.IndexOf(FindBoundary) + FindBoundary.Length + 1).
                        Replace("\"", string.Empty);

                    continue;
                }

                if (Line.IndexOf(FindAttachment) < 0)
                    continue;

                if (Line.IndexOf(FileNameToken) < 0)
                    Line = emlStream.ReadLine();

                var FileName = Line.Substring(Line.IndexOf(FileNameToken) + FileNameToken.Length).Replace("\"", string.Empty);

                do
                {
                    Line = emlStream.ReadLine();
                } while (string.IsNullOrEmpty(Line));

                while (Line.IndexOf(string.Concat("--", Boundery)) < 0 && !emlStream.EndOfStream)
                {

                    ContentBuilder.AppendLine(Line);
                    Line = emlStream.ReadLine();
                }


                Rtn._Attachments.Add(new MailAttachment { FName = FileName, FContent = ContentBuilder.ToString() });
                ContentBuilder.Clear();
            }

            emlStream.BaseStream.Position = 0;

            var MS = new MemoryStream();
            emlStream.BaseStream.CopyTo(MS);


            return Rtn;
        }

        public static MailMessage FromBase64(string content, string fileName = "")
        {
            using (MemoryStream MS = new MemoryStream(Convert.FromBase64String(content)))
            using (StreamReader MailStream = new StreamReader(MS))
                return FromStream(MailStream, fileName);
        }

        public IEnumerable<MailAttachment> Attachments => _Attachments;
    }
}
