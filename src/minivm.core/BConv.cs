using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public class BConv
    {
        private static string MagicString = "0xm";

        public static byte[] ToBytes(Instruction[] instructions)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, instructions);
                return stream.ToArray();
            }
        }
        public static Instruction[] FromBytes(byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return (Instruction[])formatter.Deserialize(stream);
            }
        }

        public static string ToBase64(Instruction[] instructions)
        {
            return MagicString + Convert.ToBase64String(Zip(ToBytes(instructions)));
        }
        public static Instruction[] FromBase64(string base64)
        {
            if (base64.StartsWith(MagicString) == false)
                throw new ArgumentException("input string is not a valid program");
            base64 = base64.Substring(MagicString.Length);
            return FromBytes(Unzip(Convert.FromBase64String(base64)));
        }

        private static byte[] Zip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }
        private static byte[] Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }
                return mso.ToArray();
            }
        }
        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
                dest.Write(bytes, 0, cnt);
        }
    }
}
