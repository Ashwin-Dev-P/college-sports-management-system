
using System.Reflection.PortableExecutable;

namespace Microsoft.Data.SqlClient
{
    internal class Program
    {

        static string CONN_STRING = "Data Source=5CG9400GXC;Initial Catalog=CollegeSportManagementSystem;Integrated Security=True;Encrypt=False;";

        public static void Display(string TableName, SqlCommand cmd)
        {
            cmd.CommandText = $"select * from {TableName}";
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"\n{TableName}");
            Console.WriteLine("====================");
            Console.WriteLine("id  name");
            Console.WriteLine("====================");

            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)}");

            }
            reader.Close();
            Console.WriteLine("====================\n");
        }

        public static void DisplaySportsInTournament(int tournamentId, SqlCommand cmd)
        {
            // Query to get all the sports present in the tournament.
            cmd.CommandText = $"SELECT id,name FROM Sports where ( id in ( select sportId from TournamentSportCombination where(tournamentId = '{tournamentId}') )  )";
            SqlDataReader reader = cmd.ExecuteReader();
            


            // Print the sports present in the tournament after all the sports are done adding.
            Console.WriteLine($"\nThe sports in the tournament are:");
            Console.WriteLine("-----------------------------------------------------------");
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)}");
            }
            Console.WriteLine("-----------------------------------------------------------\n");

            reader.Close();
        }






        public static void AddSports(string sportName, SqlCommand cmd)
        {

            cmd.CommandText = $"insert into Sports(name) values('{sportName}')";

            cmd.ExecuteNonQuery();
            Console.WriteLine("\n" + sportName + " added\n");
        }

        public static void RemoveSports(int id, SqlCommand cmd)
        {
            
            cmd.CommandText = $"delete from Sports where (id='{id}')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("\n" + "Sport" + " removed\n");
        }

        public static void RemovePlayer(int playerId, SqlCommand cmd)
        {

            cmd.CommandText = $"delete from Player where (playerId='{playerId}')";
            cmd.ExecuteNonQuery();
        }



        public static void AddTournament(string tournamentName, SqlCommand cmd)
        {
            

            cmd.CommandText = $"insert into Tournament(name) values('{tournamentName}')";
            cmd.ExecuteNonQuery();

            Console.WriteLine("\nTournament '" + tournamentName + "' created\n");

            // Getting the id of the tournament created using querying by fetching the last inserted row
            cmd.CommandText = $"SELECT TOP 1 id FROM Tournament ORDER BY id DESC";
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int tournamentId =  reader.GetInt32(0);
            reader.Close();
            

            
            // Adding sports to the tournament
            
            while (true)
            {
                Console.WriteLine("\nSelect an option:\n 1) Add sport to the tournament \n 2) Done");
                int option = Convert.ToInt32(Console.ReadLine());

                if (option == 2)
                {
                    DisplaySportsInTournament(tournamentId,cmd);

                    break;
                }
                else if(option == 1)
                {
                    Display("Sports",cmd);

                    // Get sport id as input.
                    Console.WriteLine("\nSelect the sport id:");
                    int sportId = Convert.ToInt32(Console.ReadLine());

                    // Add sport to the tournament
                    cmd.CommandText = $"insert into TournamentSportCombination(tournamentId, sportId) values('{tournamentId}','{sportId}')";
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Sport added");
                }

            }
        }

        public static void RemoveTournament(int tournamentId, SqlCommand cmd)
        {

            cmd.CommandText = $"delete from Tournament where (id='{tournamentId}')";
            cmd.ExecuteNonQuery();
        }

        public static void AddScoreBoard(int tournamentId , int sportId,  int playerId , SqlCommand cmd)
        {

            cmd.CommandText = $"insert into Scoreboard(tournamentId, sportId, playerId) values('{tournamentId}','{sportId}','{playerId}')";
            cmd.ExecuteNonQuery();
        }

        public static void EditScoreBoard(int scoreboardId, int tournamentId, int sportId, int playerId, SqlCommand cmd)
        {
            cmd.CommandText = $"update scoreboard set tournamentId='{tournamentId}', sportId='{sportId}', playerId='{playerId}' where (scoreboardId='{scoreboardId}') ";
            
            cmd.ExecuteNonQuery();
        }





        static void Main(string[] args)
        {

            //SqlConnection 
            SqlConnection conn = new SqlConnection(CONN_STRING);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();


            //AddSports("Cricket",cmd);
            //AddTournament("TournamentB", cmd);
            //RemoveTournament("Tournament1", cmd);
            //RemoveSports("Football", cmd);
            //RemovePlayer(1, cmd);
            //AddScoreBoard(6,8,4,cmd);
            //EditScoreBoard(2, 6, 8, 3,cmd);
            while (true)
            {
                Console.WriteLine("==========================================================================================================");

                Console.WriteLine("==========================================================================================================");


                Console.WriteLine("Select the required command:");
                Console.WriteLine(" 1) Add sport \n 2) Add Tournament \n 3) Remove Tournament \n 4) Remove Sport");
                Console.WriteLine(" 5) Remove Player \n 6) Add Score board \n 7) Edit Score board");
                Console.WriteLine(" 8) Exit");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                int option = Convert.ToInt32( Console.ReadLine());
                Console.WriteLine();


                if (option == 1)
                {
                    Console.WriteLine("Enter the sport name:");
                    string sportName = Console.ReadLine();
                    AddSports(sportName, cmd);

                }
                else if (option == 2)
                {
                    Console.WriteLine("Enter the tournament name:");
                    string tournamentName = Console.ReadLine();
                    AddTournament(tournamentName, cmd);
                }
                else if (option == 3)
                {
                    Display("Tournament", cmd);
                    Console.WriteLine("Enter the tournament id:");
                    int tournamentId = Convert.ToInt32(Console.ReadLine());
                    RemoveTournament(tournamentId, cmd);
                }
                else if(option == 4)
                {
                    Display("Sports",cmd);

                    Console.WriteLine("\nEnter the sport id:");
                    int sportId = Convert.ToInt32(Console.ReadLine());
                    RemoveSports(sportId, cmd);

                    Display("Sports", cmd);
                }
                else if (option == 5)
                {
                    Display("Player",cmd);

                    Console.WriteLine("Enter the player id:");
                    int playerId = Convert.ToInt32(Console.ReadLine());
                    RemovePlayer(playerId, cmd);

                }
                else if (option == 6)
                {
                    Display("Tournament", cmd);
                    Console.WriteLine("Enter the tournament id:");
                    int tournamentId = Convert.ToInt32(Console.ReadLine());

                    DisplaySportsInTournament(tournamentId, cmd);
                    Console.WriteLine("Enter the sport id:");
                    int sportId = Convert.ToInt32(Console.ReadLine());

                    Display("Player", cmd);
                    Console.WriteLine("Enter the player id of the winner:");
                    int playerId = Convert.ToInt32(Console.ReadLine());

                    AddScoreBoard(tournamentId, sportId, playerId, cmd);
                }
                else if (option == 7)
                {
                    Console.WriteLine("Enter the scoreboard id:");
                    int scoreBoardId = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the tournament id:");
                    int tournamentId = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the sport id:");
                    int sportId = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the player id:");
                    int playerId = Convert.ToInt32(Console.ReadLine());

                    EditScoreBoard( scoreBoardId, tournamentId, sportId, playerId, cmd);

                }
                else
                {
                    
                    break;
                }
            }

            conn.Close();
        }
    }
}