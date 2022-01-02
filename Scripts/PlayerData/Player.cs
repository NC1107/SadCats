using BreakInfinity;

namespace PlayerData
{
    public class Player
    {
        private string _username;
        private string _password;
        private BigDouble _score;

        public Player()
        {
            _username = "Default";
            _password = "Pass";
            _score = 0;
        }

        public Player(string u, string p, BigDouble s)
        {
            _username = u;
            _password = p;
            _score = s;
            
        }
        public Player(string u, BigDouble s)
        {
            _username = u;
            _password = null;
            _score = s;
            
        }


        public string GetUser()
        {
            return _username;
        }

        public string GetPassword()
        {
            return _password;
        }

        public BigDouble GetScore()
        {
            return _score;
        }

        public void SetUser(string newUser)
        {
            _username = newUser;
        }

        public void SetPassword(string newPassword)
        {
            _password = newPassword;
        }

        public void SetScore(BigDouble newScore)
        {
            _score = newScore;
        }

        public void IncreaseScore(int amount)
        {
            _score += amount;
        }
    }
}