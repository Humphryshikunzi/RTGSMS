from flask_migrate import Migrate
from main import app, db 
from main.home.utils import create_places_db, fill_places_db
migrate = Migrate(app,db)

#This initializes the database once to fill with initial data, later its commented out

#create_places_db()
#fill_places_db()

if __name__ == "__main__":
    app.run(debug=True, port=5053)
   