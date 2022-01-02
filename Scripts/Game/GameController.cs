using System;
using BreakInfinity;
using Firebase.Database;
using PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private Player _player;
        private Database _database;

        public TMP_Text userName;
        public TMP_Text userScore;
    
        private void Awake()
        {
            if (!PlayerPrefs.HasKey("username"))
            {
                SceneManager.LoadScene("Login");
            }
        }



        private void Start()
        {
            _database = new Database(FirebaseDatabase.DefaultInstance.RootReference);
            _database.ShowDebug(true);
            _player = _database.GetPlayer(PlayerPrefs.GetString("username"));
        }

        private void FixedUpdate()
        {
            userName.text = _player.GetUser();
            userScore.text = _player.GetScore().ToString();
        }


        public void OnClickIncrease()
        {
            _player.IncreaseScore(5);
            _database.UpdatePlayer(_player);
        }





       
   
   
   
   
   
    }
}
