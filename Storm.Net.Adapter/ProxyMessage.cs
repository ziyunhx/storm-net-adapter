using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Storm
{
    [Serializable]
    public class ProxyMessage : TBase, TAbstractBase
    {
        [Serializable]
        public struct Isset
        {
            public bool streamId;
            public bool tupleId;
            public bool evt;
            public bool data;
        }
        private string _streamId;
        private string _tupleId;
        private ProxyEvent _evt;
        private List<byte[]> _data;
        public ProxyMessage.Isset __isset;
        public string StreamId
        {
            get
            {
                return this._streamId;
            }
            set
            {
                this.__isset.streamId = true;
                this._streamId = value;
            }
        }
        public string TupleId
        {
            get
            {
                return this._tupleId;
            }
            set
            {
                this.__isset.tupleId = true;
                this._tupleId = value;
            }
        }
        public ProxyEvent Evt
        {
            get
            {
                return this._evt;
            }
            set
            {
                this.__isset.evt = true;
                this._evt = value;
            }
        }
        public List<byte[]> Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this.__isset.data = true;
                this._data = value;
            }
        }
        public void Read(TProtocol iprot)
        {
            iprot.ReadStructBegin();
            while (true)
            {
                TField tField = iprot.ReadFieldBegin();
                if (tField.Type == TType.Stop)
                {
                    break;
                }
                switch (tField.ID)
                {
                    case 1:
                        {
                            if (tField.Type == TType.String)
                            {
                                this.StreamId = iprot.ReadString();
                            }
                            else
                            {
                                TProtocolUtil.Skip(iprot, tField.Type);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (tField.Type == TType.String)
                            {
                                this.TupleId = iprot.ReadString();
                            }
                            else
                            {
                                TProtocolUtil.Skip(iprot, tField.Type);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (tField.Type == TType.I32)
                            {
                                this.Evt = (ProxyEvent)iprot.ReadI32();
                            }
                            else
                            {
                                TProtocolUtil.Skip(iprot, tField.Type);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (tField.Type == TType.List)
                            {
                                this.Data = new List<byte[]>();
                                TList tList = iprot.ReadListBegin();
                                for (int i = 0; i < tList.Count; i++)
                                {
                                    byte[] item = iprot.ReadBinary();
                                    this.Data.Add(item);
                                }
                                iprot.ReadListEnd();
                            }
                            else
                            {
                                TProtocolUtil.Skip(iprot, tField.Type);
                            }
                            break;
                        }
                    default:
                        {
                            TProtocolUtil.Skip(iprot, tField.Type);
                            break;
                        }
                }
                iprot.ReadFieldEnd();
            }
            iprot.ReadStructEnd();
        }
        public void Write(TProtocol oprot)
        {
            TStruct struc = new TStruct("ProxyMessage");
            oprot.WriteStructBegin(struc);
            TField field = default(TField);
            if (this.StreamId != null && this.__isset.streamId)
            {
                field.Name = "streamId";
                field.Type = TType.String;
                field.ID = 1;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(this.StreamId);
                oprot.WriteFieldEnd();
            }
            if (this.TupleId != null && this.__isset.tupleId)
            {
                field.Name = "tupleId";
                field.Type = TType.String;
                field.ID = 2;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(this.TupleId);
                oprot.WriteFieldEnd();
            }
            if (this.__isset.evt)
            {
                field.Name = "evt";
                field.Type = TType.I32;
                field.ID = 3;
                oprot.WriteFieldBegin(field);
                oprot.WriteI32((int)this.Evt);
                oprot.WriteFieldEnd();
            }
            if (this.Data != null && this.__isset.data)
            {
                field.Name = "data";
                field.Type = TType.List;
                field.ID = 4;
                oprot.WriteFieldBegin(field);
                oprot.WriteListBegin(new TList(TType.String, this.Data.Count));
                foreach (byte[] current in this.Data)
                {
                    oprot.WriteBinary(current);
                }
                oprot.WriteListEnd();
                oprot.WriteFieldEnd();
            }
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("ProxyMessage(");
            stringBuilder.Append("StreamId: ");
            stringBuilder.Append(this.StreamId);
            stringBuilder.Append(",TupleId: ");
            stringBuilder.Append(this.TupleId);
            stringBuilder.Append(",Evt: ");
            stringBuilder.Append(this.Evt);
            stringBuilder.Append(",Data: ");
            stringBuilder.Append(this.Data);
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}