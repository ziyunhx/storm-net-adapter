using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Thrift.Protocol;

namespace Storm
{
    public class Serializer
    {
        private static readonly Type ThriftType = typeof(TBase);
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();
        private static readonly Type ByteArrayType = typeof(byte[]);
        private static readonly byte[] EmptyArray = new byte[0];
        public static List<byte[]> ConvertToByteArray(List<object> tuple, List<Type> types)
        {
            return tuple.Select((object t, int i) => Serializer.CSharpObjectToBytes(t, types[i])).ToList<byte[]>();
        }
        public static List<byte[]> ConvertToByteArray(List<object> tuple)
        {
            return tuple.Select((object t, int i) => Serializer.CSharpObjectToBytes(t, typeof(object))).ToList<byte[]>();
        }
        public static List<object> ConvertToObjectArray(List<byte[]> raw, List<Type> types)
        {
            return raw.Select((byte[] t, int i) => Serializer.CSharpBytesToObject(t, types[i])).ToList<object>();
        }
        public static List<object> ConvertToObjectArray(List<byte[]> raw)
        {
            return raw.Select((byte[] t, int i) => Serializer.CSharpBytesToObject(t, typeof(object))).ToList<object>();
        }
        public static object CSharpBytesToObject(byte[] bytes, Type type)
        {
            if (bytes == null)
            {
                return null;
            }
            if (bytes.Count<byte>() == 0)
            {
                return null;
            }
            if (type == Serializer.ByteArrayType)
            {
                return bytes;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    {
                        return BitConverter.ToBoolean(bytes, 0);
                    }
                case TypeCode.Char:
                    {
                        return BitConverter.ToChar(bytes, 0);
                    }
                case TypeCode.SByte:
                    {
                        return (sbyte)bytes[0];
                    }
                case TypeCode.Byte:
                    {
                        return bytes[0];
                    }
                case TypeCode.Int16:
                    {
                        return BitConverter.ToInt16(bytes, 0);
                    }
                case TypeCode.UInt16:
                    {
                        return BitConverter.ToUInt16(bytes, 0);
                    }
                case TypeCode.Int32:
                    {
                        return BitConverter.ToInt32(bytes, 0);
                    }
                case TypeCode.UInt32:
                    {
                        return BitConverter.ToUInt32(bytes, 0);
                    }
                case TypeCode.Int64:
                    {
                        return BitConverter.ToInt64(bytes, 0);
                    }
                case TypeCode.UInt64:
                    {
                        return BitConverter.ToUInt64(bytes, 0);
                    }
                case TypeCode.Single:
                    {
                        return BitConverter.ToSingle(bytes, 0);
                    }
                case TypeCode.Double:
                    {
                        return BitConverter.ToDouble(bytes, 0);
                    }
                case TypeCode.DateTime:
                    {
                        return DateTime.FromBinary(BitConverter.ToInt64(bytes, 0));
                    }
                case TypeCode.String:
                    {
                        return Encoding.UTF8.GetString(bytes);
                    }
            }

            MemoryStream serializationStream = new MemoryStream(bytes, false);
            return Serializer.binaryFormatter.Deserialize(serializationStream);
        }
        public static byte[] CSharpObjectToBytes(object obj, Type type)
        {
            if (obj == null)
            {
                return Serializer.EmptyArray;
            }
            if (type == Serializer.ByteArrayType)
            {
                return (byte[])obj;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    {
                        return BitConverter.GetBytes((bool)obj);
                    }
                case TypeCode.Char:
                    {
                        return BitConverter.GetBytes((char)obj);
                    }
                case TypeCode.SByte:
                    {
                        return BitConverter.GetBytes((short)((sbyte)obj));
                    }
                case TypeCode.Byte:
                    {
                        return BitConverter.GetBytes((short)((byte)obj));
                    }
                case TypeCode.Int16:
                    {
                        return BitConverter.GetBytes((short)obj);
                    }
                case TypeCode.UInt16:
                    {
                        return BitConverter.GetBytes((ushort)obj);
                    }
                case TypeCode.Int32:
                    {
                        return BitConverter.GetBytes((int)obj);
                    }
                case TypeCode.UInt32:
                    {
                        return BitConverter.GetBytes((uint)obj);
                    }
                case TypeCode.Int64:
                    {
                        return BitConverter.GetBytes((long)obj);
                    }
                case TypeCode.UInt64:
                    {
                        return BitConverter.GetBytes((ulong)obj);
                    }
                case TypeCode.Single:
                    {
                        return BitConverter.GetBytes((float)obj);
                    }
                case TypeCode.Double:
                    {
                        return BitConverter.GetBytes((double)obj);
                    }
                case TypeCode.DateTime:
                    {
                        return BitConverter.GetBytes(((DateTime)obj).ToBinary());
                    }
                case TypeCode.String:
                    {
                        return Encoding.UTF8.GetBytes((string)obj);
                    }
            }

            MemoryStream memoryStream = new MemoryStream();
            Serializer.binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }
}