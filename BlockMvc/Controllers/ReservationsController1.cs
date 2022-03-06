using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace BlockMvc.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Reservation> reservationsList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:25856/api/reservations"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservationsList = JsonConvert.DeserializeObject<List<Reservation>>(apiResponse);
                }
            }

            return View(reservationsList);
        }

        [HttpPost]
        public async Task<IActionResult> GetReservation(int id)
        {
            Reservation reservation = new Reservation();

            using (var httpClient = new HttpClient())
            {
                using (var response = await
                httpClient.GetAsync("http://localhost:25856/api/reservations/" + id))
                {
                    string apiResponse = await
                    response.Content.ReadAsStringAsync();
                    reservation =
                    JsonConvert.DeserializeObject<Reservation>(apiResponse);
                }
            }
            return View(reservation);
        }

        public ViewResult AddReservation() => View();

        [HttpPost]
        public async Task<IActionResult> AddReservation(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new
                StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8,
                "application/json");
                using (var response = await
                httpClient.PostAsync("http://localhost:25856/api/reservations", content))
                {
                    string apiResponse = await
                    response.Content.ReadAsStringAsync();
                    receivedReservation =
                    JsonConvert.DeserializeObject<Reservation>(apiResponse);
                }
            }
            return View(receivedReservation);
        }

        public async Task<IActionResult> UpdateReservation(int id)
        {
            Reservation reservation = new Reservation();

            using (var httpClient = new HttpClient())
            {
                using (var response = await
                httpClient.GetAsync("http://localhost:25856/api/reservations/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                }
            }
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReservation(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();

            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(reservation.Id.ToString()), "Id");
                content.Add(new StringContent(reservation.Name), "Name");
                content.Add(new StringContent(reservation.StartLocation), "StartLocation");
                content.Add(new StringContent(reservation.EndLocation), "EndLocation");
                using (var response = await httpClient.PutAsync("http://localhost:25856/api/reservations", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);

                }
            }
            return View(receivedReservation);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReservation(int ReservationId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await
                httpClient.DeleteAsync("http://localhost:25856/api/reservations" +
                ReservationId))
                {
                    string apiResponse = await
                    response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }


    }
}
