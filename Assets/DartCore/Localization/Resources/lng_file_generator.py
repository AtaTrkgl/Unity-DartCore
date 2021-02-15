import os
import csv

LINE_BREAK = "<line_break>"

file_name = input("enter file name without the .csv: ") + ".csv"


# Reading the csv
keys = []
languages = []
with open(file_name, "r", encoding="utf-8") as f:
    for i, line in enumerate(list(csv.reader(f, skipinitialspace=True))):

        # Generating language lists
        if i == 0:
            for j in range(len(line) - 1):
                languages.append([])
            continue

        keys.append(line.pop(0).strip())
        print(line)
        for j, value in enumerate(line):
            languages[j].append(value.strip().replace("\n", LINE_BREAK))

# Converting the csv to language files


def convert_vals_to_lines(vals: list) -> list:
    return [val + ("\n" if i != len(vals) - 1 else "") for i, val in enumerate(vals)]


lng_file_count = 0
for file in os.listdir():
    # keys file
    if file == "_keys.txt":
        with open(file, "w", encoding="utf-8") as f:
            f.writelines(convert_vals_to_lines(keys))

    # language file
    if file.endswith(".txt") and not file.startswith("_"):
        with open(file, "w", encoding="utf-8") as f:
            f.writelines(convert_vals_to_lines(languages[lng_file_count]))
            lng_file_count += 1
