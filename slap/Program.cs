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
        /// <summary>
        /// prints how to use this tool.
        /// </summary>
        static void printUsage()
        {
            //prints out how to use. <> is for manditory, [] is for optional
            System.Console.WriteLine("slap.exe <file> [paste_lang] [private] [deletion time in seconds] -- set private to yes for private paste");
        }
        /// <summary>
        /// copies the string given to the clipboard
        /// </summary>
        /// <param name="url">the string wished to be copied</param>
        static void copyToClipboard(string url)
        {
            string command = String.Empty;
            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                //its a mac.  pbcoby for pasteboard
                command = "pbcopy";
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                //its linux.  assume that you've got X running.
                command = "xsel --clipboard --input";
            }
            if (command != String.Empty)
            {
                //for linux and mac, execute the copy to clipboard
                Process.Start("echo " + url + " | " + command);
            }
            else
            {
                //its windows, just use the winforms clipboard
                Clipboard.SetText(url);
            }
            System.Console.WriteLine("uploaded file to " + url + " and copied url to clipboard");
        }
               
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            //required params file and language
            if (args.Length < 1)
            {
                //no inputs. print how to use, then quit
                printUsage();
                Environment.Exit(1);
            }
            string paste_lang = "text"; //default to the "text"
            if (args.Length > 1)
            {
                paste_lang = args[1]; //oh, so you wanted a language, we'll take that.
            }
            //make sure a good file was put
            if (!File.Exists(args[0]))
            {
                //it wasn't!!??!? 
                printUsage(); //tell the user what's up
                Environment.Exit(1); //quit with return code 1
            }
            string paste_data = String.Empty; //the actual code that you has
            using (StreamReader sr = new StreamReader(args[0]))
            {
                paste_data = sr.ReadToEnd(); //read the file 
            }
            //submit the code with the following api as the guide
            // http://paste.kde.org/doc/api/
            //thanks the KDE team for such a great useful tool!
            PostSubmitter post = new PostSubmitter();
            post.Url = "http://paste.kde.org/";
            post.PostItems.Add("paste_data", paste_data);
            post.PostItems.Add("paste_lang", paste_lang);
            post.PostItems.Add("api_submit", "true");
            post.PostItems.Add("mode", "xml");
            if (args.Length > 2 && args[2] == "yes")
            {
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
            //generate the url based on the reponse
            url = "http://paste.kde.org/" + id + "/" + hash;
            //copy the url to clipboard
            copyToClipboard(url);
            //and we're done!
            //that wasn't so bad.
        }
    }
}
