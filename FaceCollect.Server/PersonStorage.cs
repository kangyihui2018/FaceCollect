using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FaceCollect.Entity;

namespace FaceCollect.Server
{

    class PersonStorage
    {
        private static string DirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FacePic");
        private static string FilePersons = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"person.xml");
        public static DirectoryInfo DirFacePic = Directory.CreateDirectory(DirPath);
        private static List<Person> Persons = new List<Person>();

        public static ImageInfo GetImageInfoByName(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) return null;
            ImageInfo info = new ImageInfo();
            info.FileName = Path.GetFileName(fullFilePath);
            info.ImageData = File.ReadAllBytes(fullFilePath);
            return info;
        }

        public static void Init()
        {
            if (!File.Exists(FilePersons))
            {
          
                SerializeHelper.Save(Persons, FilePersons);
            }
            else
            {
                 Persons=(List<Person>)SerializeHelper.Load(Persons.GetType(), FilePersons);
            }
        }

        public static bool AddOrEdite(Person person)
        {
            lock (Persons)
            {
               var temp= Persons.FirstOrDefault(e => e.CertificateId == person.CertificateId);
                if (temp != null)
                {
                    Persons.Remove(temp);
                }
                Persons.Add(person);
                SavePersons();
            }
            return true;
        }


        private static void SavePersons()
        {
            SerializeHelper.Save(Persons, FilePersons);
        }

        public static List<Person> GetPersons(Func<Person,bool> func)
        {
            lock (Persons)
            {
                return Persons.Where(func).ToList();
            }
        }

       
    }
}
