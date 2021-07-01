using Imazen.WebP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Services
{
    public class MediaServices
    {
        private readonly SimpleDecoder _simpleDecoder;

        public MediaServices()
        {
            _simpleDecoder = new SimpleDecoder();
        }

        public Bitmap DecodeWebp(Stream input)
        {
            byte[] bytes = ReadFully(input);
            return _simpleDecoder.DecodeFromBytes(bytes, bytes.Length);
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}