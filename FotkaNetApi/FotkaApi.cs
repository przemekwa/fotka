﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FotkaNetApi
{
    public class FotkaApi
    {
        const string OnLineUrl = "http://www.fotka.pl/online/kobiety,1-30,wszystkie/1";

        public IEnumerable<Profile> GetOnLineProfiles()
        {
            var profileHtml = FotkaApiHelper.GetProfilesHtml(OnLineUrl);

            var lines = profileHtml.Split(new string[] { "\n" }, StringSplitOptions.None);

            var profiles = from line in lines
                           let profileNameLine = Regex.Match(line, "shadowed-avatar av-96\" href=\"/profil/[a-zA-z0-9]*\"")
                           where profileNameLine.Success
                           select new Profile
                           {
                               Name = profileNameLine.Groups[0].Value.Substring(37, (profileNameLine.Groups[0].Value.Length - 37 - 1))
                           };

            return profiles;
            
        }

        public Profile GetProfile(string name)
        {
            var profileBuldier = new ProfileBuldier();

            return profileBuldier.Build(new Profile { Name = name });
        }
    }
}
