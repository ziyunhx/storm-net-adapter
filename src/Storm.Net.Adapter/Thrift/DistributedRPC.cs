/**
 * Autogenerated by Thrift Compiler (0.9.2)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Thrift;
using Thrift.Protocol;

public partial class DistributedRPC
{
    public interface Iface
    {
        string execute(string functionName, string funcArgs);
    }

    public class Client : IDisposable, Iface
    {
        public Client(TProtocol prot) : this(prot, prot)
        {
        }

        public Client(TProtocol iprot, TProtocol oprot)
        {
            iprot_ = iprot;
            oprot_ = oprot;
        }

        protected TProtocol iprot_;
        protected TProtocol oprot_;
        protected int seqid_;

        public TProtocol InputProtocol
        {
            get { return iprot_; }
        }
        public TProtocol OutputProtocol
        {
            get { return oprot_; }
        }


        #region " IDisposable Support "
        private bool _IsDisposed;

        // IDisposable
        public void Dispose()
        {
            Dispose(true);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                if (disposing)
                {
                    if (iprot_ != null)
                    {
                        ((IDisposable)iprot_).Dispose();
                    }
                    if (oprot_ != null)
                    {
                        ((IDisposable)oprot_).Dispose();
                    }
                }
            }
            _IsDisposed = true;
        }
        #endregion


        public string execute(string functionName, string funcArgs)
        {
            send_execute(functionName, funcArgs);
            return recv_execute();
        }
        public void send_execute(string functionName, string funcArgs)
        {
            oprot_.WriteMessageBegin(new TMessage("execute", TMessageType.Call, seqid_));
            execute_args args = new execute_args();
            args.FunctionName = functionName;
            args.FuncArgs = funcArgs;
            args.Write(oprot_);
            oprot_.WriteMessageEnd();
            oprot_.Transport.Flush();
        }

        public string recv_execute()
        {
            TMessage msg = iprot_.ReadMessageBegin();
            if (msg.Type == TMessageType.Exception)
            {
                TApplicationException x = TApplicationException.Read(iprot_);
                iprot_.ReadMessageEnd();
                throw x;
            }
            execute_result result = new execute_result();
            result.Read(iprot_);
            iprot_.ReadMessageEnd();
            if (result.__isset.success)
            {
                return result.Success;
            }
            if (result.__isset.e)
            {
                throw result.E;
            }
            if (result.__isset.aze)
            {
                throw result.Aze;
            }
            throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "execute failed: unknown result");
        }

    }
    public class Processor : TProcessor
    {
        public Processor(Iface iface)
        {
            iface_ = iface;
            processMap_["execute"] = execute_Process;
        }

        protected delegate void ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot);
        private Iface iface_;
        protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

        public bool Process(TProtocol iprot, TProtocol oprot)
        {
            try
            {
                TMessage msg = iprot.ReadMessageBegin();
                ProcessFunction fn;
                processMap_.TryGetValue(msg.Name, out fn);
                if (fn == null)
                {
                    TProtocolUtil.Skip(iprot, TType.Struct);
                    iprot.ReadMessageEnd();
                    TApplicationException x = new TApplicationException(TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
                    oprot.WriteMessageBegin(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID));
                    x.Write(oprot);
                    oprot.WriteMessageEnd();
                    oprot.Transport.Flush();
                    return true;
                }
                fn(msg.SeqID, iprot, oprot);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void execute_Process(int seqid, TProtocol iprot, TProtocol oprot)
        {
            execute_args args = new execute_args();
            args.Read(iprot);
            iprot.ReadMessageEnd();
            execute_result result = new execute_result();
            try
            {
                result.Success = iface_.execute(args.FunctionName, args.FuncArgs);
            }
            catch (DRPCExecutionException e)
            {
                result.E = e;
            }
            catch (AuthorizationException aze)
            {
                result.Aze = aze;
            }
            oprot.WriteMessageBegin(new TMessage("execute", TMessageType.Reply, seqid));
            result.Write(oprot);
            oprot.WriteMessageEnd();
            oprot.Transport.Flush();
        }

    }

    public partial class execute_args : TBase
    {
        private string _functionName;
        private string _funcArgs;

        public string FunctionName
        {
            get
            {
                return _functionName;
            }
            set
            {
                __isset.functionName = true;
                this._functionName = value;
            }
        }

        public string FuncArgs
        {
            get
            {
                return _funcArgs;
            }
            set
            {
                __isset.funcArgs = true;
                this._funcArgs = value;
            }
        }


        public Isset __isset;
        public struct Isset
        {
            public bool functionName;
            public bool funcArgs;
        }

        public execute_args()
        {
        }

        public void Read(TProtocol iprot)
        {
            TField field;
            iprot.ReadStructBegin();
            while (true)
            {
                field = iprot.ReadFieldBegin();
                if (field.Type == TType.Stop)
                {
                    break;
                }
                switch (field.ID)
                {
                    case 1:
                        if (field.Type == TType.String)
                        {
                            FunctionName = iprot.ReadString();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;
                    case 2:
                        if (field.Type == TType.String)
                        {
                            FuncArgs = iprot.ReadString();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;
                    default:
                        TProtocolUtil.Skip(iprot, field.Type);
                        break;
                }
                iprot.ReadFieldEnd();
            }
            iprot.ReadStructEnd();
        }

        public void Write(TProtocol oprot)
        {
            TStruct struc = new TStruct("execute_args");
            oprot.WriteStructBegin(struc);
            TField field = new TField();
            if (FunctionName != null && __isset.functionName)
            {
                field.Name = "functionName";
                field.Type = TType.String;
                field.ID = 1;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(FunctionName);
                oprot.WriteFieldEnd();
            }
            if (FuncArgs != null && __isset.funcArgs)
            {
                field.Name = "funcArgs";
                field.Type = TType.String;
                field.ID = 2;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(FuncArgs);
                oprot.WriteFieldEnd();
            }
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }

        public override string ToString()
        {
            StringBuilder __sb = new StringBuilder("execute_args(");
            bool __first = true;
            if (FunctionName != null && __isset.functionName)
            {
                if (!__first) { __sb.Append(", "); }
                __first = false;
                __sb.Append("FunctionName: ");
                __sb.Append(FunctionName);
            }
            if (FuncArgs != null && __isset.funcArgs)
            {
                if (!__first) { __sb.Append(", "); }
                __first = false;
                __sb.Append("FuncArgs: ");
                __sb.Append(FuncArgs);
            }
            __sb.Append(")");
            return __sb.ToString();
        }

    }

    public partial class execute_result : TBase
    {
        private string _success;
        private DRPCExecutionException _e;
        private AuthorizationException _aze;

        public string Success
        {
            get
            {
                return _success;
            }
            set
            {
                __isset.success = true;
                this._success = value;
            }
        }

        public DRPCExecutionException E
        {
            get
            {
                return _e;
            }
            set
            {
                __isset.e = true;
                this._e = value;
            }
        }

        public AuthorizationException Aze
        {
            get
            {
                return _aze;
            }
            set
            {
                __isset.aze = true;
                this._aze = value;
            }
        }


        public Isset __isset;

        public struct Isset
        {
            public bool success;
            public bool e;
            public bool aze;
        }

        public execute_result()
        {
        }

        public void Read(TProtocol iprot)
        {
            TField field;
            iprot.ReadStructBegin();
            while (true)
            {
                field = iprot.ReadFieldBegin();
                if (field.Type == TType.Stop)
                {
                    break;
                }
                switch (field.ID)
                {
                    case 0:
                        if (field.Type == TType.String)
                        {
                            Success = iprot.ReadString();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;
                    case 1:
                        if (field.Type == TType.Struct)
                        {
                            E = new DRPCExecutionException();
                            E.Read(iprot);
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;
                    case 2:
                        if (field.Type == TType.Struct)
                        {
                            Aze = new AuthorizationException();
                            Aze.Read(iprot);
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;
                    default:
                        TProtocolUtil.Skip(iprot, field.Type);
                        break;
                }
                iprot.ReadFieldEnd();
            }
            iprot.ReadStructEnd();
        }

        public void Write(TProtocol oprot)
        {
            TStruct struc = new TStruct("execute_result");
            oprot.WriteStructBegin(struc);
            TField field = new TField();

            if (this.__isset.success)
            {
                if (Success != null)
                {
                    field.Name = "Success";
                    field.Type = TType.String;
                    field.ID = 0;
                    oprot.WriteFieldBegin(field);
                    oprot.WriteString(Success);
                    oprot.WriteFieldEnd();
                }
            }
            else if (this.__isset.e)
            {
                if (E != null)
                {
                    field.Name = "E";
                    field.Type = TType.Struct;
                    field.ID = 1;
                    oprot.WriteFieldBegin(field);
                    E.Write(oprot);
                    oprot.WriteFieldEnd();
                }
            }
            else if (this.__isset.aze)
            {
                if (Aze != null)
                {
                    field.Name = "Aze";
                    field.Type = TType.Struct;
                    field.ID = 2;
                    oprot.WriteFieldBegin(field);
                    Aze.Write(oprot);
                    oprot.WriteFieldEnd();
                }
            }
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }

        public override string ToString()
        {
            StringBuilder __sb = new StringBuilder("execute_result(");
            bool __first = true;
            if (Success != null && __isset.success)
            {
                if (!__first) { __sb.Append(", "); }
                __first = false;
                __sb.Append("Success: ");
                __sb.Append(Success);
            }
            if (E != null && __isset.e)
            {
                if (!__first) { __sb.Append(", "); }
                __first = false;
                __sb.Append("E: ");
                __sb.Append(E == null ? "<null>" : E.ToString());
            }
            if (Aze != null && __isset.aze)
            {
                if (!__first) { __sb.Append(", "); }
                __first = false;
                __sb.Append("Aze: ");
                __sb.Append(Aze == null ? "<null>" : Aze.ToString());
            }
            __sb.Append(")");
            return __sb.ToString();
        }

    }

}