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
    }
}
