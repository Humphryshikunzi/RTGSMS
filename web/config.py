import os

SECRET_KEY = 'A very long, creative and hard to crack key'
WHOOSH_BASE = 'whoosh'
SQLALCHEMY_TRACK_MODIFICATIONS = True
SQLALCHEMY_BINDS = os.environ.get('DEV_BINDS') or {"places": "sqlite:///places.db"}
HEADERS = {
    'Authorization': 'password',
    'Content-Type': 'application/json',
}

HOST = '0.0.0.0'