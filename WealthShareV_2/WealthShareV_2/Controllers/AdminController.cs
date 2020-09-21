using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WealthShareV_2.Models;
using PagedList;
using System.Net;
using System.Data.Entity;

namespace WealthShareV_2.Controllers
{
    public class AdminController : Controller
    {

        WealthShareDBV2Entities db = new WealthShareDBV2Entities();

        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin_Tbl admin)
        {
            Admin_Tbl ad =db.Admin_Tbl.Where(x =>x.AdminEmail==admin.AdminEmail && x.AdminPassword==admin.AdminPassword).SingleOrDefault();
            if (ad != null)
            {
                Session["Id_ad"] = ad.Id_ad.ToString();
                return RedirectToAction("IssueManagement");
            }
            else
            {
                ViewBag.error = "Invalid Email Address or Password!";
            }

            return View();
        }

        public ActionResult MemberManagement(int? page)

        {
            if(Session["Id_ad"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {



                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.User_Tbl.Where(x => x.AccountStatus=="Pending").OrderByDescending(x => x.Id).ToList();
                IPagedList<User_Tbl> mem = list.ToPagedList(pageindex, pagesize);
                return View(mem);
            }
           
        }


        public ActionResult IssueManagement(int? page)
        {
            if (Session["Id_ad"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.Issue_Tbl.Where(x =>x.IssueStatus == "Pending").OrderByDescending(x => x.Id_issue).ToList();
                IPagedList<Issue_Tbl> im = list.ToPagedList(pageindex, pagesize);



                return View(im);
            }
           
        }



        
        public ActionResult ApproveIssue(int? id)
        {


            if (id== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue_Tbl issue_Tbl = db.Issue_Tbl.Find(id);
            if (issue_Tbl == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.Cat_Id = new SelectList(db.Category_Tbl, "CategoryId", "CategoryName", issue_Tbl.Cat_Id);
            ViewBag.UserId = new SelectList(db.User_Tbl, "Id", "UserName", issue_Tbl.UserId);
            return View(issue_Tbl);



        }

        // POST: Verify/Edit/5
        // To protect from overposting attacks, enable the specific properties  want to bind 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveIssue([Bind(Include = "Id_issue,UserId,Category,Title,Issue_Description,Goal,CurrentAmount,IssueStatus,issueImgPath,Cat_Id")] Issue_Tbl issue_Tbl)
        {
            if (ModelState.IsValid)
            {
                

                db.Entry(issue_Tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IssueManagement");
            }
           
            ViewBag.Cat_Id = new SelectList(db.Category_Tbl, "CategoryId", "CategoryName", issue_Tbl.Cat_Id);
            ViewBag.UserId = new SelectList(db.User_Tbl, "Id", "UserName", issue_Tbl.UserId);
            return View(issue_Tbl);
        }








        public ActionResult DeleteIssue(int? id)
        {

            Issue_Tbl issuedelete = db.Issue_Tbl.Where(x => x.Id_issue == id).SingleOrDefault();
            db.Issue_Tbl.Remove(issuedelete);
            db.SaveChanges();

            return RedirectToAction("IssueManagement");
        }




        




        public ActionResult ApproveMember(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Tbl user_Tbl = db.User_Tbl.Find(id);
            if (user_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(user_Tbl);

        }


        // POST: VerifyUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveMember([Bind(Include = "Id,UserName,UserEmail,UserPassword,DateOfBirth,Occupation,AccountStatus,imgPath")] User_Tbl user_Tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MemberManagement");
            }
            return View();
        }



      





        public ActionResult IssueFeed(int? page)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Issue_Tbl.Where(x => x.IssueStatus == "Approved").OrderByDescending(x => x.Id_issue).ToList();
            IPagedList<Issue_Tbl> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }



        public ActionResult SignoutAdmin()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("IssueFeed");
        }




    }
}