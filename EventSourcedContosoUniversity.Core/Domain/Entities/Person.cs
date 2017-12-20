﻿using System;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Person
    {
        public Guid Id { get; protected set; }

        //[Required]
        //[StringLength(50)]
        //[Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        //[Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        //[Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
    }
}
