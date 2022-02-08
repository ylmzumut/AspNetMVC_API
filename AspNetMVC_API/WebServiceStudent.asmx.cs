using AspNetMVC_API_BLL.Repository;
using AspNetMVC_API_Entity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AspNetMVC_API
{
    /// <summary>
    /// Summary description for WebServiceStudent
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceStudent : System.Web.Services.WebService
    {
        //Global alan
        StudentRepo myStudentRepo = new StudentRepo();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public List<Student> GetAll()
        {
            try
            {
                List<Student> list = myStudentRepo.GetAll();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string Insert(string name, string surname)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
                {
                    throw new Exception("name surname alanlarına mutlaka veri girilmelidir!");
                }
                Student newStudent = new Student()
                {
                    Name = name,
                    Surname = surname
                };
                int insertResult = myStudentRepo.Insert(newStudent);
                if (insertResult > 0)
                {
                    //1.yol
                    //return "Kayıt başarılı olarak eklendi. id" + newStudent.Id;
                    //2.yol
                    string jsonString = JsonConvert.SerializeObject(newStudent);
                    return jsonString;
                }
                else
                {
                    throw new Exception("Kayıt ekleme işleminde beklenmedik bir hata oluştu!");
                }


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    Student student = myStudentRepo.GetById(id);
                    if (student == null)
                    {
                        throw new Exception("Öğrenci bulunamadığı için silme işlemi başarısızdır!");
                    }
                    int deleteResult = myStudentRepo.Delete(student);
                    if (deleteResult > 0)
                    {
                        return "Kayıt silme işlemi başarılıdır.";
                    }
                    else
                    {
                        throw new Exception("Beklenmedik bir hata oluştuğu için kayıt silinemedi!");
                    }
                }
                else
                {
                    throw new Exception("Gönderilen id değeri sıfırdan büyük olmalıdır!");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string Update(int currentid, string newname, string newsurname)
        {
            try
            {
                if (currentid <= 0)
                {
                    throw new Exception("Gönderilen id değeri sıfırdan büyük olmalıdır!");
                }
                if (string.IsNullOrEmpty(newname) && string.IsNullOrEmpty(newsurname))
                {
                    throw new Exception("Gönderilen verilerden newname ve newsurname boş geçilemez!");
                }
                Student currentStudent = myStudentRepo.GetById(currentid);
                if (currentStudent == null)
                {
                    throw new Exception("Öğrenci bulunamadığı için güncelleme işlemi başarısızdır!");
                }
                //eğer newname parametresi dolu ise isim güncellenecek
                if (!string.IsNullOrEmpty(newname))
                {
                    currentStudent.Name = newname;
                }
                //eğer newsurname parametresi dolu ise soyisim güncellenecek
                if (!string.IsNullOrEmpty(newsurname))
                {
                    currentStudent.Surname = newsurname;
                }
                int updateResult = myStudentRepo.Update();
                if (updateResult > 0)
                {
                    return "Kayıt başarılı bir şekilde güncellendi.";
                }
                else
                {
                    throw new Exception("Beklenmedik bir hata nedeniyle kayıt güncelleme başarısız!");
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
