from __future__ import print_function
import socket
import sys
backlog = 1
size = 1024
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
host = '0.0.0.0'
port = 12345
with socket.socket() as s:
    s.bind((host, port))
    print(f'socket binded to {port}')
    s.listen()
    con, addr = s.accept()
    with con:
        while True:
            data = con.recv(1024)
            if data:
                print(str(data, "utf-8"))
            con.sendall(data)