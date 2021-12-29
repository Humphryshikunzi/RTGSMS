from flask_login import UserMixin
from main import app, db
import flask_whooshalchemy as fwa
from flask.globals import session

from flask_login import UserMixin
from itsdangerous import TimedJSONWebSignatureSerializer as Serializer

from main import app, ma 
from main import db, login_manager

conn = db.engine.connect()

@login_manager.user_loader
def load_user(user_id):
    _ = user_id.split("/")[-1]
    email = user_id.split("/")[0]
    session_user = session['user']
    username = session_user['userName']
    return UserModel(email=email, username=username)


class UserModel(UserMixin):
    def __init__(self, email, username, **kwargs):
        super(UserModel, self).__init__(**kwargs)
        self.email = email
        self.id = self.email + "/" + "randomnumbers"
        self.image_file = "default.jpg"
        self.username = username


class User(db.Model, UserMixin):
    id = db.Column(db.Integer, primary_key=True)
    id_element = db.Column(db.String(120), unique=True, nullable=False)
    userName = db.Column(db.String(20), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True, nullable=False)
    password = db.Column(db.String(60), nullable=False)
    role = db.Column(db.String(20), nullable=False, default='customer')

    def get_reset_token(self, expires_sec=1800):
        s = Serializer(app.config['SECRET_KEY'], expires_sec)
        return s.dumps({'user_id': self.id}).decode('utf-8')

    @staticmethod
    def verify_reset_token(token):
        s = Serializer(app.config['SECRET_KEY'])
        try:
            user_id = s.loads(token)['user_id']
        except:
            return None
        return User.query.get(user_id)

    def __repr__(self):
        return f"User('{self.userName}', '{self.email}')"

    def get_user_data():
        user = session['user']
     

class UserSchema(ma.SQLAlchemySchema):
    class Meta:
        model = User

    id = ma.auto_field()
    id_element = ma.auto_field()
    userName = ma.auto_field()
    email = ma.auto_field()
    password = ma.auto_field()
    role = ma.auto_field()


class UserModel(UserMixin):
    def __init__(self, email, username, **kwargs):
        super(UserModel, self).__init__(**kwargs)
        self.email = email
        self.id = self.email + "/" + "randomnumbers"
        self.image_file = "default.jpg"
        self.username = username


class PlacesContainer(db.Model):
    __searchable__ = ['name', 'lat', 'long','is_prone','ever_affected']
    __bind_key__ = 'places'
    __tablename__ = 'places'
    # This is the container eg english, kiswahili
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(120), nullable=False)
    lat = db.Column(db.Float(10), nullable=False)
    long = db.Column(db.Float(10), nullable=False)
    is_prone = db.Column(db.Boolean(6), nullable=False)
    ever_affected = db.Column(db.Boolean(6), nullable=False)
   

    def __repr__(self):
        return f"{self.name}"


fwa.whoosh_index(app=app, model= PlacesContainer)        