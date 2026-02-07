
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

namespace BlazorDBDemo
{
    
    class DBAccessObj
    {
        private SqlConnectionStringBuilder builder;
        private SqlConnection connection;
        private bool connectionOpen;




        public DBAccessObj()
        {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "KAB-SQL\\KABSQL22";
            builder.UserID = "csharp";
            builder.Password = "BlueCat12";
            builder.InitialCatalog = "JCA";
            builder.TrustServerCertificate = true;
            connection = new SqlConnection(builder.ConnectionString);

        }

        public bool UpdateAlbum(int albumid, string albumname, string artistname, string gen)
        {
            bool success = false;
            int artistid = GetArtistID(artistname);
            int genid = GetGenreID(gen);
            if (artistid != -1 && genid != -1)
            {
                if (OpenDBConnection())
                {
                    string sql = $"UPDATE album set TITLE='{albumname}', ARTISTID={artistid}, GENREID={genid} WHERE ALBUMID={albumid}";
                    SqlCommand command = new SqlCommand(sql, connection);
                    int recs = command.ExecuteNonQuery();
                    success = recs == 1;
                    CloseDBConnection();    
                }
            }
            return success;
        }

        public bool DeleteAlbum(int albumid)
        {
            bool success = false;
            if (OpenDBConnection())
            {
                string sql = $"DELETE FROM album WHERE ALBUMID={albumid}";
                SqlCommand command = new SqlCommand(sql, connection);
                int recs = command.ExecuteNonQuery();
                success = recs == 1;
                CloseDBConnection();    
            }
            return success;
        }

        public int GetArtistID(string artistname)
        {
            int artid = -1;
            string sqlget = $"SELECT ArtistID from Artist WHERE Name='{artistname}'";
            if (OpenDBConnection())
            {
                SqlCommand getID = new SqlCommand(sqlget, connection);
                SqlDataReader reader = getID.ExecuteReader();
                if (reader.Read())
                {
                    artid = reader.GetInt32(0);
                }
                reader.Close();
                CloseDBConnection();
            }
            return artid;
        }

        public int GetGenreID(string genrename)
        {
            int genid = -1;
            string sqlget = $"SELECT GenreID from Genre WHERE Name='{genrename}'";
            if (OpenDBConnection())
            {
                SqlCommand getID = new SqlCommand(sqlget, connection);
                SqlDataReader reader = getID.ExecuteReader();
                if (reader.Read())
                {
                    genid = reader.GetInt32(0);
                }
                reader.Close();
                CloseDBConnection();
            }
            return genid;
        }

        public bool AddAlbum(string at, string art, string gen)
        {
            bool success = false;
            int artid = GetArtistID(art);
            int genid = GetGenreID(gen);
            if (artid != -1 && genid != -1)
            {
                if (OpenDBConnection())
                {
                    string sql = $"INSERT INTO Album(Title, ArtistID, GenreID) VALUES('{at}',{artid},{genid})";
                    SqlCommand command = new SqlCommand(sql, connection);
                    int recs = command.ExecuteNonQuery();
                    success = recs == 1;
                    CloseDBConnection();
                }

            }
            return success;
        }

        public List<(int, string, int, string, int, string, int)> GetAlbumData()
        {
            List<(int, string, int, string, int, string, int)> al = new List<(int, string, int, string, int, string, int)>();
            string whereclause=" WHERE 1=1 ";
            string sql = "SELECT al.AlbumID, al.Title title, ar.ArtistID, ar.Name ArtistName, g.genreID, g.Name genreName, count(*) Numtracks ";
            sql+=" FROM Album al INNER JOIN Artist ar on al.artistID=ar.artistID " ;
            sql+=" INNER JOIN Genre g on al.genreID=g.genreID " ;
            sql += " INNER JOIN track t on al.albumID=t.AlbumID ";
            sql += whereclause;
            sql += " GROUP BY al.AlbumID, al.Title, ar.ArtistID, ar.Name, g.genreID, g.Name ";
            sql+=" ORDER BY ArtistName ";
            if (OpenDBConnection())
            {
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    int artistid = reader.GetInt32(2);
                    string artistname = reader.GetString(3);
                    int genreid = reader.GetInt32(4);
                    string genrename = reader.GetString(5);
                    int numtracks = reader.GetInt32(6);
                    al.Add((id, title, artistid, artistname, genreid, genrename, numtracks));
                }
                reader.Close();
                CloseDBConnection();
            }

            return al;
        }

        public List<string> GetAlbumNames()
        {
            List<string> albums = [];
            string sql = "SELECT Title FROM Album";

            if (OpenDBConnection())
            {
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                string al;
                while (reader.Read())
                {
                    al = reader.GetString(0);
                    albums.Add(al);
                }
                reader.Close();
                CloseDBConnection();
            }

            return albums;
        }

        public List<(int, string)> GetGenreData()
        {
            List<(int, string)> ge = new List<(int, string)>();
            string whereclause=" WHERE 1=1 ";
            string sql = "SELECT GenreID, Name FROM Genre ";
            sql += whereclause;
            sql+=" ORDER BY Name ";
            if (OpenDBConnection())
            {
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name= reader.GetString(1);
                    
                    ge.Add((id, name));
                }
                reader.Close();
                CloseDBConnection();
            }

            return ge;
        }

        public List<(int, string)> GetArtistData()
        {
            List<(int, string)> ar = new List<(int, string)>();
            string whereclause=" WHERE 1=1 ";
            string sql = "SELECT ArtistID, Name FROM Artist ";
            sql += whereclause;
            sql+=" ORDER BY Name; ";
            if (OpenDBConnection())
            {
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name= reader.GetString(1);
                    
                    ar.Add((id, name));
                }
                reader.Close();
                CloseDBConnection();
            }

            return ar;
        }


        private bool OpenDBConnection()
        {
            if (connectionOpen)
            {
                connection.Close();
            }
            connection.Open();
            connectionOpen = true;
            bool rval = true;
            return rval;
        }

        private bool CloseDBConnection()
        {
            if (connectionOpen)
            {
                connection.Close();
                connectionOpen = true;
            }
            bool rval = true;
            return rval;
        }

    }
}