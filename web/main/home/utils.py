from main import db
from models import PlacesContainer

def add_place_to_db(name, lat , long, is_prone, ever_affected):
    s = PlacesContainer(name=name, lat=lat, long=long, is_prone=is_prone,  ever_affected=ever_affected)
    db.session.add(s)
    db.session.commit()


def fill_places_db():  
    for key,value in places.items():
        name = key 
        lat = value[0]
        longt =value[1]
        is_prone = value[2]
        ever_affected = value[3]
        add_place_to_db(name=name, lat=lat, long=longt, is_prone=is_prone, ever_affected=ever_affected)
        break

def create_places_db():
    db.create_all(bind='places')



#dummpy places
#source = https://www.researchgate.net/publication/287448084_Overview_of_Landslide_Occurrences_in_Kenya_Causes_Mitigation_and_Challenges#:~:text=The%20Aberdare%20ranges%20traverse%20the,have%20occurred%20in%20the%20past.
 
places = {
   'Budalangi':[
        0.1225, 34.05278, True, True],    
    "Murang'a":[
        0.7167, 37.1500, False, True],
    'Mt Elgon':[
        1.1493, 34.5418, False, False],
    'Nyeri':[
        0.42013, 36.94759, False, True],
    'Kiambu':[
    -1.16667, 36.83333, True, False]
}


