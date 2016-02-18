using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VideoUploader.Models;

namespace VideoUploader.Controllers
{
    public class VideosController : Controller
    {
        private VideoUploaderContext db = new VideoUploaderContext();

        // GET: Videos
        public ActionResult Index()
        {
            return View(db.Videos.ToList());
        }

        // GET: Videos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Videos/Create
        public ActionResult Upload()
        {
            return View();
        }

        // GET: Videos/Details/5
        public ActionResult Download(Video video)
        {

            string filePath = video.VideoPath;

            FileInfo file = new FileInfo(filePath);

            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "Video";
                Response.TransmitFile(file.FullName);
                Response.Flush();
                Response.End();
            }
            return new EmptyResult();
        }


        // POST: Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "Id,Title,Description,VideoUrl,ThumbUrl,CreatedDate,FrameThumb")] Video video)
        {
            
            //Prevent a dual upload or an attack could be optimize of course check IP etc...
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["file"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    IDeposite dep = DepositeFactory.CreateDeposite();
                    video.Id = Guid.NewGuid();
                    
                    video.VideoPath = dep.DepositeFile(file, video.Id.ToString(), ConfigurationManager.AppSettings["VideoServerPath"].ToString());
                    dep.GetThumbnail(video.VideoPath, ConfigurationManager.AppSettings["ImageServerPath"].ToString(), video.FrameThumb);

                    video.VideoUrl = String.Format("{0}{1}{2}", ConfigurationManager.AppSettings["VideoUrl"].ToString(), video.Id, ".mp4");
                    video.ThumbUrl = String.Format("{0}{1}{2}", ConfigurationManager.AppSettings["ImageUrl"].ToString(), video.Id, ".jpeg");
                }
            
                if (ModelState.IsValid)
                {
                    db.Videos.Add(video);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Upload");
        }

        // GET: Videos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,VideoUrl,ThumbUrl,CreatedDate")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
