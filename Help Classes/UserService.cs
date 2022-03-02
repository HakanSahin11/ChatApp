using Chat_Application.Crud;
using Chat_Application.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Chat_Application.Help_Classes;
using static Chat_Application.Help_Classes.Salting;
namespace Chat_Application.Help_Classes
{
    public class UserService
    {
        private UserCrud _user;
       // private LocalStorageService _localStorage;
        public UserService(UserCrud user)
    //    public UserService(UserCrud user, LocalStorageService localStorage)
        {
            _user = user;
      //      _localStorage = localStorage;
        }


        public async Task<bool> InsertuserAsync(UserModel user)
        {
            try
            {
                var userOut = new UserModel();
                var salting = HashSalt(user.Password, new byte[0]);
                userOut.Password = salting.Pass;
                userOut.Salt = salting.Salt;

                userOut.Gender = user.Gender;
                userOut.Email = user.Email;
                userOut.Birthday = user.Birthday;
                userOut.Username = user.Username;

                var cryptoGraphy = new CryptoGraphy();
                var task1 = Task.Run(() =>
                {
                    userOut.FirstName = cryptoGraphy.Encrypter(user.FirstName, user.Salt);
                    userOut.LastName = cryptoGraphy.Encrypter(user.LastName, user.Salt);
                });
                task1.Wait();
                await _user.Create(userOut);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool>LoginAsync(Login Content)
        {
            try
            {
                //username in Content can be used for both username and email for credential login
                var SavedContent = _user.GetUserByUsername(Content.Username).Result;

                if (SavedContent == null)
                    SavedContent = _user.GetUserByEmail(Content.Username).Result;
                if (SavedContent == null)
                    return false;

                if (Content.Username == SavedContent.Email || Content.Username == SavedContent.Username)
                {
                    if (HashSalt(Content.Password, Convert.FromBase64String(SavedContent.Salt)).Pass == SavedContent.Password)
                    {
                        //_localStorage.SetItem("user", Content.Username).Wait();
                        /*
                        var task2 = _localStorage.GetItem<string>("user").Result;
                                    _localStorage.GetItem<string>("user")
                        var task3 = _localStorage.RemoveItem("user");
                        task3.Wait();
                        */

              //          var task2 = _localStorage.GetItem("user");
                        
                        return true;
                    }
                }



                return false;
            }
            catch
            {
                return false;
            }
        }



        public async Task<bool> test(AuthenticationStateProvider authenticationStateProvider)
        {

            return false;
        }
    }
}
