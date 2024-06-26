﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsCollection.Classes
{
    internal class Tag
    {
        public string Name { get; set; } = "";
        public int Id { get; set; }
        public ConsoleColor Color { get; set; }

        #region Tag Constructors
        public Tag()
        {

            Id = 0;
            foreach (Tag tag in Tags.OrderBy(t => t.Id))
            {
                if (tag.Id == Id)
                {
                    Id++;
                }
            }

        }
        public Tag(string line)
        {

            string[] strings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (strings.Length >= 1)
            {
                Name = strings[0];
            }

            Id = 0;
            foreach (Tag tag in Tags.OrderBy(t => t.Id))
            {
                if (tag.Id == Id)
                {
                    Id++;
                }
            }

            if (strings.Length == 2)
            {
                if (Enum.TryParse<ConsoleColor>(strings[1], true, out ConsoleColor color))
                {
                    Color = color;
                }
                else
                {
                    Color = ConsoleColor.DarkGray;
                }
            }
            else
            {
                Color = ConsoleColor.DarkGray;
            }

        }

        #endregion Tag Constructors

        public static List<Tag> Tags = new List<Tag>();

        public override string ToString()
        {
            return $"{Name} {Color}";
        }

        #region WriteTags
        public void WriteTag()
        {
            ColorConsole.WriteColor(Name, Color);
        }
        public static void WriteTag(Tag tag)
        {
            tag.WriteTag();
        }
        public static void WriteTag(Tag[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i].WriteTag();
                if (i < tags.Length - 1)
                {
                    Console.Write(", ");
                }
            }
        }
        #endregion WriteTags

        #region StringToTags
        public static Tag[] StringToTags(string line)
        {
            List<Tag> tags = new List<Tag>();
            string[] tagStrings = line.Split(',', StringSplitOptions.TrimEntries);
            foreach (string tagString in tagStrings)
            {
                Tag? tagInQuestion = GetTag(tagString);
                if (tagInQuestion != null)
                {
                    tags.Add(tagInQuestion);
                }
            }
            return tags.ToArray();
        }
        public static int[] StringToTagIds(string line)
        {
            List<int> tagIds = new List<int>();
            string[] tagStrings = line.Split(',', StringSplitOptions.TrimEntries);
            foreach (string tagString in tagStrings)
            {
                Tag? tagInQuestion = GetTag(tagString);
                if (tagInQuestion != null)
                {
                    tagIds.Add(tagInQuestion.Id);
                }
            }
            return tagIds.ToArray();
        }
        public static Tag? GetTag(string name)
        {
            return Tags.FirstOrDefault(tag => tag.Name == name);
        }

        #endregion StringToTags
    }
}
