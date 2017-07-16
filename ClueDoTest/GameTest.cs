using System;
using ClueDo.GameSetup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClueDoTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void Must_be_able_to_create_and_initialize_game_for_4_players()
        {
            var game = new Game();
            game.Initialize(4);
            Assert.AreEqual(4,game.Players.Length);
            Assert.AreEqual(2, game.PublicCards.Places.Count+game.PublicCards.Persons.Count+game.PublicCards.Weapons.Count);
        }

        [TestMethod]
        public void Must_be_able_to_make_next_turn_for_four_players()
        {
            var game = new Game();
            game.Initialize(4);
            game.DoNextTurn();
        }

    }
}
