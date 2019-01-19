using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.Logic
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel tournament)
        {
            List<TeamModel> randomizedEnteredTeams = RandomizeTeamOrder(tournament.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedEnteredTeams.Count);
            int byes = NumberOfByes(rounds, randomizedEnteredTeams.Count);
            tournament.Rounds.Add(CreateFirstRound(byes,randomizedEnteredTeams));
            CreateRemainingRounds(tournament, rounds);
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int rounds = 1;
            int value = 2;
        
            while(value < teamCount)
            {
                rounds += 1;
                value *= 2;
            }

            return rounds;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams = numberOfTeams;

            return output;
        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                currentMatchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });
                if (byes > 0 || currentMatchup.Entries.Count > 1)
                {
                    currentMatchup.MatchupRound = 1;
                    output.Add(currentMatchup);
                    currentMatchup = new MatchupModel();
                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static void CreateRemainingRounds(TournamentModel tournament, int requiredRounds)
        {
            int generatingRound = 2;
            List<MatchupModel> previousRoundMatches = tournament.Rounds[0];
            List<MatchupModel> currentRoundMatches = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            while (generatingRound <= requiredRounds)
            {
                foreach (MatchupModel match in previousRoundMatches)
                {
                    currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    if(currentMatchup.Entries.Count > 1)
                    {
                        currentMatchup.MatchupRound = generatingRound;
                        currentRoundMatches.Add(currentMatchup);
                        currentMatchup = new MatchupModel();
                    }
                }

                tournament.Rounds.Add(currentRoundMatches);
                previousRoundMatches = currentRoundMatches;
                currentRoundMatches = new List<MatchupModel>();
                generatingRound += 1;
            }
        }
    }
}

