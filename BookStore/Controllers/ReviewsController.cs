﻿using BookStore.Contracts;
using BookStore.Datastores;
using BookStore.DataTransferObjects;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStore.Controllers
{
    public class ReviewsController : ApiController
    {
        IUnitOfWork unit;

        public ReviewsController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        public IHttpActionResult Get(int bookid)
        {
            var result = unit.Reviews.All(bookid);

            if (!result.Any())
            {
                return NotFound();
            }

            var response = result.To<ReviewDto>();

            return Ok(response);
        }

        public IHttpActionResult Post([FromBody]Review review, int bookId)
        {
            review.BookId = bookId;
            var newReview = unit.Reviews.AddReview(review);
            unit.Commit();

            var url = Url.Link("DefaultApi", new { controller = "Reviews", id = newReview.Id });

            return Created(url, newReview);
        }

        public IHttpActionResult Delete(int id)
        {
            unit.Reviews.RemoveReview(id);
            unit.Commit();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
