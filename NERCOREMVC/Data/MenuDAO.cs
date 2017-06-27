using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ServiceStack.OrmLite;
using Entities;

namespace Data
{
    public class MenuDAO
    {
        public List<Menu> GetAll()
        {
            using(var dbconn = new OrmliteConnection().openConn())
            {
                return dbconn.Select<Menu>();
            } 
        }

        public List<Menu> GetRootMenu()
        {
            using (var dbconn = new OrmliteConnection().openConn())
            {
                var data = dbconn.Select<Menu>(m=>m.ParentID == 0 && m.Status == true);
                return dbconn.Select<Menu>(m=>m.ParentID < 1 && m.Status==true);
            }
        }

        public List<Menu> GetSubMenu(int ID)
        {
            using (var dbconn = new OrmliteConnection().openConn())
            {
                return dbconn.Select<Menu>(m => m.ParentID == ID && m.Status==true);
            }
        }
    }
}
