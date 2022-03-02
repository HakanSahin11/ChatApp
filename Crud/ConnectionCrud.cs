using Chat_Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat_Application.Crud
{
    public class ConnectionCrud
    {
        //DB Initialazation, with input saved in launchSettings.json
        private readonly IMongoCollection<SessionModel> _session;
        public ConnectionCrud(IDBElements settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.Database);
                _session = database.GetCollection<SessionModel>("Session");
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 4.1 - Database connection establishment  - {e.Message}");

            }
        }

        //Get all users 
        public Task<List<SessionModel>> Get() =>
            Task.Run(() =>
            _session.Find(x => true).ToList());

      
        //Get Specific user by Username
        public Task<SessionModel> GetSessionByUsername(string username) =>
            Task.Run(() => _session.Find(x => x.Username == username).FirstOrDefault());


        //Get specific user by Id
        public Task<SessionModel> GetSessionById(ObjectId id) =>
           Task.Run(() =>
           _session.Find(x => x.Id == id).FirstOrDefault());

        //Create new user
        public Task Create(SessionModel content) =>
             Task.Run(() => _session.InsertOne(content));

        //Update existing user
        public void Update(string username, SessionModel updatedUser) =>
           Task.Run(() =>
           _session.ReplaceOne(x => x.Username == username, updatedUser));

        //Delete existing user
        public void Delete(ObjectId id) =>
           Task.Run(() =>
           _session.DeleteOne(x => x.Id == id));
    }
}
