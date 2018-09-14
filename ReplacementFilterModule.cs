using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Website
{
    public class ReplacementFilterModule : IHttpModule
    {

        public String ModuleName
        {
            get { return "ReplacementFilterModule"; }
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }


        void context_BeginRequest(object sender, EventArgs e)
        {
            

            HttpContext context = HttpContext.Current;

            string fileName = context.Request.FilePath;
            string ext = string.Format("{0}", System.IO.Path.GetExtension(fileName)).ToLower();



            if (
                !string.Equals(ext, ".jpg")
                && !string.Equals(ext, ".jpeg")
                && !string.Equals(ext, ".gif")
                && !string.Equals(ext, ".png")
                && !string.Equals(ext, ".css")
                && !string.Equals(ext, ".js")

                )
            {
                //only do the replacements if we are on a secure connnection...
                context.Response.Filter = new ReplacementStream(context.Response.Filter);
            }
        }


        public void Dispose()
        {

        }
    }

    public class ReplacementStream : MemoryStream
    {
        private Stream stream;
        private StreamWriter streamWriter;

        public ReplacementStream(Stream stm)
        {
            stream = stm;
            streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string contentType = HttpContext.Current.Response.ContentType;

            if (contentType.IndexOf("text", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                contentType.IndexOf("json", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                string find = "http://cdn.agilitycms.com/cine-enterprise/"; //the string you want to match
                string replace = "https://cdn.agilitycms.com/cine-enterprise/"; //the string you want to replace

                string html = System.Text.Encoding.UTF8.GetString(buffer);
                html = html.Replace(find, replace);

                streamWriter.Write(html.ToCharArray(), 0, html.ToCharArray().Length);
                streamWriter.Flush();
            }
            else
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }


    }
}
