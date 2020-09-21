using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WealthShareV_2.Models;
using PagedList;
using Microsoft.Ajax.Utilities;

namespace WealthShareV_2.Controllers
{
    public class UserController : Controller
    {
        WealthShareDBV2Entities db = new WealthShareDBV2Entities();
        // GET: User
        
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User_Tbl user, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                User_Tbl u = new User_Tbl();
                u.UserName = user.UserName;
                u.UserEmail = user.UserEmail;
                u.UserPassword = user.UserPassword;
                u.Occupation = user.Occupation;
                u.DateOfBirth = user.DateOfBirth;
                u.imgPath = path;
                u.AccountStatus = "Pending";
               
               
                db.User_Tbl.Add(u);
                
                db.SaveChanges();


                return RedirectToAction("Login");
            }



            return View();
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User_Tbl userlogin)
        {
            User_Tbl ul = db.User_Tbl.Where(x => x.UserEmail ==userlogin.UserEmail && x.UserPassword== userlogin.UserPassword ).SingleOrDefault();

            
            
            
                if (ul != null)
                {
                     if (ul.AccountStatus == "Pending")

                     {
                         Response.Write("<script>alert('Please wait for account verification.Thank you.'); </script>");
                     }

                    else
                    {
                         Session["Id"] = ul.Id.ToString();
                         TempData["username"] = ul.UserName;
                         TempData.Keep();
                         return RedirectToAction("IssueFeed");

                    }

                   
                }
                else
                {
                    ViewBag.error = "Invalid Email Address or Password!";
                }

            

            

            return View();
        }





        public ActionResult UserProfile()

        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();


        }

        //logged in user can see create issue option

        //create Issue
        [HttpGet]
        public ActionResult CreateIssue()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                List<Category_Tbl> li = db.Category_Tbl.ToList();
                ViewBag.categorylist = new SelectList(li, "CategoryId", "CategoryName");
            }

            return View();
        }


        [HttpPost]
        public ActionResult CreateIssue(Issue_Tbl issue, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                Issue_Tbl iss = new Issue_Tbl();
                iss.Title = issue.Title;
                iss.Cat_Id = issue.Cat_Id;
                iss.Issue_Description = issue.Issue_Description;
                iss.Goal = issue.Goal;
                iss.issueImgPath = path;
                iss.UserId = Convert.ToInt32(Session["Id"].ToString());
                iss.IssueStatus = "Pending";

                db.Issue_Tbl.Add(iss);
                db.SaveChanges();

                return RedirectToAction("IssueFeed");


            }


            return View();
        }


        //issue feed
        public ActionResult IssueFeed(int? page)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Issue_Tbl.Where(x => x.IssueStatus == "Approved").OrderByDescending(x => x.Id_issue).ToList();
            IPagedList<Issue_Tbl> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }

        


  



        //pore function call korbo 
        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }



        


        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("IssueFeed");
        }


        //Individual issue details where the user can view the description, progress and organizer of the issue as well as make donations.
        //The custom model IssueDetailsModel is used to pass the data to the View from the Controller in order to show them on display.
        public ActionResult IssueDetails(int? id)
        {
            IssueDetailsModel idm = new IssueDetailsModel();
            Issue_Tbl it = db.Issue_Tbl.Where(x => x.Id_issue == id).SingleOrDefault();
            User_Tbl ut = db.User_Tbl.Where(x => x.Id == it.UserId).SingleOrDefault();

            idm.UserName = ut.UserName;
            idm.UserEmail = ut.UserEmail;
            idm.Occupation = ut.Occupation;
            idm.UserId = it.UserId;
            idm.Id_issue = it.Id_issue;
            idm.Title = it.Title;
            idm.IssueStatus = it.IssueStatus;
            idm.Issue_Description = it.Issue_Description;
            idm.Goal = it.Goal;
            idm.CurrentAmount = it.CurrentAmount;
            idm.issueImgPath = it.issueImgPath;
            idm.userImgPath = ut.imgPath;

            return View(idm);
        }

       

        
       


    }
}