import os
import subprocess
import tkinter as tk
from tkinter import Entry, Listbox, Scrollbar, Button, END, mainloop
import json
import psutil

class Launcher:
    def __init__(self, master):
        self.master = master
        master.title("Програмний Запускач")

        self.programs = self.load_programs_from_json()

        self.entry = Entry(master, width=40)
        self.entry.grid(row=0, column=0, padx=10, pady=10)
        self.entry.bind("<KeyRelease>", self.update_program_list)

        self.listbox = Listbox(master, width=40, height=10)
        self.listbox.grid(row=1, column=0, padx=10, pady=10)
        self.listbox.bind('<<ListboxSelect>>', self.on_select)

        self.scrollbar = Scrollbar(master)
        self.scrollbar.grid(row=1, column=1, sticky='ns')
        self.listbox.config(yscrollcommand=self.scrollbar.set)
        self.scrollbar.config(command=self.listbox.yview)

        self.launch_button = Button(master, text="Запустити", command=self.launch_program)
        self.launch_button.grid(row=2, column=0, pady=10)

    def load_programs_from_json(self):
        try:
            with open("programs.json", "r") as json_file:
                programs = json.load(json_file)
            return programs
        except FileNotFoundError:
            return {}

    def update_program_list(self, event):
        search_query = self.entry.get().lower()
        self.listbox.delete(0, END)
        for program in self.programs:
            if search_query in program.lower():
                self.listbox.insert(END, program)

    def on_select(self, event):
        selected_index = self.listbox.curselection()
        if selected_index:
            selected_program = self.listbox.get(selected_index)
            self.entry.delete(0, END)
            self.entry.insert(END, selected_program)

    def launch_program(self):
        selected_program = self.entry.get()
        if selected_program in self.programs:
            program_path = self.programs[selected_program]
            process = subprocess.Popen(program_path, shell=True, creationflags=subprocess.CREATE_NEW_PROCESS_GROUP)
            parent_pid = os.getpid()  # Отримати pid лаунчера
            psutil.Process(process.pid).parent = parent_pid  # Змінити батьківський pid на pid лаунчера

if __name__ == "__main__":
    root = tk.Tk()
    launcher = Launcher(root)
    root.mainloop()
