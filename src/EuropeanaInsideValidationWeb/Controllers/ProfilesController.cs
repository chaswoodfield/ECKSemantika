using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace EuropeanaInsideValidationWeb.Controllers
{
    public class ProfilesController : ApiController
    {
        private const string ProfileUploadLocation = "~/UploadedProfiles/";


        public IEnumerable<string> Get(string provider)
        {
            var profLocation = MapPath(ProfileUploadLocation+"/"+provider);
            return Directory.Exists(profLocation) ? GetSubdirectoriesContainingOnlyFiles(profLocation) : new List<string>();
        }

        public HttpResponseMessage Get(string provider, string name)
        {

            var profileLocation = MapPath(ProfileUploadLocation + "/" + provider + "/" + name+"/profile.xsd");
            if (File.Exists(profileLocation))
            {
                var ms = new MemoryStream(File.ReadAllBytes(profileLocation));
                var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StreamContent(ms)};
                response.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                return response;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Post(string provider, string name)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var location = ProfileUploadLocation + "/" + provider + "/" + name;
                var profLocation = MapPath(location);
                if (!Directory.Exists(profLocation))
                {
                    Directory.CreateDirectory(profLocation);
                }



                var postedFile = httpRequest.Files[0];

                var filePath = MapPath(location + "/" + "profile.xsd");
                var extension = Path.GetExtension(filePath);
                if (extension != null && extension.ToLower().EndsWith("xsd"))
                {
                    postedFile.SaveAs(filePath);
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }


            if (httpRequest.InputStream.Length > 0)
            {
                var location = ProfileUploadLocation + "/" + provider + "/" + name;
                var profLocation = MapPath(location);
                if (!Directory.Exists(profLocation))
                {
                    Directory.CreateDirectory(profLocation);
                }

                var filePath = MapPath(location + "/" + "profile.xsd");
                var extension = Path.GetExtension(filePath);
                if (extension != null && extension.ToLower().EndsWith("xsd"))
                {
                    var content = new StreamReader(httpRequest.InputStream).ReadToEnd();
                    File.WriteAllText(filePath, content);
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Put(string provider, string name)
        {
            return Post(provider, name);
        }

        public HttpResponseMessage Delete(string provider, string name)
        {
            var profLocation = MapPath(ProfileUploadLocation + "/" + provider+"/"+name);
            if (Directory.Exists(profLocation))
            {
                var downloadedMessageInfo = new DirectoryInfo(profLocation);

                foreach (var file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in downloadedMessageInfo.GetDirectories())
                {
                    dir.Delete(true);
                }

                Directory.Delete(profLocation);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        #region HELPERS
        private string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);

        }

        static IEnumerable<string> GetSubdirectoriesContainingOnlyFiles(string path)
        {
           return from subdirectory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                   where Directory.GetDirectories(subdirectory).Length == 0
                   select new DirectoryInfo(subdirectory).Name;


        }

        #endregion
    }
}
