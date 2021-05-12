using FoodApi.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public AccountController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "s";
        }

        [HttpGet]
        [Route("ActivateAccount")]
        public string ActivateUser(string login, int code)
        {

            UnitOfWork _uow = new UnitOfWork();
            return _uow.ActivateUser(login, code);
        }

        [HttpGet]
        [Route("LoginToAccount")]
        public string LoginUser(string login, string password)
        {

            UnitOfWork _uow = new UnitOfWork();
            return _uow.LoginUser(login, Base64Decode(password));
        }

        [HttpGet]
        [Route("CreateNewUser")]
        public string CreateUser(string login, string password, string userName)
        {

            UnitOfWork _uow = new UnitOfWork();
           return  _uow.AddUser(login, Base64Decode(password), userName);
        }

        [HttpGet]
        [Route("SendCodeAgain")]
        public void SendAgain(string login)
        {
            UnitOfWork _uow = new UnitOfWork();
           _uow.SendCodeAgain(login);
        }

        [HttpGet]
        [Route("ResetUserPassword")]
        public string ResetPassword(string login)
        {
            UnitOfWork _uow = new UnitOfWork();
            return _uow.ResetPassword(login);
        }

        [HttpGet]
        [Route("SetNewUserPassword")]
        public void SetNewPassword(string login, string password)
        {
            UnitOfWork _uow = new UnitOfWork();
            _uow.SetNewPassword(login,password);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
