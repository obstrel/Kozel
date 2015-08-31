using System;
using Kozel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Sorting
    {
        [TestMethod]
        public void SameSuite()
        {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Eight);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ten);
            Assert.IsTrue(card2.GetHashCode() > card1.GetHashCode());
        }

        [TestMethod]
        public void PermTrump()
        {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Jack);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Assert.IsTrue(card2.GetHashCode() < card1.GetHashCode());
        }

        [TestMethod]
        public void ShohaIsTop()
        {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Six);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Queen);
            Assert.IsTrue(card2.GetHashCode() < card1.GetHashCode());
        }

        [TestMethod]
        public void Case1()
        {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Eight);
            card2.IsTrump = true;
            Assert.IsTrue(card2.GetHashCode() > card1.GetHashCode());
        }

        [TestMethod]
        public void Case2() {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Six);
            card2.IsTrump = true;
            Assert.IsTrue(card2.GetHashCode() > card1.GetHashCode());
        }

        [TestMethod]
        public void TrickOwnerWithTrump() {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Eight);
            Kozel.Card card3 = new Kozel.Card(Kozel.CardSuit.Spade, Kozel.CardValue.Ten);
            Kozel.Card card4 = new Kozel.Card(Kozel.CardSuit.Heart, Kozel.CardValue.Eight);
            card2.IsTrump = true;
            Player p1 = new Player();
            Player p2 = new Player();
            Player p3 = new Player();
            Player p4 = new Player();
            Trick t = new Trick();
            t.AddMove(card1, p1);
            t.AddMove(card2, p2);
            t.AddMove(card3, p3);
            t.AddMove(card4, p4);
            Assert.AreEqual(t.GetTrickWinner(), p2);
        }

        [TestMethod]
        public void CanThrowCard() {
            Player p1 = new Player();
            Player p2 = new Player();
            Player p3 = new Player();
            Player p4 = new Player();
            Team team1 = new Team(p1, p3);
            Team team2 = new Team(p2, p4);
            Round round = new Round(team1, team2);
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Eight);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ten);
            Kozel.Card card3 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card4 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Six);

            p1.AddCard(card1);
            p2.AddCard(card2);
            p3.AddCard(card3);
            p4.AddCard(card4);
            round.Trick.AddMove(card1, p1);
            round.Trick.AddMove(card2, p2);
            round.Trick.AddMove(card3, p3);
            // round.Trick.AddMove(card4, p4);
            Assert.IsTrue(round.CanThrowCard(p4, card1));
        }

        [TestMethod]
        public void TrickOwnerOneSuit() {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Eight);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ten);
            Kozel.Card card3 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card4 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.King);
            Player p1 = new Player();
            Player p2 = new Player();
            Player p3 = new Player();
            Player p4 = new Player();
            Trick t = new Trick();
            t.AddMove(card1, p1);
            t.AddMove(card2, p2);
            t.AddMove(card3, p3);
            t.AddMove(card4, p4);
            Assert.AreEqual(t.GetTrickWinner(), p3);
        }
        [TestMethod]
        public void TrickOwnerPermTrump() {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Queen);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Jack);
            Kozel.Card card3 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card4 = new Kozel.Card(Kozel.CardSuit.Spade, Kozel.CardValue.Queen);
            card3.IsTrump = true;
            Player p1 = new Player();
            Player p2 = new Player();
            Player p3 = new Player();
            Player p4 = new Player();
            Trick t = new Trick();
            t.AddMove(card1, p1);
            t.AddMove(card2, p2);
            t.AddMove(card3, p3);
            t.AddMove(card4, p4);
            Assert.AreEqual(t.GetTrickWinner(), p1);
        }
        [TestMethod]
        public void TrickOwnerSuitWithoutTrump() {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Eight);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Spade, Kozel.CardValue.Ace);
            Kozel.Card card3 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card4 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Nine);
            Player p1 = new Player();
            Player p2 = new Player();
            Player p3 = new Player();
            Player p4 = new Player();
            Trick t = new Trick();
            t.AddMove(card1, p1);
            t.AddMove(card2, p2);
            t.AddMove(card3, p3);
            t.AddMove(card4, p4);
            Assert.AreEqual(t.GetTrickWinner(), p4);
        }

        [TestMethod]
        public void TrickOwnerSmallestSuit() {
            Kozel.Card card1 = new Kozel.Card(Kozel.CardSuit.Diamond, Kozel.CardValue.Six);
            Kozel.Card card2 = new Kozel.Card(Kozel.CardSuit.Spade, Kozel.CardValue.Ace);
            Kozel.Card card3 = new Kozel.Card(Kozel.CardSuit.Club, Kozel.CardValue.Ace);
            Kozel.Card card4 = new Kozel.Card(Kozel.CardSuit.Heart, Kozel.CardValue.Ace);
            Player p1 = new Player();
            Player p2 = new Player();
            Player p3 = new Player();
            Player p4 = new Player();
            Trick t = new Trick();
            t.AddMove(card1, p1);
            t.AddMove(card2, p2);
            t.AddMove(card3, p3);
            t.AddMove(card4, p4);
            Assert.AreEqual(t.GetTrickWinner(), p1);
        }

    }
}
