import json
import requests

from sqlalchemy import distinct
from flask import redirect, url_for, render_template, flash, request, session
from flask_login import current_user, login_user, logout_user, login_required

from main import db
from main.home import home
from main.home.forms import SearchForm, LoginForm
from models import  PlacesContainer
from models import UserModel
from config import  HEADERS, BASE_URI

def auth_user(payload):
    response = requests.post(BASE_URI + "User/authenticateUser", data=json.dumps(payload), headers=HEADERS, verify=False)
    return response

@home.route('/')
@home.route('/home')
def home_page():
    form = SearchForm()
    page = request.args.get('page', default=1, type=int)
    _places = db.session.query(distinct(PlacesContainer.name)).paginate(per_page=2, page=page)
    print(_places)
    return render_template('main/home.html', current_user=current_user, _places=_places,  form=form, bool_val=True)

@home.route('/search')
def search():
    form = SearchForm()
    page = request.args.get('page', default=1, type=int)
    results = PlacesContainer.query.whoosh_search(request.args.get('query')).paginate(per_page=2, page=page)
    if not results.items:
        flash(f"Sorry No Places found for {request.args.get('query')}.", 'danger')
    return render_template('main/home.html', _courses=results, form=form, bool_val=False, whoosh=True)

 
@home.route('/impact')
def impact():
    return render_template('main/impact.html')


@home.route('/realtime')
def realtime():
    return render_template('main/realtime.html')  

@home.route('/update_realtime/one', methods=['GET', 'POST'])
def update_realtime(): 
    if request.method == 'GET':  
        device = request.args.get('device')
        geophone = request.args.get('geophone_analog_value') 
        time = request.args.get('time')  
        lat = request.args.get('lat')
        long = request.args.get('long')  
        x_acc = request.args.get('x_acc')  
        y_acc = request.args.get('y_acc')
        z_acc = request.args.get('z_acc')

        data = {'device':device, 'geophone':geophone, 'time':time,
         'lat':lat, 'long':long, 'x_acc':x_acc, 'y_acc':y_acc, 'z_acc':z_acc
         }

    return render_template('main/realtime.html', data=data) 

@home.route("/login", methods=['GET', 'POST'])
def login():
    if current_user.is_authenticated:
        return redirect(url_for('main.home'))
    form = LoginForm()
    if form.validate_on_submit():
        email = form.email.data
        payload = {
            "email": form.email.data,
            "password": form.password.data,
        }
        response = auth_user(payload) 
        user_reponse = response.json() 
        if response.ok:
            user = UserModel(email=email, username=user_reponse['userName'])
            login_user(user=user, remember=form.remember.data)
            session['user'] = user_reponse
            next_page = request.args.get('next')
            flash("Log in successful", 'success')
            return redirect(next_page) if next_page else redirect(url_for('home.home_page'))
        else:
            flash('Login unsuccessful. Check email or password', 'danger')
    return render_template('main/login.html', title='Login', form=form)


@home.route("/logout")
def logout():
    logout_user()
    return redirect(url_for('main.home'))    



@home.route("/places")
def places_func():
    return render_template('main/places.html', title='Courses')
