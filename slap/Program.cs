using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.Xml;
using System.Diagnostics;
using System.Windows.Forms;

namespace slap
{
    class Program
    {
        static void printUsage()
        {
            System.Console.WriteLine("slap.exe <file> <paste_lang> [private] [deletion time in seconds] -- set private to yes for private paste");
        }
        static void copyToClipboard(string url)
        {
            string command = String.Empty;
            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                command = "pbcopy";
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                command = "xsel --clipboard --input";
            }
            if (command != String.Empty)
            {
                Process.Start("echo " + url + " | " + command);
            }
            else
            {
                Clipboard.SetText(url);
            }
            System.Console.WriteLine("uploaded file to " + url + " and copied url to clipboard");
        }
               
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            bool isPrivate = false;
            //required params file and language
            if (args.Length < 2)
            {
                printUsage();
                Environment.Exit(1);
            }
            string paste_lang = args[1];
            //make sure a good file was put
            if (!File.Exists(args[0]))
            {
                printUsage();
                Environment.Exit(1);
            }
            string paste_data = String.Empty;
            using (StreamReader sr = new StreamReader(args[0]))
            {
                paste_data = sr.ReadToEnd();
            }
            PostSubmitter post = new PostSubmitter();
            post.Url = "http://paste.kde.org/";
            post.PostItems.Add("paste_data", paste_data);
            post.PostItems.Add("paste_lang", paste_lang);
            post.PostItems.Add("api_submit", "true");
            post.PostItems.Add("mode", "xml");
            if (args.Length > 2 && args[2] == "yes")
            {
                isPrivate = true;
                post.PostItems.Add("paste_private", "yes");
            }
            int time;
            if (args.Length > 3 && Int32.TryParse(args[3], out time))
            {
                string paste_expire = args[3];
                post.PostItems.Add("paste_expire", paste_expire);
            }
            post.Type = PostSubmitter.PostTypeEnum.Post;
            string result = String.Empty;
            try
            {
                result = post.Post();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
            //ok, it has been posted, and the result is in XML format.
            //stored in the "result" string.

            //create an XML reader
            string url = String.Empty;
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(result);
            XmlNodeList nodelist = xdoc.DocumentElement.ChildNodes;
            string id = String.Empty;
            string hash = String.Empty;
            foreach (XmlNode outerNode in nodelist)
            {
                if (outerNode.Name == "id")
                {
                    id = outerNode.InnerText;
                }
                else if(outerNode.Name == "hash")
                {
                    hash = outerNode.InnerText;
                }
            }
            url = "http://paste.kde.org/" + id + "/" + hash;
            copyToClipboard(url);
        }
    }
}
