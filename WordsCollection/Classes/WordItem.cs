using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsCollection.Classes
{
    internal class WordItem
    {
        public string word;
        public int[] tagIds = new int[0];

        public WordItem(string line)
        {
            string[] strings = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if(strings.Length >= 1)
            {
                word = strings[0].Trim();
            }
            if(strings.Length == 2)
            {
                List<int> tagIdsList = new List<int>();

                string[] tagStrings = strings[1].Split(",", StringSplitOptions.TrimEntries);

                for (int i = 0; i < tagStrings.Length; i++) 
                {

                    Tag tag = Tag.Tags.FirstOrDefault(t => t.Name == tagStrings[i]);

                    if(tag != null)
                    {
                        tagIdsList.Add(tag.Id);
                    }

                }

                tagIds = tagIdsList.ToArray();
            }
        }

        public override string ToString()
        {
            string[] tags = new string[tagIds.Length];
            for(int i = 0; i < tags.Length; i++)
            {
                tags[i] = Tag.Tags.First(t => t.Id == tagIds[i]).Name;
            }
            string tagsString = string.Join(", ", tags);
            return $"{word} - {tagsString}";
        }

        public void WriteWord()
        {
            Console.Write(word);
            Console.Write(" - ");
            Tag[] tags = Tag.Tags.Where(t=>tagIds.Contains(t.Id)).ToArray();
            Tag.WriteTag(tags);
        }
    }
}
