using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2___Advanced_C_
{
    public class VideoGame : IComparable<VideoGame>
    {
        //Properties as listed in the CSV
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Year { get; set; } //I say string bc I don't think I'll be doing math against a year
        public string Genere { get; set; }
        public string Publisher { get; set; }
        public double NA_Sales { get; set; }
        public double EU_Sales { get; set; }
        public double JP_Sales { get; set; } //This is Japan's sales in case you forget future Zach
        public double Other_Sales { get; set; }
        public double Global_Sales { get; set; }


        //Constructors 

        public VideoGame()
        {
            Name = string.Empty;
            Platform = string.Empty;
            Year = string.Empty;
            Genere = string.Empty;
            Publisher = string.Empty;
            NA_Sales = 0;
            EU_Sales = 0;
            JP_Sales = 0;
            Other_Sales = 0;
            Global_Sales = 0;
        }

        public VideoGame(string name, string platform, string year, string genere, string publisher,
            double nA_Sales, double eU_Sales, double jP_Sales, double other_Sales, double global_Sales)
        {
            Name = name;
            Platform = platform;
            Year = year;
            Genere = genere;
            Publisher = publisher;
            NA_Sales = nA_Sales;
            EU_Sales = eU_Sales;
            JP_Sales = jP_Sales;
            Other_Sales = other_Sales;
            Global_Sales = global_Sales;
        }

        //Probably won't use this copy constuctor but Matt goes crazy and gets violent with our grade if we add one :/
        public VideoGame(VideoGame videoGame)
        {
            Name = videoGame.Name;
            Platform = videoGame.Platform;
            Year = videoGame.Year;
            Genere = videoGame.Genere;
            Publisher = videoGame.Publisher;
            NA_Sales = videoGame.NA_Sales;
            EU_Sales = videoGame.EU_Sales;
            JP_Sales = videoGame.JP_Sales;
            Other_Sales = videoGame.Other_Sales;
            Global_Sales = videoGame.Global_Sales;
        }

        //This method takes in an object and returns an int that determines its spot in the list
        //less than or greater than zero - the object is either before or follows in the sort order respectivly 
        public int CompareTo(VideoGame obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj != null)
            {
                return Comparer<string>.Default.Compare(Name, obj.Name);
            }
            else
            {
                throw new ArgumentException("Object is not REAL");
            }
        }

        public override string ToString()
        {
            string msg;
            msg = $"{Name}";//, a {Genere} game on {Platform}, released in {Year} by {Publisher} 
            return msg;
        }


    }
}
