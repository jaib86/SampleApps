using FriendStorage.Model;
using System.Collections.Generic;

namespace FriendStorage.DataAccess
{
    public class FileDataService : IDataService
    {
        private const string StorageFile = "Friends.json";

        public Friend GetFriendById(int friendId)
        {
            var friends = ReadFromFile();
            return friends.Single(f => f.Id == friendId);
        }

        public void SaveFriend(Friend friend)
        {
            if(friend.Id <= 0)
            {
                InsertFriend(friend);
            }
            else
            {

            }
        }

        public IEnumerable<Friend> GetAllFriends()
        {
            return ReadFromFile();
        }
    }
}