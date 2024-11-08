using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace REE
{
    public static class Helpers
    {
        public static byte[] ReadBytes(this Stream stream, int count)
        {
            var result = new byte[count];
            int offset = 0;
            while (offset < count)
            {
                int bytesRead = stream.Read(result, offset, count - offset);
                if (bytesRead <= 0)
                    throw new IOException();
                offset += bytesRead;
            }
            return result;
        }

        public static byte[] ReadBytes(this Stream stream)
        {
            return ReadBytes(stream, (int)stream.Length);
        }

        public static Int16 ReadInt16(this Stream stream)
        {
            return BitConverter.ToInt16(stream.ReadBytes(2), 0);
        }

        public static Int32 ReadInt32(this Stream stream)
        {
            return BitConverter.ToInt32(stream.ReadBytes(4), 0);
        }

        public static Int64 ReadInt64(this Stream stream)
        {
            return BitConverter.ToInt64(stream.ReadBytes(8), 0);
        }

        public static UInt16 ReadUInt16(this Stream stream)
        {
            return BitConverter.ToUInt16(stream.ReadBytes(2), 0);
        }

        public static UInt32 ReadUInt32(this Stream stream)
        {
            return BitConverter.ToUInt32(stream.ReadBytes(4), 0);
        }

        public static UInt64 ReadUInt64(this Stream stream)
        {
            return BitConverter.ToUInt64(stream.ReadBytes(8), 0);
        }

        public static Single ReadSingle(this Stream stream)
        {
            return BitConverter.ToSingle(stream.ReadBytes(4), 0);
        }

        public static string ReadStringUnicodeLength(this Stream stream, Int32 length)
        {
            var result = stream.ReadBytes(length * 2);
            return Encoding.Unicode.GetString(result);
        }

        public static string ReadStringLength(this Stream stream)
        {
            var length = stream.ReadInt32();
            var result = stream.ReadBytes(length);
            return Encoding.ASCII.GetString(result);
        }

        public static string ReadString(this Stream stream, int length, Encoding? encoding = null, bool trim = true)
        {
            encoding = encoding ?? Encoding.ASCII;
            var result = encoding.GetString(stream.ReadBytes(length));
            return trim ? result.Trim() : result;
        }

        public static string ReadString(this Stream stream, Encoding? encoding = null, bool trim = true)
        {
            encoding = encoding ?? Encoding.ASCII;

            int count = 0;
            int b;
            var data = new List<byte>();
            while ((b = stream.ReadByte()) > 0)
            {
                data.Add((byte)b);
                count++;
            }
            if (b < 0)
                throw new IOException();

            var result = encoding.GetString(data.ToArray(), 0, count);
            return trim ? result.Trim() : result;
        }

        public static string ReadStringByOffset(this Stream stream, uint offset, Encoding? encoding = null, bool trim = true)
        {
            stream.Position = offset;
            return ReadString(stream, encoding, trim);
        }

        public static string[] ReadStringList(this Stream stream, Encoding? encoding = null, bool trim = true)
        {
            var result = new List<string>();
            while (stream.Position < stream.Length)
                result.Add(ReadString(stream, encoding, trim));
            return result.ToArray();
        }

        public static void CopyTo(this Stream source, Stream target)
        {
            const int bufferSize = 32768;

            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            var buffer = new byte[bufferSize];
            int read;
            int count = 0;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                target.Write(buffer, 0, read);
                count += read;
            }
        }
    }

    public static class ByteArrayExtensions
    {
        public static byte[] ReadBytes(this byte[] data, int count, int startIndex = 0)
        {
            var buffer = new byte[4];
            Array.Copy(data, buffer, count);
            return buffer;
        }

        public static Int16 ReadInt16(this byte[] data, int startIndex = 0)
        {
            return BitConverter.ToInt16(data, startIndex);
        }

        public static Int32 ReadInt32(this byte[] data, int startIndex = 0)
        {
            return BitConverter.ToInt32(data, startIndex);
        }

        public static UInt16 ReadUInt16(this byte[] data, int startIndex = 0)
        {
            return BitConverter.ToUInt16(data, startIndex);
        }

        public static UInt32 ReadUInt32(this byte[] data, int startIndex = 0)
        {
            return BitConverter.ToUInt32(data, startIndex);
        }

        public static UInt64 ReadUInt64(this byte[] data, int startIndex = 0)
        {
            return BitConverter.ToUInt64(data, startIndex);
        }

        public static Single ReadSingle(this byte[] data, int startIndex = 0)
        {
            return BitConverter.ToSingle(data, startIndex);
        }

        public static Single ReadSingleBE(this byte[] data, int startIndex = 0)
        {
            var buffer = data.ReadBytes(4);
            Array.Reverse(buffer);
            return BitConverter.ToSingle(data, startIndex);
        }

        private static string ReadStringInternal(this byte[] data, ref int startIndex, Encoding? encoding, bool trim)
        {
            encoding = encoding ?? Encoding.ASCII;

            int i;
            for (i = startIndex; i < data.Length && data[i] != 0; i++) ;

            var result = encoding.GetString(data, startIndex, i - startIndex);
            startIndex = i + 1;
            return trim ? result.Trim() : result;
        }

        public static string ReadString(this byte[] data, int startIndex = 0, Encoding? encoding = null, bool trim = true)
        {
            return ReadStringInternal(data, ref startIndex, encoding, trim);
        }

        public static string[] ReadStringList(this byte[] data, int startIndex = 0, Encoding? encoding = null, bool trim = true)
        {
            var result = new List<string>();
            while (startIndex < data.Length)
                result.Add(ReadStringInternal(data, ref startIndex, encoding, trim));
            return result.ToArray();
        }

        public static Boolean IsText(this byte[] data)
        {
            foreach (byte b in data)
            {
                char c = (char)b;
                if (
                    b != 0
                    && !char.IsLetterOrDigit(c)
                    && !char.IsWhiteSpace(c)
                    && !char.IsPunctuation(c)
                    && !char.IsSeparator(c))
                    return false;
            }
            return true;
        }
    }
}
