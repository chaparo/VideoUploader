using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace VideoUploader.Models
{

    public class Video
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Title :")]
        public string Title { get; set; }


        [StringLength(1000)]
        [Display(Name = "Description :")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }

        [DataType(DataType.Text)]
        public string VideoPath { get; set; }


        [DataType(DataType.ImageUrl)]
        public string ThumbUrl { get; set; }

        private DateTime? createdDate;

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }

        [Display(Name ="Frame selected for the Thumbnail")]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public float FrameThumb { get; set; }

    }
}