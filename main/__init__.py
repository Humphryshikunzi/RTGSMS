from flask import Blueprint
from flask import Flask
from flask_login import LoginManager
from flask_sqlalchemy import SQLAlchemy
from config import *
from flask_wtf.csrf import CSRFProtect
from flask_bcrypt import Bcrypt
from flask_marshmallow import Marshmallow

home = Blueprint('home', __name__)
#from main import routes, errors


app = Flask(__name__)

app.config['SECRET_KEY'] = SECRET_KEY
app.config['WHOOSH_BASE'] = WHOOSH_BASE
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = SQLALCHEMY_TRACK_MODIFICATIONS
app.config['SQLALCHEMY_BINDS'] = SQLALCHEMY_BINDS

db = SQLAlchemy(app)

login_manager = LoginManager(app)
csrf = CSRFProtect(app)
bcrypt = Bcrypt(app)
ma = Marshmallow(app)

db.init_app(app)

login_manager.init_app(app)
login_manager.login_view = 'home.login'
login_manager.login_message_category = 'info'
login_manager.session_protection = 'strong'

csrf.init_app(app)
bcrypt.init_app(app)
ma.init_app(app)

from main.home import  home as home_blueprint

app.register_blueprint(home_blueprint)