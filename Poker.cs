using System;

using System.Collections.Generic;



namespace Euler.Core

{

    public enum Rank

    {

        Two,

        Three,

        Four,

        Five,

        Six,

        Seven,

        Eight,

        Nine,

        Ten,

        Jack,

        Queen,

        King,

        Ace,

    }



    public enum Suit

    {

        Heart,

        Spade,

        Diamond,

        Club,

    }



    public enum Criteria

    {

        HighCard,

        OnePair,

        TwoPairsSecond,

        TwoPairsFirst,

        ThreeOfAKind,

        Straight,

        Flush,

        FullHouseTwo,

        FullHouseThree,

        FourOfAKind,

        StraightFlush,

        RoyalFlush

    }



    public class Poker

    {

        internal static int CountWhereAWins()

        {

            var hands = System.IO.File.ReadAllLines(@"Z:\p054_poker.txt");



            var victoryCount = 0;



            foreach (var bothHand in hands)

            {

                if (ComparePokerHands(bothHand))

                    victoryCount++;

            }



            return victoryCount;

        }



        internal static bool ComparePokerHands(string bothHands)

        {

            if (bothHands.Length != 29)

                throw new InvalidOperationException(string.Format("Cannot analyse such fight {0}", bothHands));



            var cards = bothHands.Split(new char[] { ' ' }, StringSplitOptions.None);



            var playerAHand = new List<PokerCard>();



            for (int i = 0; i < 5; i++)

                playerAHand.Add(Create(cards[i]));



            var playerBHand = new List<PokerCard>();



            for (int i = 5; i < 10; i++)

                playerBHand.Add(Create(cards[i]));



            return Poker.IsAWinner(playerAHand, playerBHand);

        }



        internal static PokerCard Create(string input)

        {

            if (input.Length != 2)

                throw new InvalidOperationException(string.Format("Cannot read such card {0}", input));



            var figure = ExtractRank(input[0]);

            var color = ExtractSuit(input[1]);



            return new PokerCard { Color = color, Figure = figure };

        }



        internal static bool IsAWinner(List<PokerCard> playerA, List<PokerCard> playerB)

        {

            var handA = PokerHand.Create(playerA);

            var handB = PokerHand.Create(playerB);



            return handA.CompareTo(handB) > 0;

        }



        private static Rank ExtractRank(char index)

        {

            switch (index)

            {

                case '2':

                    return Rank.Two;

                case '3':

                    return Rank.Three;

                case '4':

                    return Rank.Four;

                case '5':

                    return Rank.Five;

                case '6':

                    return Rank.Six;

                case '7':

                    return Rank.Seven;

                case '8':

                    return Rank.Eight;

                case '9':

                    return Rank.Nine;

                case 'T':

                    return Rank.Ten;

                case 'J':

                    return Rank.Jack;

                case 'Q':

                    return Rank.Queen;

                case 'K':

                    return Rank.King;

                case 'A':

                    return Rank.Ace;



                default:

                    throw new InvalidOperationException(string.Format("Cannot read such rank {0}", index));

            }

        }



        private static Suit ExtractSuit(char index)

        {

            switch (index)

            {

                case 'H':

                    return Suit.Heart;

                case 'C':

                    return Suit.Club;

                case 'D':

                    return Suit.Diamond;

                case 'S':

                    return Suit.Spade;



                default:

                    throw new InvalidOperationException(string.Format("Cannot read such suit {0}", index));

            }

        }





        internal class PokerHand : IComparable<PokerHand>

        {

            public List<PokerCard> Hand { get; set; }

            public Stack<Tuple<Criteria, Rank>> WinningList { get; set; }



            public int CompareTo(PokerHand other)

            {

                while (WinningList.Count > 0)

                {

                    var myCriteria = WinningList.Pop();

                    var otherCriteria = other.WinningList.Pop();

                    var currentAnalysis = CompareCriterion(myCriteria, otherCriteria);



                    if (currentAnalysis != 0)

                        return currentAnalysis;

                }



                return 0;

            }



            private static int CompareCriterion(Tuple<Criteria, Rank> first, Tuple<Criteria, Rank> second)

            {

                var criteriaAnalysis = ((int)first.Item1).CompareTo((int)second.Item1);



                if (criteriaAnalysis != 0)

                    return criteriaAnalysis;



                var rankAnalysis = ((int)first.Item2).CompareTo((int)second.Item2);



                if (rankAnalysis != 0)

                    return rankAnalysis;



                return 0;

            }



            public static PokerHand Create(List<PokerCard> hand)

            {

                if (hand.Count != 5)

                    throw new InvalidOperationException("Cannot handle not-5 poker hands");



                hand.Sort();



                return new PokerHand(hand);

            }



            private PokerHand(List<PokerCard> hand)

            {

                Hand = hand;



                WinningList = new Stack<Tuple<Criteria, Rank>>();



                SafeSearch(SearchForRoyalFlush);

                SafeSearch(SearchForStraightFlush);

                SafeSearch(SearchForFour);

                SafeSearch(SearchForFullHouse);

                SafeSearch(SearchForFlush);

                SafeSearch(SearchForStraight);

                SafeSearch(SearchForThree);

                SafeSearch(SearchForPairs);

                SafeSearch(SearchForHighest);

            }



            private void SafeSearch(Action toExecute)

            {

                if (WinningList.Count > 0)

                    return;



                toExecute();

            }



