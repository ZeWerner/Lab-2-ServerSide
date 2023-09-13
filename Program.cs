namespace Lab_2___Advanced_C_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //This is a list of videogames that will store everything in the CSV
            List<VideoGame> games = new List<VideoGame>();

            Stack<VideoGame> recentlyPlayed = new Stack<VideoGame>();

            Queue<VideoGame> gamesToPlay = new Queue<VideoGame>();

            Dictionary<char,List<VideoGame>> gameDictionary= new Dictionary<char,List<VideoGame>>();


            //These two lines are directly from the amazing teacher Will, they help locate the root folder creating a
            //dynamic file path that doesn't need to be entered.
            string projectRootFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
            string filePath = projectRootFolder + "/videogames.csv";


            //This repeatition structure takes what is found in the CSV and breaks it apart and stores each data point in the array
            //from there it takes the broken parts and build a VideoGame object 
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] lineData = line.Split(',');

                    VideoGame game = new VideoGame(lineData[0], lineData[1], lineData[2], lineData[3], lineData[4], Convert.ToDouble(lineData[5]), Convert.ToDouble(lineData[6]), Convert.ToDouble(lineData[7]), Convert.ToDouble(lineData[8]), Convert.ToDouble(lineData[9]));

                    games.Add(game);
                }
            }
            //Using our IComparable method we practically overrided this now sorts our objects alphabetically by name 
            games.Sort();

            //I START MENU BASED STUFF HERE 
            bool menu = true;
            while (menu)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Zach's unique idea corner for stacks, queues, and dictionaries!" +
                    "\n\t\tPlease select an option below" +
                    "\n\t[1] Add a game to your Recently Played List" +
                    "\n\t[2] Show your Recetly Played Games" +
                    "\n\t[3] Add a game to your Games to Play List" +
                    "\n\t[4] Show all your Games to Play" +
                    "\n\t[5] See all games sorted by letter" +
                    "\n\t[6] End the Program\n");

                string tempInput = Console.ReadLine();

                while (int.TryParse(tempInput, out int id) == false || (tempInput != "1" && tempInput != "2" && tempInput != "3" && tempInput != "4" && tempInput != "5" && tempInput != "6") || string.IsNullOrWhiteSpace(tempInput))
                {
                    Console.WriteLine("Please enter a valid option");
                    tempInput = Console.ReadLine();
                }

                switch (Convert.ToInt32(tempInput))
                {
                    case 1:
                        SearchRecentGame(games, recentlyPlayed);
                        break;

                    case 2:
                        DisplayStack(recentlyPlayed);
                        break;

                    case 3:
                        SearchToPlayGame(games, gamesToPlay);
                        break;

                    case 4:
                        DisplayQueue(gamesToPlay);
                        break;

                    case 5:
                        DictionaryCreateAlphabetical(games, gameDictionary);
                        break;

                    case 6:
                        menu = false;
                        break;

                    default:
                        //shouldn't happen but just in case throw exception
                        throw new Exception("Invalid Input - How did you get here :(");
                }

            }
        }



        static List<VideoGame> PublisherSort(string publisher, List<VideoGame> games)
        {
            List<VideoGame> publisherSpesific = new List<VideoGame>();
            var publisherGame =
                from VideoGame in games
                where VideoGame.Publisher == publisher
                select VideoGame;

            foreach (var item in publisherGame)
            {
                publisherSpesific.Add(item);
            }
            return publisherSpesific;
        }

        static List<VideoGame> GenereSort(string genere, List<VideoGame> games)
        {
            List<VideoGame> genereSpesific = new List<VideoGame>();
            var genereGame =
                from VideoGame in games
                where VideoGame.Genere == genere
                select VideoGame;

            foreach (var item in genereGame)
            {
                genereSpesific.Add(item);
            }
            return genereSpesific;
        }

        static double PercentFinder(List<VideoGame> allGame, List<VideoGame> specificGame)
        {
            double chosenPercent = Convert.ToDouble(specificGame.Count()) / Convert.ToDouble(allGame.Count());
            return Math.Round((chosenPercent) * 100, 2);
        }

        static string ResponseGenerator(List<VideoGame> allGame, List<VideoGame> specificGame, string specificationBefore, string sortType, string specificationAfter)
        {
            string msg;
            msg = $"Out of {allGame.Count} games, {specificGame.Count} are {specificationBefore} {sortType} {specificationAfter}, which is {PercentFinder(allGame, specificGame)}%";
            return msg;
        }

        private static void PublisherData(List<VideoGame> games)
        {
            Console.WriteLine("Enter a game publisher to find out its data");
            string temp = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(temp))
            {
                Console.WriteLine("Please enter a VALID publisher");
                temp = Console.ReadLine();
            }



            List<VideoGame> publisherSpesific = PublisherSort(temp, games);

            foreach (var game in publisherSpesific)
            {
                Console.WriteLine(game);
            }
            Console.WriteLine(ResponseGenerator(games, publisherSpesific, "Developed by", temp, ""));
        }

        private static void GenereData(List<VideoGame> games)
        {
            Console.WriteLine("Enter a game genere to find out its data");
            string temp = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(temp))
            {
                Console.WriteLine("Please enter a VALID genere");
                temp = Console.ReadLine();
            }



            List<VideoGame> genereSpesific = GenereSort(temp, games);

            foreach (var game in genereSpesific)
            {
                Console.WriteLine(game);
            }
            Console.WriteLine(ResponseGenerator(games, genereSpesific, "", temp, "games"));

        }



        static void SearchRecentGame(List<VideoGame> games, Stack<VideoGame> recentlyPlayed)//set to return a list (maybe dictionary?) 
        {
            //Asks for and cleans up user input
            Console.WriteLine("Please enter the name of the game you are searching for");
            string userInput = Console.ReadLine().ToLower();

            while (string.IsNullOrWhiteSpace(userInput) || userInput.Count() < 3)
            {
                Console.WriteLine("Please enter a game name or include more of its name");
                userInput = Console.ReadLine().ToLower();
            }


            //replaces spaces with commas (I know I did the reverse of this during reading the cvs but it's fineeee)
            //then creates a list comparing the edited user input vs the edited list of all games and store the similar ones
            string userInputSeperated = userInput.Replace(' ', ',');
            var foundGames = games.Where(game => game.Name.Replace(' ',',').ToLower().Contains(userInputSeperated));


            //this counts the number of commas (which are spaces) and stores their indexes in a stack
            int commaCount = userInputSeperated.Count(c => c == ','); 
            Stack<int> commaIndex = new Stack<int>();

            for(int i = 0; i < userInputSeperated.Count(); i++)
            {
                if (userInputSeperated[i] == ',')
                {
                    commaIndex.Push(i);
                }
            }

            //If we don't find a game using the user input, we trim it until we do 
            bool wasUsed = false;
            for(int i = commaCount; i > 0; i--)
            {
                if(foundGames.Count() == 0)
                {
                    string trimmedUserInput = userInputSeperated.Remove(commaIndex.Pop());
                    foundGames = games.Where(game => game.Name.Replace(' ', ',').ToLower().Contains(trimmedUserInput));
                    wasUsed = true;
                }
            }
            if (wasUsed == true)
            {
                Console.WriteLine("We could not find that exact game, here are some games we think you could be talking about\n");
            }


            //clears out repeat game names
            List<string> foundGamesNameList = new List<string>();
            foreach (var game in foundGames)
            {
                var gameName = game.Name;
                if(!foundGamesNameList.Contains(gameName))
                {
                    foundGamesNameList.Add(gameName);
                }
            }

            //uses the found game for what it needs to do, in this case create a stack of recently
            //played games (Most recent is on top)
            int counter = 1;
            foreach(var game in foundGamesNameList)
            {
                Console.WriteLine($"[{counter}] {game}");
                counter++;
            }
            Console.WriteLine("\nEnter the number of the game you have most recently played. Enter 0 if it is not listed");
            string userGameSelect = Console.ReadLine();
            while (int.TryParse(userGameSelect, out int id) == false || userGameSelect[0] == '-' || Convert.ToInt32(userGameSelect) > counter || Convert.ToInt32(userGameSelect) < 0)
            {
                Console.WriteLine("Please enter a valid game number");
                userGameSelect = Console.ReadLine();
            }
            int userGameSelectNum = Convert.ToInt32(userGameSelect);
            if (userGameSelectNum == 0)
            {
                Console.WriteLine("Sorry we do not have the game you mentioned stored in our data\n" +
                    "Press enter to continue");
                Console.ReadLine();
            }
            else
            {
                recentlyPlayed.Push(foundGames.ElementAt(userGameSelectNum - 1));
                //recentlyPlayed.Push(foundGames[userGameSelectNum - 1]);


                Console.WriteLine("The game has successfully been stored in your Recently Played list!\n" +
                    "Press enter to contine");
                Console.ReadLine();
            }
        }

        /*////////////////////////////////////////////////////////////////////////////////////////////////////
          Seperate stack and queue search method 
        /*////////////////////////////////////////////////////////////////////////////////////////////////////

        static void SearchToPlayGame(List<VideoGame> games, Queue<VideoGame> gamesToPlay)//set to return a list (maybe dictionary?) 
        {
            //Asks for and cleans up user input
            Console.WriteLine("Please enter the name of the game you are searching for");
            string userInput = Console.ReadLine().ToLower();

            while (string.IsNullOrWhiteSpace(userInput) || userInput.Count() < 3)
            {
                Console.WriteLine("Please enter a game name or include more of its name");
                userInput = Console.ReadLine().ToLower();
            }


            //replaces spaces with commas (I know I did the reverse of this during reading the cvs but it's fineeee)
            //then creates a list comparing the edited user input vs the edited list of all games and store the similar ones
            string userInputSeperated = userInput.Replace(' ', ',');
            var foundGames = games.Where(game => game.Name.Replace(' ', ',').ToLower().Contains(userInputSeperated));


            //this counts the number of commas (which are spaces) and stores their indexes in a stack
            int commaCount = userInputSeperated.Count(c => c == ',');
            Stack<int> commaIndex = new Stack<int>();

            for (int i = 0; i < userInputSeperated.Count(); i++)
            {
                if (userInputSeperated[i] == ',')
                {
                    commaIndex.Push(i);
                }
            }

            //If we don't find a game using the user input, we trim it until we do 
            bool wasUsed = false;
            for (int i = commaCount; i > 0; i--)
            {
                if (foundGames.Count() == 0)
                {
                    string trimmedUserInput = userInputSeperated.Remove(commaIndex.Pop());
                    foundGames = games.Where(game => game.Name.Replace(' ', ',').ToLower().Contains(trimmedUserInput));
                    wasUsed = true;
                }
            }
            if (wasUsed == true)
            {
                Console.WriteLine("We could not find that exact game, here are some games we think you could be talking about\n");
            }


            //clears out repeat game names
            List<string> foundGamesNameList = new List<string>();
            foreach (var game in foundGames)
            {
                var gameName = game.Name;
                if (!foundGamesNameList.Contains(gameName))
                {
                    foundGamesNameList.Add(gameName);
                }
            }

            //uses the found game for what it needs to do, in this case create a queue of games to play 
            int counter = 1;
            foreach (var game in foundGamesNameList)
            {
                Console.WriteLine($"[{counter}] {game}");
                counter++;
            }
            Console.WriteLine("\nEnter the number of the game you have most recently played. Enter 0 if it is not listed");
            string userGameSelect = Console.ReadLine();
            while (int.TryParse(userGameSelect, out int id) == false || userGameSelect[0] == '-' || Convert.ToInt32(userGameSelect) > counter || Convert.ToInt32(userGameSelect) < 0)
            {
                Console.WriteLine("Please enter a valid game number");
                userGameSelect = Console.ReadLine();
            }
            int userGameSelectNum = Convert.ToInt32(userGameSelect);
            if (userGameSelectNum == 0)
            {
                Console.WriteLine("Sorry we do not have the game you mentioned stored in our data\n" +
                    "Press enter to continue");
                Console.ReadLine();
            }
            else
            {
                gamesToPlay.Enqueue(foundGames.ElementAt(userGameSelectNum - 1));
                //recentlyPlayed.Push(foundGames[userGameSelectNum - 1]);


                Console.WriteLine("The game has successfully been added in your Games To Play list!\n" +
                    "Press enter to contine");
                Console.ReadLine();
            }
        }


        static void DisplayStack(Stack<VideoGame> recentlyPlayed)
        {
            if(recentlyPlayed.Count == 0)
            {
                Console.WriteLine("There are no games to display, go play and add some!!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Here are the games you have played recently:");
                
                foreach (var game in recentlyPlayed)
                {
                    Console.WriteLine(game.Name);
                }
                Console.ReadLine();
            }
        }


        static void DisplayQueue(Queue<VideoGame> gamesToPlay)
        {
            if (gamesToPlay.Count == 0)
            {
                Console.WriteLine("There are no games to display, go add some!!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Here are the games you have played recently:");

                foreach (var game in gamesToPlay)
                {
                    Console.WriteLine(game.Name);
                }
                Console.ReadLine();
            }
        }


        static void DictionaryCreateAlphabetical(List<VideoGame> games, Dictionary<char, List<VideoGame>> gameDictionary)
        {
            List<VideoGame> nestedList = new List<VideoGame>();
            List<char> allChar = new List<char>();
            foreach (var game in games)
            {
                char ABC = game.Name[0];
                if (!allChar.Contains(ABC))
                {
                    allChar.Add(ABC);
                }
            }

            foreach (var letter in allChar)
            {
                foreach (var game in games)
                {
                    if (letter == game.Name[0])
                    {
                        nestedList.Add(game);
                        
                    }
                    
                }
                gameDictionary[letter] = new List<VideoGame> (nestedList);
                nestedList.Clear();
            }

            Console.WriteLine("Please select a letter to view to see if our collection has a specific game");
            string userLetter = Console.ReadLine();
            while(string.IsNullOrWhiteSpace(userLetter) || userLetter.Length > 1)
            {
                Console.WriteLine("Please enter a valid LETTER, it is not that hard people");
                userLetter = Console.ReadLine();
            }
            char userLetterChar = Convert.ToChar(userLetter.ToUpper());

            Console.WriteLine($"Here are the games that start with {userLetterChar}");
            foreach (KeyValuePair<char, List<VideoGame>> entry in gameDictionary)
            {
                if(entry.Key == userLetterChar)
                {
                    foreach(var item in entry.Value)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            Console.WriteLine("\nPress enter to continue");
            Console.ReadLine();
        }

    }
}