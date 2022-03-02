using Chat_Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat_Application.Crud
{
    public class UserCrud
    {
        //DB Initialazation, with input saved in launchSettings.json
        private readonly IMongoCollection<UserModel> _users;
        public UserCrud(IDBElements settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.Database);
                _users = database.GetCollection<UserModel>("User");
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 4.1 - Database connection establishment  - {e.Message}");

            }
        }

        //Get all users 
        public Task<List<UserModel>> Get() =>
            Task.Run(() =>
            _users.Find(x => true).ToList());

        //Get Specific user by Email (for loging mainly)
        public Task<UserModel> GetUserByEmail(string email) =>
           Task.Run(() =>
          _users.Find(x => x.Email == email).FirstOrDefault());

        //Get Specific user by Username (for loging mainly)
        public Task<UserModel> GetUserByUsername(string username) =>
            Task.Run(() => _users.Find(x => x.Username == username).FirstOrDefault());
        

        //Get specific user by Id
        public Task<UserModel> GetUserById(ObjectId id) =>
           Task.Run(() =>
           _users.Find(x => x.Id == id).FirstOrDefault());

        //Create new user
        public Task Create(UserModel user) =>
             Task.Run(() => _users.InsertOne(user));

        //Update existing user
        public void Update(string email, UserModel updatedUser) =>
           Task.Run(() =>
           _users.ReplaceOne(x => x.Email == email, updatedUser));

        //Delete existing user
        public void Delete(ObjectId id) =>
           Task.Run(() =>
           _users.DeleteOne(x => x.Id == id));
    }
}
