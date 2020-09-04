using Application.Match.Entities;
using Engine.Framework;
using Schema.Protobuf.Message.Administrator;
using Schema.Protobuf.Message.Common;
using Schema.Protobuf.Message.Enums;
using Schema.Protobuf.Message.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Scheduler
{
    public class MatchMaker : Engine.Framework.Scheduler
    {
        public MatchMaker() : base(Singleton<Engine.Framework.Layer>.Instance) { }

        //myc0058 이 두개를 하나로 묶은 클래스가 필요하다.
        private Rank sortedUsers = new Rank();
        private Dictionary<long, User> matchingUsers = new Dictionary<long, User>();

        private Rank sortedGameServers = new Rank();
        private Dictionary<long, GameServerState> gameServerStates = new Dictionary<long, GameServerState>();

        protected override void OnSchedule(long deltaTicks)
        {
            try
            {
                List<User> users = Matching(2);
                if (users == null)
                {
                    return;
                }

                long roomID = Engine.Framework.Api.UniqueKey;
                GameServerState bestGameServer = GetBestGameServerID();
                if (bestGameServer == null)
                {
                    //if does not exist gameserver, try again
                    BeginMatching(users);
                    return;
                }

                foreach (var item in users)
                {
                    EndMatching endMatching = new EndMatching();
                    endMatching.MatchingInfo = new MatchingInfo();
                    endMatching.ResponseCode = ResponseCode.Success;
                    endMatching.MatchingInfo.GameServerId = bestGameServer.GameServerID;
                    endMatching.MatchingInfo.LobbyDelegatorID = bestGameServer.LobbyDelegatorID;
                    endMatching.MatchingInfo.RoomId = roomID;
                    var delegator = Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Get(item.LobbyUID);
                    delegator?.Delegate(item.UID, item.UID, endMatching);
                }
            }
            catch (Exception ex)
            {
                Engine.Framework.Api.Logger.Error(ex.StackTrace);
            }
        }

        private GameServerState GetBestGameServerID()
        {
            lock (sortedGameServers)
            {
                if (gameServerStates.Count < 1) return null;

                long key = sortedGameServers.FirstKey();

                if (gameServerStates.TryGetValue(key, out var gameServerState) == true)
                {
                    return gameServerState;
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddOrUpdateGameServerState(GameServerState gameServerState)
        {
            lock (sortedGameServers)
            {
                gameServerStates.Remove(gameServerState.GameServerID);
                gameServerStates.Add(gameServerState.GameServerID, gameServerState);

                sortedGameServers.AddOrUpdate(gameServerState.RunningGameCount, gameServerState.GameServerID);
            }
        }

        private List<User> Matching(int userCount)
        {
            List<User> users = new List<User>();

            lock (sortedUsers)
            {
                if (userCount > matchingUsers.Count)
                    return null;

                for (int i = 0; i < userCount; i++)
                {
                    var key = sortedUsers.PopFirstKey();
                    if (matchingUsers.Remove(key, out var user) == true)
                    {
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public bool BeginMatching(User user)
        {
            lock (sortedUsers)
            {
                if (matchingUsers.ContainsKey(user.UID) == true)
                {
                    return false;
                }

                sortedUsers.AddOrUpdate(user.MMR, user.UID);
                matchingUsers.Add(user.UID, user);

                return true;
            }
        }

        public void BeginMatching(List<User> users)
        {
            lock (sortedUsers)
            {
                foreach (User user in users)
                {
                    sortedUsers.AddOrUpdate(user.MMR, user.UID);
                    matchingUsers.Add(user.UID, user);
                }
            }
        }

        public void CancelMatching(User user)
        {
            lock (sortedUsers)
            {
                sortedUsers.Remove(user.UID);
                matchingUsers.Remove(user.UID);
            }
        }
    }
}
