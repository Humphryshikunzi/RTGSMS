from flask import render_template
from main.home import  home


@home.app_errorhandler(404)
def page_not_found(e):
    return render_template('errors/404.html', title='page not found'), 404


@home.app_errorhandler(500)
def internal_server_error(e):
    return render_template('errors/500.html', title='internal server error'), 500
