using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;

namespace DynamicLINQTwoLists
{
    public class Program
    {
        static void Main(string[] args) 
        {
            var users1 = GetUsers1();
            var users2 = GetUsers2();

            #region Matching Users for [Name, Address]

            Console.WriteLine("Matching Users for [Name, Address]: ");
            var uSearch1 = from first1 in users1
                           join second1 in users2
                                       on new { first1.Name, first1.Address } equals new { second1.Name, second1.Address }
                           select first1;
            foreach(var user in uSearch1)
            {
                Console.WriteLine($"Id: {user.Id} Name: {user.Name} Address: {user.Address} Height: {user.Height}");
            }

            Console.WriteLine();
            Console.WriteLine();
            #endregion

            #region Matching Users for dynamic fields
            //Used this => Install-Package System.Linq.Dynamic.Core

            Console.Write("Please enter search criteria [Id, Name, Address, Height] comma seperated: ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine();

            //Replace any white spaces
            input = Regex.Replace(input, @"\s+", "");
            var querry = "";

            //Build the Where clause (like first.Name == second.Name ...)
            foreach (var q in input.Split(','))
            {
                querry += (querry.Length > 0 ? " && " : "") + "first." + q + " == " + "second." + q;
            }

            //Get the result using System.Linq.Dynamic.Core
            var uSearch2 = users1.AsQueryable().Where("first => @0.Any(second => " + querry + ")", users2);

            Console.WriteLine("Matching Users for Dynamic Querry: ");

            Console.WriteLine();
            Console.WriteLine();

            foreach (var user in uSearch2)
            {
                Console.WriteLine($"Id: {user.Id} Name: {user.Name} Address: {user.Address} Height: {user.Height}");
            }

            #endregion

            Console.Write("Press any key to exit...");
            Console.ReadLine();

        }


        public static List<User> GetUsers1()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Kamal",
                    Address = "Colombo",
                    Height = 10.5
                },
                new User
                {
                    Id = 2,
                    Name = "Nimal",
                    Address = "Kandy",
                    Height = 14
                },
                new User
                {
                    Id = 3,
                    Name = "Vimal",
                    Address = "Kurunegala",
                    Height = 12.5
                },
                new User
                {
                    Id = 4,
                    Name = "Namal",
                    Address = "Gampaha",
                    Height = 11
                },

            };
        }

        public static List<User> GetUsers2()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Kamal",
                    Address = "Colombo",
                    Height = 10.5
                },
                new User
                {
                    Id = 2,
                    Name = "Nimal",
                    Address = "Kandy",
                    Height = 15
                },
                new User
                {
                    Id = 3,
                    Name = "Vimal",
                    Address = "Kaluthara",
                    Height = 12.5
                },
                new User
                {
                    Id = 4,
                    Name = "Himal",
                    Address = "Gampaha",
                    Height = 11
                },

            };
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Height { get; set; }

    }
}
