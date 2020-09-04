// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Common.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Schema.Protobuf.Message.Common {

  /// <summary>Holder for reflection information generated from Common.proto</summary>
  public static partial class CommonReflection {

    #region Descriptor
    /// <summary>File descriptor for Common.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxDb21tb24ucHJvdG8SHlNjaGVtYS5Qcm90b2J1Zi5NZXNzYWdlLkNvbW1v",
            "biI3CgpHYW1lUmVzdWx0Eg4KBlJvb21JRBgBIAEoAxILCgNJZHgYAiABKAMS",
            "DAoEUmFuaxgDIAEoBSJOCgxNYXRjaGluZ0luZm8SFAoMR2FtZVNlcnZlcklk",
            "GAEgASgDEhgKEExvYmJ5RGVsZWdhdG9ySUQYAiABKAMSDgoGUm9vbUlkGAMg",
            "ASgDYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Common.GameResult), global::Schema.Protobuf.Message.Common.GameResult.Parser, new[]{ "RoomID", "Idx", "Rank" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Schema.Protobuf.Message.Common.MatchingInfo), global::Schema.Protobuf.Message.Common.MatchingInfo.Parser, new[]{ "GameServerId", "LobbyDelegatorID", "RoomId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class GameResult : pb::IMessage<GameResult> {
    private static readonly pb::MessageParser<GameResult> _parser = new pb::MessageParser<GameResult>(() => new GameResult());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameResult> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Schema.Protobuf.Message.Common.CommonReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameResult() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameResult(GameResult other) : this() {
      roomID_ = other.roomID_;
      idx_ = other.idx_;
      rank_ = other.rank_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameResult Clone() {
      return new GameResult(this);
    }

    /// <summary>Field number for the "RoomID" field.</summary>
    public const int RoomIDFieldNumber = 1;
    private long roomID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long RoomID {
      get { return roomID_; }
      set {
        roomID_ = value;
      }
    }

    /// <summary>Field number for the "Idx" field.</summary>
    public const int IdxFieldNumber = 2;
    private long idx_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Idx {
      get { return idx_; }
      set {
        idx_ = value;
      }
    }

    /// <summary>Field number for the "Rank" field.</summary>
    public const int RankFieldNumber = 3;
    private int rank_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Rank {
      get { return rank_; }
      set {
        rank_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameResult);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameResult other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoomID != other.RoomID) return false;
      if (Idx != other.Idx) return false;
      if (Rank != other.Rank) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RoomID != 0L) hash ^= RoomID.GetHashCode();
      if (Idx != 0L) hash ^= Idx.GetHashCode();
      if (Rank != 0) hash ^= Rank.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (RoomID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(RoomID);
      }
      if (Idx != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(Idx);
      }
      if (Rank != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Rank);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (RoomID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(RoomID);
      }
      if (Idx != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Idx);
      }
      if (Rank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Rank);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameResult other) {
      if (other == null) {
        return;
      }
      if (other.RoomID != 0L) {
        RoomID = other.RoomID;
      }
      if (other.Idx != 0L) {
        Idx = other.Idx;
      }
      if (other.Rank != 0) {
        Rank = other.Rank;
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
          case 8: {
            RoomID = input.ReadInt64();
            break;
          }
          case 16: {
            Idx = input.ReadInt64();
            break;
          }
          case 24: {
            Rank = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class MatchingInfo : pb::IMessage<MatchingInfo> {
    private static readonly pb::MessageParser<MatchingInfo> _parser = new pb::MessageParser<MatchingInfo>(() => new MatchingInfo());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MatchingInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Schema.Protobuf.Message.Common.CommonReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MatchingInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MatchingInfo(MatchingInfo other) : this() {
      gameServerId_ = other.gameServerId_;
      lobbyDelegatorID_ = other.lobbyDelegatorID_;
      roomId_ = other.roomId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MatchingInfo Clone() {
      return new MatchingInfo(this);
    }

    /// <summary>Field number for the "GameServerId" field.</summary>
    public const int GameServerIdFieldNumber = 1;
    private long gameServerId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long GameServerId {
      get { return gameServerId_; }
      set {
        gameServerId_ = value;
      }
    }

    /// <summary>Field number for the "LobbyDelegatorID" field.</summary>
    public const int LobbyDelegatorIDFieldNumber = 2;
    private long lobbyDelegatorID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long LobbyDelegatorID {
      get { return lobbyDelegatorID_; }
      set {
        lobbyDelegatorID_ = value;
      }
    }

    /// <summary>Field number for the "RoomId" field.</summary>
    public const int RoomIdFieldNumber = 3;
    private long roomId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long RoomId {
      get { return roomId_; }
      set {
        roomId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MatchingInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MatchingInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (GameServerId != other.GameServerId) return false;
      if (LobbyDelegatorID != other.LobbyDelegatorID) return false;
      if (RoomId != other.RoomId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (GameServerId != 0L) hash ^= GameServerId.GetHashCode();
      if (LobbyDelegatorID != 0L) hash ^= LobbyDelegatorID.GetHashCode();
      if (RoomId != 0L) hash ^= RoomId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (GameServerId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(GameServerId);
      }
      if (LobbyDelegatorID != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(LobbyDelegatorID);
      }
      if (RoomId != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(RoomId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (GameServerId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(GameServerId);
      }
      if (LobbyDelegatorID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(LobbyDelegatorID);
      }
      if (RoomId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(RoomId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MatchingInfo other) {
      if (other == null) {
        return;
      }
      if (other.GameServerId != 0L) {
        GameServerId = other.GameServerId;
      }
      if (other.LobbyDelegatorID != 0L) {
        LobbyDelegatorID = other.LobbyDelegatorID;
      }
      if (other.RoomId != 0L) {
        RoomId = other.RoomId;
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
          case 8: {
            GameServerId = input.ReadInt64();
            break;
          }
          case 16: {
            LobbyDelegatorID = input.ReadInt64();
            break;
          }
          case 24: {
            RoomId = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code