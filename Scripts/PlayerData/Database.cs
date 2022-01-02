using System;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

namespace PlayerData
{
    public class Database
    {
        private readonly FirebaseDatabase _firebase;
        private JObject _database;
        private List<Player> _players = new List<Player>();
        private string ScoreboardPath { get; } = "Scoreboard";
        private string LoginPath { get; } = "Login";

        private bool ShowDebugPrints { get; set; } = true;

        //Overloaded Constructor, creates object based from firebase reference
        public Database(DatabaseReference rootDatabaseReference)
        {
            _database = null;
            _firebase = rootDatabaseReference.Database;
            UpdateDatabase();
        }

        //Simple Debugger
        private void Print(string message)
        {
            if (ShowDebugPrints)
            {
                UnityEngine.Debug.Log(message + "\n");
            }
        }

        //Toggle debugger from outside
        public void ShowDebug(bool choice)
        {
            ShowDebugPrints = choice;
        }


        private void UpdateDatabase()
        {
            _firebase.RootReference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompleted) return;
                _database = JObject.Parse(task.Result.GetRawJsonValue());
            });
        }


        //Find out whether or not user exists in database
        private bool FindUser(string user)
        {
            return _database[ScoreboardPath]!.ToString().Contains(user);
        }

        //checks if user exists
        //checks if password matches
        //if so returns true

        public bool LoginUser(string user, string password)
        {
            if (FindUser(user))
            {
                Print("User was found\n Checking password");
                if (_database[LoginPath]![user]!.ToString() == password) return true;
                Print("Invalid Password");
            }
            else
            {
                Print("user does not exist \n creating new user");
                AddUser(user, password);
                return true;
            }

            return false;
        }

        private void AddUser(string user, string password)
        {
            //check if user exists, if so, leave, otherwise add user to database
            if (FindUser(user)) return;
            _firebase.RootReference.Child(ScoreboardPath).Child(user).SetValueAsync(0);
            _firebase.RootReference.Child(LoginPath).Child(user).SetValueAsync(password);
            Print("user was added to system");
            UpdateDatabase();
        }


        public Player GetPlayer(string user)
        {
            if (!FindUser(user))
            {
                Print("Not Found");
                return null;
            }
            var p = new Player();
            p.SetUser(user);
            p.SetScore(new BigDouble(Convert.ToDouble(_database[ScoreboardPath]![user])));
            p.SetPassword(_database[LoginPath]![user]!.ToString());
            return p;

        }

        public void SavePlayer(string user)
        {
            var player = GetPlayer(user);
            PlayerPrefs.SetString("username", player.GetUser());
            PlayerPrefs.SetString("password", player.GetPassword());
            PlayerPrefs.SetString("score", player.GetScore().ToString());
        }

        public Player LoadPlayer(string user)
        {
            var player = new Player();
            player.SetUser(PlayerPrefs.GetString("username"));
            player.SetPassword(PlayerPrefs.GetString("password"));
            player.SetScore(new BigDouble(Convert.ToDouble(PlayerPrefs.GetString("score"))));
            return player;
        }


        public void UpdatePlayer(Player p)
        {
            //Check if user exists
            //check for changes in database versus new given data
            // if (p.GetScore() == GetPlayer(p.GetUser()).GetScore()) return;
            // if (p.GetPassword() == GetPlayer(p.GetUser()).GetPassword()) return;

            _firebase.RootReference.Child(ScoreboardPath).Child(p.GetUser()).SetValueAsync(p.GetScore().ToString());
            _firebase.RootReference.Child(LoginPath).Child(p.GetUser()).SetValueAsync(p.GetPassword());
            SavePlayer(p.GetUser());
        }
    }
}