            private void SearchForRoyalFlush()

            {

                if (!AllSameSuit())

                    return;



                if (Hand[0].Figure == Rank.Ten

                    && Hand[1].Figure == Rank.Jack

                    && Hand[2].Figure == Rank.Queen

                    && Hand[3].Figure == Rank.King

                    && Hand[4].Figure == Rank.Ace)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Rank.Ace));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.RoyalFlush, Rank.Ten));

                }

            }



            private void SearchForStraightFlush()

            {

                if (!AllSameSuit())

                    return;



                if (Hand[1].Figure == Hand[0].Figure + 1

                    && Hand[2].Figure == Hand[1].Figure + 1

                    && Hand[3].Figure == Hand[2].Figure + 1

                    && Hand[4].Figure == Hand[3].Figure + 1)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.StraightFlush, Hand[0].Figure));

                }

            }



            private void SearchForFour()

            {

                if (Hand[0].Figure == Hand[1].Figure

                 && Hand[1].Figure == Hand[2].Figure

                 && Hand[2].Figure == Hand[3].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.FourOfAKind, Hand[0].Figure));

                    return;

                }



                if (Hand[1].Figure == Hand[2].Figure

                 && Hand[2].Figure == Hand[3].Figure

                 && Hand[3].Figure == Hand[4].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[0].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.FourOfAKind, Hand[1].Figure));

                }

            }



            private void SearchForFullHouse()

            {

                if (Hand[0].Figure == Hand[1].Figure

                 && Hand[1].Figure == Hand[2].Figure)

                {

                    if (Hand[3].Figure != Hand[4].Figure)

                        return;



                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.FullHouseTwo, Hand[3].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.FullHouseThree, Hand[0].Figure));

                    return;

                }



                if (Hand[2].Figure == Hand[3].Figure

                 && Hand[3].Figure == Hand[4].Figure)

                {

                    if (Hand[0].Figure != Hand[1].Figure)

                        return;



                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.FullHouseTwo, Hand[0].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.FullHouseThree, Hand[3].Figure));

                    return;

                }

            }



            private void SearchForFlush()

            {

                if (!AllSameSuit())

                    return;



                WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                WinningList.Push(new Tuple<Criteria, Rank>(Criteria.Flush, Hand[4].Figure));

            }



            private void SearchForStraight()

            {

                if (Hand[1].Figure == Hand[0].Figure + 1

                    && Hand[2].Figure == Hand[1].Figure + 1

                    && Hand[3].Figure == Hand[2].Figure + 1

                    && Hand[4].Figure == Hand[3].Figure + 1)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.Straight, Hand[0].Figure));

                }

            }



            private void SearchForThree()

            {

                if (Hand[0].Figure == Hand[1].Figure

                 && Hand[1].Figure == Hand[2].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.ThreeOfAKind, Hand[0].Figure));

                    return;

                }



                if (Hand[1].Figure == Hand[2].Figure

                 && Hand[2].Figure == Hand[3].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.ThreeOfAKind, Hand[1].Figure));

                    return;

                }



                if (Hand[2].Figure == Hand[3].Figure

                 && Hand[3].Figure == Hand[4].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[1].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.ThreeOfAKind, Hand[2].Figure));

                    return;

                }

            }



            private void SearchForPairs()

            {

                if (Hand[0].Figure == Hand[1].Figure)

                {

                    if (Hand[2].Figure == Hand[3].Figure)

                    {

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.TwoPairsSecond, Hand[0].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.TwoPairsFirst, Hand[2].Figure));

                    }

                    else if (Hand[3].Figure == Hand[4].Figure)

                    {

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[2].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.TwoPairsSecond, Hand[0].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.TwoPairsFirst, Hand[3].Figure));

                    }

                    else

                    {

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.OnePair, Hand[1].Figure));

                    }

                    return;

                }



                if (Hand[1].Figure == Hand[2].Figure)

                {

                    if (Hand[3].Figure == Hand[4].Figure)

                    {

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[0].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.TwoPairsSecond, Hand[1].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.TwoPairsFirst, Hand[3].Figure));

                    }

                    else

                    {

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                        WinningList.Push(new Tuple<Criteria, Rank>(Criteria.OnePair, Hand[1].Figure));

                    }

                    return;

                }



                if (Hand[2].Figure == Hand[3].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.OnePair, Hand[2].Figure));

                }



                if (Hand[3].Figure == Hand[4].Figure)

                {

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[2].Figure));

                    WinningList.Push(new Tuple<Criteria, Rank>(Criteria.OnePair, Hand[3].Figure));

                }

            }



            private void SearchForHighest()

            {

                WinningList.Push(new Tuple<Criteria, Rank>(Criteria.HighCard, Hand[4].Figure));

            }



            private bool AllSameSuit()

            {

                var suit = Hand[0].Color;



                for (var i = 1; i < 5; i++)

                {

                    if (Hand[i].Color != suit)

                        return false;

                }



                return true;

            }

        }



        internal class PokerCard : IComparable<PokerCard>

        {

            public Suit Color { get; set; }

            public Rank Figure { get; set; }



            public int CompareTo(PokerCard other)

            {

                return ((int)Figure).CompareTo((int)other.Figure);

            }



            public override string ToString()

            {

                return string.Format("{0}-{1}", Figure, Color);

            }

        }

    }

}

