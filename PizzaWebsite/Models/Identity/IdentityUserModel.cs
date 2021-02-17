using DataLibrary.Models.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityUserModel : IUser
    {
        private string id;
        private string userName;
        private string passwordHash;
        private string email;
        private int currentCartId;
        private int confirmOrderCartId;
        private int orderConfirmationId;
        private bool isBanned;
        private bool emailConfirmed;
        private string phoneNumber;
        private bool phoneNumberConfirmed;
        private string zipCode;
        private IList<string> roles;

        public IdentityUserModel()
        {
            roles = new List<string>();
        }

        public IdentityUserModel(UserModel user) : this()
        {
            id = user.Id.ToString();
            userName = user.UserName;
            passwordHash = user.PasswordHash;
            email = user.Email;
            currentCartId = user.CurrentCartId;
            confirmOrderCartId = user.ConfirmOrderCartId;
            orderConfirmationId = user.OrderConfirmationId;
            isBanned = user.IsBanned;
            emailConfirmed = user.EmailConfirmed;
            phoneNumber = user.PhoneNumber;
            phoneNumberConfirmed = user.PhoneNumberConfirmed;
            zipCode = user.ZipCode;

            foreach (var role in user.Roles)
            {
                roles.Add(role);
            }
        }

        public UserModel ToDbModel()
        {
            UserModel user = new UserModel();
            bool isNewUser = id == null;

            if (!isNewUser)
            {
                user.Id = int.Parse(id);
            }

            user.UserName = userName;
            user.PasswordHash = passwordHash;
            user.Email = email;
            user.CurrentCartId = currentCartId;
            user.ConfirmOrderCartId = confirmOrderCartId;
            user.OrderConfirmationId = orderConfirmationId;
            user.IsBanned = isBanned;
            user.EmailConfirmed = emailConfirmed;
            user.PhoneNumber = phoneNumber;
            user.PhoneNumberConfirmed = phoneNumberConfirmed;
            user.ZipCode = zipCode;

            foreach (var role in roles)
            {
                user.Roles.Add(role);
            }

            return user;
        }

        public string UserName { get => userName; set => userName = value; }
        public string Email { get => email; set => email = value; }
        public string PasswordHash { get => passwordHash; set => passwordHash = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
        public string Id { get => id; set => id = value; }
        public bool EmailConfirmed { get => emailConfirmed; set => emailConfirmed = value; }
        public IList<string> Roles { get => roles; set => roles = value; }
        public int OrderConfirmationId { get => orderConfirmationId; set => orderConfirmationId = value; }
    }
}