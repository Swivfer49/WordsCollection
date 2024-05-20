This is a work in progress console application with the purpose of organizing "words".

Words can also come with tags.

The reasons why I am making this will be added later

here is a list of all currently available commands

|Utility Commands|
- help -> details all commands
- save -> saves all changes to the file
- exit -> exits the program
- clear -> clears the console
- switchfile [Name] -> switches to the file with the name [Name], it will create a new one if the file does not exist
- showtags -> shows all tags

|Word Alterations|
- createword [WordName] -> adds a new word to the word bank with the name [WordName]
- createword [WordName] - [Tag1], [Tag2],... -> adds a new word to the word bank with the name [WordName], inluding the tags [Tag1], [Tag2],...
- removeword [WordName] -> removes the word with the name [WordName] if such word exsists
- renameword [WordName] to [NewName] -> renames the word with the name [WordName] to [NewName]

|Tag Alterations|
- renametag [TagName] to [NewName] -> renames the tag with the name [TagName] to [NewName]
- createtag [TagName] [Color]\(Optional) -> creates a new tag named [TagName] with the color [Color]
- color [TagName] [Color] -> sets the color of [TagName] to [Color]
- deletetag [TagName] -> deletes [TagName] and removes it from all words

|Basic Word Searching|
- findwords startswith [string] -> lists all words that start with [string]
- findwords endswith [string] -> lists all words that end with [string]
- findwords contains [string] -> lists all words that contain [string]
- findwords all -> lists all words
- findwords hidetags... -> lists all words that match the selector, but does not include tags in output
- findword [WordName] -> finds the word named [WordName]

|Word Searching Using Tags|
- findwords withtags [Tag1], [Tag2],... -> lists all words that have all the tags in [Tag1], [Tag2],...
- findwords withouttags [Tag1], [Tag2],... -> lists all words that have none of the tags in [Tag1], [Tag2],...

|Tagging Words|
- addtags [Tag1], [Tag2],... to [WordName] -> adds all tags in [Tag1], [Tag2],... to the word [WordName]
- removetags [Tag1], [Tag2],... from [WordName] -> removes all tags [Tag1], [Tag2],... from [WordName]
- forcetags [Tag1], [Tag2],... on [WordName] -> sets the tags of [WordName] to the list of tags
