using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Vin.ly
{
    public class DataStoreLogIn
    {
        private static bool loggedIn;
        private static string userName;

        // These are for future iterations of this program, so that I can use them for fragments or separate activities.
        // For the purposes of this project, however, we can just populate a single item.
        private static string editWineName;
        private static string editWineType;
        private static string editWineRegion;
        private static int editWineVarietal;

        public static void setGlobalUser(string uName)
        {
            userName = uName;
        }

        public static void setGlobalLoggedIn(bool logIn)
        {
            loggedIn = logIn;
        }

        public static void setGlobaleditNamen(string editName)
        {
            editWineName = editName;
        }

        public static void setGlobaleditWineTye(string editType)
        {
            editWineType = editType;
        }

        public static void setGlobaleditWineRegion(string editRegion)
        {
            editWineRegion = editRegion;
        }

        public static void setGlobaleditWineRegion(int editYear)
        {
            editWineVarietal = editYear;
        }
                
        public static string getGlobalUser()
        {
            return userName;
        }

        public static bool getGlobalBoolLoggedIn()
        {
            return loggedIn;
        }

        public static string getGlobalEditWineName()
        {
            return editWineName;
        }

        public static string getGlobalEditWineType()
        {
            return editWineType;
        }
        public static string getGlobalEditWineRegion()
        {
            return editWineRegion;
        }
        public static int getGlobalEditWineVarietal()
        {
            return editWineVarietal;
        }


    

        
    }
}