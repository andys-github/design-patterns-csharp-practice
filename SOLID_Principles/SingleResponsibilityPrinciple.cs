using System;
using System.Collections.Generic;
using System.IO;

namespace SOLID_Principles
{
    public class SingleResponsibilityPrinciple
    {
        public static void Execute()
        {
            var j = new Journal();
            j.AddEntry("It is a fine morning today.");
            j.AddEntry("Lets go outside and play!");
            Console.WriteLine(j.ToString());
        }
    }

    public class Journal
    {
        private readonly List<string> entries = new List<string>();
        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count}: {text}");
            return count;
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }
        
        // Suppose we want to Save/Load Journal data
        // to/from files/Uri, etc, like so:
        public void Save(string filename)
        {
            File.WriteAllText(filename, ToString());
        }

        public void Save(Uri uri)
        {
            // Send file contents to an endpoint, etc
        }

        public Journal Load(string filename)
        {
            // Load journal contents from
            // specified file and return it
            return new Journal();
        }

        public Journal Load(Uri uri)
        {
            // Fetch journal contents from API
            // and return it
            return new Journal();
        }
        
        // Adding all these Save/Load methods
        // is a lot of additional work for the Journal Class
        // This violates the Single Responsibility Principle
        
        // To rectify this, we would create a new class
        // say Persistence class and write all the logic for
        // saving/loading data from Journals
    }

    public class Persistence
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, j.ToString());
            }
            else
            {
                File.AppendAllText(filename, j.ToString());
            }
        }
    }
}