// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Authentication.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Schema.Protobuf.Message.Authentication {

  /// <summary>Holder for reflection information generated from Authentication.proto</summary>
  public static partial class AuthenticationReflection {

    #region Descriptor
    /// <summary>File descriptor for Authentication.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AuthenticationReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRBdXRoZW50aWNhdGlvbi5wcm90bxImU2NoZW1hLlByb3RvYnVmLk1lc3Nh",
            "Z2UuQXV0aGVudGljYXRpb24aC0VudW1zLnByb3RvIiIKB0VuY3JpcHQSCwoD",
            "S2V5GGUgASgJEgoKAklWGGYgASgJIqoBCgVMb2dpbhIKCgJJZBgBIAEoCRJF",
            "Cg5DbGllbnRQbGF0Zm9ybRgCIAEoDjItLlNjaGVtYS5Qcm90b2J1Zi5NZXNz",
            "YWdlLkVudW1zLkNsaWVudFBsYXRmb3JtEkEKDFJlc3BvbnNlQ29kZRhlIAEo",
            "DjIrLlNjaGVtYS5Qcm90b2J1Zi5NZXNzYWdlLkVudW1zLlJlc3BvbnNlQ29k",
            "ZRILCgNJZHgYZiABKAMiSgoGTG9nb3V0EkAKC1JlcG9uc2VDb2RlGGUgASgO",
            "MisuU2NoZW1hLlByb3RvYnVmLk1lc3NhZ2UuRW51bXMuUmVzcG9uc2VDb2Rl",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Schema.Protobuf.Message.Enums.EnumsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Authentication.Encript), global::Schema.Protobuf.Message.Authentication.Encript.Parser, new[]{ "Key", "IV" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Authentication.Login), global::Schema.Protobuf.Message.Authentication.Login.Parser, new[]{ "Id", "ClientPlatform", "ResponseCode", "Idx" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Authentication.Logout), global::Schema.Protobuf.Message.Authentication.Logout.Parser, new[]{ "ReponseCode" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Encript : pb::IMessage<Encript> {
    private static readonly pb::MessageParser<Encript> _parser = new pb::MessageParser<Encript>(() => new Encript());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Encript> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Schema.Protobuf.Message.Authentication.AuthenticationReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Encript() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Encript(Encript other) : this() {
      key_ = other.key_;
      iV_ = other.iV_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Encript Clone() {
      return new Encript(this);
    }

    /// <summary>Field number for the "Key" field.</summary>
    public const int KeyFieldNumber = 101;
    private string key_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Key {
      get { return key_; }
      set {
        key_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "IV" field.</summary>
    public const int IVFieldNumber = 102;
    private string iV_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string IV {
      get { return iV_; }
      set {
        iV_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Encript);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Encript other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Key != other.Key) return false;
      if (IV != other.IV) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Key.Length != 0) hash ^= Key.GetHashCode();
      if (IV.Length != 0) hash ^= IV.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Key.Length != 0) {
        output.WriteRawTag(170, 6);
        output.WriteString(Key);
      }
      if (IV.Length != 0) {
        output.WriteRawTag(178, 6);
        output.WriteString(IV);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Key.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(Key);
      }
      if (IV.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(IV);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Encript other) {
      if (other == null) {
        return;
      }
      if (other.Key.Length != 0) {
        Key = other.Key;
      }
      if (other.IV.Length != 0) {
        IV = other.IV;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 810: {
            Key = input.ReadString();
            break;
          }
          case 818: {
            IV = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Login : pb::IMessage<Login> {
    private static readonly pb::MessageParser<Login> _parser = new pb::MessageParser<Login>(() => new Login());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Login> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Schema.Protobuf.Message.Authentication.AuthenticationReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Login() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Login(Login other) : this() {
      id_ = other.id_;
      clientPlatform_ = other.clientPlatform_;
      responseCode_ = other.responseCode_;
      idx_ = other.idx_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Login Clone() {
      return new Login(this);
    }

    /// <summary>Field number for the "Id" field.</summary>
    public const int IdFieldNumber = 1;
    private string id_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "ClientPlatform" field.</summary>
    public const int ClientPlatformFieldNumber = 2;
    private global::Schema.Protobuf.Message.Enums.ClientPlatform clientPlatform_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Schema.Protobuf.Message.Enums.ClientPlatform ClientPlatform {
      get { return clientPlatform_; }
      set {
        clientPlatform_ = value;
      }
    }

    /// <summary>Field number for the "ResponseCode" field.</summary>
    public const int ResponseCodeFieldNumber = 101;
    private global::Schema.Protobuf.Message.Enums.ResponseCode responseCode_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Schema.Protobuf.Message.Enums.ResponseCode ResponseCode {
      get { return responseCode_; }
      set {
        responseCode_ = value;
      }
    }

    /// <summary>Field number for the "Idx" field.</summary>
    public const int IdxFieldNumber = 102;
    private long idx_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Idx {
      get { return idx_; }
      set {
        idx_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Login);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Login other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (ClientPlatform != other.ClientPlatform) return false;
      if (ResponseCode != other.ResponseCode) return false;
      if (Idx != other.Idx) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (ClientPlatform != 0) hash ^= ClientPlatform.GetHashCode();
      if (ResponseCode != 0) hash ^= ResponseCode.GetHashCode();
      if (Idx != 0L) hash ^= Idx.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (ClientPlatform != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) ClientPlatform);
      }
      if (ResponseCode != 0) {
        output.WriteRawTag(168, 6);
        output.WriteEnum((int) ResponseCode);
      }
      if (Idx != 0L) {
        output.WriteRawTag(176, 6);
        output.WriteInt64(Idx);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (ClientPlatform != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ClientPlatform);
      }
      if (ResponseCode != 0) {
        size += 2 + pb::CodedOutputStream.ComputeEnumSize((int) ResponseCode);
      }
      if (Idx != 0L) {
        size += 2 + pb::CodedOutputStream.ComputeInt64Size(Idx);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Login other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.ClientPlatform != 0) {
        ClientPlatform = other.ClientPlatform;
      }
      if (other.ResponseCode != 0) {
        ResponseCode = other.ResponseCode;
      }
      if (other.Idx != 0L) {
        Idx = other.Idx;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 16: {
            clientPlatform_ = (global::Schema.Protobuf.Message.Enums.ClientPlatform) input.ReadEnum();
            break;
          }
          case 808: {
            responseCode_ = (global::Schema.Protobuf.Message.Enums.ResponseCode) input.ReadEnum();
            break;
          }
          case 816: {
            Idx = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Logout : pb::IMessage<Logout> {
    private static readonly pb::MessageParser<Logout> _parser = new pb::MessageParser<Logout>(() => new Logout());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Logout> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Schema.Protobuf.Message.Authentication.AuthenticationReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Logout() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Logout(Logout other) : this() {
      reponseCode_ = other.reponseCode_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Logout Clone() {
      return new Logout(this);
    }

    /// <summary>Field number for the "ReponseCode" field.</summary>
    public const int ReponseCodeFieldNumber = 101;
    private global::Schema.Protobuf.Message.Enums.ResponseCode reponseCode_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Schema.Protobuf.Message.Enums.ResponseCode ReponseCode {
      get { return reponseCode_; }
      set {
        reponseCode_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Logout);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Logout other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ReponseCode != other.ReponseCode) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ReponseCode != 0) hash ^= ReponseCode.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ReponseCode != 0) {
        output.WriteRawTag(168, 6);
        output.WriteEnum((int) ReponseCode);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ReponseCode != 0) {
        size += 2 + pb::CodedOutputStream.ComputeEnumSize((int) ReponseCode);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Logout other) {
      if (other == null) {
        return;
      }
      if (other.ReponseCode != 0) {
        ReponseCode = other.ReponseCode;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 808: {
            reponseCode_ = (global::Schema.Protobuf.Message.Enums.ResponseCode) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
