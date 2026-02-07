using System.Reflection;
using Microsoft.Identity.Client;

namespace BlazorDBDemo
{

    class Album
    {
        private int albumid;
        private string title;
        private int artistid;
        private string artistname;
        private int genreid;
        private string genrename;
        private int numbertracks;

        public int Albumid { get { return albumid; } set { albumid = value; } }
        public string Title { get { return title; } set { title = value; } }
        public int Artistid { get { return artistid; } set { artistid = value; } }
        public string ArtistName { get { return artistname; } set { artistname = value; } }
        public int Genreid { get { return genreid; } set { genreid = value; } }
        public string Genrename { get { return genrename; } set { genrename = value; } }
        public int NumberTracks { get { return numbertracks; } set { numbertracks = value; } }


        public Album(int i, string t, int ar, string arname, int g, string gname, int numtracks)
        {
            albumid = i;
            title = t;
            artistid = ar;
            artistname = arname;
            genreid = g;
            genrename = gname;
            numbertracks = numtracks;

        }  
        
    }

    class Albums
    {
        private List<Album> albumsList;
        private bool loaded;
        private bool lastAction;
        private DBAccessObj data;

        public List<Album> AlbumsList { get { return albumsList; } }
        public bool LastAction{ get { return lastAction; } }
        public bool AlbumsLoaded{ get{return loaded;}}

        public Albums()
        {

            loaded = false;
            lastAction = true;
            data = new DBAccessObj();
            albumsList = new List<Album>();
        }

        public void AddAlbum(int id, string title, int artistid, string artistname, int genreid, string genrename, int numtracks)
        {
            albumsList.Add(new Album(id, title, artistid, artistname, genreid, genrename, numtracks));
        }

        public void DeleteAlbum(int albumid)
        {
            bool lastAction = data.DeleteAlbum(albumid);
        }

        public void UpdateAlbum(Album albumdta)
        {
            bool lastAction = data.UpdateAlbum(albumdta.Albumid, albumdta.Title, albumdta.ArtistName,albumdta.Genrename);
        }

        public void InsertNewAlbum(string alName, string artName, string genreName)
        {
            
            bool lastAction = data.AddAlbum(alName, artName, genreName);
        }
        

        public void LoadAlbums()
        {
            
            albumsList = new List<Album>();
            List<(int, string, int, string, int, string, int)> dataloader = data.GetAlbumData();
            foreach ((int alID, string title, int artistid, string artistName, int genreID, string genreName, int numTracks) item in dataloader)
            {
                AddAlbum(item.alID, item.title, item.artistid, item.artistName,item.genreID, item.genreName, item.numTracks);
            }
            loaded=true;

        }


    }

    class Artist
    {
        private int artistid;
        private string artistname;  

        public int Artistid{ get {return artistid;}}
        public string Artistname { get { return artistname;}}

        public Artist(int aid=-1, string aname = "")
        {
            artistid=aid;
            artistname=aname;
        }

    }

    class Artists
    {
        private List<Artist> artistList;
        private bool loaded;
        private bool lastAction;
        private DBAccessObj data;

        public List<Artist> ArtistList {get{return artistList;}}
        public bool ArtistLoaded {get {return loaded;}}

        public Artists()
        {
            artistList=new List<Artist>();
            loaded=false;
            lastAction=false;
            data = new DBAccessObj();
            
            List<(int, string)> artdta = data.GetArtistData();
            foreach ((int id, string name) a in artdta)
            {
                artistList.Add(new Artist(a.id, a.name));    
            }
            loaded=true;
        }
        
    }

    class Genre
    {
        private int genreid;
        private string genrename;  

        public int Genreid { get { return genreid; } set { genreid = value; } }
        public string Genrename { get { return genrename; } set { genrename = value; } }
        
        public Genre(int gid=-1, string gname="")
        {
            genreid=gid;
            genrename=gname;
            
        }

    }

    class Genres
    {
        private List<Genre> genreList;
        private bool loaded;
        private bool lastAction;
        private DBAccessObj data;

        public List<Genre> GenreList {get{return genreList;}}
        public bool GenreLoaded {get {return loaded;}}

        public Genres()
        {
            genreList=new List<Genre>();
            loaded=false;
            lastAction=false;
            data = new DBAccessObj();
            
            List<(int, string)> gendta = data.GetGenreData();
            foreach ((int id, string name) g in gendta)
            {
                genreList.Add(new Genre(g.id, g.name));    
            }
            loaded=true;
        }

    }
}