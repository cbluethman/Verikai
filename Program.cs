using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Verikai
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var client = new WebClient())
            {
                client.DownloadFile("http://devchallenge.verikai.com/data.tsv", "data.tsv");
            }

            var delimiter = "\t";

            using (TextReader tr = File.OpenText("data.tsv"))
            using (TextWriter tw = File.CreateText("unencrypted.tsv"))
            {
                string line;

                line = tr.ReadLine();
                tw.WriteLine(line + delimiter + "age" +
                     delimiter + "cost" );

                while ((line = tr.ReadLine()) != null)
                {
                    string[] items = line.Split(delimiter[0]);

                    items[2] = items[2][0].ToString().ToUpper();

                    string[] dob = items[3].Split(',');

                    if ( dob.Length > 1)
                    {
                        items[3] = MonthConvert(dob[1].TrimStart()) +
                        "/" + Regex.Replace(dob[1], @"[^0-9]", "") +
                        "/" + dob[2].TrimStart();
                    }

                    String age = CalculateAge( DateTime.Parse(items[3]) );

                    items[5] = Regex.Replace(items[5], @"[^0-9]", "");

                    tw.Write(string.Join(delimiter, items));
                    tw.Write(delimiter);
                    tw.Write(age);
                    tw.Write(delimiter);
                    tw.Write("cost");
                    tw.Write(System.Environment.NewLine);
                }
            }
        }

        static string MonthConvert( string s)
        {
            string r;

            s = s.Substring( 0, 3 ).ToUpper();

            switch ( s )
            {
                case "JAN":
                   r = "1";
                   break;
                case "FEB":
                    r = "2";
                    break;
                case "MAR":
                    r = "3";
                    break;
                case "APR":
                    r = "4";
                    break;
                case "MAY":
                    r = "5";
                    break;
                case "JUN":
                    r = "6";
                    break;
                case "JUL":
                    r = "7";
                    break;
                case "AUG":
                    r = "8";
                    break;
                case "SEP":
                    r = "9";
                    break;
                case "OCT":
                    r = "10";
                    break;
                case "NOV":
                    r = "11";
                    break;
                case "DEC":
                    r = "12";
                    break;
                default:
                    r = "?";
                    break;
            }

            return (r);
        }

        public static string CalculateAge(DateTime birthDay)
        {
            int years = DateTime.Now.Year - birthDay.Year;

            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
            {
                years--;
            }

            return ( years.ToString() );
        }
    }
}
