"""
Running this code will create a csv file called
dartcore_localization.csv where the first column
contains the keys and the following columns contain
the localized values. After editing the csv run the 
"lng_file_generator.py" to turn it back to seperate
language files. Note that if you add a new language
to the game after creating the csv the conversion will
not work.
"""

import os
import csv

KEYS_FILE = "_keys.txt"
LINE_BREAK = "<line_break>"

def get_striped_list(list_to_strip: list) -> list:
    # strip to remove /n's.
    return [i.strip().replace(LINE_BREAK, "\n") for i in list_to_strip]


# Converting files to lists
keys = []
languages = []
for file in os.listdir():
    # keys file
    if file == KEYS_FILE:
        with open(file, "r", encoding="utf-8") as f:
            keys = get_striped_list(f.readlines())

    # language file
    if file.endswith(".txt") and not file.startswith("_"):
        with open(file, "r", encoding="utf-8") as f:
        	lines = f.readlines()
        	last_line = lines[len(f.readlines()) - 1]
        	if last_line == "\n":
        		lines.append("")

        	languages.append(get_striped_list(lines))

# Generating row dict from index
def get_row_dict(index: int) -> dict:
    row_dict = {"Keys": keys[index]}
    for language in languages:
    	# language[0] is the name of the language.
    	row_dict[language[0]] = language[index]

    return row_dict


# Creating the csv
with open("dartcore_localization.csv", "w", newline="", encoding="utf-8") as f:
    fieldnames = ["Keys"] + [i[0] for i in languages]
    writer = csv.DictWriter(f, fieldnames=fieldnames, delimiter=",")

    writer.writeheader()  # writing the field names
    for i in range(len(keys)):
        writer.writerow(get_row_dict(i))
