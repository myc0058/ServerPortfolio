// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Authentication.proto
// </auto-generated>
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
            "Z2UuQXV0aGVudGljYXRpb24aC0VudW1zLnByb3RvGgpEYXRhLnByb3RvIiIK",
            "B0VuY3JpcHQSCwoDS2V5GAEgASgJEgoKAklWGAIgASgJIpsBCgVMb2dpbhIK",
            "CgJJZBgBIAEoCRINCgVPQXV0aBgCIAEoCRILCgNJZHgYAyABKAUSMwoJRXJy",
            "b3JDb2RlGAQgASgOMiAuU2NoZW1hLlByb3RvYnVmLkVudW1zLkVycm9yQ29k",
            "ZRI1CghQbGF0Zm9ybRgFIAEoDjIjLlNjaGVtYS5Qcm90b2J1Zi5FbnVtcy5Q",
            "bGF0Zm9ybVR5cGUiSgoGTG9nb3V0EgsKA0lkeBgBIAEoBRIzCglFcnJvckNv",
            "ZGUYAiABKA4yIC5TY2hlbWEuUHJvdG9idWYuRW51bXMuRXJyb3JDb2RlYgZw",
            "cm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Schema.Protobuf.Enums.EnumsReflection.Descriptor, global::Schema.Protobuf.DataReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Authentication.Encript), global::Schema.Protobuf.Message.Authentication.Encript.Parser, new[]{ "Key", "IV" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Authentication.Login), global::Schema.Protobuf.Message.Authentication.Login.Parser, new[]{ "Id", "OAuth", "Idx", "ErrorCode", "Platform" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Authentication.Logout), global::Schema.Protobuf.Message.Authentication.Logout.Parser, new[]{ "Idx", "ErrorCode" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Encript : pb::IMessage<Encript> {
    private static readonly pb::MessageParser<Encript> _parser = new pb::MessageParser<Encript>(() => new Encript());
    private pb::UnknownFieldSet _unknownFields;
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
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Encript Clone() {
      return new Encript(this);
    }

    /// <summary>Field number for the "Key" field.</summary>
    public const int KeyFieldNumber = 1;
    private string key_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Key {
      get { return key_; }
      set {
        key_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "IV" field.</summary>
    public const int IVFieldNumber = 2;
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
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Key.Length != 0) hash ^= Key.GetHashCode();
      if (IV.Length != 0) hash ^= IV.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Key.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Key);
      }
      if (IV.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(IV);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Key.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Key);
      }
      if (IV.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(IV);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
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
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Key = input.ReadString();
            break;
          }
          case 18: {
            IV = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Login : pb::IMessage<Login> {
    private static readonly pb::MessageParser<Login> _parser = new pb::MessageParser<Login>(() => new Login());
    private pb::UnknownFieldSet _unknownFields;
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
      oAuth_ = other.oAuth_;
      idx_ = other.idx_;
      errorCode_ = other.errorCode_;
      platform_ = other.platform_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
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

    /// <summary>Field number for the "OAuth" field.</summary>
    public const int OAuthFieldNumber = 2;
    private string oAuth_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OAuth {
      get { return oAuth_; }
      set {
        oAuth_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Idx" field.</summary>
    public const int IdxFieldNumber = 3;
    private int idx_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Idx {
      get { return idx_; }
      set {
        idx_ = value;
      }
    }

    /// <summary>Field number for the "ErrorCode" field.</summary>
    public const int ErrorCodeFieldNumber = 4;
    private global::Schema.Protobuf.Enums.ErrorCode errorCode_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Schema.Protobuf.Enums.ErrorCode ErrorCode {
      get { return errorCode_; }
      set {
        errorCode_ = value;
      }
    }

    /// <summary>Field number for the "Platform" field.</summary>
    public const int PlatformFieldNumber = 5;
    private global::Schema.Protobuf.Enums.PlatformType platform_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Schema.Protobuf.Enums.PlatformType Platform {
      get { return platform_; }
      set {
        platform_ = value;
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
      if (OAuth != other.OAuth) return false;
      if (Idx != other.Idx) return false;
      if (ErrorCode != other.ErrorCode) return false;
      if (Platform != other.Platform) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (OAuth.Length != 0) hash ^= OAuth.GetHashCode();
      if (Idx != 0) hash ^= Idx.GetHashCode();
      if (ErrorCode != 0) hash ^= ErrorCode.GetHashCode();
      if (Platform != 0) hash ^= Platform.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
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
      if (OAuth.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(OAuth);
      }
      if (Idx != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Idx);
      }
      if (ErrorCode != 0) {
        output.WriteRawTag(32);
        output.WriteEnum((int) ErrorCode);
      }
      if (Platform != 0) {
        output.WriteRawTag(40);
        output.WriteEnum((int) Platform);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (OAuth.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OAuth);
      }
      if (Idx != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Idx);
      }
      if (ErrorCode != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ErrorCode);
      }
      if (Platform != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Platform);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
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
      if (other.OAuth.Length != 0) {
        OAuth = other.OAuth;
      }
      if (other.Idx != 0) {
        Idx = other.Idx;
      }
      if (other.ErrorCode != 0) {
        ErrorCode = other.ErrorCode;
      }
      if (other.Platform != 0) {
        Platform = other.Platform;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 18: {
            OAuth = input.ReadString();
            break;
          }
          case 24: {
            Idx = input.ReadInt32();
            break;
          }
          case 32: {
            errorCode_ = (global::Schema.Protobuf.Enums.ErrorCode) input.ReadEnum();
            break;
          }
          case 40: {
            platform_ = (global::Schema.Protobuf.Enums.PlatformType) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Logout : pb::IMessage<Logout> {
    private static readonly pb::MessageParser<Logout> _parser = new pb::MessageParser<Logout>(() => new Logout());
    private pb::UnknownFieldSet _unknownFields;
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
      idx_ = other.idx_;
      errorCode_ = other.errorCode_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Logout Clone() {
      return new Logout(this);
    }

    /// <summary>Field number for the "Idx" field.</summary>
    public const int IdxFieldNumber = 1;
    private int idx_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Idx {
      get { return idx_; }
      set {
        idx_ = value;
      }
    }

    /// <summary>Field number for the "ErrorCode" field.</summary>
    public const int ErrorCodeFieldNumber = 2;
    private global::Schema.Protobuf.Enums.ErrorCode errorCode_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Schema.Protobuf.Enums.ErrorCode ErrorCode {
      get { return errorCode_; }
      set {
        errorCode_ = value;
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
      if (Idx != other.Idx) return false;
      if (ErrorCode != other.ErrorCode) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Idx != 0) hash ^= Idx.GetHashCode();
      if (ErrorCode != 0) hash ^= ErrorCode.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Idx != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Idx);
      }
      if (ErrorCode != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) ErrorCode);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Idx != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Idx);
      }
      if (ErrorCode != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ErrorCode);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Logout other) {
      if (other == null) {
        return;
      }
      if (other.Idx != 0) {
        Idx = other.Idx;
      }
      if (other.ErrorCode != 0) {
        ErrorCode = other.ErrorCode;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Idx = input.ReadInt32();
            break;
          }
          case 16: {
            errorCode_ = (global::Schema.Protobuf.Enums.ErrorCode) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code