#!/usr/bin/env python3

import os, sys, sqlite3

db_name = "timepunch.sqlite"

tables = {
    "Users": """CREATE TABLE IF NOT EXISTS Users(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            email TEXT UNIQUE NOT NULL,
            password TEXT NOT NULL,
            salt TEXT NOT NULL,
            role INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER))
        )""",
    "Courses": """CREATE TABLE IF NOT EXISTS Courses(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            desc TEXT NOT NULL,
            professor_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(professor_id) REFERENCES User(id)
        )""",
    "Projects": """CREATE TABLE IF NOT EXISTS Projects(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            desc TEXT NOT NULL,
            course_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(course_id) REFERENCES Courses(id)
        )""",
    "Groups": """CREATE TABLE IF NOT EXISTS Groups(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            project_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(project_id) REFERENCES Projects(id)
        )""",
    "Timesheet": """CREATE TABLE IF NOT EXISTS Timesheet(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            created TEXT NOT NULL,
            user_id INTEGER NOT NULL,
            group_id INTEGER NOT NULL,
            desc TEXT NOT NULL,
            start TEXT NOT NULL,
            time_worked TEXT NOT NULL,
            FOREIGN KEY(user_id) REFERENCES Users(id),
            FOREIGN KEY(group_id) REFERENCES Groups(id)
        )""",
    "Users_To_Courses": """CREATE TABLE IF NOT EXISTS Users_To_Courses(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            course_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(user_id) REFERENCES USERS(id),
            FOREIGN KEY(course_id) REFERENCES Courses(id)
        )""",
    "Projects_To_Courses": """CREATE TABLE IF NOT EXISTS Projects_To_Courses(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            project_id INTEGER NOT NULL,
            course_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(project_id) REFERENCES Projects(id),
            FOREIGN KEY(course_id) REFERENCES Courses(id)
        )""",
    "Groups_To_Projects": """CREATE TABLE IF NOT EXISTS Groups_To_Projects(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            group_id INTEGER NOT NULL,
            project_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(group_id) REFERENCES Groups(id),
            FOREIGN KEY(project_id) REFERENCES Projects(id)
        )""",
    "Users_To_Groups": """CREATE TABLE IF NOT EXISTS Users_To_Groups(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            group_id INTEGER NOT NULL,
            createdOn INTEGER DEFAULT (CAST(STRFTIME('%s', 'now') AS INTEGER)),
            FOREIGN KEY(user_id) REFERENCES Users(id),
            FOREIGN KEY(group_id) REFERENCES Groups(id)
        )"""
}

try:
    connection = sqlite3.connect(db_name)
    cursor = connection.cursor()
    cursor.execute("PRAGMA foreign_keys=ON")
    for table, statement in tables.items():
        print("Creating table", table)
        # removing multiple spaces into just one
        statement = " ".join(statement.split())
        cursor.execute(statement)

except Exception as ex:
    print("Exception: ", ex)
