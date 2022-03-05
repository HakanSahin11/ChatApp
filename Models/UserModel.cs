using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Chat_Application.Models
{
    public class UserModel
    {
        public UserModel(ObjectId? id, string email, string username, string password, string firstName, string lastName, string gender, DateTime birthday, string salt)
        {
            Id = id;
            Email = email;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Birthday = birthday;
            Salt = salt;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Salt { get; set; }

    }
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime Birthday { get; set; } = DateTime.ParseExact("01/01/2000", "d", CultureInfo.InvariantCulture);
    }

    public class SessionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }


    public class Login
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }

    public class SaltHash
    {
        public SaltHash(string pass, string salt)
        {
            Pass = pass;
            Salt = salt;
        }
        public string Pass { get; set; }
        public string Salt { get; set; }
    }
}