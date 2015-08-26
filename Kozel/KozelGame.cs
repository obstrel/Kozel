using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel
{
    public class KozelGame
    {
        public static string Name { get { return "Kozel"; } }

        private Team team1;
        private Team team2;
        private int activePlayer = 0;
        private int activeRound = 0;
        List<Player> players = new List<Player>(4) { new Player(), new Player(), new Player(), new Player() };
        List<Round> rounds = new List<Round>(8) { new Round(), new Round(), new Round(), new Round(), new Round(), new Round(), new Round(), new Round() };

        public Team Team1 { get  { return team1; }}
        public Team Team2 { get { return team2; } }
        public Player ActivePlayer { get { return players[activePlayer]; } }
        public Round ActiveRound { get { return rounds[activeRound]; } }


        private CardSuit? GetTrumpSuit()
        {
            Random r = new Random();
            Array ar = Enum.GetValues(typeof(CardSuit));
            return (CardSuit)ar.GetValue((int)r.Next(3));
        }

        public event EventHandler GameStarted;
        public event EventHandler<PlayerEventArgs> PlayerMadeMove;
        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;
        public event EventHandler<PlayerEventArgs> CardsResorted;

        private Queue<Card> deck = new Queue<Card>(32);

        public KozelGame() {
            FillDeck(deck);
            team1 = new Team(players[0], players[2]);
            team2 = new Team(players[1], players[3]);
            foreach(Player player in players)
            {
                player.PlayerMadeMove += Player_PlayerMadeMove;
                player.CardsResorted += Player_CardsResorted;
            }
        }

        private void Player_CardsResorted(object sender, PlayerEventArgs e)
        {
            if(CardsResorted != null)
            {
                CardsResorted(this, new PlayerEventArgs(e.Player));
            }
        }

        private void Player_PlayerMadeMove(object sender, PlayerEventArgs e)
        {
            if(PlayerMadeMove != null)
                PlayerMadeMove(this, new PlayerEventArgs(e.Player));
            activePlayer = activePlayer == 3 ? 0 : activePlayer + 1;
            if(ActivePlayerChanged != null)
            {
                ActivePlayerChanged(this, new PlayerEventArgs(ActivePlayer));
            }
        }

        private int GetActivePlayer()
        {
            for(int i = 0; i < 4; i++)
            {
                if (players[i].GetCards().Any(c => { return c.Suit == CardSuit.Diamond && c.Value == CardValue.Six; }))
                    return i;
            }
            return 0;
        }

        public void Start()
        {
            rounds[0].TrumpSuit = CardSuit.Diamond;
            DealCards(deck, players);
            //            TrumpSuit = (CardSuit)GetTrumpSuit();
            activePlayer = GetActivePlayer();
            rounds[0].SetTrumpCards(players);
            SortCards();
            if (GameStarted != null)
                GameStarted(this, new EventArgs());
        }

        private void SortCards()
        {
            foreach(Player p in players)
            {
                p.SortCards();
            }
        }



        private void DealCards(Queue<Card> deck, List<Player> players)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    players[j].AddCard(deck.Dequeue());
                }
            }
        }


        private void FillDeck(Queue<Card> deck)
        { 
            List<Card> cardList = new List<Card> {
                new Card(CardSuit.Diamond, CardValue.Six), new Card(CardSuit.Heart, CardValue.Six), new Card(CardSuit.Spade, CardValue.Six), new Card(CardSuit.Club, CardValue.Six),
                new Card(CardSuit.Diamond, CardValue.Eight), new Card(CardSuit.Heart, CardValue.Eight), new Card(CardSuit.Spade, CardValue.Eight), new Card(CardSuit.Club, CardValue.Eight),
                new Card(CardSuit.Diamond, CardValue.Nine), new Card(CardSuit.Heart, CardValue.Nine), new Card(CardSuit.Spade, CardValue.Nine), new Card(CardSuit.Club, CardValue.Nine),
                new Card(CardSuit.Diamond, CardValue.King), new Card(CardSuit.Heart, CardValue.King), new Card(CardSuit.Spade, CardValue.King), new Card(CardSuit.Club, CardValue.King),
                new Card(CardSuit.Diamond, CardValue.Ten), new Card(CardSuit.Heart, CardValue.Ten), new Card(CardSuit.Spade, CardValue.Ten), new Card(CardSuit.Club, CardValue.Ten),
                new Card(CardSuit.Diamond, CardValue.Ace), new Card(CardSuit.Heart, CardValue.Ace), new Card(CardSuit.Spade, CardValue.Ace), new Card(CardSuit.Club, CardValue.Ace),
                new Card(CardSuit.Diamond, CardValue.Jack), new Card(CardSuit.Heart, CardValue.Jack), new Card(CardSuit.Spade, CardValue.Jack), new Card(CardSuit.Club, CardValue.Jack),
                new Card(CardSuit.Diamond, CardValue.Queen), new Card(CardSuit.Heart, CardValue.Queen), new Card(CardSuit.Spade, CardValue.Queen), new Card(CardSuit.Club, CardValue.Queen),
            };

            Random rnd = new Random();
            
            while(cardList.Count > 0) {
                int index = rnd.Next(cardList.Count - 1);
                deck.Enqueue(cardList[index]);
                cardList.RemoveAt(index);
            }
            
        }
    }
}
