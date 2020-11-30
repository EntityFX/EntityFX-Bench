
import os

class Writer(object):
    
    @property
    def use_console(self) -> bool:
        return self.__use_console

    @use_console.setter
    def use_console(self, value) -> bool:
        self.__use_console = value
        return self.__use_console
    
    @property
    def file_path(self) -> str:
        return self.__file_path
    
    def __init__(self, file_path : str=None) -> None:
        self.__output = ""
        self.__use_console = True
        self.__use_file = False
        if (file_path is not None): 
            self.__file_path = file_path
            self.__use_file = True
            self.__stream = open(file_path, "a+", 1)

        self.__is_color = True
        if os.name == "nt":
            self.__is_color = False

    @property
    def output(self) -> str:
        return str(self.__output)
    
    def write_line(self, format : str=None, *args : object) -> None:
        if format is not None:
            self.write_color("\033[1;30m", format, *args)
        if (self.use_console): 
            print('')
            self.__output += '\n'
            if (self.__use_file): 
                self.__stream.write('\n')
       
    def write_header(self, format : str, *args : object) -> None:
        self.write_color("\033[1;36m", format, *args)
        self.write_line()
       
    def write_color(self, color : str, format : str, *args : object) -> None:
        formatted = format.format(*args)
        self.__output += formatted
        if (self.use_console): 
            to_print = '{color}{formatted}\033[0m'.format(color=color, formatted=formatted) if self.__is_color else formatted
            print(to_print, end='', flush=True)
            if (self.__use_file): 
                self.__stream.write(formatted)

    def write(self, format : str, *args: object) -> None:
        self.write_color("\033[1;30m", format, *args)
    
    def write_value(self, format : str, *args: object) -> None:
        self.write_color("\033[1;32m", format, *(args))
    
    def write_title(self, format : str, *args: object) -> None:
        self.write_color("\033[1;37m", format, *(args))
